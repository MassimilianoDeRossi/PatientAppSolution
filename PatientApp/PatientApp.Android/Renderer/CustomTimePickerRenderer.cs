using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using PatientApp.Views.Controls;
using PatientApp.Droid;
using Android.Content;

[assembly: ExportRendererAttribute(typeof(TimePicker), typeof(CustomTimePickerRenderer))]

namespace PatientApp.Droid
{
    public class CustomTimePickerRenderer : TimePickerRenderer
    {
        public CustomTimePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;
 
                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                Control.LayoutParameters = layoutParams;
                Control.SetPadding(0, 0, 0, 0);
                SetPadding(0, 0, 0, 0);
            }
        }
    }
}
