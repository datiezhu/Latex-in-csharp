namespace Moosetrail.LaTeX.Exceptions
{
    public class LaTeXParseCommandException : LaTeXException
    {
        public LaTeXParseCommandException() { }

        public LaTeXParseCommandException(string failingCodePart, string code, string message = null)
            :base(message)
        {
            FailingCode = failingCodePart;

            if (code.Length > 100)
                InArea = code.Substring(0, 100) + "...";
            else
                InArea = code;
        }

        public string FailingCode { get; private set; }

        public string InArea { get; private set; }
    }
}