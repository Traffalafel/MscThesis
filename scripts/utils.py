import os 
import re
import matplotlib.pyplot as plt

def find_file(root_dir, file_name):
    for dirpath, _, filenames in os.walk(root_dir):
        for f in filenames:
            if f == file_name:
                return os.path.join(dirpath, f)
    return None

def get_files_recurse(root_dir, pattern=None):
    for dirpath, _, filenames in os.walk(root_dir):
        for f in filenames:
            if pattern is None or re.match(pattern, f) is not None:
                yield dirpath, f

def get_filename(file_path):
    name = os.path.split(file_path)[1]
    return os.path.splitext(name)[0]

def get_file_lines(file_path, property):
    pattern = f"^(\w+);{property};(\w+)\t"
    with open(file_path, 'r') as fd:
        lines = fd.readlines()
    for line in lines:
        match = re.match(pattern, line)
        if match is None:
            continue
        optimizer = match.group(1)
        parameter = match.group(2)
        yield optimizer, parameter, re.sub(pattern, '', line).replace('\n', '')

def remove_parens(s):
    return s.replace('(', '').replace(')', '')

def clean_line(line):
    split = line.split(';')
    points = [remove_parens(s).split(',') for s in split]
    points = [[float(p[0]), float(p[1])] for p in points]
    xs = [p[0] for p in points]
    ys = [p[1] for p in points]
    return xs, ys

def create_chart(vals, prop):
    lines = []
    vals = sorted(vals, key=lambda v: v[0]) # sort by size
    for size,filepath,param in vals:
        for _, _, line in get_file_lines(filepath, prop):
            lines.append((size, line))

    for (size, line) in lines:
        xs, ys = clean_line(line)
        plt.scatter(xs, ys, label=f"n={size}", marker=".")
