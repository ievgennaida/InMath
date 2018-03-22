using System;
using InMath.LexicalAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InMath.Text
{
    [TestClass]
    public class NumbersTests : LexicalTestsBase
    {
        [TestMethod]
        public void CheckStartOfAllOperations()
        {
            CheckTokens(null, "(", "2", ")");
            CheckTokens(null, "(", "2", "+", "2", ")");

            foreach (var operatorToCheck in operators)
            {
                CheckTokens(null, "2", operatorToCheck, "2");
                CheckTokens(null, "(", "2", operatorToCheck, "2", ")");
            }

            //)
            // *
            // /
            // -
            //+
            // .
            //,
            //_



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

        [TestMethod]
        public void CheckSimpleNumbers()
        {
            CheckTokens(null, "2");
            CheckTokens(null, "22");
            CheckTokens(null, "223");

            CheckTokens(null, "2.2");
            CheckTokens(null, "22.34");
            CheckTokens(null, "223.445");

            CheckTokens(null, "(", "2.2", ")");
            CheckTokens(null, "(", "22.34", ")");
            CheckTokens(null, "(", "223.445", ")");
        }
    }
}
