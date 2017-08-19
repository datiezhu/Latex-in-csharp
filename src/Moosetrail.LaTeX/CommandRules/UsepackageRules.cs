using System.Collections.Generic;
using Moosetrail.LaTeX.Arguments;

namespace Moosetrail.LaTeX.CommandRules
{
    public class UsepackageRules : ArgumentCommandRules
    {
        public int NbrOfRequriredArguments => 1;
        public int NbrOfOptionalArguments => -1;
        public IEnumerable<string> AllowedRequiredArguments => new List<string>();
        public IEnumerable<Argument> OptionalArguments => new List<Argument>();
    }
}