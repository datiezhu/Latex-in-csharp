using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.Elements
{
    [TestFixture]
    public class Document_Specs
    {
        private Document SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new Document();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_LaTeXElement()
        {
            Assert.IsInstanceOf<LaTeXElement>(SUT);
        }

        #region Empty Constructor

        [Test]
        public void constructor_should_set_elements_to_empty()
        {
            // When 
            SUT = new Document();

            // Then
            Assert.IsEmpty(SUT.Elements);
        }

        #endregion Empty Constructor
    }
}