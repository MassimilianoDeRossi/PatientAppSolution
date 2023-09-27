using Xamarin.Forms;

namespace PatientApp
{
  public class MainNavigationPage : NavigationPage
  {
    public MainNavigationPage()
    {
      BackgroundColor = (Color)App.Current.Resources["DefaultBackgroundColor"];
      BarBackgroundColor = (Color)App.Current.Resources["DefaultBackgroundColor"]; 
      BarTextColor = Color.White;
    }

    public MainNavigationPage(Page page) : base(page)
    {
      BackgroundColor = (Color)App.Current.Resources["DefaultBackgroundColor"];
      BarBackgroundColor = (Color)App.Current.Resources["DefaultBackgroundColor"];
      BarTextColor = Color.White;
    }
  }
}
