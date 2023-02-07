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
            double K) : base(problemSize)
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
            x.Fitness = fitnessFunction.ComputeFitness(x);
            
            var y = RandomUtils.SampleBitString(_random.Value, _probs);
            y.Fitness = fitnessFunction.ComputeFitness(y);

            if (x.Fitness < y.Fitness)
            {
                // swap
                (x, y) = (y, x);
                (x.Fitness, y.Fitness) = (y.Fitness, x.Fitness);
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

            var individuals = new List<BitString> { x, y };
            return new RunIteration
            {
                Population = individuals
            };
        }

    }
}
