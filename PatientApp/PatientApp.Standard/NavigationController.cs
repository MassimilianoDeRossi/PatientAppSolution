using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;

using PatientApp.Views;
using PatientApp.Services;

namespace PatientApp
{
  /// <summary>
  /// Scoped Navigation Manager
  /// A scope for each tab of the main tabbed page
  /// </summary>
  public class NavigationController
  {
    private bool isNavigating = false;

    // Navigation targets constant names
    public const string HOME_PAGE = "HOMEPAGE";
    public const string USER_PROFILE = "USER_PROFILE";
    public const string PIN_SITE_CARE_TIME = "PIN_SITE_CARE_TIME";
    public const string INSIGHT_MESSAGES = "INSIGHT_MESSAGES";
    public const string PERSONAL_GOAL = "PERSONAL_GOAL";
    public const string PRIVACY_AND_LEGAL_TERMS = "PRIVACY_AND_LEGAL_TERMS";
    public const string CREDITS = "CREDITS";
    public const string SETTINGS = "SETTINGS";
    public const string PRESCRIPTION_START = "PRESCRIPTION_START";
    public const string QRCODE_SCAN = "QRCODE_SCAN";
    public const string ALL_MY_DAILY_TASKS = "ALL_MY_DAILY_TASKS";
    public const string HOW_DO_YOU_FEEL = "HOW_DO_YOU_FEEL";
    public const string PRESCRIPTION_HELP_POPUP = "PRESCRIPTION_HELP_POPUP";
    public const string WIZARD_USER_SETTINGS = "WIZARD_USER_SETTINGS";
    public const string SELECT_MOOD_POPUP_PAGE = "SELECT_MOOD_POPUP_PAGE";
    public const string CLEANING_SOLUTION_INFO_POPUP_PAGE = "CLEANING_SOLUTION_INFO_POPUP_PAGE";
    public const string MOTIVATIONAL_MESSAGE_POPUP_PAGE = "MOTIVATIONAL_MESSAGE_POPUP_PAGE";
    public const string SHOPPING_LIST = "SHOPPING_LIST";
    public const string GET_AN_INFECTION_SURVEY = "GET_AN_INFECTION_SURVEY";
    public const string PIN_SITE_CARE = "PIN_SITE_CARE";
    public const string PIN_SITE_CARE_VIDEO = "PIN_SITE_CARE_VIDEO";
    public const string STRUT_ADJUSTMENT_VIDEO = "STRUT_ADJUSTMENT_VIDEO";
    public const string MY_DIARY = "MY_DIARY";
    public const string STRUT_ADJUSTMENT_RECAP = "STRUT_ADJUSTMENT_RECAP";
    public const string STRUT_ADJUSTMENT_SAMPLE = "STRUT_ADJUSTMENT_SAMPLE";
    public const string STRUT_ADJUSTMENT_DETAIL = "STRUT_ADJUSTMENT_DETAIL";
    public const string STRUT_ADJUSTMENT_WIZARD = "STRUT_ADJUSTMENT_WIZARD";
    public const string STRUT_ADJUSTMENT_WIZARDCOMPLETED = "STRUT_ADJUSTMENT_WIZARDCOMPLETED";
    public const string MY_PRESCRIPTIONS = "MY_PRESCRIPTIONS";
    public const string SUPPORT = "SUPPORT";
    public const string LANGUAGE = "LANGUAGE";
    public const string TIME_LAPSE_ALBUMS = "TIME_LAPSE_ALBUMS";
    public const string TIME_LAPSE_IMAGES = "TIME_LAPSE_IMAGES";
    public const string TIME_LAPSE_ZOOM = "TIME_LAPSE_ZOOM";
    public const string TIME_LAPSE_VIDEO = "TIME_LAPSE_VIDEO";
    public const string SIGNS_OF_INFECTION = "SIGNS_OF_INFECTION";
    public const string SURGEON_CONTACT = "SURGEON_CONTACT";
    public const string TEST = "TEST";

    /// <summary>
    /// The NavigationPage used to manage the app navigation
    /// </summary>
    public static MainNavigationPage NavigationPage { get; protected set; }

    /// <summary>
    /// A flag used to disable the back navigation 
    /// </summary>
    public bool CanNavigateBack { get; set; } = true;

