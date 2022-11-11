import matplotlib.pyplot as plt
import sys

# Renders plot from data file

file_path = sys.argv[1]

with open(file_path, 'r') as fd:
    lines = fd.readlines()

x_prop = None
y_prop = None

def remove_parens(s):
    return s.replace('(', '').replace(')', '')

for line in lines:

    # clean line
    line = line.replace('\n', '')
    if (len(line) == 0):
        continue

    cols = line.split('\t')

    info = cols[0].split(';')
    name = info[0]

    y = info[1]
    if y_prop == None:
        y_prop = y
    elif y_prop != y:
        raise "Y property must be the same for all lines"

    x = info[2]
    if x_prop == None:
        x_prop = x
    elif x_prop != x:
        raise "X property must be the same for all lines"

    points = cols[1].split(';')
    points = [remove_parens(point) for point in points]
    points = [point.split(',') for point in points]
    xs = [float(x) for (x,_) in points]
    ys = [float(y) for (_,y) in points]

    plt.plot(xs, ys, label=name)

plt.xlabel(x_prop)
plt.ylabel(y_prop)

# Put a legend below current axis
plt.legend(loc='upper left', fancybox=False, shadow=False, ncol=1, prop={'size': 8})

plt.show()
