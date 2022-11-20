import matplotlib.pyplot as plt
import sys
import os

CHARTS_DIR_PATH = r"C:\Users\traff\Desktop\charts"

# Renders plot from data file

def get_filename(path):
    (_, tail) = os.path.split(path)
    [name, _] = tail.rsplit('.', 1)
    return name

def remove_parens(s):
    return s.replace('(', '').replace(')', '')

def main():

    if (len(sys.argv) < 5):
        print("Usage: <y_property> <output_file> <input_file_1> <input_file_2> ...")
        return

    y_prop = sys.argv[1]
    output_file = sys.argv[2]
    file_paths = sys.argv[3:]

    output_lines = []

    for file_path in file_paths:
        with open(file_path, 'r') as fd:
            lines = fd.readlines()

        for line in lines:

            # clean line
            line = line.replace('\n', '')
            if (len(line) == 0):
                continue

            cols = line.split('\t')
            if len(cols) == 1:
                continue # skip fittest info

            info = cols[0].split(';')
            if len(info) <= 2:
                continue # skip item data

            line_y_prop = info[1]
            if (line_y_prop == y_prop):
                output_lines.append(line + '\n')

    with open(output_file, 'w+') as fd:
        fd.writelines(output_lines)

main()