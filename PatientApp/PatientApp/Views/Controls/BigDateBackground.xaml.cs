using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PatientApp.Utilities;

namespace PatientApp.Views.Controls
{
    /// <summary>
    ///  User control that display current datetime in big format 
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BigDateBackground : Grid
    {
        public DateTime DisplayDate
        {
            get { return (DateTime)GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }

        public static readonly BindableProperty DisplayDateProperty =
            BindableProperty.Create("DisplayDate", typeof(DateTime), typeof(BigDateBackground), DateTime.Today,
                BindingMode.TwoWay, null, (obj, oldValue, newValue) => { (obj as BigDateBackground).UpdateControls(); }
                );



        public BigDateBackground()
        {
            InitializeComponent();
            UpdateControls();
        }

        private void UpdateControls()
        {
            LblDay.Text = this.DisplayDate.ToString("dd");
            LblMonth.Text = this.DisplayDate.ToString("MMM").ToUpper();
            LblDayOfWeek.Text = this.DisplayDate.ToString("dddd").ToFirstLetterUpper();
        }

    }
}
