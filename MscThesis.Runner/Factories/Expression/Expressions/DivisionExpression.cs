
using System;

namespace MscThesis.Runner.Factories.Expression
{
    internal class DivisionExpression : IExpression
    {
        private IExpression _e1;
        private IExpression _e2;

        public DivisionExpression(IExpression e1, IExpression e2)
        {
            _e1 = e1;
            _e2 = e2;
        }

        public double Compute(int n)
        {
            var denominator = _e2.Compute(n);
            if (denominator == 0.0)
            {
                throw new Exception("Cannot divide by zero");
            }

            return _e1.Compute(n) / denominator;
        }
    }
}
