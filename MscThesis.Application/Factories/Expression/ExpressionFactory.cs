using System;
using System.Globalization;

namespace MscThesis.Runner.Factories.Expression
{
    public class ExpressionFactory : IExpressionFactory
    {
        public IExpression BuildExpression(string expression)
        {
            var scanner = new Scanner(expression);
            var parser = new Parser();
            var tree = parser.Parse(scanner);
            return BuildTerm(tree);
        }

        private IExpression BuildTerm(ParseTree node)
        {
            var mul = node.Children[0];
            var exprMul = BuildMul(mul);

            var term_e = node.Children[1];

            if (term_e.Children.Count == 0)
            {
                return exprMul;
            }
            else
            {
                var op = term_e.Children[0];
                var term = term_e.Children[1];
                var exprTerm = BuildTerm(term);

                if (op.Token.Symbol == Symbol.Plus)
                {
                    return new AdditionExpression(exprMul, exprTerm);
                }
                else
                {
                    return new SubtractionExpression(exprMul, exprTerm);
                }
            }
        }

        private IExpression BuildMul(ParseTree node)
        {
            var expr = node.Children[0];
            var exprExpr = BuildExpr(expr);

            var mul_e = node.Children[1];
            if (mul_e.Children.Count == 0)
            {
                return exprExpr;
            }
            else
            {
                var op = mul_e.Children[0];
                var mul = mul_e.Children[1];
                var exprMul = BuildMul(mul);

                if (op.Token.Symbol == Symbol.Asterisk)
                {
                    return new MultiplicationExpression(exprExpr, exprMul);
                }
                else
                {
                    return new DivisionExpression(exprExpr, exprMul);
                }
            }
        }

        private IExpression BuildExpr(ParseTree node)
        {
            var numChildren = node.Children.Count;

            if (numChildren == 3)
            {
                var term = node.Children[1];
                return BuildTerm(term);
            }
            if (numChildren == 4)
            {
                var op = node.Children[0];
                var term = node.Children[2];
                var termExpr = BuildTerm(term);
                if (op.Token.Symbol == Symbol.Log)
                {
                    return new LogExpression(termExpr);
                }
                else
                {
                    return new SqrtExpression(termExpr);
                }
            }
            else
            {
                var val = node.Children[0];
                var cn = val.Children[0];
                if (cn.Token.Symbol == Symbol.N)
                {
                    return new VariableExpression();
                }
                else
                {
                    var value = Convert.ToDouble(cn.Token.Value, CultureInfo.InvariantCulture);
                    return new ConstantExpression(value);
                }
            }
        }
    }
}
