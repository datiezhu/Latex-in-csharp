namespace Moosetrail.LaTeX.CodeGenerator
{
    /// <summary>
    /// Contract for code genertors for LaTeX elements
    /// </summary>
    /// <typeparam name="TElement">The type of element the code genertor can handle</typeparam>
    public interface CodeGenerator<in TElement> where TElement:LaTeXElement
    {
        /// <summary>
        /// Generates code that represent the element
        /// </summary>
        /// <param name="element">The element to generate code for</param>
        /// <returns>
        /// The generated LaTeX compilable code
        /// </returns>
        string Generate(TElement element);
    }
}