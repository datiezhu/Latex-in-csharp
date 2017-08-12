using System.Collections.Generic;

namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Represents all objects that starts with code like \item Content goes here
    /// </summary>
    public class Inline : LaTeXElement
    {
        /// <summary>
        /// Creates a new instance of the Inline class
        /// </summary>
        /// <param name="type">The type of the inline code block</param>
        public Inline(InlineCommand type)
        {
            Type = type;
            InnerElements = new List<LaTeXElement>();
        }

        /// <summary>
        /// Get the type of the inline
        /// </summary>
        public InlineCommand Type { get; private set; }

        /// <summary>
        /// Get/Set the elements of the inline
        /// </summary>
        public List<LaTeXElement> InnerElements { get; set; }
    }
}