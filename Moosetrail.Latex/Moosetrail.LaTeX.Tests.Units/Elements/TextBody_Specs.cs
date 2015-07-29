﻿using Moosetrail.LaTeX.Elements;
using NUnit.Framework;

namespace Moosetrail.LaTeX.Tests.Units.Elements
{
    [TestFixture]
    public class TextBody_Specs
    {
        private TextBody SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new TextBody();
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
        public void constructor_should_set_the_text_given()
        {
            // When 
            SUT = new TextBody("My text");

            // Then
            Assert.AreEqual("My text", SUT.TheText);
        }

        #endregion Constructor

        #region TheText

        [Test]
        public void theText_should_be_able_toset()
        {
            // When 
            SUT.TheText = "My text";

            // Then
            Assert.AreEqual("My text", SUT.TheText);
        }
        [Test]
        public void theText_should_trim_the_text_to_set()
        {
            // When 
            SUT.TheText = " My text ";

            // Then
            Assert.AreEqual("My text", SUT.TheText);
        }

        #endregion TheText
    }
} 