using System.Collections.Generic;

namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Represents all objects defined ass \command{Content goes here}
    /// </summary>
    public class Formatter : LaTeXElement
    {
        /// <summary>
        /// Creates a new instance of the Formatter class
        /// </summary>
        /// <param name="type">The type of the formatter code block</param>
        public Formatter(FormatterCommand type)
        {
            Type = type;
            InnerElements = new List<LaTeXElement>();
        }

        /// <summary>
        /// Get the type of the formatter
        /// </summary>
        public FormatterCommand Type { get; private set; }

        /// <summary>
        /// Get/Set the elements of the formatter
        /// </summary>
        public List<LaTeXElement> InnerElements { get; set; }
    }
}