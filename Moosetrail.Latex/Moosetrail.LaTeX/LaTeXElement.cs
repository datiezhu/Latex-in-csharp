using System;
using System.Collections.Generic;

namespace Moosetrail.LaTeX
{
    /// <summary>
    /// Contract for all different LaTeXElements that thi class supports
    /// </summary>
    public interface LaTeXElement
    {
        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
         IEnumerable<string> CodeIndicators { get; }

        /// <summary>
        /// Sets the child elements of the current element
        /// </summary>
        /// <param name="elements">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        void SetChildElement(params LaTeXElement[] elements);

        /// <summary>
        /// Parses the code given the object and sets the data to the object 
        /// from the front of the string. 
        /// </summary>
        /// <param name="code">The code to parse</param>
        /// <returns>
        /// What is left of the string after the parser is done. 
        /// The code that have been parsed into the object will have been removed
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the code string doesn't start with one of the accepted code indicators</exception>
        string ParseCode(string code);
    }
}