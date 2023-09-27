using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using PatientApp.Views.Controls;
using PatientApp.Droid;
using Android.Graphics.Drawables;
using Android.Content;

[assembly: ExportRendererAttribute(typeof(RoundedButton), typeof(RoundedButtonRenderer))]

namespace PatientApp.Droid
{
    public class RoundedButtonRenderer : ButtonRenderer
    {
        private GradientDrawable _normal, _pressed;

        public RoundedButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var button = e.NewElement;

                // Create a drawable for the button's normal state
                _normal = new Android.Graphics.Drawables.GradientDrawable();

                Android.Graphics.Color backGroundColor;
                if (button.BackgroundColor.R == -1.0 && button.BackgroundColor.G == -1.0 && button.BackgroundColor.B == -1.0)
                    backGroundColor = Android.Graphics.Color.ParseColor("#ff2c2e2f");
                else
                    backGroundColor = button.BackgroundColor.ToAndroid();

                _normal.SetColor(backGroundColor);
                _normal.SetCornerRadius(button.CornerRadius);

                // Create a drawable for the button's pressed state
                _pressed = new Android.Graphics.Drawables.GradientDrawable();
                _pressed.SetColor(backGroundColor);
                _pressed.SetCornerRadius(button.CornerRadius);

                // Add the drawables to a state list and assign the state list to the button
                var sld = new StateListDrawable();
                sld.AddState(new int[] { Android.Resource.Attribute.StatePressed }, _pressed);
                sld.AddState(new int[] { }, _normal);
                Control.SetBackgroundDrawable(sld);

                UpdatePadding();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var button = (Xamarin.Forms.Button)sender;

            if (e.PropertyName == nameof(RoundedButton.Padding))
            {
                UpdatePadding();
            }

            if (_normal != null && _pressed != null)
            {
                if (e.PropertyName == "BorderRadius")
                {
                    _normal.SetCornerRadius(button.CornerRadius);
                    _pressed.SetCornerRadius(button.CornerRadius);
                }
            }
        }

        private void UpdatePadding()
        {
            var element = this.Element as RoundedButton;
            if (element != null)
            {
                this.Control.SetPadding(
                    (int)element.Padding.Left,
                    (int)element.Padding.Top,
                    (int)element.Padding.Right,
                    (int)element.Padding.Bottom
                );
            }
        }

    }
}
