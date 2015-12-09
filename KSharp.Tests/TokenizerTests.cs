using KSharp.Lexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KSharp.Tests
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void CanTokenizeAnIdentifier()
        {
            // arrange
            var reader = new FakeSourceReader("asdf");
            var tokenizer = new Tokenizer(reader);

            // act
            var result = tokenizer.GetToken();

            // assert
            Assert.AreEqual((int)TokenType.Identifier, result);
        }

        [TestMethod]
        public void CanGetKeywordToken()
        {
            // arrange
            var reader = new FakeSourceReader("def");
            var tknzr = new Tokenizer(reader);

            // act
            var result = tknzr.GetToken();

            // assert
            Assert.AreEqual((int)TokenType.Def, result);
        }

        [TestMethod]
        public void CanGetNumberToken()
        {
            // arrange
            var r = new FakeSourceReader("asdf 1232 asdf");
            var t = new Tokenizer(r);

            // act
            var result1 = t.GetToken();
            var result2 = t.GetToken();
            var result3 = t.GetToken();

            // assert
            Assert.AreEqual((int)TokenType.Identifier, result1);
            Assert.AreEqual((int)TokenType.Number, result2);
            Assert.AreEqual((int)TokenType.Identifier, result3);
        }

        [TestMethod]
        public void CanGetTwoTokens()
        {
            // arrange
            var reader = new FakeSourceReader("asdf def");
            var t = new Tokenizer(reader);

            // act
            var result1 = t.GetToken();
            var result2 = t.GetToken();

            // assert
            Assert.AreEqual((int)TokenType.Identifier, result1);
            Assert.AreEqual((int)TokenType.Def, result2);
        }

        [TestMethod]
        public void CanGetEndOfFile()
        {
            // arrange
            var reader = new FakeSourceReader("asdf def");
            var t = new Tokenizer(reader);

            // act
            var result1 = t.GetToken();
            var result2 = t.GetToken();
            var result3 = t.GetToken();

            // assert
            Assert.AreEqual((int)TokenType.Identifier, result1);
            Assert.AreEqual((int)TokenType.Def, result2);
            Assert.AreEqual((int)TokenType.Eof, result3);
        }

        [TestMethod]
        public void CanIgnoreNewlinesAndComments()
        {
            // arrange
            var reader = new FakeSourceReader(
                "asdf     "
                + Environment.NewLine
                + Environment.NewLine
                + "#asdfsadf"
                + Environment.NewLine
                + "def"
                + Environment.NewLine);
            var t = new Tokenizer(reader);

            // act
            var result1 = t.GetToken();
            var result2 = t.GetToken();
            var result3 = t.GetToken();

            // assert
            Assert.AreEqual((int)TokenType.Identifier, result1);
            Assert.AreEqual((int)TokenType.Def, result2);
            Assert.AreEqual((int)TokenType.Eof, result3);
        }

        [TestMethod]
        public void CanHandleNonAlphaNumericToken()
        {
            // arrange
            var r = new FakeSourceReader("=");
            var t = new Tokenizer(r);

            // act
            var result = t.GetToken();

            // assert
            Assert.AreEqual((int)'=', result);
        }
    }

    public class FakeSourceReader : ISourceReader
    {
        private char[] _source;
        private int _currentIndex;
        private int _sourceLength;

        public FakeSourceReader(string source)
        {
            _source = source.ToCharArray();
            _sourceLength = source.Length;
        }

        public int GetChar()
        {
            if (_currentIndex >= _sourceLength)
            {
                return -1;
            }

            var thisChar = _source[_currentIndex];
            _currentIndex++;

            return thisChar;
        }
    }
}