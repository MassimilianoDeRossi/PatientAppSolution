using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Share;
using Plugin.Share.Abstractions;

using Xamarin.Forms;

using PatientApp.DataModel.SqlEntities;
using PatientApp.Interfaces;

namespace PatientApp.ViewModels
{
  /// <summary>
  /// Class that handle Shopping List Patients Item.
  /// </summary>
  public class ShoppingListViewModel : BaseViewModel
  {
    public ObservableCollection<ViewShoppingItem> ShoppingList { get; set; }

    public ViewShoppingItem SelectedViewShoppingItem { get; set; }

    public Command ClickOnInfoForCleaningSolutionCommand { get; set; }

    public Command ShareCheckedListCommand { get; set; }

    public Command UserClosePopupCommand { get; set; }

    public Command ItemTappedCommand { get; set; }

    public ShoppingListViewModel(ILocalDatabaseService dbService) : base(dbService, null, null)
    {
      ClickOnInfoForCleaningSolutionCommand = new Command(ClickOnInfoForCleaningSolutionCommandExecute);
      UserClosePopupCommand = new Command(UserClosePopupCommandExecute);
      ItemTappedCommand = new Command(ItemTappedCommandExecute);
      ShareCheckedListCommand = new Command(ShareCheckedListCommandExecute);

      Task.Run(async () =>
      {
        ShoppingList = await GetShoppingList();
      });
      
    }

    private async void ShareCheckedListCommandExecute()
    {
      var shoppingItemTextList = Resources.PatientApp.ShoppintListSharingPrefix + "\n";

      //;List checked 
      foreach (var shoppingItem in ShoppingList.Where(x => x.IsChecked))
      {
        //shoppingItemTextList += "- " + shoppingItem.Description + "\n";
        shoppingItemTextList += "- " + Localization.LocalizationManager.GetText(shoppingItem.Description) + "\n";
      }

      var message = new ShareMessage()
      {
        Text = shoppingItemTextList,


      };

      // Share Shopping List selected.
      await CrossShare.Current.Share(message);
    }

    private void ItemTappedCommandExecute()
    {
      SelectedViewShoppingItem?.ToggleChecked();
#if ENABLE_TEST_CLOUD
            if (App.TestModel.TestModeOn && SelectedViewShoppingItem != null)
            {
                for (int i = 0; i < ShoppingList.Count; i++) //used for to avoid problem with missing resx file
                {
                    //if (SelectedViewShoppingItem.Description == ShoppingList[i].Description)
                    //{
                    App.TestModel.ShoppingListCheckBoxStatus[i] = ShoppingList[i].IsChecked;
                    //    break;
                    //}
                }
            }
#endif
      //Save Current Selected/Unselect shopping element converting it before in the real db entity.
      var shoppingItemToUpdate = new ShoppingItem()
      {
        Description = SelectedViewShoppingItem.Description,
        IsChecked = SelectedViewShoppingItem.IsChecked,
        Id = SelectedViewShoppingItem.Id
      };
      _dbService.SaveShoppingItem(shoppingItemToUpdate);
    }

    private void ClickOnInfoForCleaningSolutionCommandExecute(object sender)
    {
      App.NavigationController.NavigateTo(NavigationController.CLEANING_SOLUTION_INFO_POPUP_PAGE, true);
    }

    private async void UserClosePopupCommandExecute(object obj)
    {
      await App.NavigationController.ClosePopupAsync();
    }

    private async Task<ObservableCollection<ViewShoppingItem>> GetShoppingList()
    {
      var shoppingList = await _dbService.GetShoppingItems();
      var viewShoppingList = new ObservableCollection<ViewShoppingItem>();
      foreach (var shoppingItem in shoppingList)
      {
        viewShoppingList.Add(new ViewShoppingItem()
        {
          Description = shoppingItem.Description,
          Id = shoppingItem.Id,
          IsChecked = shoppingItem.IsChecked
        });
      }

      return viewShoppingList;
    }
  }
}
