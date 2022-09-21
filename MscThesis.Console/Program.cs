using MscThesis.Application;
using System;
using System.Linq;

namespace MscThesis.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var runner = new TestRunner();

            var results = runner.TestMIMIC();

            Console.WriteLine($"Fittest: {results.Fittest}");
            Console.WriteLine($"Number of iterations: {results.NumIterations}");
            Console.WriteLine($"Best fitnesses: {string.Join(", ", results.BestFitnesses)}");
            Console.WriteLine($"Total function calls: {results.NumFunctionCalls}");
            Console.WriteLine($"Population sizes: {string.Join(", ", results.PopulationSizes)}");

            //var minEntropies = results.Statistics["MinEntropy"];
            //var minEntropiesFormatted = string.Join(", ", minEntropies);
            //Console.WriteLine($"Min entropies: {minEntropiesFormatted}");

        }
    }
}
