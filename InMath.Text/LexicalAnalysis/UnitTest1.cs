using System;
using InMath.LexicalAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InMath.Text
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var sfm = new LexicalStackMachine();
            var results = sfm.Parse(" 2+2  ");
            Assert.IsTrue(results != null, "State machine results cannot be null");
            Assert.IsTrue(results.Tokens.Count == 3);
            Assert.IsTrue(results.Tokens[0].Value == "2");
            Assert.IsTrue(results.Tokens[1].Value == "+");
            Assert.IsTrue(results.Tokens[2].Value == "2");

            results = sfm.Parse("22+22-33");
            Assert.IsTrue(results != null, "State machine results cannot be null");
            Assert.IsTrue(results.Tokens.Count == 5);
            Assert.IsTrue(results.Tokens[0].Value == "22");
            Assert.IsTrue(results.Tokens[1].Value == "+");
            Assert.IsTrue(results.Tokens[2].Value == "22");

            results = sfm.Parse("22+22-33");
            Assert.IsTrue(results != null, "State machine results cannot be null");
            Assert.IsTrue(results.Tokens.Count == 5);
            Assert.IsTrue(results.Tokens[0].Value == "22");
            Assert.IsTrue(results.Tokens[1].Value == "+");
            Assert.IsTrue(results.Tokens[2].Value == "22");
            //14.2e20
            //123.2e-40
            //1.2e+50
            //2.3E23
            //4.5E-50
            //3.2E+40


            // Cannot start or end with _
            // 1_2
            // _a+2
            // b_+3
            // a_b+4
            // aa_bb+4
            // 4+aa_bb+4
        }
    }
}
