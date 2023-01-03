import sys
import re
import os
import matplotlib.pyplot as plt
import utils
import argparse, sys

RESULTS_DIR = "results"
FILENAME_PATTERN_TEMPLATE = "*1(_\d+)?_variable*2"
CHARTS_DIR = r"G:\My Drive\Dokumenter\DTU\Speciale\charts"
TRANSLATIONS = {
    "ProblemSize": "Problem size",
    "NumFitnessCalls": "Avg. fitness function calls",
    "CpuTimeSeconds": "Avg. CPU time (seconds)",
    "NumberIterations": "Iteration number",
    "BestFitness": "Avg. best fitness",
}

def translate(s, args):
    if args.final:
        return TRANSLATIONS[s]
    else:
        return s

def main():

    parser = argparse.ArgumentParser()
    parser.add_argument('name')
    parser.add_argument('property')
    parser.add_argument('--variable')
    parser.add_argument("--title")
    parser.add_argument("--x_limit")
    parser.add_argument("--y_limit")
    parser.add_argument("--save", action=argparse.BooleanOptionalAction)
    parser.add_argument("--line", action=argparse.BooleanOptionalAction)
    parser.add_argument("--hide-legend", action=argparse.BooleanOptionalAction)
    parser.add_argument("--final", action=argparse.BooleanOptionalAction)
    args = parser.parse_args()

    if args.title is not None:
        plt.title(args.title)
    else:
        plt.title(args.name)

    if args.variable is None:
        # Create single result chart
        file_path = utils.find_file(RESULTS_DIR, f"{args.name}.txt")
        lines = utils.get_file_lines(file_path, args.property)
        for optimizer, parameter, data in lines:
            xs, ys = utils.clean_line(data)
            plt.scatter(xs, ys, marker=".", label=optimizer)
            if args.line:
                plt.plot(xs, ys)
        plt.xlabel(translate(parameter, args))
        plt.ylabel(translate(args.property, args))  
    else:
        # Create variable chart
        vals = []
        pattern = FILENAME_PATTERN_TEMPLATE.replace("*1", args.name).replace("*2", args.variable)
        for dirpath, f in utils.get_files_recurse(RESULTS_DIR, pattern):
            match = re.match(pattern, f)
            size = int(match.group(1).replace('_', ''))
            filepath = os.path.join(dirpath, f)
            val = (size, filepath, args.variable)
            vals.append(val)
        utils.create_chart(vals, args.property)
        plt.xlabel(translate(args.variable, args))
        plt.ylabel(translate(args.property, args))  

    if args.hide_legend is None:
        plt.legend(bbox_to_anchor=(1, 1), loc='upper left')
        plt.tight_layout()

    if args.x_limit is not None:
        x_limit = float(args.x_limit)
        plt.gca().set_xlim(right=x_limit)
    if args.y_limit is not None:
        y_limit = float(args.y_limit)
        plt.gca().set_ylim(top=y_limit)

    if args.save:
        file_path = os.path.join(CHARTS_DIR, f"{args.name} {args.variable} {args.property}.png")
        plt.savefig(file_path, dpi=300)
        plt.cla()
        plt.clf()
        print(f"Saved to file {file_path}")
    else:
        plt.show()

main()