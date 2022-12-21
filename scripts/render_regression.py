import math
import sys
import matplotlib.pyplot as plt
import numpy as np

def create_function(s):
    s = s.replace("log", "math.log")    
    s = s.replace("sqrt", "math.sqrt")    
    s = "lambda n:" + s
    return eval(s)

def clean_points(line):
    line = line.replace(" ", "")
    return [float(p) for p in line.split(',')]

def main():

    if len(sys.argv != 2):
        print("Usage: <file_path>")
        return

    file_path = sys.argv[1]

    with open(file_path, 'r') as fd:
        lines = [line.replace('\n', '') for line in fd.readlines()]

    parameter = lines[0]
    xs = clean_points(lines[1])
    ys = clean_points(lines[2])

    end_x = max(xs) + 50
    dx = np.linspace(0, end_x, 100)

    plt.scatter(xs, ys)

    for line in lines[3:]:
        f = create_function(line)
        plt.plot(dx, [f(x) for x in dx], label=line)

    plt.legend()
    plt.xlabel("Problem size")
    plt.ylabel(parameter)
    plt.show()

main()