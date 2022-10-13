using System;

namespace MscThesis.Runner.Factories.Expression
{
    internal class SqrtExpression : IExpression
    {
        private IExpression _e;

        public SqrtExpression(IExpression e)
        {
            _e = e;
        }

        public double Compute(int n)
        {
            var val = _e.Compute(n);
            if (val < 0)
            {
                throw new Exception("Cannot take square root of negative number");
            }

            return Math.Sqrt(val);
        }
    }
}
