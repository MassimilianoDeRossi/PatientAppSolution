using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PatientApp.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LabelDatePickerControl : Grid
    {
        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public static readonly BindableProperty SelectedDateProperty =
            BindableProperty.Create("SelectedDate", typeof(DateTime), typeof(LabelDatePickerControl), System.DateTime.Today,
              BindingMode.TwoWay, null, (obj, oldValue, newValue) => { (obj as LabelDatePickerControl).UpdateValues(); }
              );


        public string DisplayFormat
        {
            get { return (string)GetValue(DisplayFormatProperty); }
            set { SetValue(DisplayFormatProperty, value); }
        }

        public static readonly BindableProperty DisplayFormatProperty =
            BindableProperty.Create("DisplayFormat", typeof(string), typeof(LabelDatePickerControl), null,
              BindingMode.OneWay, null, (obj, oldValue, newValue) => { (obj as LabelDatePickerControl).UpdateValues(); }
              );


        public DateTime MinimumDate
        {
            get { return (DateTime)GetValue(MinimumDateProperty); }
            set { SetValue(MinimumDateProperty, value); }
        }

        public static readonly BindableProperty MinimumDateProperty =
            BindableProperty.Create("MinimumDate", typeof(DateTime), typeof(LabelDatePickerControl), DateTime.MinValue,
              BindingMode.OneWay, null, (obj, oldValue, newValue) => { (obj as LabelDatePickerControl).UpdateValues(); }
              );

        public DateTime MaximumDate
        {
            get { return (DateTime)GetValue(MaximumDateProperty); }
            set { SetValue(MaximumDateProperty, value); }
        }

        public static readonly BindableProperty MaximumDateProperty =
            BindableProperty.Create("MaximumDate", typeof(DateTime), typeof(LabelDatePickerControl), DateTime.MaxValue,
              BindingMode.OneWay, null, (obj, oldValue, newValue) => { (obj as LabelDatePickerControl).UpdateValues(); }
              );


        public string MaximumDateDisplayText
        {
            get { return (string)GetValue(MaximumDateDisplayTextProperty); }
            set { SetValue(MaximumDateDisplayTextProperty, value); }
        }

        public static readonly BindableProperty MaximumDateDisplayTextProperty =
            BindableProperty.Create("MaximumDateDisplayText", typeof(string), typeof(LabelDatePickerControl), null,
              BindingMode.OneWay, null, (obj, oldValue, newValue) => { (obj as LabelDatePickerControl).UpdateValues(); }
              );

        public LabelDatePickerControl()
        {
            InitializeComponent();
            dpInternal.DateSelected += DpInternal_DateSelected;
            UpdateValues();
            UpdateLabel();
        }

        private void DpInternal_DateSelected(object sender, DateChangedEventArgs e)
        {
            this.SelectedDate = e.NewDate;
            UpdateLabel();
        }

        private void UpdateValues()
        {
            dpInternal.MinimumDate = this.MinimumDate;
            dpInternal.MaximumDate = this.MaximumDate;
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            if (!string.IsNullOrEmpty(this.MaximumDateDisplayText) && this.SelectedDate.Date == this.MaximumDate.Date)
            {
                btnOpenPicker.Text = this.MaximumDateDisplayText;
            }
            else
            {
                if (!string.IsNullOrEmpty(this.DisplayFormat))
                    btnOpenPicker.Text = this.SelectedDate.Date.ToString(this.DisplayFormat);
                else
                    btnOpenPicker.Text = this.SelectedDate.Date.ToString();
            }
        }

        private void btnOpenPicker_Clicked(object sender, EventArgs e)
        {
            dpInternal.Focus();
        }
    }
}
