using System;

namespace KSharp.Lexer
{
    public class SourceReader : ISourceReader
    {
        public int GetChar()
        {
            return Console.Read();
        }
    }
}
