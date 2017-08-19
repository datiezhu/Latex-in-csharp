﻿using System.Collections.Generic;
using Moosetrail.LaTeX.Arguments;

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
        public Command(CommandType type, IEnumerable<string> requriredArguments = null, IEnumerable<string> optionalArguments = null)
        {
            Type = type;
            InnerElements = new List<LaTeXElement>();
            RequriredArguments = requriredArguments;
            OptionalArguments = optionalArguments;
        }

        /// <summary>
        /// Get the type of the inline
        /// </summary>
        public CommandType Type { get; private set; }

        /// <summary>
        /// Get/Set the elements of the inline
        /// </summary>
        public List<LaTeXElement> InnerElements { get; private set; }

        public IEnumerable<string> RequriredArguments { get; private set; }

        public IEnumerable<string> OptionalArguments { get; private set; }
    }
}