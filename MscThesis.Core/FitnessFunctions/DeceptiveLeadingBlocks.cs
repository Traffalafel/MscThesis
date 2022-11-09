using MscThesis.Core.Formats;
using System;

namespace MscThesis.Core.FitnessFunctions
{
    public class DeceptiveLeadingBlocks : FitnessFunction<BitString>
    {
        private readonly int _blockSize;
        private readonly int _numBlocks;

        public override FitnessComparison Comparison => FitnessComparison.Maximization;

        public DeceptiveLeadingBlocks(int size, int blockSize) : base(size) 
        {
            if (size % blockSize != 0)
            {
                throw new Exception("Block size must be a multiple of problem size");
            }

            _blockSize = blockSize;
            _numBlocks = size / blockSize;
        }

        public override double? Optimum(int problemSize)
        {
            return problemSize;
        }

        protected override double Compute(BitString instance)
        {
            var vals = instance.Values;
            var numLeadingBlocks = 0;

            for (int i = 0; i < _numBlocks; i++)
            {
                var offset = i * _blockSize;
                var allOnes = true;

                for (int j = 0; j < _blockSize; j++)
                {
                    if (!vals[offset + j])
                    {
                        allOnes = false;
                        break;
                    }
                }

                if (!allOnes)
                {
                    break;
                }

                numLeadingBlocks++;
            }

            if (numLeadingBlocks == _numBlocks)
            {
                return _size;
            }

            var offsetActive = numLeadingBlocks * _blockSize;
            var allZeros = true;
            for (int j = 0; j < _blockSize; j++)
            {
                if (vals[offsetActive + j])
                {
                    allZeros = false;
                    break;
                }
            }

            if (allZeros)
            {
                return numLeadingBlocks * _blockSize + 1;
            }
            else
            {
                return numLeadingBlocks * _blockSize;
            }
        }
    }
}
