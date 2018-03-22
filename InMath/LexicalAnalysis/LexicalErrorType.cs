using System;
using System.Collections.Generic;
using System.Text;

namespace InMath.LexicalAnalysis
{
    public enum LexicalErrorType
    {
        UnexpectedCharacter = 100,
        CannotBeStartedFromUnderscope = 101,
        CannotBeEndedWithUnderscope = 102
       // ScientificNotationNotFinished
    }
}
