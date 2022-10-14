using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Core.FitnessFunctions.TSP
{
    public class TSP : FitnessFunction<Permutation>
    {
        private double? _optimum;

        public TSP(string TSPfromTSPlib_10hif9s)
        {
            _optimum = 420;
            throw new NotImplementedException();
        }

        public TSP(double[,] distances)
        {
            _optimum = null;

            throw new NotImplementedException();
        }

        public override double? Optimum => _optimum;

        protected override double Compute(Permutation instance)
        {
            throw new NotImplementedException();
        }
    }
}
