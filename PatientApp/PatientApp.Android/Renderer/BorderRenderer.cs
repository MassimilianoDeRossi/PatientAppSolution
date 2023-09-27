
using Android.Graphics;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using PatientApp.Views.Controls;
using PatientApp.Droid;

[assembly: ExportRendererAttribute(typeof(Border), typeof(BorderRenderer))]

namespace PatientApp.Droid
{
  public class BorderRenderer : VisualElementRenderer<Border>
  {
    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);
      //HandlePropertyChanged (sender, e);
      BorderRendererVisual.UpdateBackground(Element, this.ViewGroup);
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Border> e)
    {
      base.OnElementChanged(e);
      BorderRendererVisual.UpdateBackground(Element, this.ViewGroup);
    }

    /*void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Content")
			{
				BorderRendererVisual.UpdateBackground (Element, this.ViewGroup);
			}
		}*/

    protected override void DispatchDraw(Canvas canvas)
    {
      if (Element.IsClippedToBorder)
      {
        canvas.Save(SaveFlags.Clip);
        BorderRendererVisual.SetClipPath(this, canvas);
        base.DispatchDraw(canvas);
        canvas.Restore();
      }
      else
      {
        base.DispatchDraw(canvas);
      }
    }
  }
}
