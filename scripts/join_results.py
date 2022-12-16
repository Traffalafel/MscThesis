import sys
import re
import os
import matplotlib.pyplot as plt

RESULTS_DIR = "results"

def get_filename(file_path):
    name = os.path.split(file_path)[1]
    return os.path.splitext(name)[0]

def find_matching_files(dir_path, pattern):
    for dirpath, _, filenames in os.walk(dir_path):
        for f in filenames:
            if re.match(pattern, f):
                yield os.path.join(dirpath, f)
    return None

def get_files(file_path):
    file_name = get_filename(file_path)
    pattern = file_name.replace('_variable', "_\d+_variable")
    return find_matching_files(RESULTS_DIR, pattern)

def get_file_lines(file_path, parameter):
    pattern = f"^\w+;{parameter};\w+\t"
    with open(file_path, 'r') as fd:
        lines = fd.readlines()
    for line in lines:
        if not re.match(pattern, line):
            continue
        yield re.sub(pattern, '', line).replace('\n', '')

def remove_parens(s):
    return s.replace('(', '').replace(')', '')

def clean_line(line):
    split = line.split(';')
    points = [remove_parens(s).split(',') for s in split]
    points = [[float(p[0]), float(p[1])] for p in points]
    xs = [p[0] for p in points]
    ys = [p[1] for p in points]
    return xs, ys

def main():

    file_path = sys.argv[1]
    parameter = sys.argv[2]

    lines = []
    for line in get_file_lines(file_path, parameter):
        lines.append((file_path, line))
    files = get_files(file_path)
    for file in files:
        for line in get_file_lines(file, parameter):
            lines.append((file, line))
    
    for (file, line) in lines:
        file_name = get_filename(file)
        xs, ys = clean_line(line)
        plt.plot(xs, ys, label=file_name)
    
    plt.legend()
    plt.show()

main()