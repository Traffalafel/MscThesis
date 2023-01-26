
namespace MscThesis.Framework.Factories.Expression
{
    internal class VariableExpression : IExpression
    {
        public double Compute(int n)
        {
            return (double)n;
        }
    }
}
