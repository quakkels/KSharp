namespace KSharp.Lexer
{
    public static class Token
    {
        public static string Identifier;
        public static double NumberValue;

        public static int GetToken()
        {
            var lastChar = ' ';

            while (lastChar == ' ')
            {
                lastChar = GetChar();
            }
            return 0;
        }

        public static char GetChar()
        {
            return 's';
        }
    }
}
