using DH.Helpdesk.Web.Infrastructure;
using DH.HelpDesk.SpecFlow.Drivers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
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

            driver = _scenarioContext.Get<SeleniumDriver>("SeleniumDriver").Setup((string)data.OS, (string)data.BrowserVersion, (string)data.Browser);
            driver.Url = "https://localhost:449/";
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
            //Thread.Sleep(5000);
        }

        [Then(@"I should be able to see the Administrator button")]
        public void ThenIShouldBeAbleToSeeTheAdministratorButton()
        {
            try
            {
                var el = driver.FindElements(By.Id("btnAdminStart"));

                Assert.IsTrue(el.Count > 0);

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [Given(@"I login as the an admin user")]
        public void GivenILoginAsTheAnAdminUser(Table table)
        {
            GivenILaunchTheDH_HelpDesk_WebApplication(table);
            WhenIEnterTheFollowingDetails(table);
            WhenIClickLoginButton();
        }

        [Given(@"I give the following user the following starting page")]
        public void GivenIGiveTheFollowingUserTheFollowingStartingPage(Table table)
        {
            driver.FindElement(By.Id("userMenuList")).Click();

            driver.FindElement(By.Id("btnAdminStart")).Click();

            driver.SwitchTo().Window(driver.WindowHandles.Last());

            driver.FindElement(By.Id("btnAdminUsers")).Click();

            var userLnk = driver.FindElements(By.ClassName("userIdUsersAdminLnk")).Where(x => x.Text == "DS").FirstOrDefault();

            if (userLnk != null)
            {
                userLnk.Click();
            }

            var optionLnk = driver.FindElements(By.CssSelector("a[href='#subfragment-5']")).FirstOrDefault();

            if (optionLnk != null)
            {
                optionLnk.Click();
            }


            var startPageSelect = driver.FindElement(By.Id("User_StartPage"));

            var selectElement = new SelectElement(startPageSelect);

            selectElement.SelectByText("Ärendeöversikt");

            if (driver.FindElements(By.Id("btnSave")).Count > 0)
            {
                driver.FindElement(By.Id("btnSave")).Click();
            }

            driver.FindElement(By.Id("drpUserAdmin")).Click();

            driver.FindElement(By.Id("btnUserAdminClose")).Click();

            driver.SwitchTo().Window(driver.WindowHandles.FirstOrDefault());
        }

        [Given(@"I logout from the admin user")]
        public void GivenILogoutFromTheAdminUser(Table table)
        {
            driver.FindElement(By.Id("userMenuList")).Click();

            driver.FindElement(By.Id("btnUserLogout")).Click();
        }

        [When(@"I login as the user")]
        public void WhenILoginAsTheUser(Table table)
        {
            WhenIEnterTheFollowingDetails(table);
            WhenIClickLoginButton();
        }

        [Then(@"I should be able to see the Case Summary page")]
        public void ThenIShouldBeAbleToSeeTheCaseSummaryPage()
        {
            try
            {
                var el = driver.FindElements(By.ClassName("case-overview"));

                Assert.IsTrue(el.Count > 0);

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

    }
}
