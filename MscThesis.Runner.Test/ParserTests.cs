using MscThesis.Runner.Factories.Expression;
using System;
using Xunit;

namespace MscThesis.Runner.Test
{
    public class ParserTests
    {
        [Fact]
        public void Test()
        {
            var s = "log(0.1)*1/n-20.1";

            var factory = new ExpressionFactory();
            var expr = factory.BuildExpression(s);
            
            var n = 9;
            
            var actual = Math.Log2(0.1) * 1 / n - 20.1;
            Assert.Equal(actual, expr.Compute(n), 4);
        }
    }
}
