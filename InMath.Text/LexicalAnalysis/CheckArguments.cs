using System;
using InMath.LexicalAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InMath.Text
{
    [TestClass]
    public class CheckArguments : LexicalTestsBase
    {

        [TestMethod]
        public void CheckTheAlphabet()
        {
            for (var c = 'A'; c <= 'z'; c++)
            {
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                {
                    if (Char.IsLetter(c))
                    {
                        CheckTokens(null, c.ToString(), "+", c.ToString() + c.ToString());
                        CheckTokens(null, "(", c.ToString(), "+", c.ToString() + c.ToString(), ")", "\\", "2", c.ToString());
                    }
                }
            }
        }

        [TestMethod]
        public void CheckArgumentsStart()
        {
            CheckTokens(null, "(", "2", ")");
            CheckTokens(null, "(", "2", "+", "2", ")");

            foreach (var operatorToCheck in operators)
            {
                CheckTokens(null, "2", operatorToCheck, "2");
                CheckTokens(null, "(", "2", operatorToCheck, "2", ")");
            }
        }

        [TestMethod]
        public void CheckSimpleArguments()
        {
            CheckTokens(null, "я");
            CheckTokens(null, "x", "*", "x");
            CheckTokens(null, "xx", "*", "x");
            CheckTokens(null, "xx", "*", "xy");
            CheckTokens(null, "xx", "*", "x", "+", "1");
            CheckTokens(null, "xx", "*", "xyx", "+", "13");

            CheckTokens(null, "(" , "x", "*", "x", ")");
            CheckTokens(null, "(", "xx", "*", "xx", ")");
            CheckTokens(null, "(", "xy", "*", "xx", ")");
        }
    }
}
