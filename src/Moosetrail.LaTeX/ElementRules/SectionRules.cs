using System.Collections.Generic;
using Moosetrail.LaTeX.Elements;

namespace Moosetrail.LaTeX.ElementRules
{
    public class SectionRules : Rules
    {
        public bool AllowOtherElementsInside => false;
        public bool AllowSelfInside => false;

        public IEnumerable<LaTeXElement> HigherRankingElements => new List<LaTeXElement>
        {
            new DocumentClass(),
            new Document(),
            new Formatter(FormatterCommand.chapter)
        };

        public string ElementIsHigherRankingExceptionMessage
            => "A Section can't have a DocumentClass, Document, Chapter or Section as a child";
    }
}