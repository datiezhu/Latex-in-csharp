using System;
using Moosetrail.LaTeX.Elements;

namespace Moosetrail.LaTeX.ElementRules
{
    internal static class ElementsRulesFactory
    {
        internal static Rules GetRules(FormatterCommand forCommand)
        {
            switch (forCommand)
            {
                case FormatterCommand.chapter:
                    return new ChapterRules();
                case FormatterCommand.section:
                    return new SectionRules();
                case FormatterCommand.subsection:
                    return new SubsectionRules();
                case FormatterCommand.subsubsection:
                    return new SubsubsectionRules();
                default:
                    throw new NotSupportedException();
            }
        }
    }
}