using System;
using PatientApp.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(CustomTabbedPageRenderer))]
namespace PatientApp.iOS.Renderer
{
    public class CustomTabbedPageRenderer : TabbedRenderer
    {
      public CustomTabbedPageRenderer()
      {
        TabBar.TintColor = UIColor.FromRGB(Convert.ToSingle(App.ToolbarTintColor.R), Convert.ToSingle(App.ToolbarTintColor.G), Convert.ToSingle(App.ToolbarTintColor.B)); ;
        TabBar.BarTintColor = UIColor.FromRGB(Convert.ToSingle(App.ToolbarColor.R), Convert.ToSingle(App.ToolbarColor.G), Convert.ToSingle(App.ToolbarColor.B));
        TabBar.SelectedImageTintColor = UIColor.FromRGB(Convert.ToSingle(App.ToolbarSelectedImageTintColor.R), Convert.ToSingle(App.ToolbarSelectedImageTintColor.G), Convert.ToSingle(App.ToolbarSelectedImageTintColor.B));
    }
  }
}