    /// <summary>
    /// Controller of app navigation system
    /// </summary>
    public NavigationController()
    {
      MessagingCenter.Instance.Subscribe<BaseContentPage>(this, Messaging.Messages.ANDROID_BACKBUTTON_PRESSED, (msg) => { NavigateBack(); });
    }

    /// <summary>
    /// Create and initialize the navigation system
    /// </summary>
    public async void InitializeNavigation()
    {
      if (NavigationPage != null && NavigationPage.Navigation.NavigationStack.Any())
      {
        await PopToRootAsync();
        NavigationPage = null;
      }

      var settings = Settings.AppSettings.Instance;

      // Restore user selected language or set english as default
      Localization.LocalizationManager.SetCurrentLanguage(settings.CurrentLanguageCode);

#if ENABLE_TEST_CLOUD
            NavigationPage = new MainNavigationPage(new HomePage());
#else
      // Check Device integrity
      var safe = DependencyService.Get<IDeviceIntegrityService>().IsSafe();
      if (safe)
      {
        // User has accepted legal terms for this specific build version?
        if (settings.AreLegalTermsAccepted && settings.LegalTermsAcceptedVersion.Equals(App.RuntimVersion))
          NavigationPage = new MainNavigationPage(new HomePage());
        else
          NavigationPage = new MainNavigationPage(new LegalTermsPage());
      }
      else
      {
        // TODO: PREDISPORRE TESTI LOCALIZED
        await App.Current.MainPage.DisplayAlert(Localization.LocalizationManager.GetText("AlertDeviceIntegrityCheckFailedTitle"), Localization.LocalizationManager.GetText("AlertDeviceIntegrityCheckFailedMessage"), "OK");
        App.Current.Quit();        
      }
#endif
      App.SetMainPage(NavigationPage);
    }

    /// <summary>
    /// Move navigation to a named target page
    /// </summary>
    /// <param name="target">The string identifying the target</param>
    /// <param name="popup">Navigation in popup (modal) mode</param>
    public void NavigateTo(string target, bool popup = false)
    {
      Device.BeginInvokeOnMainThread(async () =>
      {
        await Navigate(target, popup);
      });
    }

    /// <summary>
    /// Navigate to already instantiated page 
    /// </summary>
    /// <param name="page">The instantiated page</param>
    /// <param name="doPush">If True push the page in navigation stack. If False replace the main page</param>
    /// <param name="popup">Navigation in popup (modal) mode</param>
    public void NavigateToPage(ContentPage page, bool doPush = true, bool popup = false)
    {
      Device.BeginInvokeOnMainThread(async () =>
      {
        await Navigate(page, doPush, popup);
      });
    }

    /// <summary>
    /// Navigate one step back in the navigation stack (close the popup is current displayed page is a popup)
    /// </summary>
    public void NavigateBack()
    {
      if (isNavigating)
        return;

      if (!CanNavigateBack)
        return;

      isNavigating = true;
      Device.BeginInvokeOnMainThread(async () =>
      {
        if (NavigationPage.Navigation.ModalStack.Any())
        {
          await NavigationPage.Navigation.PopModalAsync(true);
        }
        else if (NavigationPage.Navigation.NavigationStack.Any())
        {
          await NavigationPage.PopAsync(true);
        }
        isNavigating = false;
      });
    }

    /// <summary>
    /// Navigate back to the home page 
    /// </summary>
    public void NavigateToHome()
    {
      NavigateBackToPageOrDefault(typeof(HomePage), HOME_PAGE);
    }

    /// <summary>
    /// Close a popup page
    /// </summary>
    /// <returns></returns>
    public async Task ClosePopupAsync()
    {
      await App.Current.MainPage.Navigation.PopPopupAsync();
    }

