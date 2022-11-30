import matplotlib.pyplot as plt
import sys
import os
import numpy as np

CHARTS_DIR_PATH = r"C:\Users\traff\Desktop\charts"

# Renders plot from data file

def get_filename(path):
    (_, tail) = os.path.split(path)
    [name, _] = tail.rsplit('.', 1)
    return name

def remove_parens(s):
    return s.replace('(', '').replace(')', '')

def main():

    if len(sys.argv) != 3:
        print("Usage: <file_path> show/save")
        return

    file_path = sys.argv[1]
    display = sys.argv[2]

    with open(file_path, 'r') as fd:
        lines = fd.readlines()

    title = lines[0]
    x_label = lines[1]
    y_label = lines[2]

    lines = lines[3:]

    num_lines = 0
    line_length = None

    line_points = []

    for line in lines:

        # clean line
        line = line.replace('\n', '')
        if (len(line) == 0):
            continue
        num_lines += 1

        points = line.split(';')

        num_points = len(points)
        if line_length is None:
            line_length = num_points
        elif line_length != num_points:
            raise "Lines must have same number of points!"

        points = [remove_parens(point) for point in points]
        points = [point.split(',') for point in points]
        line_points.append(points)

    x_vals = []
    y_vals = []
    zs = np.zeros((num_lines, line_length))

    for x_idx, ps in enumerate(line_points):
        x_val = None
        for y_idx, p in enumerate(ps):

            x,y,z = (float(v) for v in p)

            if len(y_vals) <= y_idx:
                y_vals.append(y)
            elif y_vals[y_idx] != y:
                raise "Y values must be same"

            if x_val is None:
                x_val = x
            elif x_val != x:
                raise "X values must be same"

            zs[y_idx,x_idx] = z
            
        x_vals.append(x_val)

    x_vals = [str(x_val) for x_val in x_vals]
    y_vals = [str(y_val) for y_val in y_vals]

    y_vals.reverse()
    zs = np.flip(zs, axis=0)

    fig, ax = plt.subplots()
    im = ax.imshow(zs)

    # Show all ticks and label them with the respective list entries
    ax.set_xticks(np.arange(len(x_vals)), labels=x_vals)
    ax.set_yticks(np.arange(len(y_vals)), labels=y_vals)

    # Rotate the tick labels and set their alignment.
    plt.setp(ax.get_xticklabels(), rotation=45, ha="right",
            rotation_mode="anchor")

    # Loop over data dimensions and create text annotations.
    for i in range(len(x_vals)):
        for j in range(len(y_vals)):
            text = ax.text(j, i, zs[i, j], ha="center", va="center", color="w")

    ax.set_title(title)
    ax.set_xlabel(x_label)
    ax.set_ylabel(y_label)
    fig.tight_layout()

    file_name = get_filename(file_path)

    if display == 'save':
        file_path_new = os.path.join(CHARTS_DIR_PATH, file_name + ".png")
        plt.savefig(file_path_new, dpi=300)
    if display == 'show':
        plt.show()

main()