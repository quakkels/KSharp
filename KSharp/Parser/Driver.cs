using System;

namespace KSharp.Parser
{
    public class Driver
    {
        private Parsr _parser;

        public Driver(Parsr parser)
        {
            _parser = parser;
        }

        public void MainLoop()
        {
            while (true)
            {
                Console.Write("ready> ");
                _parser.GetNextToken();

                if (_parser.CurrentToken.Type == Lexer.TokenType.Eof)
                {
                    return;
                }

                if (_parser.CurrentToken.IsChar(';'))
                {
                    _parser.GetNextToken();
                }
                else if (_parser.CurrentToken.Type == Lexer.TokenType.Def)
                {
                    HandleDefinition();
                }
                else if (_parser.CurrentToken.Type == Lexer.TokenType.Extern)
                {
                    HandleExtern();
                }
                else
                {
                    HandleTopLevelExpression();
                }
            }
        }


        public void HandleDefinition()
        {
            try
            {
                _parser.ParseDefinition();
                Console.WriteLine("Parsed a function definition.");
            }
            catch (Exception)
            {
                // Skip token for error recovery.
                _parser.GetNextToken();
            }
        }

        public void HandleExtern()
        {
            try
            {
                _parser.ParseExtern();
                Console.WriteLine("Parsed an extern");
            }
            catch (Exception)
            {
                // Skip token for error recovery.
                _parser.GetNextToken();
            }
        }

        public void HandleTopLevelExpression()
        {
            // Evaluate a top-level expression into an anonymous function.
            try
            {
                _parser.ParseTopLevelExpression();

                Console.WriteLine("Parsed a top-level expression");
            }
            catch (Exception)
            {
                // Skip token for error recovery.
                _parser.GetNextToken();
            }
        }
    }
}
