using KSharp.Lexer;

namespace KSharp.Tests.Fakes
{
    public class FakeSourceReader : ISourceReader
    {
        public string Source;
        private int _currentIndex;

        public FakeSourceReader(string source)
        {
            Source = source;
        }

        public int GetChar()
        {
            if (_currentIndex >= Source.Length)
            {
                return -1;
            }

            var thisChar = Source[_currentIndex];
            _currentIndex++;

            return thisChar;
        }
    }
}
