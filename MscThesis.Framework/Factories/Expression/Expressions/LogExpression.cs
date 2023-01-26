using System;

namespace MscThesis.Framework.Factories.Expression
{
    internal class LogExpression : IExpression
    {
        private IExpression _e;

        public LogExpression(IExpression e)
        {
            _e = e;
        }

        public double Compute(int n)
        {
            return Math.Log2(_e.Compute(n));
        }
    }
}
