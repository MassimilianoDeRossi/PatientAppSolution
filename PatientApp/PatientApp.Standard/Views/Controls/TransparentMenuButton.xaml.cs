using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PatientApp.Views.Controls
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class TransparentMenuButton : Grid
  {  
    public Command Command
    {
      get { return (Command)GetValue(CommandProperty); }
      set { SetValue(CommandProperty, value); }
    }

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create("Command", typeof(Command), typeof(TransparentMenuButton), null,
          BindingMode.TwoWay, null, (obj, oldValue, newValue) => { (obj as TransparentMenuButton).UpdateControls(); }
          );

    public object CommandParameter
    {
      get { return (object)GetValue(CommandParameterProperty); }
      set { SetValue(CommandParameterProperty, value); }
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create("CommandParameter", typeof(object), typeof(TransparentMenuButton), null,
          BindingMode.TwoWay, null, (obj, oldValue, newValue) => { (obj as TransparentMenuButton).UpdateControls(); }
          );

    public TransparentMenuButton()
    {
      InitializeComponent();

      UpdateControls();     
    }

    protected void UpdateControls()
    {
      TapGesture.Command = this.IsEnabled ? this.Command : null;
      TapGesture.CommandParameter = this.CommandParameter;      
    }

   
  }
}
