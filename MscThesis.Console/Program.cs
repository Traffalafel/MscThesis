using MscThesis.Runner;
using System;
using System.Linq;

namespace MscThesis.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var runner = new TestRunner();

            var result = runner.TestMIMIC();

            Console.WriteLine($"Fittest: {result.GetFittest()}");

            foreach (var testCase in result.GetCases())
            {
                Console.WriteLine($"{testCase}:");

                foreach (var property in result.GetItemProperties(testCase))
                {
                    var value = result.GetItemValue(testCase, property);
                    Console.WriteLine($"{property}: {value}");
                }

                foreach (var property in result.GetSeriesProperties(testCase))
                {
                    var values = result.GetSeriesValues(testCase, property);
                    Console.WriteLine($"{property}: {string.Join(", ", values)}");
                }

                Console.Write("\n");
            }


        }
    }
}
