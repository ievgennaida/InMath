using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace InMath.LexicalAnalysis
{
    [DebuggerDisplay("Tok {Value}, {Type}")]
    public class LexicalToken
    {
        public LexicalTokenType Type { get; set; }
        public string Value { get; set; }
        public LexicalPosition Position { get; set; }
    }
}
