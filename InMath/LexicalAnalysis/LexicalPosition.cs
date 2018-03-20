using System;
using System.Collections.Generic;
using System.Text;

namespace InMath.LexicalAnalysis
{
    /// <summary>
    /// The position of token in the text.
    /// </summary>
    public class LexicalPosition
    {
        public int Lenght { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
