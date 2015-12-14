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
            _currentIndex = -1;
        }

        public int GetChar()
        {
            _currentIndex++;

            if (_currentIndex >= Source.Length)
            {
                return -1;
            }

            var thisChar = Source[_currentIndex];

            return thisChar;
        }
    }
}
