using MscThesis.Runner.Factories.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MscThesis.Runner.Test
{
    public class ParserTests
    {
        public static Dictionary<string, Func<int, double>> TestCases = new Dictionary<string, Func<int, double>>
        {
            { "10", (n) => 10 },
            { "n", (n) => n },
            { "n-100", (n) => n-100 },
            { "2*4*n", (n) => 2*4*n },
            { "sqrt(n) + 2", (n) => Math.Sqrt(n) + 2 },
            { "(n*(n+1))/2", (n) => (n*(n+1))/2 },
            { "n^2", (n) => Math.Pow(n,2) },
            { "log(0.1)*1/n-20.1", (n) => Math.Log2(0.1) * 1 / n - 20.1 }
        };

        public static IEnumerable<object[]> Keys
        {
            get => TestCases.Select(kv => new string[1] { kv.Key });
        }

        public static int NumValues = 100;
        public static IEnumerable<int> Values = Enumerable.Range(1, NumValues);

        [Theory]
        [MemberData(nameof(Keys))]
        public void TestExpression(string expressionStr)
        {
            var factory = new ExpressionFactory();
            var expression = factory.BuildExpression(expressionStr);

            var target = TestCases[expressionStr];
            
            foreach (var n in Values)
            {
                var expected = target.Invoke(n);
                var actual = expression.Compute(n);
                Assert.Equal(expected, actual, 4);
            }
        }
    }
}
