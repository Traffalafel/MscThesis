import sys
import re
import os
import matplotlib.pyplot as plt
import utils

RESULTS_DIR = "results"
FILENAME_PATTERN_TEMPLATE = "*1(_\d+)?_variable*2"

def main():

    if len(sys.argv) != 5:
        print("Usage: <name> <variable> <prop> show/save")
        return

    name = sys.argv[1]
    param = sys.argv[2]
    prop = sys.argv[3]
    display = sys.argv[4]

    pattern = FILENAME_PATTERN_TEMPLATE.replace("*1", name).replace("*2", param)

    vals = []
    for dirpath, f in utils.get_files_recurse(RESULTS_DIR, pattern):
        match = re.match(pattern, f)
        size_group = match.group(1)
        if size_group is not None:
            size = int(size_group.replace('_', ''))
        else:
            size = 100

        key = f"{name}_variable{param}"
        filepath = os.path.join(dirpath, f)
        val = (size, filepath, param)
        vals.append(val)

    utils.create_chart(key, vals, prop)

    if display == "show":
        plt.show()
        return
    else:
        plt.savefig(display, dpi=300)
        plt.cla()
        plt.clf()

main()