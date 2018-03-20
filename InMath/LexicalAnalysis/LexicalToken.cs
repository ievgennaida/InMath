using System;
using System.Collections.Generic;
using System.Text;

namespace InMath.LexicalAnalysis
{
    public class LexicalToken
    {
        public LexicalTokenType Type { get; set; }
        public string Value { get; set; }
        public LexicalPosition Position { get; set; }
    }
}
