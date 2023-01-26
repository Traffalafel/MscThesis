using System;
using System.Globalization;

namespace MscThesis.Framework.Factories.Expression
{
    public class ExpressionFactory : IExpressionFactory
    {
        public IExpression BuildExpression(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new Exception("Expression cannot be null or white-space");
            }

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

            if (numChildren != 2)
            {
                throw new Exception();
            }

            var fst = node.Children[0];
            var snd = node.Children[1];

            if (fst.Token.Symbol == Symbol.VAL)
            {
                var valExpr = BuildVal(fst);
                if (snd.Children.Count == 0)
                {
                    return valExpr;
                }
                else
                {
                    var power = BuildVal(snd.Children[1]);
                    return new PowerExpression(valExpr, power);
                }
            }
            else
            {
                var valExpr = BuildVal(snd);
                if (fst.Token.Symbol == Symbol.Log)
                {
                    return new LogExpression(valExpr);
                }
                else if (fst.Token.Symbol == Symbol.Sqrt)
                {
                    return new SqrtExpression(valExpr);
                }
                else if (fst.Token.Symbol == Symbol.Exp)
                {
                    return new ExpExpression(valExpr);
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        private IExpression BuildVal(ParseTree node)
        {
            var numChildren = node.Children.Count;

            if (numChildren == 3)
            {
                return BuildTerm(node.Children[1]);
            }
            else
            {
                var child = node.Children[0];
                if (child.Token.Symbol == Symbol.N)
                {
                    return new VariableExpression();
                }
                else
                {
                    var value = Convert.ToDouble(child.Token.Value, CultureInfo.InvariantCulture);
                    return new ConstantExpression(value);
                }
            }
        }

    }
}
