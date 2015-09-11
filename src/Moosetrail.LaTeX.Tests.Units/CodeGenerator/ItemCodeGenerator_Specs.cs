using Moosetrail.LaTeX.CodeGenerator;
using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.CodeGenerator
{
    [TestFixture]
    public class ItemCodeGenerator_Specs
    {
        private ItemCodeGenerator SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new ItemCodeGenerator();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void should_be_CodeGenerator()
        {
            Assert.IsInstanceOf<CodeGenerator<Item>>(SUT);
        }

        #region Generate

        [Test]
        public void generate_should_return_code_for_item()
        {
            // Given 
            var item = new Item();
            item.Elements.Add(new TextBody("My awesome text"));

            // When 
            var result = SUT.Generate(item);

            // Then
            Assert.AreEqual(@"\item My awesome text" + "\n ", result);
        }

        [Test]
        public void generate_should_return_code_for_item_with_several_items()
        {
            // Given 
            var item = new Item();
            item.Elements.Add(new TextBody("My awesome text"));
            item.Elements.Add(new TextBody("My other text"));

            // When 
            var result = SUT.Generate(item);

            // Then
            Assert.AreEqual(@"\item My awesome text" + "\n\t My other text\n ", result);
        }

        #endregion Generate
    }
}