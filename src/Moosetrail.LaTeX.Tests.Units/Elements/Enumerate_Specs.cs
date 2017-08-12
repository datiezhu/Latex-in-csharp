using System.Collections.Generic;
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
            CollectionAssert.Contains(SUT.ItemList[0].Elements, new TextBody("My text"));
        }

        [Test]
        [TestCaseSource(nameof(_noneAddItems))]
        public void addItem_should_not_add_item_wit_only_formating(string str)
        {
            // When 
            SUT.AddItem(str);

            // Then
            Assert.IsEmpty(SUT.ItemList);
        }

        [Test]
        [TestCaseSource(nameof(_noneAddItems))]
        public void addItem_should_remove_starting_textFormat_from_item(string str)
        {
            // When 
            SUT.AddItem(str + "My item");

            // Then
            CollectionAssert.Contains(SUT.ItemList[0].Elements, new TextBody("My item"));
        }

        #endregion AddItem

        #region ToString

        [Test]
        public void toString_should_return_code_for_enumerate()
        {
            // Given 
            SUT.AddItem("My item");
            SUT.AddItem("My second item");

            // When 
            var result = SUT.ToString();

            // Then
            Assert.AreEqual(@"\begin{enumerate}" + "\n\t" +
                @"\item My item" + "\n \t" +
                @"\item My second item" + "\n" +
                @" \end{enumerate}" + "\n", result);
        }

        #endregion ToString

        #region TestHelpers

        private static readonly List<string> _noneAddItems = new List<string>
        {
            "\n",
            "\t",
            "\n\t",
            "\r",
            "\n\r",
            "\r\n",
            "\t\n"
        }; 

        #endregion TestHelpers
    }
}