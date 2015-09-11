﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Moosetrail.LaTeX
{
    /// <summary>
    /// Contract for a parser responsible for parsing a specific LaTeX element
    /// </summary>
    public interface LaTeXElementParser
    {
        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> CodeIndicators { get; }

        /// <summary>
        /// Gets an element, same as the ParseCode but without anything set, just an empty object
        /// </summary>
        /// <returns>A LatexObject</returns>
        LaTeXElement GetEmptyElement();

        /// <summary>
        /// Sets the child elements of a given element
        /// </summary>
        /// <param name="element">The element to set the children to</param>
        /// <param name="children">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element isn't supported or the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        void SetChildElement(LaTeXElement element, params LaTeXElement[] children);

        /// <summary>
        /// Parses the code given the object and sets the data to the object 
        /// from the front of the string. 
        /// </summary>
        /// <param name="code">The code to parse and handle</param>
        /// <returns>
        /// The newly parsed object. The string builder will also have been updted, the code parsed is removed
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the code string doesn't start with one of the accepted code indicators or the element isn't supported by the parser</exception>
        LaTeXElement ParseCode(StringBuilder code);
    }
}