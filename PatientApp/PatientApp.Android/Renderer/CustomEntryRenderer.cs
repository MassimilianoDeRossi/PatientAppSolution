
using Android.Graphics;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using PatientApp.Views.Controls;
using PatientApp.Droid;
using Android.Content;

[assembly: ExportRendererAttribute(typeof(Entry), typeof(CustomEntryRenderer))]

namespace PatientApp.Droid
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetPadding(10, 0, 10, 0);
            }
        }
    }
}
