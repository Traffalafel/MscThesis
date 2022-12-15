import sys
import os 
import re
import json
import shutil

SPECS_DIR_PATH = "Specifications"
RESULTS_TMP_DIR_PATH = "results_tmp"

is_tmp_pattern = r'([\w_]+)_tmp\.txt'
is_split_pattern = r'([\w_]+)_pt([0-9]+)'

def remove_optimizer(lines, optimizer):
    return lines

def find_file_recurse(dir_path, file_name):
    for dirpath, _, filenames in os.walk(dir_path):
        for f in filenames:
            if f == file_name:
                return os.path.join(dirpath, f)
    return None

def main():

    files = [f for f in os.listdir(RESULTS_TMP_DIR_PATH)]
    if len(files) == 0:
        print("No files found")
        return

    for f in files:

        tmp_match = re.match(is_tmp_pattern, f)
        if tmp_match is None:
            print(f"{f} is not tmp")
            continue

        print(f)

        id = None
        name = tmp_match.group(1)
        split_match = re.match(is_split_pattern, f)
        if split_match is not None:
            name = split_match.group(1)
            id = int(split_match.group(2))
    
        file_path = os.path.join(RESULTS_TMP_DIR_PATH, f)
        with open(file_path, 'r') as fd:
            lines = fd.readlines()

        optimizers = set()
        optimizers_empty = set()

        line_pattern = r'^(\S+);\w+;'
        for line in lines:
            line = line.replace('\n', '')
            line_match = re.match(line_pattern, line)
            if line_match is None:
                continue
            optimizer = line_match.group(1)

            optimizers.add(optimizer)
            split = line.split('\t')
            if len(split) > 1 and len(split[1]) == 0:
                optimizers_empty.add(optimizer)

        # Fix specs
        if id is None:
            spec_file_name = f"{name}.json"
        else:
            spec_file_name = f"{name}_pt{id}.json"
        spec_file_path = find_file_recurse(SPECS_DIR_PATH, spec_file_name)
        if spec_file_path is None:
            print(f"Could not find {spec_file_name}")
            return

        with open(spec_file_path, 'r') as fd:
            spec_contents = fd.read()

        spec_json_missing = json.loads(spec_contents)
        spec_json_missing["Optimizers"] = [o for o in spec_json_missing["Optimizers"] if o["Name"] in optimizers_empty]
        num_missing = len(spec_json_missing["Optimizers"])

        spec_json_complete = json.loads(spec_contents)
        spec_json_complete["Optimizers"] = [o for o in spec_json_complete["Optimizers"] if o["Name"] not in optimizers_empty]

        if id is None:
            spec_name_complete = f"{name}_pt1.json"
            spec_name_missing = f"{name}_pt2.json"
        else:
            spec_name_complete = f"{name}_pt{id}.json"
            id_missing = id
            while True:
                id_missing += 1
                spec_name_missing = f"{name}_pt{id_missing}.json"
                if find_file_recurse(SPECS_DIR_PATH, spec_name_missing) is None:
                    break

        spec_dir = os.path.split(spec_file_path)[0]
        spec_path_complete = os.path.join(spec_dir, spec_name_complete)
        spec_path_missing = os.path.join(spec_dir, spec_name_missing)

        if num_missing > 0:
            shutil.copy(spec_file_path, spec_path_missing)
            with open(spec_path_missing, 'w') as fd:
                fd.write(json.dumps(spec_json_missing, indent=2))
        else:
            print(f"Skipping {spec_path_missing}")
        
        shutil.move(spec_file_path, spec_path_complete)
        with open(spec_path_complete, 'w') as fd:
            fd.write(json.dumps(spec_json_complete, indent=2))

        lines_out = []
        line_pattern = r'^(\S+);\w+;'
        for line in lines:
            line = line.replace('\n', '')
            line_match = re.match(line_pattern, line)
            if line_match is None:
                lines_out.append(line)
                continue
            optimizer = line_match.group(1)
            if optimizer not in optimizers_empty:
                lines_out.append(line)
        lines_out = [l + '\n' for l in lines_out]

        file_name_new = f.replace('_tmp', '')
        file_path_new = os.path.join(RESULTS_TMP_DIR_PATH, file_name_new)
        shutil.move(file_path, file_path_new)
        with open(file_path_new, 'w') as fd:
            fd.writelines(lines_out)

main()