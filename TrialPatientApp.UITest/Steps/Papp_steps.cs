using NUnit.Framework;
using System;
using System.Data;
using TechTalk.SpecFlow;
using Xamarin.UITest;
using System.Linq;
using System.IO;
using Xamarin.UITest.Queries;

namespace PatientApp.UITest.Steps
{ 
    [Binding]
    public class Papp_steps
    {
        /*
        protected static IApp app;
        protected Platform platform;

        public void FeatureBase(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
            FeatureContext.Current.Add("App", app);
        }


        public Papp_steps()
        {
            app = FeatureContext.Current.Get<IApp>("App");
        }
        public partial class myownclass : FeatureBase
        {
            public myownclass(Platform platform) : base(platform)
            {
            }
        }

        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .ApkFile(@"C:\Users\dmanzati\AppData\Local\Xamarin\Mono for Android\Archives\2017-07-27\PatientApp.Android 7-27-17 11.00 AM.apkarchive\com.orthofix.myhexplan.apk")
                    .StartApp();
            }

            return ConfigureApp
                .iOS
                .StartApp();
        }
        */

        [Given(@"User navigate to '(.*)' page")]
        public void GivenUserNavigateToPage(string p0)
        {
            //app.Back();
            Console.Write("Background setted");
            //ScenarioContext.Current.Pending();
        }

        [When(@"User tap on '(.*)' option")]
        public void WhenUserTapOnOption(string p0)
        {
            Console.Write("tapped on: " + p0);
            //ScenarioContext.Current.Pending();
        }

        [When(@"User navigate to '(.*)' page")]
        public void WhenUserNavigateToPage(string p0)
        {
            Console.Write("actual page: " + p0);
            //ScenarioContext.Current.Pending();
        }

        [When(@"User enter nickname with '(.*)'")]
        public void WhenUserEnterNicknameWith(string p0)
        {
            Console.Write("nickname inserted: " + p0);
            //ScenarioContext.Current.Pending();
        }

        [When(@"User upload photo with '(.*)'")]
        public void WhenUserUploadPhotoWith(string p0)
        {
            Console.Write("uploaded photo: " + p0);
            //ScenarioContext.Current.Pending();
        }

        [When(@"User set daily time with '(.*)'")]
        public void WhenUserSetDailyTimeWith(string p0)
        {
            Console.Write("Daily time selected: " + p0);
            //ScenarioContext.Current.Pending();
        }

        [Then(@"'(.*)' page is visualized")]
        public void ThenPageIsVisualized(string p0)
        {
            Console.Write("visualized page: " + p0);
            //ScenarioContext.Current.Pending();
        }

        [Then(@"The '(.*)' should be '(.*)'")]
        public void ThenTheShouldBe(string p0, string p1)
        {
            Console.Write("field: " + p0 + " contains: " + p1);
            //ScenarioContext.Current.Pending();
        }

        [Then(@"The nickname should be '(.*)'")]
        public void ThenTheNicknameShouldBe(string p0)
        {
            Console.Write("nickname should be: " + p0);
            //ScenarioContext.Current.Pending();
        }

        [Then(@"The photo should be '(.*)'")]
        public void ThenThePhotoShouldBe(string p0)
        {
            Console.Write("photo should be: " + p0);
            //ScenarioContext.Current.Pending();
        }

        [Then(@"Anonymous Profile settings are successfully saved")]
        public void ThenAnonymousProfileSettingsAreSuccessfullySaved(Table table)
        { 
            var dataTable = Utils.TableExtensions.ToDataTable(table);
            var settings = new ProfSettings();
            WhenUserNavigateToPage("Welcome User Anonymous");
            WhenUserNavigateToPage("User Profile");
            foreach (DataRow row in dataTable.Rows)
            {
                Console.Write(settings.nickname = row.ItemArray[0].ToString());
                Console.Write(settings.photo = row.ItemArray[1].ToString());
                Console.Write(settings.time = row.ItemArray[2].ToString());
                ThenTheShouldBe("nickname", row.ItemArray[0].ToString());
                ThenTheShouldBe("photo", row.ItemArray[1].ToString());
            }
            WhenUserNavigateToPage("Pin Site Care");
            foreach (DataRow row in dataTable.Rows)
            {
                Console.Write(settings.time = row.ItemArray[2].ToString());
                ThenTheShouldBe("set daily time", settings.time);
            }
            //ScenarioContext.Current.Pending();

        }

        private class ProfSettings
        {
            public string nickname;
            public string photo;
            public string time;
        }

        [Then(@"'(.*)' element is visualized")]
        public void ThenElementIsVisualized(string p0)
        {
            //ScenarioContext.Current.Pending();
        }

        [Then(@"'(.*)' is correct")]
        public void ThenIsCorrect(string p0)
        {
            //ScenarioContext.Current.Pending();
        }

    }
}
