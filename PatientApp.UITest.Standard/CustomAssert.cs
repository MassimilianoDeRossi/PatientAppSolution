using NUnit.Framework;
using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace PatientApp.UITest
{
    /// <summary>
    /// It contains a colection of static methods that implement the mostly used assert 
    /// </summary>
    public static class CustomAssert
    {
        private static Platform _platform;
        private static IApp _app;

        /// <summary>
        /// Initialize utility data. Used only when test starts
        /// </summary>
        public static void Init(Platform platform, IApp app)
        {
            _platform = platform;
            _app = app;
        }

        /// <summary>
        /// Check if an element current status is visible and the element is actually visible on view and viceversa
        /// </summary>
        public static void IsVisibilityCorrect(string marked, string currentStatus, string visibleStatus, string hideStatus)
        {
            if (!Utils.IsVisible(marked) && currentStatus == visibleStatus)
            {
                Assert.Fail(marked + " not founded!");
            }
            else if (Utils.IsVisible(marked) && currentStatus == hideStatus)
            {
                Assert.Fail(marked + " founded!");
            }
        }

        /// <summary>
        /// Check if an element current status is visible and the element is actually visible on view and viceversa
        /// </summary>
        public static void IsVisibilityCorrect(Func<AppQuery, AppQuery> query, string currentStatus, string visibleStatus, string hideStatus, string elementName)
        {
            if (!Utils.IsVisible(query) && currentStatus == visibleStatus)
            {
                Assert.Fail(elementName + " not founded!");
            }
            else if (Utils.IsVisible(query) && currentStatus == hideStatus)
            {
                Assert.Fail(elementName + " founded!");
            }
        }
    }
}
