using System;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace PremierLeagueTests.TestUtils
{
    public class TestBase : IDisposable
    {
        protected IWebDriver Driver;
        private static readonly ILogger<TestBase> _logger;

        static TestBase()
        {
            // Configure the logger
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConsole() // Log to console
                    .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information); // Specify namespace explicitly
            });

            _logger = loggerFactory.CreateLogger<TestBase>();
        }

        public TestBase()
        {
            _logger.LogInformation("Initializing WebDriver.");
            this.Driver = new ChromeDriver();
            this.Driver.Navigate().GoToUrl("https://www.espn.com/soccer/stats/_/league/eng.1");
            _logger.LogInformation("Navigated to Premier League ESPN Page.");
        }

        // Helper function for clicking an element
        protected void ClickElement(By locator)
        {
            _logger.LogInformation($"Attempting to click element: {locator}");
            var element = WaitForElementToBeClickable(locator);
            element.Click();
            _logger.LogInformation("Element clicked successfully.");
        }

        // Helper function for sending keys to an element
        protected void SendKeys(By locator, string text)
        {
            _logger.LogInformation($"Sending keys to element: {locator}");
            var element = WaitForElementToBeVisible(locator);
            element.Clear();
            element.SendKeys(text);
            _logger.LogInformation($"Text '{text}' sent to element successfully.");
        }

        // Helper function for waiting for an element to be clickable
        protected IWebElement WaitForElementToBeClickable(By locator, int timeoutInSeconds = 10)
        {
            _logger.LogInformation($"Waiting for element to be clickable: {locator}");
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }

        // Helper function for waiting for an element to be visible
        protected IWebElement WaitForElementToBeVisible(By locator, int timeoutInSeconds = 10)
        {
            _logger.LogInformation($"Waiting for element to be visible: {locator}");
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

        public void Dispose()
        {
            _logger.LogInformation("Quitting WebDriver.");
            Driver.Quit();
            _logger.LogInformation("WebDriver closed successfully.");
        }
    }
}
