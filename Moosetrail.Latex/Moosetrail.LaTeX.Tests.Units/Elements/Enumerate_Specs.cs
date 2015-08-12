using System.Linq;
using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.Elements
{
    [TestFixture]
    public class Enumerate_Specs
    {
        private Enumerate SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new Enumerate();
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
        public void constructor_should_set_itemLIst_to_empty()
        {
            // When
            SUT = new Enumerate(); 

            // Then
            Assert.IsEmpty(SUT.ItemList);
        }

        #endregion Constructor

        #region AddItem

        [Test]
        public void addItem_should_add_item_to_itemList()
        {
            // When 
            SUT.AddItem("My text");

            // Then
            Assert.AreEqual(1, SUT.ItemList.Count());
            CollectionAssert.Contains(SUT.ItemList, new TextBody("My text"));
        }

        #endregion AddItem

        #region ToCode

        [Test]
        public void toCode_should_return_valid_latex_text()
        {
            // Given 
            SUT.AddItem("My first text");
            SUT.AddItem("My second text");

            // When 
            var result = SUT.ToCode();

            // Then
            Assert.AreEqual(@"\begin{enumerate}\item My first text \item My second text \end{enumerate}", result);
        }

        #endregion ToCode
    }
}