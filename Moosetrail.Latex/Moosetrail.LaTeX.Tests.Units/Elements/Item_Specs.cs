using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.Elements
{
    [TestFixture]
    public class Item_Specs
    {
        private Item SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new Item();
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

        #region Constructor

        [Test]
        public void constructor_should_set_elements_to_empty_list()
        {
            // When 
            SUT = new Item();

            // Then
            Assert.IsEmpty(SUT.Elements);
        }

        #endregion Constructor 
    }
}