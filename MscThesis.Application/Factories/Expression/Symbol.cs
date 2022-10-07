
namespace MscThesis.Runner.Factories.Expression
{
    internal enum Symbol
    {
        // Terminals
        Constant,
        N,
        ParensOpen,
        ParensClose,
        Sqrt,
        Log,
        Plus,
        Minus,
        Asterisk,
        Slash,
        End,

        // Non-terminals
        TERM,
        TERM_E,
        MUL,
        MUL_E,
        EXPR,
        VAL
    }
}
