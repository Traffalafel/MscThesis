import math
import sys
import matplotlib.pyplot as plt
import numpy as np
import os
import utils
import re

INFO_PATTERN = r"([\w ]+): (.+)"
CHARTS_DIR = r"G:\My Drive\Dokumenter\DTU\Speciale\charts\regression"

def create_function(s):
    s = s.replace("log", "math.log2")    
    s = s.replace("sqrt", "math.sqrt")    
    s = "lambda n:" + s
    return eval(s)

def clean_points(line):
    line = line.replace(" ", "")
    return [float(p) for p in line.split(',')]

def main():

    if len(sys.argv) != 3:
        print("Usage: <file_path> show/save")
        return

    file_path = sys.argv[1]
    display_option = sys.argv[2]

    with open(file_path, 'r') as fd:
        lines = [line.replace('\n', '') for line in fd.readlines()]

    info = dict()
    c = 0
    while c < len(lines) and re.match(INFO_PATTERN, lines[c]) is not None:
        match = re.match(INFO_PATTERN, lines[c])
        key = match.group(1)
        val = match.group(2)
        info[key] = val
        c += 1

    parameter = info['Param']
    title = info['Title']
    xs = clean_points(info['X'])
    ys = clean_points(info['Y'])

    x_limit = None
    if "X limit" in info: 
        x_limit = float(info['X limit'])
    y_limit = None
    if "Y limit" in info: 
        y_limit = float(info['Y limit'])

    end_x = max(xs) + 50
    dx = np.linspace(1, end_x, 100)

    plt.scatter(xs, ys)

    for line in lines[c:]:
        f = create_function(line)
        plt.plot(dx, [f(x) for x in dx], label=line)

    plt.legend()
    plt.title(title)
    plt.xlabel("Problem size")
    plt.ylabel(parameter)

    if x_limit is not None:
        plt.gca().set_xlim(right=x_limit)
    if y_limit is not None:
        plt.gca().set_ylim(top=y_limit)

    if display_option == "show":
        plt.show()
    else:
        file_name = utils.get_filename(file_path)
        out_file_path = os.path.join(CHARTS_DIR, f"{file_name} regression.png")
        plt.savefig(out_file_path, dpi=300)
        plt.cla()
        plt.clf()   

main()