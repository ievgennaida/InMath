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
            else
            {
                return anyOtherSymbol;
            }
        }

        public LexicalStackMachine()
        {
            // States are harcoded as an array for a performance. 
            // This way is much faster than anything else (read files and etc).
            states = new short[18][];

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
            typesColumn = 17;

            // Columns: number(0), char(1), (2	)3	*4	/5	-6	+7	.8	,9	_10	^11	space12	[13	]14	!15  any other16	generated types.17
            // start                  0  1  2  3  4  5  6  7  8  9   10   11   12  13 14 15 16 17 
            states[0] = new short[] { 1, 9, 3, 4, 5, 7, 6, 8, 0, 12, 0, 101, 14, 100, 100, 100, 0, 21 };
            // integer                0  1  2  3  4  5  6  7  8   9  10 11 12 13 14 15 16 17 
            states[1] = new short[] { 1, 0, 0, 0, 0, 0, 0, 0, 11, 0, 0, 0, 0, 0, 0, 0, 0, 8 };
            // real                   0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 17 
            states[2] = new short[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9 };
            // (                      0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 17 
            states[3] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 7, };
            // )                      0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 17 
            states[4] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 11 };
            // *                      0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 17 
            states[5] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 12 };
            // -                      0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 17 
            states[6] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 13 };

            // /                      0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 17 
            states[7] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 19 };

            // +                      0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 17 
            states[8] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 14 };

            // argument               0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 17 
            states[9] = new short[] { 9, 9, 0, 0, 0, 0, 0, 0, 0, 0, 9, 0, 0, 0, 0, 0, 0, 1 };

            // ^                       0  1  2  3  4  5  6  7  8  9  10 11 12  13 14 15 16 17 
            states[10] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 15 };

            // .                       0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 17 
            states[11] = new short[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 23 };

            // ,                       0  1  2  3  4  5  6  7  8  9  10 11 12  13 14 15 16 17 
            states[12] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 17 };

            //  _                      0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 17 
            states[13] = new short[] { 9, 9, 0, 0, 0, 0, 0, 0, 0, 0, 9, 0, 0, 0, 0, 0, 0, 1 };

            // Space                   0  1  2  3  4  5  6  7  8  9  10 11 12  13 14 15 16 17 
            states[14] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 14, 0, 0, 0, 0, 25 };

            // [
            states[15] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };

            // ]
            states[16] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25 };

            // !
            states[17] = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25 };

            ValidateTable();
        }

        private void ValidateTable()
        {
            // Check that table properly initialized.
            if (states == null)
            {
                throw new InvalidOperationException("States table cannot be null");
            }
            else
            {
                var index = 0;
                int? lastLenght = null;
                foreach (var state in states)
                {
                    if (state == null)
                    {
                        throw new InvalidOperationException("State table row " + index + "  cannot be null");
                    }
                    else if (!lastLenght.HasValue)
                    {
                        lastLenght = state.Length;
                    }
                    else if (state.Length != lastLenght)
                    {
                        throw new InvalidOperationException("State table row " + index + "  cannot be null");
                    }

                    index++;
                }
            }
        }
    }
}
