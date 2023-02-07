
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
        Exp,
        Plus,
        Minus,
        Asterisk,
        Slash,
        End,
        Power,

        // Non-terminals
        TERM,
        TERM_E,
        MUL,
        MUL_E,
        EXPR,
        POW,
        VAL
    }
}
