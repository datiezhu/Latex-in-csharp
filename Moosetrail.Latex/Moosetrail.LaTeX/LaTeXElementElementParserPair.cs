namespace Moosetrail.LaTeX
{
    /// <summary>
    /// Pair of Latex Element and thats Element Parser
    /// </summary>
    internal class LaTeXElementElementParserPair
    {
      /// <summary>
      /// Creates a new pair
      /// </summary>
      /// <param name="element">The element of the pair</param>
      /// <param name="parser">The parser for the element</param>
        public LaTeXElementElementParserPair(LaTeXElement element, LaTeXElementParser parser)
        {
            Element = element;
            Parser = parser;
        }

        /// <summary>
        /// Get the element
        /// </summary>
        public LaTeXElement Element { get; private set; }

        /// <summary>
        /// Get the parser
        /// </summary>
        public LaTeXElementParser Parser { get; private set; }

    }
}