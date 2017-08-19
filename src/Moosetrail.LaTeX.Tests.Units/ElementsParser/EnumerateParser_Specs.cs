using System;
using System.Text;
using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.ElementsParser
{
    [TestFixture]
    public class EnumerateParser_Specs : EnvelopeParser_Specs
    {
        #region SetChildElement

        [Test]
        public void setChildElement_should_throw_if_child_isnt_item()
        {
            // Given 

            // Then
            var ex = Assert.Throws<ArgumentException>(() => SUT.SetChildElement(new Enumerate(), new TextBody()));
            Assert.AreEqual("Enumerate can only hold Item objects", ex.Message);
        }

        #endregion SetChildElement

        #region ParseCode

        [Test]
        public void parseCode_should_return_empty_enumerate()
        {
            // Given 
            const string code = @"\begin{enumerate} \item Item 1 \item Item 2 \end{enumerate}" +
                                @"\begin{enumerate} \item Item 3 \item Item 4 \end{enumerate}";

            // When 
            var enumerate = SUT.ParseCode(new StringBuilder(code));

            // Then
            Assert.AreEqual(EnvelopeCommand.enumerate, enumerate.Type);
            Assert.IsEmpty(enumerate.InnerElements);
        }

        #endregion ParseCode
    }
}