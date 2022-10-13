
using System;

namespace MscThesis.Runner.Factories.Expression
{
    internal class PowerExpression : IExpression
    {
        private IExpression _baseExpr;
        private IExpression _power;

        public PowerExpression(IExpression baseExpr, IExpression power)
        {
            _baseExpr = baseExpr;
            _power = power;
        }

        public double Compute(int n)
        {
            return Math.Pow(_baseExpr.Compute(n), _power.Compute(n));
        }
    }
}
