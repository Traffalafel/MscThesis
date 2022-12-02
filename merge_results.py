import sys
import os
import shutil
import re

def main():

    name = sys.argv[1]
    dir_path = sys.argv[2]

    files = [f for f in os.listdir(dir_path)]
    pattern = f'{name}_pt\d+.txt'
    files = [f for f in files if re.match(pattern, f)]
    files = [os.path.join(dir_path, f) for f in files]

    if len(files) == 0:
        return

    fittest = []
    items = []
    series = []

    for f in files:
        with open(f, 'r') as fd:
            lines = fd.readlines()
        lines = [line.replace('\n', '') for line in lines]
        c = 0
        while lines[c] != "Fittest:":
            c += 1
        c += 1
        while lines[c] != "Items:":
            fittest.append(lines[c])
            c += 1
        c += 1
        while lines[c] != "Series:":
            items.append(lines[c])
            c += 1
        c += 1
        while c < len(lines):
            series.append(lines[c])
            c += 1

    dst_path = os.path.join(dir_path, f"{name}.txt")
    shutil.copy(files[0], dst_path)

    with open(dst_path, 'r') as fd:
        lines = fd.readlines()

    lines = [line.replace('\n', '') for line in lines]
    lines_out = []
    c = 0
    while lines[c] != "Fittest:":
        lines_out.append(lines[c])
        c += 1
    
    lines_out.append("Fittest:")
    for l in fittest:
        lines_out.append(l)
    lines_out.append("Items:")
    for l in items:
        lines_out.append(l)
    lines_out.append("Series:")
    for l in series:
        lines_out.append(l)

    with open(dst_path, 'w') as fd:
        content = '\n'.join(lines_out)
        fd.write(content)

main()