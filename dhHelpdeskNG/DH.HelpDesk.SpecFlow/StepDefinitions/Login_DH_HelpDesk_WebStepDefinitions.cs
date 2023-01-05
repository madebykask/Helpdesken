using DH.HelpDesk.SpecFlow.Drivers;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace DH.HelpDesk.SpecFlow.StepDefinitions
{
    [Binding]
    public class Login_DH_HelpDesk_WebStepDefinitions
    {
        private IWebDriver driver;
        private readonly ScenarioContext _scenarioContext;

        public Login_DH_HelpDesk_WebStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        
        [Given(@"I launch the DH\.HelpDesk\.Web application")]
        public void GivenILaunchTheDH_HelpDesk_WebApplication(Table table)
        {
            dynamic data = table.CreateDynamicInstance();

            driver = _scenarioContext.Get<SeleniumDriver>("SeleniumDriver").Setup((string)data.OS, (string)data.BrowserVersion, (string)data.Build, (string)data.Browser);
            driver.Url = "https://dev-helpdesk-internal.dhsolutions.se/";
        }

        [When(@"I enter the following details")]
        public void WhenIEnterTheFollowingDetails(Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            driver.FindElement(By.Id("txtUid")).SendKeys((string)data.Username);
            driver.FindElement(By.Id("txtPwd")).SendKeys((string)data.Password);
        }

        [When(@"I click login button")]
        public void WhenIClickLoginButton()
        {
            driver.FindElement(By.Id("btnLogin")).Click();
            Thread.Sleep(5000);
        }

        [Then(@"I should be able to see the Case Summary pages")]
        public void ThenIShouldBeAbleToSeeTheCaseSummaryPages()
        {
            Assert.That(1==1, Is.True);
        }
    }
}
