using System;
using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.Elements
{
    [TestFixture]
    public class DocumentClass_Specs
    {
        private DocumentClass SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new DocumentClass();
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
        
        #region EmptyContrcutor

        [Test]
        public void constructor_should_create_empty_usePackages()
        {
            // When 
            SUT = new DocumentClass();

            // Then
            Assert.IsEmpty(SUT.UsePackages);
        }

        #endregion EmptyContrcutor

        
    }
}