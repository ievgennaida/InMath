using InMath.LexicalAnalysis.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InMath.LexicalAnalysis
{
    /// <summary>
    /// Lexical analysis, lexing or tokenization is the process of converting a sequence of characters into a sequence of tokens.
    /// </summary>
    public partial class LexicalStackMachine
    {

        private short[][] states;
        private readonly short numberColumn;
        private readonly short charColumn;
        private readonly short leftBracketColumn;
        private readonly short rightBracketColumn;
        private readonly short multiplicationColumn;
        private readonly short divisionColumn;
        private readonly short minusColumn;
        private readonly short plusColumn;
        private readonly short dotColumn;
        private readonly short comaColumn;
        private readonly short underlineColumn;
        private readonly short rootColumn;
        private readonly short spaceColumn;
        private readonly short anyOtherSymbol;
        private readonly short squareBracketLeftColumn;
        private readonly short squareBracketRightColumn;
        private readonly short typesColumn;

        private int GetColumnIndex(char toCheck)
        {
            if (Char.IsDigit(toCheck) && ((int)toCheck) >= 48 && ((int)toCheck) <= 57)
            {
                // For now only default digits are supported.
                return numberColumn;
            }
            else if (toCheck == '(')
            {
                return leftBracketColumn;
            }
            else if (toCheck == ')')
            {
                return rightBracketColumn;
            }
            else if (toCheck == '*' || toCheck == '⋅' || toCheck == '×' || toCheck == '∙')
            {
                return multiplicationColumn;
            }
            else if (toCheck == '/' || toCheck == '÷')
            {
                return divisionColumn;
            }
            else if (toCheck == '-')
            {
                return minusColumn;
            }
            else if (toCheck == '+')
            {
                return plusColumn;
            }
            else if (toCheck == '.')
            {
                return dotColumn;
            }
            else if (toCheck == ',')
            {
                return comaColumn;
            }
            else if (toCheck == '_')
            {
                return underlineColumn;
            }
            else if (toCheck == '^')
            {
                return rootColumn;
            }
            else if (toCheck == ' ')
            {
                return spaceColumn;
            }
            else if (toCheck == '[')
            {
                return squareBracketLeftColumn;
            }
            else if (toCheck == ']')
            {
                return squareBracketRightColumn;
            }
            else if (Char.IsLetter(toCheck))
            {
                return charColumn;
            }
            else if (toCheck == '(')
            {
                return anyOtherSymbol;
            }

            return 0;
        }

        public LexicalStackMachine()
        {
            // States are harcoded as an array for a performance. This way is much faster than anything else.
            states = new short[17][];

            numberColumn = 0;
            charColumn = 1;
            leftBracketColumn = 2;
            rightBracketColumn = 3;
            multiplicationColumn = 4;
            divisionColumn = 5;
            minusColumn = 6;
            plusColumn = 7;
            dotColumn = 8;
            comaColumn = 9;
            underlineColumn = 10;
            rootColumn = 11;
            spaceColumn = 12;
            anyOtherSymbol = 13;
            squareBracketLeftColumn = 14;
            squareBracketRightColumn = 15;
            typesColumn = 14;
       
            // Columns              number, char, (	)	*	/	-	+	.	,	_	^	space	[	]	any other	generated types.
            //start
            states[0] = new short[] { 1, 35, 3, 4, 5, 7, 6, 8, 0, 12, 0, 10, 14, 0, 0, 0, 21 };
            // integer 
            states[1] = new short[] { 1, 0, 0, 0, 0, 0, 0, 0, 11, 0, 0, 0, 0, 0, 0, 0, 8 };
            // real
            states[2] = new short[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9 };
            // (
            states[3] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 7, };
            // )
            states[4] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 11 };
            // *
            states[5] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 12 };
            // -
            states[6] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 13 };

            // /
            states[7] = new short[] { 0, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 19 };

            // +
            states[8] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 14 };

            // argument
            states[9] = new short[] { 9, 9, 0, 0, 0, 0, 0, 0, 0, 0, 9, 0, 0, 0, 0, 0, 1 };

            // ^
            states[10] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 15 };

            // .
            states[11] = new short[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 23 };

            // ,
            states[12] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 17 };

            //  _
            states[13] = new short[] { 9, 9, 0, 0, 0, 0, 0, 0, 0, 0, 9, 0, 0, 0, 0, 0, 1 };

            // Space
            states[14] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 14, 0, 0, 0, 25 };

            // [
            states[13] = new short[] { 9, 9, 0, 0, 0, 0, 0, 0, 0, 0, 9, 0, 0, 0, 0, 0, 1 };

            // ]
            states[14] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 14, 0, 0, 0, 25 };
        }
    }
}
