using System.Linq;
using System.Text;
using Moosetrail.LaTeX.Elements;

namespace Moosetrail.LaTeX.CodeGenerator
{
    /// <summary>
    /// Code generator for Item elements
    /// </summary>
    public class ItemCodeGenerator : CodeGenerator<Item>
    {
        /// <summary>
        /// Generates code that represent the element
        /// </summary>
        /// <param name="element">The element to generate code for</param>
        /// <returns>
        /// The generated LaTeX compilable code
        /// </returns>
        public string Generate(Item element)
        {
            var code = new StringBuilder(@"\item ");

            foreach (var elementCode in element.Elements.Select(CodeGeneratorFactory.GenerateCode))
            {
                code.AppendFormat("{0}\n\t ", elementCode);
            }
            code.Remove(code.Length - 2, 1);

            return code.ToString();
        }
    }
}