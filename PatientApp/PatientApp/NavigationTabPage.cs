using Xamarin.Forms;

namespace PatientApp
{
  public class NavigationTabPage : CustomTabbedPage
  {
    public NavigationTabPage()
    {
      BackgroundColor = (Color)App.Current.Resources["DefaultBackgroundColor"];
      //BarBackgroundColor = Color.Red;
      //BarTextColor = Color.Yellow;      
      

    }

    public NavigationPage AddNavigationPage(Page page)
    {
      var navigationPage = new NavigationPage(page)
      {
        Title = page.Title
      };
      navigationPage.Icon = page.Icon.File;

      this.Children.Add(navigationPage);
      return navigationPage;
    }

  }
}
