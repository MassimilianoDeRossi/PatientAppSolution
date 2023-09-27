using System.ComponentModel;
using PatientApp.iOS.Renderer;
using PatientApp.Views.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RoundedButton), typeof(RoundedButtonRenderer))]
namespace PatientApp.iOS.Renderer
{
  public class RoundedButtonRenderer : ButtonRenderer
  {
    protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
    {
      base.OnElementChanged(e);

      if (Control != null && e.NewElement != null)
      {
        var button = (RoundedButton)e.NewElement;

        button.SizeChanged += (s, args) =>
        {
          button.BorderRadius = (int)(button.Height / 2.0); // (int)(button.BorderRadius / 2.0);
        };
      }
      UpdatePadding();
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);
      if (e.PropertyName == nameof(RoundedButton.Padding))
      {
        UpdatePadding();
      }
    }

    private void UpdatePadding()
    {
      var element = this.Element as RoundedButton;
      if (element != null)
      {
        this.Control.ContentEdgeInsets = new UIEdgeInsets(

            (int)element.Padding.Top,
            (int)element.Padding.Left,
            (int)element.Padding.Bottom,
            (int)element.Padding.Right
        );
      }
    }
  }
}
