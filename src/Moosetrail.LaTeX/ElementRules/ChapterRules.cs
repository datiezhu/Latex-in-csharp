using System.Collections.Generic;
using Moosetrail.LaTeX.Elements;

namespace Moosetrail.LaTeX.ElementRules
{
    internal class ChapterRules : Rules
    {
        public bool AllowOtherElementsInside => false;
        public bool AllowSelfInside => false;

        public IEnumerable<LaTeXElement> HigherRankingElements => new List<LaTeXElement>
        {
            new DocumentClass(),
            new Document()
        };

        public string ElementIsHigherRankingExceptionMessage
            => "A Chapter can't have a DocumentClass, Document or Chapter as a child";
    }
}