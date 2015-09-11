using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.Elements
{
    [TestFixture]
    public class Section_Specs
    {
        private Section SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new Section();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_LaTeXElement()
        {
            Assert.IsInstanceOf<ContentContext>(SUT);
        }

        #region Constrcutor Empty

        [Test]
        public void constructor_should_set_elements_to_empty()
        {
            // When 
            SUT = new Section();

            // Then
            Assert.IsEmpty(SUT.Elements);
        }

        #endregion Constrcutor Empty

    }
}