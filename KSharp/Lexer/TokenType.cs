namespace KSharp.Lexer
{
    public enum TokenType
    {
        Eof = -1,

        // commands
        Def = -2,
        Extern = -3,

        Identifier = -4,
        Number = -5,
        Character = -6
    }
}
