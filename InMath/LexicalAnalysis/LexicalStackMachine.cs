using InMath.LexicalAnalysis.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InMath.LexicalAnalysis
{
    /// <summary>
    /// Lexical analysis (tokenization) is the process of converting a sequence of characters into a sequence of tokens.
    /// </summary>
    public partial class LexicalStackMachine
    {
        public Task<LexicalResults> ParseAsync(string input)
        {
            var task = new Task<LexicalResults>(()=> { return Parse(input); });
            return task;
        }

        public bool TryGetState(int column, int row, out int result)
        {
            result = -1;
            if (states.Length > column && states[column].Length > row)
            {
                result = states[column][row];
                return true;
            }

            return false;
        }

        /// <summary>
        /// Parse math equasion and return the collections of tokens.
        /// </summary>
        /// <param name="inputString">The string to be parsed.</param>
        /// <returns>The results.</returns>
        public LexicalResults Parse(string inputString)
        {
            this.ValidateTable();
            var results = new LexicalResults();
            results.Input = inputString;

            if (string.IsNullOrEmpty(inputString))
            {
                return results;
            }

            int lexemStart = 0;
            var currentState = 0;
            //var errorFormat = "{2} '{0}' Позиція: {1}";
            for (int i = 0; i < inputString.Length; i++)
            {
                var input = inputString[i];
                var rowIndex = GetColumnIndex(input);

                // Error state is replaced.
                if (currentState >= 100)
                {
                    currentState = 0;
                }

                var lastState = currentState;
                if (!TryGetState(currentState, rowIndex, out currentState))
                {
                    throw new LexicalAnalysisException(string.Format("Виникла помилка на {0}x{1}", rowIndex, currentState), i);
                }
                else
                {
                    var isLastSymbol = i == inputString.Length - 1;
                    var isErrorState = currentState >= 100;
                    var isTerminalState = currentState == 0 || isErrorState;
                    // terminal state.
                    if (isTerminalState || isLastSymbol)
                    {
                        int result = 1;
                        if (isErrorState || TryGetState(lastState, typesColumn, out result))
                        {

                            isErrorState = result == 21 || result == 23;
                            //result = currentState;
                            var lenght = (!isTerminalState && isLastSymbol) ? inputString.Length - lexemStart : i - lexemStart;
                            var position= new LexicalPosition()
                            {
                                Row = lexemStart,
                                Column = i,
                                Lenght = lenght
                            };

                            if (lenght == 0)
                            {
                                lenght = 1;
                            }

                            var value = string.Empty;
                            if (inputString.Length >= lexemStart + lenght)
                            {
                                if (lexemStart == i && lenght == 1)
                                {
                                    value = input.ToString();
                                }
                                else
                                {
                                    value = inputString.Substring(lexemStart, lenght);
                                }
                            }

                            // errors have number more than 100
                            if (isErrorState)
                            {
                                results.HasErrors = true;
                                if (lenght == 0)
                                {
                                    lenght = 1;
                                }

                                results.Tokens.Add(new LexicalError()
                                {
                                    Type = (LexicalTokenType)result,
                                    Value = value,
                                    Position = position,
                                });

                                lexemStart = i + 1;

                            }
                            else
                            {
                                var token = new LexicalToken();
                                token.Type = (LexicalTokenType)result;
                                token.Value = value;
                                token.Position = position;
                                results.Tokens.Add(token);
                                lexemStart = i;
                            }

                            if (!isLastSymbol || isTerminalState)
                            {
                                if (!isErrorState)
                                {
                                    i--;
                                }

                                continue;
                            }
                        }
                        else
                        {
                            throw new LexicalAnalysisException("Таблиця має невірний тип.", i);
                        }
                    }
                }
            }

            return results;
        }
    }
}
