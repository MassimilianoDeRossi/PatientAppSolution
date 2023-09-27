using PatientApp.iOS.Renderer;
using PatientApp.Views.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomTimePicker), typeof(CustomTimePickerRenderer))]
namespace PatientApp.iOS.Renderer
{
  public class CustomTimePickerRenderer : TimePickerRenderer
  {
    protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
    {
      base.OnElementChanged(e);

      if (e.NewElement != null && Control != null)
      {
        var picker = e.NewElement as CustomTimePicker;
        switch (picker.HorizontalTextAlignment)
        {
          case TextAlignment.Start:
            Control.TextAlignment = UITextAlignment.Left;
            break;
          case TextAlignment.Center:
            Control.TextAlignment = UITextAlignment.Center;
            break;
          case TextAlignment.End:
            Control.TextAlignment = UITextAlignment.Right;
            break;
        }
      }
    }
  }
}
