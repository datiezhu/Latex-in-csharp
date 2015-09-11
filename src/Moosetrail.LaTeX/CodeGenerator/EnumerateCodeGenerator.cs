using System.Text;
using Moosetrail.LaTeX.Elements;

namespace Moosetrail.LaTeX.CodeGenerator
{
    /// <summary>
    /// Code generator for for enumerate elements
    /// </summary>
    public class EnumerateCodeGenerator : CodeGenerator<Enumerate>
    {
        /// <summary>
        /// Generates code that represent the element
        /// </summary>
        /// <param name="element">The element to generate code for</param>
        /// <returns>
        /// The generated LaTeX compilable code
        /// </returns>
        public string Generate(Enumerate element)
        {
            var code = new StringBuilder(@"\begin{enumerate}" + "\n\t");

            foreach (var item in element.ItemList)
            {
                var itemCode = CodeGeneratorFactory.GenerateCode(item);
                code.Append(itemCode);

                if (element.ItemList.IndexOf(item) != element.ItemList.Count - 1)
                    code.Append("\t");
            }

            code.Append(@"\end{enumerate}" + "\n");

            return code.ToString();
        }
    }
}