using Xamarin.Forms;

namespace PatientApp.Views.Controls
{
    /// <summary>
    /// Custom Control based on BoxView with extra rounded options for the border radius (with the options of border types).
    /// </summary>
    public class RoundedBox : BoxView
    {
        public static BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(RoundedBox), default(int), defaultBindingMode: BindingMode.OneWay);

        public int CornerRadius
        {
            get { return (int) GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static BindableProperty BorderType = BindableProperty.Create(nameof(BorderTypeToRound), typeof(BorderType), typeof(RoundedBox), default(BorderType), defaultBindingMode: BindingMode.OneWay);

        public BorderType BorderTypeToRound
        {
            get { return (BorderType)GetValue(BorderType); }
            set { SetValue(BorderType, value); }
        }
    }

    public enum BorderType
    {
        AllCorners,
        Left,
        Right,
        
    }
}