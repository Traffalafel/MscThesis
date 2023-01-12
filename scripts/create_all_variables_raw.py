import re
import os
import matplotlib.pyplot as plt
import utils

RESULTS_DIR = "results"
FILENAME_PATTERN = "([a-zA-z0-9_]+)(_\d+)_variable(\w+)\.txt"
PROPERTIES = ["BestFitness", "NumberFitnessCalls", "CpuTimeSeconds"]
CHARTS_DIR = r"G:\My Drive\Dokumenter\DTU\Speciale\raw charts\variable"

def main():

    names = dict()
    for dirpath, f in utils.get_files_recurse(RESULTS_DIR, FILENAME_PATTERN):
        match = re.match(FILENAME_PATTERN, f)
        name = match.group(1)
        size_group = match.group(2)
        if size_group is not None:
            size = int(size_group.replace('_', ''))
        else:
            size = 100
        param = match.group(3)

        key = f"{name}_variable{param}"
        filepath = os.path.join(dirpath, f)
        val = (size, filepath, param)
        if key not in names:
            names[key] = [val]
        else:
            names[key].append(val)

    for key in names:
        for prop in PROPERTIES:
            utils.create_chart(names[key], prop, None)
            plt.title(key)
            plt.ylabel(prop)
            output_path = os.path.join(CHARTS_DIR, f"{key} {prop}.png")
            print(output_path)
            plt.legend(bbox_to_anchor=(1, 1), loc='upper left')
            plt.tight_layout()
            plt.savefig(output_path, dpi=300)
            plt.cla()
            plt.clf()

main()