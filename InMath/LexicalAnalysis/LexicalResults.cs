using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace InMath.LexicalAnalysis
{
    public class LexicalResults
    {
        public string Input { get; internal set; }
        public List<LexicalToken> Tokens { get; internal set; } = new List<LexicalToken>();

        public List<LexicalError> Errors
        {
            get
            {
                return Tokens.Where(p => p is LexicalError).Cast<LexicalError>().ToList();
            }
        }

        public bool HasErrors { get; internal set; }
    }
}
