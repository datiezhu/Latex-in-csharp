using System.Collections.Generic;

namespace Moosetrail.LaTeX.ElementRules
{
    internal interface Rules
    {
        bool AllowOtherElementsInside { get; }

        bool AllowSelfInside { get;  }

        IEnumerable<LaTeXElement> HigherRankingElements { get; }

        string ElementIsHigherRankingExceptionMessage { get; }
    }
}