namespace KSharp.Lexer
{
    public static class Keywords
    {
        public const string DEF = "maq";
        public const string EXTERN = "nov";

        public static bool IsDef(this string identifier)
        {
            return identifier == DEF;
        }

        public static bool IsExtern(this string identifier)
        {
            return identifier == EXTERN;
        }
    }
}
