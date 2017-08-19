using System.Collections.Generic;

namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Represents a block of code that start with a \begin{} tag and end with an \end{} tag 
    /// </summary>
    public class Environment : LaTeXElement
    {
        /// <summary>
        /// Creates a new instance of the Environment class
        /// </summary>
        /// <param name="type">The type of the envelope code block</param>
        public Environment(EnvironmentType type)
        {
            Type = type;
            InnerElements = new List<LaTeXElement>();
        }

        /// <summary>
        /// Get the type of the section
        /// </summary>
        public EnvironmentType Type { get; private set; }

        /// <summary>
        /// Get/Set the elements of the section
        /// </summary>
        public List<LaTeXElement> InnerElements { get; set; }
    }
}