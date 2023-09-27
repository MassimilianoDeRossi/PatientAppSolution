using Xamarin.Forms;

namespace PatientApp.Views.Controls
{
  public class Border : ContentView
  {

    public int CornerRadius
    {
      get { return (int)GetValue(CornerRadiusProperty); }
      set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create("CornerRadius", typeof(int), typeof(Border), 1);


    public static readonly BindableProperty StrokeProperty =
        BindableProperty.Create("Stroke", typeof(Color), typeof(Border), Color.Transparent);

    public Color Stroke
    {
      get { return (Color)GetValue(StrokeProperty); }
      set { SetValue(StrokeProperty, value); }
    }

    public static readonly BindableProperty StrokeThicknessProperty =
        BindableProperty.Create("StrokeThickness", typeof(Thickness), typeof(Border), default(Thickness));

    public Thickness StrokeThickness
    {
      get { return (Thickness)GetValue(StrokeThicknessProperty); }
      set { SetValue(StrokeThicknessProperty, value); }
    }

    public static readonly BindableProperty IsClippedToBorderProperty =
        BindableProperty.Create("IsClippedToBorder", typeof(bool), typeof(Border), default(bool));

    public bool IsClippedToBorder
    {
      get { return (bool)GetValue(IsClippedToBorderProperty); }
      set { SetValue(IsClippedToBorderProperty, value); }
    }

    // cross-platform way to take into account stroke thickness
    protected override void LayoutChildren(double x, double y, double width, double height)
    {
      x += StrokeThickness.Left;
      y += StrokeThickness.Top;

      width -= StrokeThickness.HorizontalThickness;
      height -= StrokeThickness.VerticalThickness;

      base.LayoutChildren(x, y, width, height);
    }
  }
}