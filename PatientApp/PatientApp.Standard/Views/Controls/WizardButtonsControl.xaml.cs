using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PatientApp.Views.Controls
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class WizardButtonsControl : Grid
  {
    public string PrevButtonText
    {
      get { return (string)GetValue(PrevButtonTextProperty); }
      set { SetValue(PrevButtonTextProperty, value); }
    }

    public static readonly BindableProperty PrevButtonTextProperty =
        BindableProperty.Create("PrevButtonText", typeof(string), typeof(WizardButtonsControl), "Back",
          BindingMode.OneWay, null, (obj, oldValue, newValue) => { (obj as WizardButtonsControl).UpdateControls(); }
          );

    public string NextButtonText
    {
      get { return (string)GetValue(NextButtonTextProperty); }
      set { SetValue(NextButtonTextProperty, value); }
    }

    public static readonly BindableProperty NextButtonTextProperty =
        BindableProperty.Create("NextButtonText", typeof(string), typeof(WizardButtonsControl), "Next",
          BindingMode.OneWay, null, (obj, oldValue, newValue) => { (obj as WizardButtonsControl).UpdateControls(); }
          );

    public WizardButtonsControl()
    {
      InitializeComponent();      
    }

    protected void UpdateControls()
    {
      LblPrev.Text = this.PrevButtonText;
      LblNext.Text = this.NextButtonText;
    }

  }
}
