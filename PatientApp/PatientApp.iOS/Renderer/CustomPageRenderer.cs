using System;
using System.Reflection;
using Foundation;
using PatientApp.iOS.Renderer;
using PatientApp.Views.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Page), typeof(CustomPageRenderer))]
namespace PatientApp.iOS.Renderer

{   /// <summary>
    /// Custom Renderer on page to fix PageAppearing event lifecycle (on iOS it is fired after view is shown)
    /// </summary>
    public class CustomPageRenderer : PageRenderer
    {
        private static readonly FieldInfo appearedField = typeof(PageRenderer).GetField("_appeared", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo disposedField = typeof(PageRenderer).GetField("_disposed", BindingFlags.NonPublic | BindingFlags.Instance);

        private IPageController PageController => this.Element as IPageController;

        private bool Appeared
        {
            get { return (bool)appearedField.GetValue(this); }
            set { appearedField.SetValue(this, value); }
        }

        private bool Disposed
        {
            get { return (bool)disposedField.GetValue(this); }
            set { disposedField.SetValue(this, value); }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (this.Appeared || this.Disposed)
            {
                return;
            }

            // by setting this to true, we also ensure that PageRenderer does not invoke SendAppearing a second time when ViewDidAppear fires
            this.Appeared = true;
            PageController.SendAppearing();
        }    
    }
}