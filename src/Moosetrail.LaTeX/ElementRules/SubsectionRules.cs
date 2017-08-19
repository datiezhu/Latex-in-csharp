using System.Collections.Generic;
using Moosetrail.LaTeX.Elements;

namespace Moosetrail.LaTeX.ElementRules
{
    public class SubsectionRules : Rules
    {
        public bool AllowOtherElementsInside => false;
        public bool AllowSelfInside => false;

        public IEnumerable<LaTeXElement> HigherRankingElements => new List<LaTeXElement>
        {
            new DocumentClass(),
            new Document(),
            new Formatter(FormatterCommand.chapter),
            new Formatter(FormatterCommand.section)
        };

        public string ElementIsHigherRankingExceptionMessage
            => "A Subsection can't have a DocumentClass, Document, Chapter, Section or Subsection as a child";
    }
}