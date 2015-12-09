﻿using System;

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

        public int GetToken()
        {
            int lastChar = ' ';

            while (char.IsWhiteSpace((char)lastChar))
            {
                lastChar = _sourceReader.GetChar();
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
                    return (int)TokenType.Def;
                }

                if (_identifier == "extern")
                {
                    return (int)TokenType.Extern;
                }

                return (int)TokenType.Identifier;
            }

            if(char.IsDigit((char)lastChar) || (char)lastChar == '.')
            {
                var number = string.Empty;

                do
                {
                    number += (char)lastChar;
                    lastChar = _sourceReader.GetChar();
                }
                while (char.IsDigit((char)lastChar) || (char)lastChar == '.');

                return (int)TokenType.Number;
            }

            while (lastChar == '#')
            {
                do
                {
                    lastChar = _sourceReader.GetChar();
                } while (lastChar != (int)TokenType.Eof && lastChar != '\n' && lastChar != '\r');

                if (lastChar != (int)TokenType.Eof)
                {
                    return GetToken();
                }
            }

            if (lastChar == (int)TokenType.Eof)
            {
                return (int)TokenType.Eof;
            }

            var thisChar = lastChar;
            lastChar = _sourceReader.GetChar();
            return thisChar;
        }
    }
}
