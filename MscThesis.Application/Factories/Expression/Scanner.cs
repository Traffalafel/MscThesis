
namespace MscThesis.Runner.Factories.Expression
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
                    _current = new Token
                    {
                        Symbol = Symbol.Log
                    };
                    _currentIdx += 3;
                    return;
                case 's':
                    _current = new Token
                    {
                        Symbol = Symbol.Sqrt
                    };
                    _currentIdx += 4;
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
                    while (char.IsDigit(c) || c == '.' || c == 'E')
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
                    _current = new Token
                    {
                        Value = s,
                        Symbol = Symbol.Constant
                    };
                    return;
            }
        }
    }
}
