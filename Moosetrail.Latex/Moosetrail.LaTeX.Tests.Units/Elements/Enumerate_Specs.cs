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

        #region CodeConstructor

        private const string ListString = @"\begin{enumerate} \item Item 1 \item Item 2 \end{enumerate}";

        [Test]
        public void codeConstructor_should_set_itemList()
        {
            // When 
            SUT = new Enumerate(ListString);

            // Then
            CollectionAssert.AreEqual(
                new List<TextBody>
                {
                    new TextBody("Item 1"),
                    new TextBody("Item 2")
                },
                SUT.ItemList);
        }

        #endregion CodeConstructor

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
            CollectionAssert.Contains(SUT.ItemList, new TextBody("My item"));
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

        #region TestHelpers

        readonly List<string> _noneAddItems = new List<string>
        {
            @"\n",
            @"\t",
            @"\n\t",
            @"\r",
            @"\n\r",
            @"\r\n",
            @"\t\n"
        }; 

        #endregion TestHelpers
    }
}