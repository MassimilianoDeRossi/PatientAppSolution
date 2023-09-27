using Xamarin.Forms;

namespace PatientApp.Views.Controls
{
  public class RoundedButton : Button
  {

      public Color DisabledColor { get; set; }

    public static BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(RoundedButton), default(Thickness), defaultBindingMode: BindingMode.OneWay);



    public Thickness Padding
    {
      get { return (Thickness)GetValue(PaddingProperty); }
      set { SetValue(PaddingProperty, value); }
    }
  }
}