﻿
namespace MscThesis.Runner.Factories.Expression
{
    internal class SubtractionExpression : IExpression
    {
        private IExpression _e1;
        private IExpression _e2;

        public SubtractionExpression(IExpression e1, IExpression e2)
        {
            _e1 = e1;
            _e2 = e2;
        }

        public double Compute(int n)
        {
            return _e1.Compute(n) - _e2.Compute(n);
        }
    }
}
