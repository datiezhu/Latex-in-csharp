namespace Moosetrail.LaTeX.Elements
{
    internal static class DocumentOrganisation
    {
        public static bool ElementIsMainContentDevider(LaTeXElement element)
        {
            return element is DocumentClass ||
                   element is Document ||
                   element is Chapter ||
                   element is Section ||
                   element is Subsection ||
                   element is Subsubsection;
        }
    }
}