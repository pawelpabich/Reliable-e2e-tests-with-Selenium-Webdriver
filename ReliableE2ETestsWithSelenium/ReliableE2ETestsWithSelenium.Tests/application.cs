using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Threading;
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

        [Test]
        public void should_refresh_product_list()
        {
            Given_I_am_on_product_listing_page();
            When_I_refresh_product_list();
            Then_I_can_see_new_list_of_products();
        }

        private void When_I_refresh_product_list()
        {
            DB.InsertProducts(new[] { "Product1", "Product2", "Product3", "Product4" });
            browser.FindElementByCssSelector("#refresh-list").Click();
        }

        private void Then_I_can_see_new_list_of_products()
        {
            
            try
            {
                Thread.Sleep(2000);
                var products = FindDisplayedProducts();
                Assert.AreEqual(4, products.Count);
            }
            catch (AssertionException)
            {
                TakeScreenshot();
                throw;
            }            
        }

        private void Then_I_can_see_list_of_products()
        {
            var products = FindDisplayedProducts();   
            Assert.AreEqual(3, products.Count);
        }

        private void TakeScreenshot()
        {
            var camera = (ITakesScreenshot)browser;
            var screenshot = camera.GetScreenshot();
            screenshot.SaveAsFile(Guid.NewGuid().ToString("N") + ".png", ImageFormat.Png);
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
