import sys
import re
import os

VIEWS_DIR_PATH = "views"
RESULTS_DIR_PATH = "results"

def all_files_recurse(dir_path):
    for dirpath, _, filenames in os.walk(dir_path):
        for f in filenames:
            yield os.path.join(dirpath, f)

def get_filename(file_path):
    return os.path.split(file_path)[1]

def main():

    if len(sys.argv) != 3:
        print("Usage: <name> <size>")
        return

    name = sys.argv[1]
    size = sys.argv[2]

    file_paths = all_files_recurse(RESULTS_DIR_PATH)

    file_pattern = f'{name}_PerturbedTSP_([\d\.]+)_{size}.txt'
    line_pattern = f"(.+);BestFitness\t([\d\.]+)"

    results = dict()

    for file_path in file_paths:

        file_name = get_filename(file_path)
        match_file = re.match(file_pattern, file_name)
        if match_file is None:
            continue

        perturbation = match_file.group(1)

        with open(file_path, 'r') as fd:
            lines = fd.readlines()
        lines = [line.replace('\n', '') for line in lines]

        for line in lines:

            match_line = re.match(line_pattern, line)

            if match_line is None:
                continue

            optimizer = match_line.group(1)
            fitness = match_line.group(2)

            if optimizer not in results:
                results[optimizer] = [(perturbation, fitness)]
            else:
                results[optimizer].append((perturbation, fitness))

    for optimizer in results:
        r = sorted(results[optimizer], key=lambda tup: tup[0])
        print(f"{optimizer};BestFitness;Perturbation\t{';'.join(f'({p},{f})' for p,f in r)}")

main()
