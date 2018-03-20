using System;
using System.Collections.Generic;
using System.Text;

namespace InMath.LexicalAnalysis
{
    public class LexicalResults
    {

        public List<LexicalToken> Tokens { get; internal set; } = new List<LexicalToken>();
        public List<LexicalError> Errors { get; internal set; }

        public bool HasErrors
        {
            get
            {
                return Errors != null && Errors.Count > 0;
            }
        }
    }
}
