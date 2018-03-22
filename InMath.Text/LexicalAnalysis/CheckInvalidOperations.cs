using System;
using InMath.LexicalAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InMath.Text
{
    [TestClass]
    public class CheckInvalidOperations : LexicalTestsBase
    {
        [TestMethod]
        public void InvalidSymbolsAreMarked()
        {
            CheckTokens("=", new string[] { "=" }, new string[] { "=" });
            CheckTokens("$", new string[] { "$" }, new string[] { "$" });
            CheckTokens("$2", new string[] { "$", "2" }, new string[] { "$", "2" });
        }

        [TestMethod]
        public void CheckSpace()
        {
            CheckTokens("      2         ", new string[] { "         " }, new string[] { "      ", "2", "         " });
            CheckTokens("2 2", new string[] { " " }, new string[] { "2", " ", "2" });
        }

        [TestMethod]
        public void CannotBeStartedFromTheUnderscore()
        {
            CheckTokens("_a", new string[] { "_" }, new string[] { "_", "a" });
        }
    }
}
