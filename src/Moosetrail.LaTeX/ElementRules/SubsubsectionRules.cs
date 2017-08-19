using System.Collections.Generic;
using Moosetrail.LaTeX.Elements;

namespace Moosetrail.LaTeX.ElementRules
{
    public class SubsubsectionRules : Rules
    {
        public bool AllowOtherElementsInside => false;
        public bool AllowSelfInside => false;

        public IEnumerable<LaTeXElement> HigherRankingElements => new List<LaTeXElement>
        {
            new DocumentClass(),
            new Document(),
            new Formatter(FormatterCommand.chapter),
            new Formatter(FormatterCommand.section),
            new Formatter(FormatterCommand.subsection)
        };

        public string ElementIsHigherRankingExceptionMessage
            => "A Subsubsection can't have a DocumentClass, Document, Chapter, Section, Subsection or Subsubsection as a child";
    }
}