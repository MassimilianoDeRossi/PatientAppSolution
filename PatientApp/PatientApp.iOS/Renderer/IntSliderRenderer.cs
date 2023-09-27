using PatientApp.iOS.Renderer;
using PatientApp.Views.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(IntSlider), typeof(IntSliderRenderer))]
namespace PatientApp.iOS.Renderer
{
  public class IntSliderRenderer : SliderRenderer
  {
    
    protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
    {
      base.OnElementChanged(e);

      if (e.NewElement != null && Control != null)
      {
        Control.MinimumTrackTintColor = UIColor.FromRGBA(0, 0, 0, 0);
        Control.MaximumTrackTintColor = UIColor.FromRGBA(0, 0, 0, 0);
      }
    }
  }
}
