﻿using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using ReliableE2ETestsWithSelenium.Infrastructure;

namespace ReliableE2ETestsWithSelenium.Tests
{
    [TestFixture]
    public class application
    {
        private FirefoxDriver browser;

        [SetUp]
        public void Setup()
        {
            browser = new FirefoxDriver();
        }

        [TearDown]
        public void CleanUp()
        {
            browser.Quit();
        }

        [Test]
        public void should_list_products_on_home_page()
        {
            Given_I_am_on_product_listing_page();
            Then_I_can_see_list_of_products();
        }

        private void Then_I_can_see_list_of_products()
        {
            var products = browser.FindElementsByCssSelector("#product-list li");
            Assert.AreEqual(3, products.Count);
        }

        private void Given_I_am_on_product_listing_page()
        {
            DB.InsertProducts(new[] {"Product1", "Product2", "Product3"});

            browser.Navigate().GoToUrl("http://localhost:2066/Home/Test1");
        }
    }
}