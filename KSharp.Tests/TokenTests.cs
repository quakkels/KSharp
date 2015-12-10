using KSharp.Lexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KSharp.Tests
{
    [TestClass]
    public class TokenTests
    {
        [TestMethod]
        public void CanValidateCharacter()
        {
            var t = new Token(TokenType.Character, 'a');

            var result = t.IsChar('a');

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReturnsFalseWhenCharacterDoesNotMatch()
        {
            var t = new Token(TokenType.Character, 'a');

            var result = t.IsChar('b');

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReturnsFalseWhenNotCharType()
        {
            var t = new Token(TokenType.Identifier, 'a');

            var result = t.IsChar('a');

            Assert.IsFalse(result);
        }
    }
}
