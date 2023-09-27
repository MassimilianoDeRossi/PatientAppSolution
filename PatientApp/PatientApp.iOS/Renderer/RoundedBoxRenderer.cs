using System.ComponentModel;
using CoreAnimation;
using CoreGraphics;
using PatientApp.iOS.Renderer;
using PatientApp.Views.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RoundedBox), typeof(RoundedBoxRenderer))]
namespace PatientApp.iOS.Renderer
{
    /// <summary>
    /// Render use to round corner of standard box view
    /// </summary>
    public class RoundedBoxRenderer : BoxRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            BackgroundColor = Element.Color.ToUIColor();
        }

        public override void Draw(CGRect rect)
        {
            var element = Element as RoundedBox;

            var borderToRound = element.BorderTypeToRound == BorderType.AllCorners ? UIRectCorner.AllCorners: element.BorderTypeToRound == BorderType.Left? (UIRectCorner.TopLeft | UIRectCorner.BottomLeft): (UIRectCorner.TopRight | UIRectCorner.BottomRight);

            var mPath = UIBezierPath.FromRoundedRect(Layer.Bounds, (borderToRound), new CGSize(width: element.CornerRadius, height: element.CornerRadius));
            var maskLayer = new CAShapeLayer
            {
                Frame = Layer.Bounds,
                Path = mPath.CGPath
            };
            Layer.Mask = maskLayer;
        }

    }
}