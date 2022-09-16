using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using MscThesis.Core.TerminationCriterion;
using System;

namespace MscThesis.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var problemSize = 50;
            var initialPopSize = 1000;
            var quartile = 0.5d;
            var maxIterations = 10;
            var epsilon = 10E-6;

            var selection = new QuartileSelectionOperator<BitString>(quartile);
            var termination = new StagnationTerminationCriterion<BitString>(epsilon, maxIterations);

            var mimic = new MIMIC(initialPopSize, selection, termination);

            var oneMax = new OneMax();
            var jumpOffsetSpike = new JumpOffsetSpike(8);
            var optimal = mimic.Optimize(jumpOffsetSpike, problemSize);

            Console.WriteLine(optimal.ToString());
        }
    }
}
