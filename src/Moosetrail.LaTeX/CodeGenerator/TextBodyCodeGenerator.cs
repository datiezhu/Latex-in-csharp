using Moosetrail.LaTeX.Elements;

namespace Moosetrail.LaTeX.CodeGenerator
{
    /// <summary>
    /// Generates code for text elements
    /// </summary>
    public class TextBodyCodeGenerator : CodeGenerator<TextBody>
    {
        /// <summary>
        /// Generates code that represent the element
        /// </summary>
        /// <param name="element">The element to generate code for</param>
        /// <returns>
        /// The generated LaTeX compilable code
        /// </returns>
        public string Generate(TextBody element)
        {
            return element.TheText;
        }
    }
}