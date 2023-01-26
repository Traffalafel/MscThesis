
using System;
using System.Collections.Generic;

namespace MscThesis.Framework.Factories.Expression
{
    internal class Scanner
    {
        private Token _current;
        private string _content;
        private int _currentIdx;

        public Scanner(string content)
        {
            _content = content.Replace(" ", "");
            _currentIdx = 0;
            _current = null;

            MoveNext();
        }

        public Token Current => _current;

        public void MoveNext()
        {
            if (_currentIdx >= _content.Length)
            {
                _current = new Token
                {
                    Symbol = Symbol.End
                };
                return;
            }

            var c = _content[_currentIdx];

            switch (c)
            {
                case '+':
                    _current = new Token
                    {
                        Symbol = Symbol.Plus
                    };
                    _currentIdx += 1;
                    return;
                case '-':
                    _current = new Token
                    {
                        Symbol = Symbol.Minus
                    };
                    _currentIdx += 1;
                    return;
                case '*':
                    _current = new Token
                    {
                        Symbol = Symbol.Asterisk
                    };
                    _currentIdx += 1;
                    return;
                case '/':
                    _current = new Token
                    {
                        Symbol = Symbol.Slash
                    };
                    _currentIdx += 1;
                    return;
                case '^':
                    _current = new Token
                    {
                        Symbol = Symbol.Power
                    };
                    _currentIdx += 1;
                    return;
                case '(':
                    _current = new Token
                    {
                        Symbol = Symbol.ParensOpen
                    };
                    _currentIdx += 1;
                    return;
                case ')':
                    _current = new Token
                    {
                        Symbol = Symbol.ParensClose
                    };
                    _currentIdx += 1;
                    return;
                case 'l':
                    if (_content.Length - _currentIdx < 3 || _content.Substring(_currentIdx, 3) != "log")
                    {
                        throw new Exception();
                    }
                    _current = new Token
                    {
                        Symbol = Symbol.Log
                    };
                    _currentIdx += 3;
                    return;
                case 's':
                    if (_content.Length - _currentIdx < 4 ||  _content.Substring(_currentIdx, 4) != "sqrt")
                    {
                        throw new Exception();
                    } 
                    _current = new Token
                    {
                        Symbol = Symbol.Sqrt
                    };
                    _currentIdx += 4;
                    return;
                case 'e':
                    if (_content.Length - _currentIdx < 3 || _content.Substring(_currentIdx, 3) != "exp")
                    {
                        throw new Exception();
                    }
                    _current = new Token
                    {
                        Symbol = Symbol.Exp
                    };
                    _currentIdx += 3;
                    return;
                case 'n':
                    _current = new Token
                    {
                        Symbol = Symbol.N
                    };
                    _currentIdx += 1;
                    return;
                default:
                    var s = "";
                    if (!IsAllowedConstantChar(c))
                    {
                        throw new Exception();
                    }
                    do
                    {
                        if (c != 'E')
                        {
                            s += c;
                            _currentIdx += 1;

                            if (_currentIdx >= _content.Length)
                            {
                                break;
                            }
                            c = _content[_currentIdx];
                        }
                        else
                        {
                            s += "E-";
                            _currentIdx += 2;

                            if (_currentIdx >= _content.Length)
                            {
                                break;
                            }
                            c = _content[_currentIdx];
                        }
                    }
                    while (IsAllowedConstantChar(c));

                    _current = new Token
                    {
                        Value = s,
                        Symbol = Symbol.Constant
                    };
                    return;
            }
        }

        private bool IsAllowedConstantChar(char c)
        {
            return char.IsDigit(c) || c == '.' || c == 'E';
        }

    }
}
