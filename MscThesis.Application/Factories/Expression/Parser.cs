using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Runner.Factories.Expression
{
    internal class Parser
    {
        private Symbol[][] _rules;
        private int[,] _parsingTable;

        private static HashSet<Symbol> _terminals = new HashSet<Symbol>
        {
            Symbol.N, Symbol.Constant, Symbol.End, Symbol.ParensOpen, Symbol.ParensClose,
            Symbol.Sqrt, Symbol.Log, Symbol.Plus, Symbol.Minus, Symbol.Asterisk, Symbol.Slash
        };

        public Parser()
        {
            _rules = new Symbol[][]
            {
                // TERM
                /*  1 */ new Symbol[] { Symbol.MUL, Symbol.TERM_E },
                
                // TERM_E
                /*  2 */ new Symbol[] { Symbol.Plus, Symbol.TERM },
                /*  3 */ new Symbol[] { Symbol.Minus, Symbol.TERM },
                /*  4 */ new Symbol[] { },
                
                // MUL
                /*  5 */ new Symbol[] { Symbol.EXPR, Symbol.MUL_E },

                // MUL_E
                /*  6 */ new Symbol[] { Symbol.Asterisk, Symbol.MUL },
                /*  7 */ new Symbol[] { Symbol.Slash, Symbol.MUL },
                /*  8 */ new Symbol[] { },

                // EXPR
                /*  9 */ new Symbol[] { Symbol.VAL, Symbol.POW },
                /* 10 */ new Symbol[] { Symbol.Log, Symbol.VAL },
                /* 11 */ new Symbol[] { Symbol.Sqrt, Symbol.VAL },

                // POW
                /* 12 */ new Symbol[] { Symbol.Power, Symbol.VAL },
                /* 13 */ new Symbol[] { },

                // VAL
                /* 14 */ new Symbol[] { Symbol.ParensOpen, Symbol.TERM, Symbol.ParensClose },
                /* 15 */ new Symbol[] { Symbol.Constant },
                /* 16 */ new Symbol[] { Symbol.N},
            };

            var numSymbols = Enum.GetNames(typeof(Symbol)).Length;
            _parsingTable = new int[numSymbols, numSymbols];

            // Row 1
            _parsingTable[(int)Symbol.TERM, (int)Symbol.N] = 1;
            _parsingTable[(int)Symbol.TERM, (int)Symbol.Constant] = 1;
            _parsingTable[(int)Symbol.TERM, (int)Symbol.ParensOpen] = 1;
            _parsingTable[(int)Symbol.TERM, (int)Symbol.Log] = 1;
            _parsingTable[(int)Symbol.TERM, (int)Symbol.Sqrt] = 1;

            // Row 2
            _parsingTable[(int)Symbol.TERM_E, (int)Symbol.Plus] = 2;
            _parsingTable[(int)Symbol.TERM_E, (int)Symbol.Minus] = 3;
            _parsingTable[(int)Symbol.TERM_E, (int)Symbol.End] = 4;
            _parsingTable[(int)Symbol.TERM_E, (int)Symbol.ParensClose] = 4;

            // Row 3
            _parsingTable[(int)Symbol.MUL, (int)Symbol.N] = 5;
            _parsingTable[(int)Symbol.MUL, (int)Symbol.Constant] = 5;
            _parsingTable[(int)Symbol.MUL, (int)Symbol.ParensOpen] = 5;
            _parsingTable[(int)Symbol.MUL, (int)Symbol.Log] = 5;
            _parsingTable[(int)Symbol.MUL, (int)Symbol.Sqrt] = 5;

            // Row 4
            _parsingTable[(int)Symbol.MUL_E, (int)Symbol.Asterisk] = 6;
            _parsingTable[(int)Symbol.MUL_E, (int)Symbol.Slash] = 7;
            _parsingTable[(int)Symbol.MUL_E, (int)Symbol.End] = 8;
            _parsingTable[(int)Symbol.MUL_E, (int)Symbol.Plus] = 8;
            _parsingTable[(int)Symbol.MUL_E, (int)Symbol.Minus] = 8;
            _parsingTable[(int)Symbol.MUL_E, (int)Symbol.ParensClose] = 8;

            // Row 5
            _parsingTable[(int)Symbol.EXPR, (int)Symbol.N] = 9;
            _parsingTable[(int)Symbol.EXPR, (int)Symbol.Constant] = 9;
            _parsingTable[(int)Symbol.EXPR, (int)Symbol.ParensOpen] = 9;
            _parsingTable[(int)Symbol.EXPR, (int)Symbol.Log] = 10;
            _parsingTable[(int)Symbol.EXPR, (int)Symbol.Sqrt] = 11;

            // Row 6
            _parsingTable[(int)Symbol.POW, (int)Symbol.Power] = 12;
            _parsingTable[(int)Symbol.POW, (int)Symbol.End] = 13;
            _parsingTable[(int)Symbol.POW, (int)Symbol.ParensClose] = 13;
            _parsingTable[(int)Symbol.POW, (int)Symbol.Slash] = 13;
            _parsingTable[(int)Symbol.POW, (int)Symbol.Asterisk] = 13;
            _parsingTable[(int)Symbol.POW, (int)Symbol.Plus] = 13;
            _parsingTable[(int)Symbol.POW, (int)Symbol.Minus] = 13;

            // Row 7
            _parsingTable[(int)Symbol.VAL, (int)Symbol.ParensOpen] = 14;
            _parsingTable[(int)Symbol.VAL, (int)Symbol.Constant] = 15;
            _parsingTable[(int)Symbol.VAL, (int)Symbol.N] = 16;
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
                var foundSymbol = scanner.Current.Symbol;
                var node = stack.Pop();

                var symbolStack = node.Token.Symbol;
                if (foundSymbol == symbolStack)
                {
                    if (foundSymbol == Symbol.End)
                    {
                        break;
                    }

                    node.Token.Value = scanner.Current.Value;
                    scanner.MoveNext();
                }
                else
                {
                    if (IsTerminal(foundSymbol) && IsTerminal(symbolStack))
                    {
                        throw new Exception();
                    }
                    if (foundSymbol == Symbol.End)
                    {
                        continue;
                    }

                    // Push rule to stack
                    var ruleIdx = _parsingTable[(int)node.Token.Symbol, (int)foundSymbol];

                    if (ruleIdx == 0)
                    {
                        throw new Exception();
                    }

                    var rule = _rules[ruleIdx-1];
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

        private bool IsTerminal(Symbol s)
        {
            return _terminals.Contains(s);
        }

    }

}
