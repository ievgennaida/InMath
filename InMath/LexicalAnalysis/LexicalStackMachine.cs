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
            if (states != null && states.Length > column && states[column] != null && states[column].Length > row)
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
            var results = new LexicalResults();
            int lexemStart = 0;
            var currentState = 0;
            //var errorFormat = "{2} '{0}' Позиція: {1}";
            for (int i = 0; i < inputString.Length; i++)
            {
                var input = inputString[i];
                var rowIndex = GetColumnIndex(input);
                if (!TryGetState(currentState, rowIndex, out currentState))
                {
                    throw new LexicalAnalysisException(string.Format("Виникла помилка на {0}x{1}", rowIndex, currentState), i);
                }
                else
                {
                    var lastState = currentState;
                    var isLastSymbol = i == inputString.Length - 1;
                    var isTerminalState = currentState == 0;
                    // terminal state.
                    if (isTerminalState || isLastSymbol)
                    {
                        int result;
                        if (TryGetState(lastState, typesColumn, out result))
                        {
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
                                value = inputString.Substring(lexemStart, lenght);
                            }

                            // errors have number more than 100
                            if (result >= 100)
                            {
                                //    if (errors.ContainsKey((LexemType)result))
                                //    {
                                //        var type = (LexemType)result;
                                //        String name = String.Format(errorFormat,
                                //                                    inputString.Substring(lexemStart,lenght),
                                //                                    lexemStart + 0, errors[type], Environment.NewLine);
                                //        tmpErrors.Add(new Lexem(name, type, new LexemPosition(inputString.Length, i)));
                                //    }
                                //    else
                                //    {
                                //        String name = inputString.Substring(lexemStart, inputString.Length - lexemStart);
                                //        tokens.Add(new Lexem(name, (LexemType)result, new LexemPosition(inputString.Length, i)));
                                //    }


                                if (lenght == 0)
                                {
                                    lenght = 1;
                                }

                                results.Errors.Add(new LexicalError()
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
                                i--;
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

            //for (int i = 0; i < tokens.Count; i++)
            //{
            //    Lexem lexem = tokens[i];
            //    if (lexem.Type == LexemType.Space)
            //    {
            //        if (i < tokens.Count - 1)
            //        {
            //            if (LexemUtils.IsConstant(tokens[i + 1]))
            //            {
            //                tokens[i + 1].Type = LexemType.Error;
            //                tokens[i + 1].Name = "Неочікуваний символ ' '";
            //                i++;
            //            }
            //        }
            //        tokens.Remove(lexem);
            //    }
            //}
            return results;
        }
    }
}
