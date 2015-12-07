using System;

namespace KSharp.Lexer
{
    public class Tokenizer : ITokenizer
    {
        private readonly ISourceReader _sourceReader;
        private string _identifier = string.Empty;

        public Tokenizer() 
            : this(new SourceReader())
        {
        }
    
        public Tokenizer(ISourceReader sourceReader)
        {
            _sourceReader = sourceReader;
        }

        public TokenType GetToken()
        {
            int lastChar = ' ';

            while (char.IsWhiteSpace((char)lastChar))
            {
                lastChar = _sourceReader.GetChar();
            }

            if (lastChar == (int)TokenType.Eof)
            {
                return TokenType.Eof;
            }

            if (char.IsLetter((char)lastChar))
            {
                _identifier = Convert.ToString((char)lastChar);
                lastChar = _sourceReader.GetChar();
                while (char.IsLetterOrDigit((char)lastChar))
                {
                    _identifier += (char)lastChar;
                    lastChar = _sourceReader.GetChar();
                    if (lastChar == -1)
                    {
                        break;
                    }
                }

                if (_identifier == "def")
                {
                    return TokenType.Def;
                }

                if (_identifier == "extern")
                {
                    return TokenType.Extern;
                }

                return TokenType.Identifier;
            }

            while (lastChar == '#')
            {
                do
                {
                    lastChar = _sourceReader.GetChar();
                } while (lastChar != null && lastChar != '\n' && lastChar != '\r');

                if (lastChar != -1)
                {
                    return GetToken();
                }
            }

            return TokenType.Eof;
        }
    }
}
