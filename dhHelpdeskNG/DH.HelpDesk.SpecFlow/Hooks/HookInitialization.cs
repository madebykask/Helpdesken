using DH.HelpDesk.SpecFlow.Drivers;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace DH.HelpDesk.SpecFlow.Hooks
{
    [Binding]
    public sealed class HookInitialization
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        private TechTalk.SpecFlow.ScenarioContext _scenarioContext;

        public HookInitialization(TechTalk.SpecFlow.ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        //[BeforeScenario("@tag1")]
        //public void BeforeScenarioWithTag()
        //{
        //    // Example of filtering hooks using tags. (in this case, this 'before scenario' hook will execute if the feature/scenario contains the tag '@tag1')
        //    // See https://docs.specflow.org/projects/specflow/en/latest/Bindings/Hooks.html?highlight=hooks#tag-scoping

        //    //TODO: implement logic that has to run before executing each scenario

            
            
        //    SeleniumDriver seleniumDriver = new SeleniumDriver(_scenarioContext);
        //    _scenarioContext.Set(seleniumDriver, "SeleniumDriver");
        //}

        [BeforeScenario]
        public void BeforeScenario()
        {
            // Example of ordering the execution of hooks
            // See https://docs.specflow.org/projects/specflow/en/latest/Bindings/Hooks.html?highlight=order#hook-execution-order

            //TODO: implement logic that has to run before executing each scenario
            SeleniumDriver seleniumDriver = new SeleniumDriver(_scenarioContext);
            _scenarioContext.Set(seleniumDriver, "SeleniumDriver");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
            _scenarioContext.Get<IWebDriver>("WebDriver").Quit();
        }
    }
}