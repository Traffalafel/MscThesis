using MscThesis.Core;
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
            var mimic = new MIMIC(initialPopSize, quartile);

            var oneMax = new OneMax();
            var optimal = mimic.Optimize(oneMax, problemSize);

            Console.WriteLine(optimal.ToString());
        }
    }
}
