using System;
using Moosetrail.LaTeX.Elements;

namespace Moosetrail.LaTeX.CodeGenerator
{
    /// <summary>
    /// Factory for generating code
    /// </summary>
    public static class CodeGeneratorFactory
    {
        /// <summary>
        /// Get the code for a specific element 
        /// </summary>
        /// <typeparam name="T">The type of the element to get code for</typeparam>
        /// <param name="element">The element to get the code for</param>
        /// <returns>
        /// The generated code
        /// </returns>
        /// <exception cref="NotSupportedException">Thrown if the element isn't supported by the factory method</exception>
        public static string GenerateCode<T>(T element) where T : LaTeXElement
        {
            switch (element.GetType().FullName)
            {
                case "Moosetrail.LaTeX.Elements.Enumerate":
                    {
                        var generator = new EnumerateCodeGenerator();
                        return generator.Generate(element as Enumerate);
                    }
                case "Moosetrail.LaTeX.Elements.Item":
                {
                    var generator = new ItemCodeGenerator();
                    return generator.Generate(element as Item);
                }
                case "Moosetrail.LaTeX.Elements.TextBody":
                {
                    var generator = new TextBodyCodeGenerator();
                    return generator.Generate(element as TextBody);
                }
                default:
                    throw new NotSupportedException("The element isn't supported by the factory");
            }
        }
        
    }
}