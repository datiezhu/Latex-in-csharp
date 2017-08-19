using System.Collections.Generic;

namespace Moosetrail.LaTeX.Elements
{
    /// <summary>
    /// Represents all objects that starts with code like \item Content goes here
    /// </summary>
    public class Command : LaTeXElement
    {
        /// <summary>
        /// Creates a new instance of the Command class
        /// </summary>
        /// <param name="type">The type of the inline code block</param>
        public Command(CommandType type)
        {
            Type = type;
            InnerElements = new List<LaTeXElement>();
        }

        /// <summary>
        /// Get the type of the inline
        /// </summary>
        public CommandType Type { get; private set; }

        /// <summary>
        /// Get/Set the elements of the inline
        /// </summary>
        public List<LaTeXElement> InnerElements { get; set; }

        public List<string> RequriredArguments { get; set; }

        public List<string> OptionalArguments { get; set; }
    }
}