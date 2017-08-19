using System.Collections.Generic;
using Moosetrail.LaTeX.Arguments;

namespace Moosetrail.LaTeX.CommandRules
{
    public class DocumentclassRules : ArgumentCommandRules
    {
        public int NbrOfRequriredArguments => 1;

        /// <summary>
        /// Get the number of optiona options that are allowed, -1 indicates no limit
        /// </summary>
        public int NbrOfOptionalArguments => -1;

        public IEnumerable<string> AllowedRequiredArguments => new[] { "article", "book", "letter", "report", "slides" };

        public IEnumerable<Argument> OptionalArguments => new List<Argument>
        {
            new PatternArgument("[0-9]+pt", null, new []{"slides"}),
            new StandardArgument("a4paper"),
            new StandardArgument("a5paper"),
            new StandardArgument("b5paper"),
            new StandardArgument("executivepaper"),
            new StandardArgument("legalpaper"),
            new StandardArgument("letterpaper"),
            new StandardArgument("draft", null, null, new []{"final"}),
            new StandardArgument("final", null, null, new []{"draft"}),
            new StandardArgument("fleqn"),
            new StandardArgument("landscape"),
            new StandardArgument("leqno"),
            new StandardArgument("openbib"),
            new StandardArgument("titlepage", null, null, new []{"notitlepage"}),
            new StandardArgument("notitlepage", null, null, new []{"titlepage"}),
            new StandardArgument("onecolumn", null, new []{"slides"}, new []{"twocolumn"}),
            new StandardArgument("twocolumn", null, new []{"slides"}, new []{"onecolumn"}),
            new StandardArgument("oneside", null, new []{"slides"}, new []{"twoside"}),
            new StandardArgument("twoside", null, new []{"slides"}, new []{"oneside"}),
            new StandardArgument("openright", null, new []{"slides"}, new []{"openany"}),
            new StandardArgument("openany", null, new []{"slides"}, new []{"openright"}),
            new StandardArgument("clock", new []{"slides"})
        };
    }
}