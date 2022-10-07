using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Runner.Factories.Expression
{
    internal class Parser
    {
        private Symbol[][] _rules;
        private int[,] _parsingTable;

        public Parser()
        {
            _rules = new Symbol[][]
            {
                /*  0 */ new Symbol[] { Symbol.MUL, Symbol.TERM_E },
                /*  1 */ new Symbol[] { Symbol.Plus, Symbol.TERM },
                /*  2 */ new Symbol[] { Symbol.Minus, Symbol.TERM },
                /*  3 */ new Symbol[] { },
                /*  4 */ new Symbol[] { Symbol.EXPR, Symbol.MUL_E },
                /*  5 */ new Symbol[] { Symbol.Asterisk, Symbol.MUL },
                /*  6 */ new Symbol[] { Symbol.Slash, Symbol.MUL },
                /*  7 */ new Symbol[] { },
                /*  8 */ new Symbol[] { Symbol.ParensOpen, Symbol.TERM, Symbol.ParensClose },
                /*  9 */ new Symbol[] { Symbol.Log, Symbol.ParensOpen, Symbol.TERM, Symbol.ParensClose },
                /* 10 */ new Symbol[] { Symbol.Sqrt, Symbol.ParensOpen, Symbol.TERM, Symbol.ParensClose },
                /* 11 */ new Symbol[] { Symbol.VAL },
                /* 12 */ new Symbol[] { Symbol.Constant },
                /* 13 */ new Symbol[] { Symbol.N},
            };

            var numSymbols = Enum.GetNames(typeof(Symbol)).Length;
            _parsingTable = new int[numSymbols, numSymbols];

            // Row 1
            _parsingTable[(int)Symbol.TERM, (int)Symbol.N] = 0;
            _parsingTable[(int)Symbol.TERM, (int)Symbol.Constant] = 0;
            _parsingTable[(int)Symbol.TERM, (int)Symbol.ParensOpen] = 0;
            _parsingTable[(int)Symbol.TERM, (int)Symbol.Log] = 0;
            _parsingTable[(int)Symbol.TERM, (int)Symbol.Sqrt] = 0;

            // Row 2
            _parsingTable[(int)Symbol.TERM_E, (int)Symbol.Plus] = 1;
            _parsingTable[(int)Symbol.TERM_E, (int)Symbol.Minus] = 2;
            _parsingTable[(int)Symbol.TERM_E, (int)Symbol.End] = 3;
            _parsingTable[(int)Symbol.TERM_E, (int)Symbol.ParensClose] = 3;

            // Row 3
            _parsingTable[(int)Symbol.MUL, (int)Symbol.N] = 4;
            _parsingTable[(int)Symbol.MUL, (int)Symbol.Constant] = 4;
            _parsingTable[(int)Symbol.MUL, (int)Symbol.ParensOpen] = 4;
            _parsingTable[(int)Symbol.MUL, (int)Symbol.Log] = 4;
            _parsingTable[(int)Symbol.MUL, (int)Symbol.Sqrt] = 4;

            // Row 4
            _parsingTable[(int)Symbol.MUL_E, (int)Symbol.Asterisk] = 5;
            _parsingTable[(int)Symbol.MUL_E, (int)Symbol.Slash] = 6;
            _parsingTable[(int)Symbol.MUL_E, (int)Symbol.End] = 7;
            _parsingTable[(int)Symbol.MUL_E, (int)Symbol.Plus] = 7;
            _parsingTable[(int)Symbol.MUL_E, (int)Symbol.Minus] = 7;
            _parsingTable[(int)Symbol.MUL_E, (int)Symbol.ParensClose] = 7;

            // Row 5
            _parsingTable[(int)Symbol.EXPR, (int)Symbol.ParensOpen] = 8;
            _parsingTable[(int)Symbol.EXPR, (int)Symbol.Log] = 9;
            _parsingTable[(int)Symbol.EXPR, (int)Symbol.Sqrt] = 10;
            _parsingTable[(int)Symbol.EXPR, (int)Symbol.N] = 11;
            _parsingTable[(int)Symbol.EXPR, (int)Symbol.Constant] = 11;

            // Row 6
            _parsingTable[(int)Symbol.VAL, (int)Symbol.Constant] = 12;
            _parsingTable[(int)Symbol.VAL, (int)Symbol.N] = 13;
        }

        public ParseTree Parse(Scanner scanner)
        {
            var root = new ParseTree
            {
                Parent = null,
                Token = new Token
                {
                    Symbol = Symbol.TERM
                }
            };

            var stack = new Stack<ParseTree>();

            stack.Push(new ParseTree
            {
                Parent = null,
                Token = new Token {
                    Symbol = Symbol.End 
                }
            });
            stack.Push(root);

            while (true)
            {
                var symbol = scanner.Current.Symbol;
                var node = stack.Pop();

                if (symbol == node.Token.Symbol)
                {
                    if (symbol == Symbol.End)
                    {
                        break;
                    }

                    node.Token.Value = scanner.Current.Value;
                    scanner.MoveNext();
                }
                else
                {
                    // Push rule to stack
                    var ruleIdx = _parsingTable[(int)node.Token.Symbol, (int)symbol];
                    var rule = _rules[ruleIdx];
                    foreach (var symb in rule.Reverse())
                    {
                        var newNode = new ParseTree
                        {
                            Parent = node,
                            Token = new Token
                            {
                                Symbol = symb
                            }
                        };
                        node.Children.Add(newNode);
                        stack.Push(newNode);
                    }
                    node.Children.Reverse();
                }
            }

            return root;
        }
    }

}