    /// <summary>
    /// Perform default navigation to a named target page
    /// </summary>
    /// <param name="target"></param>
    /// <param name="popup"></param>
    /// <returns></returns>
    private async Task Navigate(string target, bool popup)
    {
      if (isNavigating)
        return;

      isNavigating = true;

      GC.Collect();

      var doPush = false;
      var page = default(Page);
      var langCode = Localization.LocalizationManager.GetCurrentLanguageCode();

      // Instantiate the page related to the target
      switch (target)
      {

        case HOME_PAGE:
          doPush = false;
          page = PagesFactory.GetPage<HomePage>(langCode);
          break;
        case USER_PROFILE:
          doPush = true;
          page = PagesFactory.GetPage<ProfilePage>(langCode);
          break;
        case SURGEON_CONTACT:
          doPush = true;
          page = PagesFactory.GetPage<SurgeonContactPage>(langCode);
          break;
        case SETTINGS:
          doPush = true;
          page = PagesFactory.GetPage<UserSettingsPage>(langCode);
          break;
        case SUPPORT:
          doPush = true;
          page = PagesFactory.GetPage<SupportPage>(langCode, false);
          break;
        case LANGUAGE:
          doPush = true;
          page = PagesFactory.GetPage<LanguagePage>(langCode);
          break;
        case TIME_LAPSE_ALBUMS:
          doPush = true;
          page = PagesFactory.GetPage<TimeLapseAlbumsPage>(langCode);
          break;
        case TIME_LAPSE_IMAGES:
          doPush = true;
          page = PagesFactory.GetPage<TimeLapseImagesPage>(langCode);
          break;
        case TIME_LAPSE_ZOOM:
          doPush = true;
          page = new TimeLapseImageZoomPopup();
          break;
        case TIME_LAPSE_VIDEO:
          doPush = true;
          page = PagesFactory.GetPage<TimeLapseVideoPage>(langCode);
          break;
        case TEST:
          doPush = true;
          page = PagesFactory.GetPage<TestPage>(langCode);
          break;
        case STRUT_ADJUSTMENT_RECAP:
          doPush = true;
          page = PagesFactory.GetPage<StrutAdjustmentRecapPage>(langCode);
          break;
        case STRUT_ADJUSTMENT_SAMPLE:
          doPush = true;
          page = PagesFactory.GetPage<StrutAdjustmentSamplePage>(langCode);
          break;
        case STRUT_ADJUSTMENT_DETAIL:
          doPush = true;
          page = PagesFactory.GetPage<StrutAdjustmentDetailPage>(langCode);
          break;
        case STRUT_ADJUSTMENT_WIZARD:
          doPush = true;
          page = PagesFactory.GetPage<StrutAdjustmentWizardPage>(langCode);
          break;
        case STRUT_ADJUSTMENT_WIZARDCOMPLETED:
          doPush = true;
          page = PagesFactory.GetPage<StrutAdjustmentWizardCompletedPage>(langCode);
          break;
        case PIN_SITE_CARE:
          doPush = true;
          page = PagesFactory.GetPage<PinSiteCarePage>(langCode);
          break;
        case PIN_SITE_CARE_VIDEO:
          doPush = true;
          page = PagesFactory.GetPage<PinSiteCareVideoPage>(langCode);
          break;
        case STRUT_ADJUSTMENT_VIDEO:
          doPush = true;
          page = PagesFactory.GetPage<StrutAdjustmentVideoPage>(langCode);
          break;
        case MY_PRESCRIPTIONS:
          doPush = true;
          page = PagesFactory.GetPage<MyPrescriptionsPage>(langCode);
          break;
        case MY_DIARY:
          doPush = true;
          page = PagesFactory.GetPage<MyDiaryPage>(langCode);
          break;
        case SHOPPING_LIST:
          doPush = true;
          page = PagesFactory.GetPage<ShoppingListPage>(langCode);
          break;
        case GET_AN_INFECTION_SURVEY:
          doPush = true;
          page = PagesFactory.GetPage<GetAnInfectionSurveyPage>(langCode);
          break;
        case SIGNS_OF_INFECTION:
          doPush = true;
          page = PagesFactory.GetPage<SignsOfInfectionPage>(langCode);
          break;
        case PRESCRIPTION_START:
          doPush = true;
          page = PagesFactory.GetPage<PrescriptionStartPage>(langCode);
          break;
        case QRCODE_SCAN:
          doPush = true;
          page = PagesFactory.GetPage<QrCodeScanPage>(langCode);
          break;
        case PIN_SITE_CARE_TIME:
          doPush = true;
          page = PagesFactory.GetPage<PinSiteCareTimePage>(langCode);
          break;
        case INSIGHT_MESSAGES:
          doPush = true;
          page = PagesFactory.GetPage<InsightMessagePage>(langCode);
          break;
        case PERSONAL_GOAL:
          doPush = true;
          page = PagesFactory.GetPage<PersonalGoalPage>(langCode);
          break;
        case PRIVACY_AND_LEGAL_TERMS:
          doPush = true;
          page = PagesFactory.GetPage<PrivacyAndLegalTermsPage>(langCode, false);
          break;
        case CREDITS:
          doPush = true;
          page = PagesFactory.GetPage<CreditsPage>(langCode);
          break;
        case WIZARD_USER_SETTINGS:
          doPush = true;
          page = PagesFactory.GetPage<WizardUserSettingsPage>(langCode, false);
          break;
        case ALL_MY_DAILY_TASKS:
          doPush = true;
          page = PagesFactory.GetPage<AllMyDailyTasksPage>(langCode);
          break;
        case HOW_DO_YOU_FEEL:
          doPush = true;
          page = PagesFactory.GetPage<HowDoYouFeelPage>(langCode);
          break;
        case SELECT_MOOD_POPUP_PAGE:
          doPush = true;
          page = new SelectMoodPopupPage();
          break;
        case PRESCRIPTION_HELP_POPUP:
          doPush = true;
          page = new PrescriptionHelpPopup();
          break;
        case CLEANING_SOLUTION_INFO_POPUP_PAGE:
          doPush = true;
          page = new CleaningSolutionInfoPopupPage();
          break;
        case MOTIVATIONAL_MESSAGE_POPUP_PAGE:
          doPush = true;
          page = new MotivationalMessagePopupPage();
          break;
        default:
          await App.Current.MainPage.DisplayAlert(target, "Feature not yet implemented", "OK");
          break;
      }

      await Navigate(page, doPush, popup);
      isNavigating = false;
    }

