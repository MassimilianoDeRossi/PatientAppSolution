using NUnit.Framework;
using Xamarin.UITest;
using TechTalk.SpecFlow;


namespace PatientApp.UITest
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class FeatureBase
    {
        protected static IApp app;
        protected Platform platform;

        public FeatureBase(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            FeatureContext.Current.Remove("App");
            app = AppInitializer.StartApp(platform);
            FeatureContext.Current.Add("App", app);
        }
    }

}
