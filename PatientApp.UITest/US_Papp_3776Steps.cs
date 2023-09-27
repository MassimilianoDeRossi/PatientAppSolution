using System;
using System.Diagnostics;
using TechTalk.SpecFlow;

namespace PatientApp.UITest
{
    [Binding]
    public class US_Papp_3776Steps
    {
        [Given(@"'(.*)' page")]
        public void GivenPage(string p0)
        {
            ////ScenarioContext.Current.Pending
            Console.Write("step1");
        }
        
        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
         //   //ScenarioContext.Current.Pending
        }
        
        [When(@"press '(.*)' button")]
        public void WhenPressButton(string p0)
        {
            ////ScenarioContext.Current.Pending
        }
        
        [When(@"press '(.*)'")]
        public void WhenPress(string p0)
        {
            ////ScenarioContext.Current.Pending
        }
        
        [When(@"Upload (.*) on '(.*)' page")]
        public void WhenUploadOnPage(string p0, string p1)
        {
            ////ScenarioContext.Current.Pending
        }
        
        [When(@"Insert (.*) on '(.*)' page")]
        public void WhenInsertOnPage(string p0, string p1)
        {
            ////ScenarioContext.Current.Pending
        }
        
        [When(@"set pin site care daily time")]
        public void WhenSetPinSiteCareDailyTime()
        {
            ////ScenarioContext.Current.Pending
        }
        
        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            //ScenarioContext.Current.Pending
        }
        
        //[Then(@"'(.*)' page is visualized")]
        //public void ThenPageIsVisualized(string p0)
        //{
        //    ////ScenarioContext.Current.Pending
        //}
        
        [Then(@"User profile is saved")]
        public void ThenUserProfileIsSaved()
        {
            ////ScenarioContext.Current.Pending
        }
        
        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            ////ScenarioContext.Current.Pending
        }
        [When(@"User tap on '(.*)' option")]
        public void WhenUserTapOnOption(string p0)
        {
            ScenarioContext.Current.Pending();
        }

    }
}
