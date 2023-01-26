using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System.Collections.Generic;

namespace MscThesis.Core.Algorithms.BitStrings
{
    public class CGA : Optimizer<BitString>
    {
        private readonly double _kInv;
        private double[] _probs;

        public CGA(
            int problemSize,
            FitnessComparison comparisonStrategy,
            double K) : base(problemSize, comparisonStrategy)
        {
            _kInv = 1.0d / K;
        }

        public override ISet<Property> StatisticsProperties => new HashSet<Property>();

        protected override void Initialize(FitnessFunction<BitString> fitnessFunction)
        {
            _probs = new double[_problemSize];
            for (int i = 0; i < _problemSize; i++)
            {
                _probs[i] = 0.5d;
            }
        }

        protected override RunIteration NextIteration(FitnessFunction<BitString> fitnessFunction)
        {
            var x = RandomUtils.SampleBitString(_random.Value, _probs);
            var y = RandomUtils.SampleBitString(_random.Value, _probs);

            var fitnessX = fitnessFunction.ComputeFitness(x);
            var fitnessY = fitnessFunction.ComputeFitness(y);

            if (fitnessX < fitnessY)
            {
                // swap
                (x, y) = (y, x);
                (fitnessX, fitnessY) = (fitnessY, fitnessX);
            }

            var marginMin = 1.0d / _problemSize;
            var marginMax = 1.0d - 1.0d / _problemSize;

            for (int i = 0; i < _problemSize; i++)
            {
                if (x.Values[i] && !y.Values[i])
                {
                    _probs[i] += _kInv;
                }
                if (!x.Values[i] && y.Values[i])
                {
                    _probs[i] -= _kInv;
                }

                if (_probs[i] < marginMin)
                {
                    _probs[i] = marginMin;
                }
                else if (_probs[i] > marginMax)
                {
                    _probs[i] = marginMax;
                }
            }

            var xInd = new IndividualImpl<BitString>(x, fitnessX);
            var yInd = new IndividualImpl<BitString>(y, fitnessY);
            var individuals = new List<Individual<BitString>> { xInd, yInd };

            var population = new Population<BitString>(individuals, _comparisonStrategy);
            return new RunIteration
            {
                Population = population
            };
        }

    }
}
