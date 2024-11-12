using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;
using System;
using Microsoft.Extensions.Logging;

namespace PremierLeagueTests.TestUtils
{
    public class TestBase : IDisposable
    {
        protected IWebDriver Driver;
        //private static readonly ILogger<Program> _logger;

        public TestBase()
        {
            this.Driver = new ChromeDriver();
            this.Driver.Navigate().GoToUrl("https://www.premierleague.com/");
            
        }

        // Helper function for clicking an element
        protected void ClickElement(By locator)
        {
            var element = WaitForElementToBeClickable(locator);
            element.Click();
        }

        // Helper function for sending keys to an element
        protected void SendKeys(By locator, string text)
        {
            var element = WaitForElementToBeVisible(locator);
            element.Clear();
            element.SendKeys(text);
        }

        // Helper function for waiting for an element to be clickable
        protected IWebElement WaitForElementToBeClickable(By locator, int timeoutInSeconds = 10)
        {
            
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }

        // Helper function for waiting for an element to be visible
        protected IWebElement WaitForElementToBeVisible(By locator, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

        public void Dispose()
        {
            Driver.Quit();
        }
    }
}
