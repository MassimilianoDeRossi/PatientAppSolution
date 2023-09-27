using System;
using Foundation;
using PatientApp.iOS.Renderer;
using PatientApp.Views.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomListView), typeof(CustomListViewRenderer))]
namespace PatientApp.iOS.Renderer
{   
    /// <summary>
    /// Custom Rendering Override on ListView control on IOS 
    /// </summary>
    public class CustomListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SeparatorInset = UIEdgeInsets.Zero;
            }
        }
    }
}