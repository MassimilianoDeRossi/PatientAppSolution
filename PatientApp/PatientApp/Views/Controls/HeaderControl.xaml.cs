using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PatientApp.Views.Controls
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class HeaderControl : Grid
  {
    public string Title
    {
      get { return (string)GetValue(TitleProperty); }
      set { SetValue(TitleProperty, value); }
    }

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create("Title", typeof(string), typeof(HeaderControl), "",
          BindingMode.OneWay, null, (obj, oldValue, newValue) => { (obj as HeaderControl).UpdateControls(); }
          ); 

    public HeaderControl()
    {
      InitializeComponent();
      UpdateControls();      
    }

    protected void UpdateControls()
    {
      LblTitle.Text = this.Title;
    }

  }
}
