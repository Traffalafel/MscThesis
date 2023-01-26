using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Framework.Factories.Expression
{
    internal class ParseTree
    {
        public Token Token { get; set; }

        public ParseTree Parent { get; set; }
        public List<ParseTree> Children { get; set; } = new List<ParseTree>();

        public override string ToString()
        {
            switch (Token.Symbol)
            {
                case Symbol.Plus:
                    return "+";
                case Symbol.Minus:
                    return "-";
                case Symbol.Asterisk:
                    return "*";
                case Symbol.Slash:
                    return "/";
                case Symbol.Sqrt:
                    return "sqrt";
                case Symbol.Log:
                    return "log";
                case Symbol.ParensOpen:
                    return "(";
                case Symbol.ParensClose:
                    return ")";
                case Symbol.N:
                    return "n";
                case Symbol.Constant:
                    return Token.Value;
                default:
                    return string.Concat(Children.Select(c => c.ToString()));
            }

        }
    }
}
