
namespace MscThesis.Framework.Factories.Expression
{
    internal class ConstantExpression : IExpression
    {
        private double _value;

        public ConstantExpression(double value)
        {
            _value = value;
        }   

        public double Compute(int n)
        {
            return _value;
        }
    }
}