    /// <summary>
    /// Perform default navigation to an istantiated page
    /// </summary>
    /// <param name="page"></param>
    /// <param name="doPush"></param>
    /// <param name="popup"></param>
    /// <returns></returns>
    private async Task Navigate(Page page, bool doPush, bool popup)
    {
      if (page != null)
      {
        // Create the Navigation Page only first time
        // and set as app main page
        if (NavigationPage == null)
        {
          NavigationPage = new MainNavigationPage(page);
          App.SetMainPage(NavigationPage);
        }

        // Navigate back if the page to display is already in the navigation stack
        if (NavigationPage.Navigation.NavigationStack.Contains(page))
        {
          while (NavigationPage.CurrentPage != page)
          {
            await NavigationPage.PopAsync(false);
          }
        }

        // Push the page or replace the root navigation item
        if (doPush)
        {
          if (popup)
          {
            await NavigationPage.Navigation.PushPopupAsync(page as PopupPage);
          }
          else
          {
            await NavigationPage.PushAsync(page);
          }
        }
        else
        {
          ReplaceRootAsync(page);
        }
      }
    }

    /// <summary>
    /// Navigate back to a page if present in the navigation stack. Otherwide navigate to a default target
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="defaultTarget"></param>
    private void NavigateBackToPageOrDefault(Type pageType, string defaultTarget)
    {
      Device.BeginInvokeOnMainThread(async () =>
      {
        isNavigating = true;
        if (NavigationPage.Navigation.NavigationStack.Any(p => p.GetType() == pageType))
        {
          while (NavigationPage.CurrentPage.GetType() != pageType)
          {
            await NavigationPage.PopAsync(false);
          }
        }
        else
        {
          NavigateTo(defaultTarget);
        }
        isNavigating = false;
      });
    }

    private async void ReplaceRootAsync(Page page)
    {
      if (NavigationPage.Navigation.NavigationStack.Any())
      {
        if (NavigationPage.Navigation.NavigationStack.Contains(page))
          NavigationPage.Navigation.RemovePage(page);

        var root = NavigationPage.Navigation.NavigationStack[0];
        NavigationPage.Navigation.InsertPageBefore(page, root);
        await PopToRootAsync();
      }
      else
      {
        await NavigationPage.Navigation.PushAsync(page);
      }
      GC.Collect();
    }

    private async Task PopToRootAsync()
    {
      while (NavigationPage.Navigation.ModalStack.Count > 0)
      {
        await NavigationPage.Navigation.PopModalAsync(false);
      }
      while (NavigationPage.CurrentPage != NavigationPage.Navigation.NavigationStack[0])
      {
        await NavigationPage.PopAsync(false);
      }
    }

  }
}
