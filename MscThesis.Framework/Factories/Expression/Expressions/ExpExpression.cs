using System;

namespace MscThesis.Framework.Factories.Expression
{
    internal class ExpExpression : IExpression
    {
        private IExpression _e;

        public ExpExpression(IExpression e)
        {
            _e = e;
        }

        public double Compute(int n)
        {
            var val = _e.Compute(n);
            return Math.Exp(val);
        }
    }
}
