using System;
using Xamarin.Forms;

namespace PatientApp.Views.Controls
{
    public class IntSlider : Slider
    {
        public int StepValue { get; set; } = 1;

        public IntSlider()
        {
            this.ValueChanged += IntSlider_ValueChanged;
            if (Device.RuntimePlatform == Device.Android)
            {

                // BUG: https://bugzilla.xamarin.com/show_bug.cgi?id=54887
                //      Should be fixed on 2.6.0-pre1
                // It simulate a click (the first one that isn't intercepted)

                Value = 5;
                Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
                {
                    if (Value == 0)
                    {
                        Value = 0.1;
                    }
                    return false;
                });
            }
        }

        /// <summary>
        /// Round current value to integer on each value changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IntSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newValue = Math.Round(e.NewValue / StepValue) * StepValue;
            if (this.Value != newValue)
                this.Value = newValue;
        }
    }
}
