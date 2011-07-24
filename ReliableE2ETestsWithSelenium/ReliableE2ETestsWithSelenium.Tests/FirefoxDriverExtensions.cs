using System;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium.Firefox;

namespace ReliableE2ETestsWithSelenium.Tests
{
    public static class FirefoxDriverExtensions
    {
        public static void SetPageLoadTimeout(this FirefoxDriver browser, TimeSpan pageLoadTimeout)
        {
            // Hack to workaround the following issue. Basically we need to be able to set the page load timeout which is not exposed
            // http://code.google.com/p/selenium/issues/detail?id=687&q=get()&colspec=ID%20Stars%20Type%20Status%20Priority%20Milestone%20Owner%20Summary
            var commandExecutorProperty = browser.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(p => p.Name.Contains("CommandExecutor")).Single();
            var commandExecutorObject = commandExecutorProperty.GetValue(browser, null);

            var httpExecutorField = commandExecutorObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(f => f.Name.Contains("executor")).Single();
            var httpExecutorObject = httpExecutorField.GetValue(commandExecutorObject);

            var timeoutField = httpExecutorObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(fi => fi.Name.Contains("serverResponseTimeout")).Single();

            timeoutField.SetValue(httpExecutorObject, pageLoadTimeout);
        }
    }
}