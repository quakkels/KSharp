namespace KSharp.Lexer
{
    public enum TokenType
    {
        Eof = -1,

        // commands
        Def,
        Extern,

        // primary
        Identifier,
        Number

    }
}
