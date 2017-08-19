using System.Collections.Generic;
using Moosetrail.LaTeX.Arguments;

namespace Moosetrail.LaTeX.CommandRules
{
    public interface CommandRules
    {
        int NbrOfRequriredArguments { get; }

        /// <summary>
        /// Get the number of optiona options that are allowed, -1 indicates no limit
        /// </summary>
        int NbrOfOptionalArguments { get; }

        IEnumerable<string> AllowedRequiredArguments { get; }

        IEnumerable<Argument> OptionalArguments { get; }
    }
}