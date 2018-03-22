using System;
using InMath.LexicalAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InMath.Text
{
    public class LexicalTestsBase
    {
        protected readonly string[] operators = new string[]
        {
                    "+","-","/","*"
        };

        protected readonly string[] brakets = new string[]
       {
            "(",")","[","]"
       };

        protected readonly string numberSeparator = ".";

        protected void CheckTokens(params string[] tokens)
        {
            var input = string.Join(string.Empty, tokens);
            CheckTokens(input, null, tokens);
        }

        protected void CheckTokens(string input, string[] errors, string[] tokens)
        {
            var sfm = new LexicalStackMachine();
            var results = sfm.Parse(input);
            Assert.IsTrue(results != null, "State machine results cannot be null");
            Assert.IsTrue(results.Tokens != null, "Tokens collection cannot be null");

            if (tokens != null)
            {
                Assert.IsTrue(results.Tokens.Count == tokens.Length, "Invalid number of tokens is returned. Returned:" + results.Tokens.Count + ". Expected:" + tokens.Length);
                for (int i = 0; i < results.Tokens.Count; i++)
                {
                    var returned = results.Tokens[i].Value;
                    var expected = tokens[i];

                    Assert.IsTrue(returned == expected, "Unexpected token is returned:" + returned + ". Expected:" + expected + ". Index:" + i);
                }
            }

            // We expect some errors. So check them.
            if (errors != null)
            {
                Assert.IsTrue(results.HasErrors == true, "No errors are returned by sfm");
                var currentErrors= results.Errors;

                for (int i = 0; i < currentErrors.Count; i++)
                {
                    var returned = currentErrors[i].Value;
                    var expected = errors[i];

                    Assert.IsTrue(returned == expected, "Error is missing:" + returned + ". Expected:" + expected + ". Index:" + i);
                }
            }
        }
    }
}
