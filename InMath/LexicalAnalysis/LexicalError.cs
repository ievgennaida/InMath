using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace InMath.LexicalAnalysis
{
    [DebuggerDisplay("Error {Value}, {Type}")]
    public class LexicalError : LexicalToken
    {
    }
}
