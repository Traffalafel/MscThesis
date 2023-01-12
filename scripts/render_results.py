import sys
import re
import os
import matplotlib.pyplot as plt
import utils
import math
import argparse, sys
from scipy.optimize import curve_fit
from sklearn.metrics import r2_score
import numpy as np

# You are hereby entering the spaghetti zone
# Enter at your own risk

RESULTS_DIR = "results"
FILENAME_PATTERN_TEMPLATE = "*1(_\d+)?_variable*2"
CHARTS_DIR_DEFAULT = r"G:\My Drive\Dokumenter\DTU\Speciale\charts"
TRANSLATIONS = {
    "ProblemSize": "Problem size",
    "NumberFitnessCalls": "Avg. fitness function calls",
    "CpuTimeSeconds": "Avg. CPU time (seconds)",
    "NumberIterations": "Iteration number",
    "BestFitness": "Avg. best fitness",
    "K": "K",
    "Pop": "Population size",
    "Gap": "Gap size",
    "Shedding": "Shedding interval",
    "Sampling": "Sampling intensity",
    "Perturbation": "Perturbation"
}

def translate(s, args):
    if args.report:
        return TRANSLATIONS[s]
    else:
        return s

def get_func(s):
    match s:
        case "log":
            return lambda x,a: a*np.log2(x)
        case "linear":
            return lambda x,a: a*x
        case "quadratic":
            return lambda x,a: a*(x**2)
        case "cubic":
            return lambda x,a: a*(x**3)
        case "quartic":
            return lambda x,a: a*(x**4)
        case "power":
            return lambda x,a,n: a*(x**n)
        case "loglinear":
            return lambda x,a: a*x*np.log2(x)
        case "logsqrt":
            return lambda x,a: a*np.log2(x)*np.sqrt(x)
        case "logpower":
            return lambda x,a,b: a*(x**b)*np.log2(x)
        case "sqrt":
            return lambda x,a: a*np.sqrt(x)
        case "exponential":
            return lambda x,a,b: a*(b**x)
    raise Exception("incorrect fit parameter")

def clean_legend(s, args):
    if args.clean_legend:
        s = s.split('_')[0]
    return s

def main():

    parser = argparse.ArgumentParser()
    parser.add_argument('name')
    parser.add_argument('property')
    parser.add_argument('--variable')
    parser.add_argument("--title")
    parser.add_argument("--x-limit")
    parser.add_argument("--y-limit")
    parser.add_argument("--output-dir")
    parser.add_argument("--fit")
    parser.add_argument("--compare")
    parser.add_argument("--compare-fit")
    parser.add_argument("--compare-own-x", action=argparse.BooleanOptionalAction)
    parser.add_argument("--save", action=argparse.BooleanOptionalAction)
    parser.add_argument("--line", action=argparse.BooleanOptionalAction)
    parser.add_argument("--hide-legend", action=argparse.BooleanOptionalAction)
    parser.add_argument("--clean-legend", action=argparse.BooleanOptionalAction)
    parser.add_argument("--report", action=argparse.BooleanOptionalAction)
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
            xs, ys = utils.bound(xs, ys, args.x_limit)
            plt.scatter(xs, ys, marker=".", label=clean_legend(optimizer, args))
            if args.line:
                plt.plot(xs, ys)

        if args.fit is not None:
            fit_func = get_func(args.fit)
            fit_params, _ = curve_fit(fit_func, xs, ys)
            f = lambda x: fit_func(x, *fit_params)
            y_pred = [f(x) for x in xs]
            score = r2_score(ys, y_pred)
            margin_x = sum(xs[i] - xs[i-1] for i in range(1, len(xs))) / (2*(len(xs)-1))
            end_x = max(xs) + margin_x
            dx = np.linspace(1, end_x, 100)
            plt.plot(dx, [f(x) for x in dx])
            print(args.name)
            for p in fit_params:
                print(f"{p}")
            print(f"R^2 score: {score}")
        
        if args.compare is not None:
            to_compare = args.compare.split(',')
            to_compare_fit = args.compare_fit.split(',')

            for name_compare, fit_compare in zip(to_compare, to_compare_fit):
                file_path_compare = utils.find_file(RESULTS_DIR, f"{name_compare}.txt")
                lines_compare = utils.get_file_lines(file_path_compare, args.property)
                for optimizer_compare, _, data in lines_compare:
                    xs_compare, ys_compare = utils.clean_line(data)
                    xs_compare, ys_compare = utils.bound(xs_compare, ys_compare, args.x_limit)
                    plt.scatter(xs_compare, ys_compare, marker=".", label=clean_legend(optimizer_compare,args))
                    if args.line:
                        plt.plot(xs_compare, ys_compare)

                fit_func = get_func(fit_compare)
                fit_params, _ = curve_fit(fit_func, xs_compare, ys_compare)
                f = lambda x: fit_func(x, *fit_params)
                y_pred = [f(x) for x in xs_compare]
                score = r2_score(ys_compare, y_pred)
                margin_x = sum(xs_compare[i] - xs_compare[i-1] for i in range(1, len(xs_compare))) / (2*(len(xs_compare)-1))
                if args.compare_own_x:
                    end_x = max(xs_compare) + margin_x
                else:
                    end_x = max(xs) + margin_x
                dx = np.linspace(1, end_x, 100)
                plt.plot(dx, [f(x) for x in dx])

        plt.xlabel(translate(parameter, args))
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
        utils.create_chart(vals, args.property, args.x_limit)
        parameter = args.variable

    plt.xlabel(translate(parameter, args))
    plt.ylabel(translate(args.property, args))  

    if args.hide_legend is None:
        plt.legend(bbox_to_anchor=(1, 1), loc='upper left')
        plt.tight_layout()

    if args.y_limit is not None:
        y_limit = float(args.y_limit)
        plt.gca().set_ylim(top=y_limit)

    if args.save:
        if args.output_dir is not None:
            output_dir = args.output_dir
        else:
            output_dir = CHARTS_DIR_DEFAULT
        file_path = os.path.join(output_dir, f"{args.name} {parameter} {args.property}.png")
        plt.savefig(file_path, dpi=300)
        plt.cla()
        plt.clf()
        print(f"Saved to file {file_path}")
    else:
        plt.show()

main()