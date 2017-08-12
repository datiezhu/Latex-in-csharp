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
                case FormatterCommand.chapter: return new ChapterRules();
                default: throw new NotSupportedException();
            }
        }
    }
}