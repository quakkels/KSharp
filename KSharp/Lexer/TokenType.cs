namespace KSharp.Lexer
{
    public enum TokenType
    {
        Eof = -1,

        // commands
        Def = -2,
        Extern = -3,

        // primary
        Identifier = -4,
        Number = -5
    }
}
