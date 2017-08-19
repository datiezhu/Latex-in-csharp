using System;
using System.Collections.Generic;
using System.Text;

namespace Moosetrail.LaTeX.ElementsParser
{
    public interface LaTexElementParser<TElement> where TElement:LaTeXElement
    {
        /// <summary>
        /// Get all the code indicators that the element accepts as startingpoints to parse
        /// </summary>
        IEnumerable<string> CodeIndicators { get; }

        /// <summary>
        /// Gets an element, same as the ParseCode but without anything set, just an empty object
        /// </summary>
        /// <returns>A LatexObject</returns>
        TElement GetEmptyElement();

        /// <summary>
        /// Sets the child elements of a given element
        /// </summary>
        /// <param name="element">The element to set the children to</param>
        /// <param name="children">The elements to set</param>
        /// <exception cref="NotSupportedException">Thrown if the element isn't supported or the element doesn't support child items</exception>
        /// <exception cref="ArgumentException">Thrown if the any element in the list isn't a supported child element</exception>
        void SetChildElement(TElement element, params LaTeXElement[] children);

        /// <summary>
        /// Parses the code given the object and sets the data to the object 
        /// from the front of the string. 
        /// </summary>
        /// <param name="code">The code to parse and handle</param>
        /// <returns>
        /// The newly parsed object. The string builder will also have been updted, the code parsed is removed
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the code string doesn't start with one of the accepted code indicators or the element isn't supported by the parser</exception>
        TElement ParseCode(StringBuilder code);

        /// <summary>
        /// Parses the code given the object and sets the data to the object 
        /// from the front of the string. Returns the created object and the code after the element was ended
        /// </summary>
        /// <param name="code">The code to parse and handle</param>
        /// <returns>
        /// A Tuple with the newly parsed object. The second object is a string with the parsed code removed from the code given as an argument to the function
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the code string doesn't start with one of the accepted code indicators or the element isn't supported by the parser</exception>
        Tuple<TElement, string> ParseCode(string code);
    }
}