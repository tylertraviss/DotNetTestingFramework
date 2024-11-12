using System;
using System.IO;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Serilog;
using Xunit;

namespace PremierLeagueTests.TestUtils
{
    public class TestBase : IDisposable
    {
        protected IWebDriver Driver;
        private static readonly ILogger<TestBase> _logger;

        static TestBase()
        {
            // Ensure Logs directory exists
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("Logs/test_log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Create a logger factory that uses Serilog
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            _logger = loggerFactory.CreateLogger<TestBase>();
        }

        public TestBase()
        {
            _logger.LogInformation("Initializing WebDriver.");
            this.Driver = new ChromeDriver();
            this.Driver.Navigate().GoToUrl("https://www.espn.com/soccer/stats/_/league/eng.1");
            _logger.LogInformation("Navigated to Premier League homepage.");
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

        /// <summary>
        /// Checks if an element exists on the page.
        /// </summary>
        /// <param name="locator">The locator to find the element.</param>
        /// <param name="timeoutInSeconds">Maximum time to wait for the element.</param>
        /// <returns>True if the element exists, otherwise false.</returns>
        protected bool ElementExists(By locator, int timeoutInSeconds = 5)
        {
            _logger.LogInformation($"Checking existence of element: {locator}");
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(driver => driver.FindElements(locator).Count > 0);
                _logger.LogInformation($"Element exists: {locator}");
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                _logger.LogWarning($"Element does not exist: {locator}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while checking element existence: {locator}. Exception: {ex.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            _logger.LogInformation("Quitting WebDriver.");
            Driver.Quit();
            _logger.LogInformation("WebDriver closed successfully.");
            Log.CloseAndFlush(); // Ensure all logs are flushed
        }
    }
}
