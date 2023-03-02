using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.HelpDesk.SpecFlow.Drivers
{
    public class SeleniumDriver
    {
        private IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;

        public SeleniumDriver(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;

        public IWebDriver Setup(string os, string version, string browserName)
        {
            dynamic capability = GetBrowserOptions(browserName);


            capability.AddAdditionalOption("platform", os);
            capability.AddAdditionalOption("version", version);
            capability.AddAdditionalOption("name", browserName);
            //capability.AddAdditionalOption("build", build);


            _driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capability.ToCapabilities(), TimeSpan.FromSeconds(60));
            _driver.Manage().Window.Maximize();
            _scenarioContext.Set(_driver, "WebDriver");
            return _driver;

        }

        private dynamic GetBrowserOptions(string browserName)
        {

            if (browserName.ToLower() == "chrome")
            {
                return new ChromeOptions();
            }
            if (browserName.ToLower() == "firefox")
            {
                return new FirefoxOptions();
            }
            if (browserName.ToLower() == "microsoft edge")
            {
                return new EdgeOptions();
            }
            if (browserName.ToLower() == "safari")
            {
                return new SafariOptions();
            }

            //If none of the above, return ChromeOptions
            return new ChromeOptions();
        }
    }
}
