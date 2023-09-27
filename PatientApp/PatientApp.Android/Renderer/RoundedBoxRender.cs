using System;
using Android.Graphics;
using Android.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PatientApp.Views.Controls;
using PatientApp.Droid.Renderer;
using Android.Content;

[assembly: ExportRenderer(typeof(RoundedBox), typeof(RoundedBoxRender))]
namespace PatientApp.Droid.Renderer
{
    class RoundedBoxRender : BoxRenderer
    {
        private float _cornerRadius;
        private RectF _bounds;
        private Path _path;

        public RoundedBoxRender(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);
            if (Element == null)
            {
                return;
            }
            var element = (RoundedBox)Element;
            _cornerRadius = TypedValue.ApplyDimension(ComplexUnitType.Dip, element.CornerRadius, Context.Resources.DisplayMetrics);
        }
        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            if (w != oldw || h != oldh)
            {
                _bounds = new RectF(0, 0, w, h);
            }
            var element = Element as RoundedBox;

            var borderToRound = element.BorderTypeToRound;
            float[] radii = new float[8];
            if (borderToRound == BorderType.AllCorners)
            {
                for (int i = 0; i < 8; i++)
                {
                    radii[i] = _cornerRadius;
                }
            }
            else if (borderToRound == BorderType.Left)
            {
                radii[0] = _cornerRadius;
                radii[1] = _cornerRadius;
                radii[6] = _cornerRadius;
                radii[7] = _cornerRadius;
            } 
            else //right
            {
                radii[2] = _cornerRadius;
                radii[3] = _cornerRadius;
                radii[4] = _cornerRadius;
                radii[5] = _cornerRadius;
            }
            _path = new Path();
            _path.Reset();
            _path.AddRoundRect(_bounds, radii, Path.Direction.Cw);
            _path.Close();
        }
        public override void Draw(Canvas canvas)
        {
            canvas.Save();
            canvas.ClipPath(_path);
            base.Draw(canvas);
            canvas.Restore();
        }
    }
}