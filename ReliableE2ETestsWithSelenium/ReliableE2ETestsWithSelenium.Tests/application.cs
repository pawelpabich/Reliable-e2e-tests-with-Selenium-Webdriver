using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
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
            DB.Reset();
            browser = new FirefoxDriver();
        }

        [TearDown]
        public void CleanUp()
        {
            browser.Quit();
            DB.Reset();
        }

        [Test]
        public void should_list_products_on_home_page()
        {
            Given_I_am_on_product_listing_page();
            Then_I_can_see_list_of_products();
        }

        private void Then_I_can_see_list_of_products()
        {
            var products = FindDisplayedProducts();   
            Assert.AreEqual(3, products.Count);
        }

        private void Given_I_am_on_product_listing_page()
        {
            DB.InsertProducts(new[] {"Product1", "Product2", "Product3"});
            browser.Navigate().GoToUrl("http://localhost:2066");
        }

        private ReadOnlyCollection<IWebElement> FindDisplayedProducts()
        {
            return browser.FindElementsByCssSelector("#product-list li");
        }
    }
}
