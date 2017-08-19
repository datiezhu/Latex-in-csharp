using System.Text.RegularExpressions;

namespace Moosetrail.LaTeX.ElementsParser
{
    internal class RawArgumentCommand
    {
        public RawArgumentCommand(Match match)
        {
            FullCommand = match.Value;
            setCommandType(match);
            setRequiredArguments(match);
            setOptionalArguments(match);
        }

        private void setCommandType(Match match)
        {
            CommandType = "";

            var index = 1;
            var flag = true;
            while (index < match.Groups.Count)
            {
                if (match.Groups[index].Success)
                {
                    CommandType = match.Groups[index].Value;
                }

                if (flag)
                {
                    index = index + 3;
                    flag = false;
                }
                else
                {
                    index = index + 2;
                    flag = true;
                }

            }
        }

        private void setRequiredArguments(Match match)
        {
            RequriredArguments = "";

            var index = 3;
            var flag = true;
            while (index < match.Groups.Count)
            {
                if (match.Groups[index].Success)
                {
                    RequriredArguments = match.Groups[index].Value;
                }

                if (flag)
                {
                    index = index + 2;
                    flag = false;
                }
                else
                {
                    index = index + 3;
                    flag = true;
                }

            }
        }

        private void setOptionalArguments(Match match)
        {
            OptionalArguments = "";

            var index = 2;
            while (index < match.Groups.Count)
            {
                if (match.Groups[index].Success)
                {
                    OptionalArguments = match.Groups[index].Value;
                }

                index = index + 5;
            }
        }

        public string FullCommand { get; set; }

        public string CommandType { get; private set; }

        public string RequriredArguments { get; private set; }

        public string OptionalArguments { get; private set; }
    }
}