using System;
using System.Collections.ObjectModel;
using PatientApp.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using PatientApp.DataModel.SqlEntities;
using PatientApp.Interfaces;
using PatientApp.Views;
using PatientApp.Utilities;
using PatientApp.Services;

namespace PatientApp.ViewModels
{
  public enum BoneTypeSegmentEnum
  {
    Undefined = 1000,
    LongBone = 10,
    Tibia = 20,
    Femur = 30,
    Ankle = 40,
    ForeFoot = 50,
    HindFoot = 60
  }
  public enum AnatomiesTypeEnum
  {
    Left = 10,
    Right = 20
  }

  /// <summary>
  /// A class contianing a summary of prescription informations
  /// </summary>
  public class PrescriptionDetail
  {

    public string Title { get; set; }
    public string FrameID { get; set; }
    public string Site { get; set; }
    public int BadgeCount { get; set; }
    public bool AllDone { get; set; }

    /// <summary>
    /// The list of expired strut adjustments related to the prescription
    /// </summary>
    public List<StrutAdjustment> ExpiredStrutAdjustments { get; set; }
  }

  /// <summary>
  /// A class contianing a summary of strut adjustment informations
  /// </summary>
  public class StrutDetail : ObservableObject
  {
    int _number = 0;
    public int Number
    {
      get { return _number; }
      set { SetProperty(ref _number, value); }
    }

    private int? _click = null;
    public int? Click
    {
      get { return _click; }
      set
      {
        SetProperty(ref _click, value);
        OnPropertyChanged(nameof(HasClick));
      }
    }

    string _directionImage = null;
    public string DirectionImage
    {
      get { return _directionImage; }
      set { SetProperty(ref _directionImage, value); }
    }

    string _directionAnimImagePrefix = null;
    public string DirectionAnimImagePrefix
    {
      get { return _directionAnimImagePrefix; }
      set { SetProperty(ref _directionAnimImagePrefix, value); }
    }

    string _backgroundImageName = null;
    public string BackgroundImageName
    {
      get { return _backgroundImageName; }
      set { SetProperty(ref _backgroundImageName, value); }
    }

    string _imageName = null;
    public string ImageName
    {
      get { return _imageName; }
      set { SetProperty(ref _imageName, value); }
    }

    int? _length = 0;
    public int? Length
    {
      get { return _length; }
      set { SetProperty(ref _length, value); }
    }

    public bool HasClick
    {
      get { return this.Click.HasValue && this.Click.Value != 0; }
    }

  }

  /// <summary>
  /// ViewModel for strut adjustments views
  /// </summary>
  public class StrutAdjustmentViewModel : BaseViewModel
  {
    ObservableCollection<PrescriptionDetail> _prescriptionRecap = null;
    public ObservableCollection<PrescriptionDetail> PrescriptionRecap
    {
      get { return _prescriptionRecap; }
      set { SetProperty(ref _prescriptionRecap, value); }
    }

    private PrescriptionDetail _selectedPrescription = null;
    public PrescriptionDetail SelectedPrescription
    {
      get { return _selectedPrescription; }
      set { SetProperty(ref _selectedPrescription, value); }
    }

    private StrutAdjustment _selectedAdjustment = null;
    public StrutAdjustment SelectedAdjustment
    {
      get { return _selectedAdjustment; }
      set { SetProperty(ref _selectedAdjustment, value); }
    }

    public StrutDetail SelectedStrut { get; set; }

    string _detailTitle = null;
    public string DetailTitle
    {
      get { return _detailTitle; }
      set { SetProperty(ref _detailTitle, value); }
    }

    string _detailDateTime = null;
    public string DetailDateTime
    {
      get { return _detailDateTime; }
      set { SetProperty(ref _detailDateTime, value); }
    }

    public Command OpenDetailCommand { get; set; }
    public Command StartWizardCommand { get; set; }
    public Command PostponeCommand { get; set; }
    public Command PlayVideoCommand { get; set; }
    public Command WizardPrevCommand { get; set; }
    public Command WizardNextCommand { get; set; }
    public Command WizardCloseCommand { get; set; }

    public StrutAdjustmentTemplateSelector WizardTemplateSelector { get; set; }

    /// <summary>
    /// The list of pages to show in carousel wizard
    /// </summary>
    public ObservableCollection<int> WizardPages { get; set; }

    int _wizardPosition = 0;

    /// <summary>
    /// The index of the view displayed in the carousel 
    /// </summary>
    public int WizardPosition
    {
      get { return _wizardPosition; }
      set
      {
        SetProperty(ref _wizardPosition, value);
        WizardNextCommand.ChangeCanExecute();
        WizardPrevCommand.ChangeCanExecute();

        OnPropertyChanged(nameof(WizardPrevButtonText));
        OnPropertyChanged(nameof(WizardNextButtonText));
        CanGoBack = _wizardPosition > 0;
      }
    }

    /// <summary>
    /// The label to show on the wizard previous button
    /// </summary>
    public string WizardPrevButtonText => Resources.PatientApp.BtnBack;

    /// <summary>
    /// The label to show on the wizard next button (can change depending on current displayed view)
    /// </summary>
    public string WizardNextButtonText
    {
      get
      {
        // Show next if it is not the last page - done otherwise
        if (WizardTemplateSelector != null && WizardPosition < WizardTemplateSelector.ItemsCount - 1)
          return Resources.PatientApp.BtnNext;
        else
          return Resources.PatientApp.BtnDone;
      }
    }

    bool _cangoBack = false;
    public bool CanGoBack
    {
      get { return _cangoBack; }
      set
      {
        SetProperty(ref _cangoBack, value);
      }
    }


    public StrutAdjustmentViewModel(ILocalDatabaseService dbService, ISystemUtility sysUtility) : base(dbService, null, sysUtility)
    {
      PlayVideoCommand = new Command(PlayVideoCommandExecute);
      OpenDetailCommand = new Command(OpenDetailCommandCommandExecute);
      StartWizardCommand = new Command(StartWizardCommandExecute);
      PostponeCommand = new Command(PostponeCommandExecute);
      WizardPrevCommand = new Command(WizardPrevCommandExecute, WizardPrevCommandCanExecute);
      WizardNextCommand = new Command(WizardNextCommandExecute, WizardNextCommandCanExecute);
      WizardCloseCommand = new Command(WizardCloseCommandExecute);

      MessagingCenter.Subscribe<BaseContentPage>(this, Messaging.Messages.VIEW_APPEARING_MESSAGE, async (page) =>
      {
        if (page is StrutAdjustmentRecapPage)
        {
          IsBusy = true;
                // Show a warning about expired strut adjustments skipping (deletion) only first time
                if (AppSettings.Instance.ShowStrutsSkippedWarning)
          {
            await App.Current.MainPage.DisplayAlert(Resources.PatientApp.AlertMsgTitle_PrescriptionViewModel, Resources.PatientApp.AlertSkipPreviousStruts, Resources.PatientApp.BtnOK);
            await DeleteOlderThanTodayStruts();
          }
          await LoadRecap();
          IsBusy = false;
        }
        else if (page is StrutAdjustmentVideoPage)
        {
                // Give use the option to rotate device (will be disabled on back in BaseViewModel or in Android back button broadcast receiver in this viewmodel)
                DependencyService.Get<IOrientationManager>().DisableForcedOrientation();
        }
      });


      MessagingCenter.Instance.Subscribe<BaseContentPage>(this, Messaging.Messages.ANDROID_BACKBUTTON_PRESSED,
          (page) =>
          {
                  // Disable screen orientation 
                  DependencyService.Get<IOrientationManager>().ForcePortrait();

                  // Manage android back button pressed
                  if (page is StrutAdjustmentWizardPage)
              OnAndroidBackButtonPressed();
          });

      this.PropertyChanged += StrutAdjustmentViewModel_PropertyChanged;
    }

    private void StrutAdjustmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(WizardPosition))
      {
        // If the current displayed view index has changed, updates view model properties
        SelectedStrut.Number = WizardPosition + 1;
        SelectedStrut.ImageName = string.Concat("Exagon_strut_", SelectedStrut.Number);
        SelectedStrut.BackgroundImageName = string.Concat("bkg_strut_", SelectedStrut.Number);
        if (App.TestModel != null && App.TestModel.TestModeOn)
        {
          App.TestModel.SelectedStrutBackgroundImageName = SelectedStrut.BackgroundImageName;
        }
        switch (SelectedStrut.Number)
        {
          case 1:
            SelectedStrut.Click = SelectedAdjustment.Click1;
            SelectedStrut.Length = SelectedAdjustment.Length1;
            break;
          case 2:
            SelectedStrut.Click = SelectedAdjustment.Click2;
            SelectedStrut.Length = SelectedAdjustment.Length2;
            break;
          case 3:
            SelectedStrut.Click = SelectedAdjustment.Click3;
            SelectedStrut.Length = SelectedAdjustment.Length3;
            break;
          case 4:
            SelectedStrut.Click = SelectedAdjustment.Click4;
            SelectedStrut.Length = SelectedAdjustment.Length4;
            break;
          case 5:
            SelectedStrut.Click = SelectedAdjustment.Click5;
            SelectedStrut.Length = SelectedAdjustment.Length5;
            break;
          case 6:
            SelectedStrut.Click = SelectedAdjustment.Click6;
            SelectedStrut.Length = SelectedAdjustment.Length6;
            break;
        }

        if (SelectedStrut.Click.HasValue)
        {
          // Update images
          SelectedStrut.DirectionAnimImagePrefix = SelectedStrut.Click.Value >= 0 ? "direction_plus_" : "direction_minus_";
          if (SelectedStrut.Click.HasValue && SelectedStrut.Click.Value != 0)
          {
            SelectedStrut.DirectionImage = SelectedStrut.Click.Value > 0 ? "direction_plus.png" : "direction_minus.png";
            if (App.TestModel != null && App.TestModel.TestModeOn)
            {
              App.TestModel.SelectedStrutDirectionImage = SelectedStrut.DirectionImage;
            }
          }
          else
          {
            SelectedStrut.DirectionImage = null;
            if (App.TestModel != null && App.TestModel.TestModeOn)
            {
              App.TestModel.SelectedStrutDirectionImage = null;
            }
          }
        }
        else
        {
          SelectedStrut.DirectionAnimImagePrefix = "";
        }

        OnPropertyChanged(nameof(SelectedStrut));

      }
    }

    /// <summary>
    /// Initialize the wizard template selector with the corrent panels 
    /// depending on user logging state
    /// </summary>        
    private void InitWizard()
    {
      WizardTemplateSelector = new StrutAdjustmentTemplateSelector();
      WizardPages = new ObservableCollection<int>();
      for (int i = 0; i < WizardTemplateSelector.ItemsCount; i++)
        WizardPages.Add(i);

      WizardPosition = 0;
      // Force redraw
      OnPropertyChanged(nameof(WizardPosition));
    }


    private bool WizardPrevCommandCanExecute()
    {
      return WizardPosition > 0;
    }

    /// <summary>
    /// Manage previous button command (go to previous page if possible)
    /// </summary>
    private void WizardPrevCommandExecute()
    {
      int newPos = Math.Max(WizardPosition - 1, 0);
      if (newPos != WizardPosition)
        WizardPosition = newPos;
    }

    private bool WizardNextCommandCanExecute()
    {
      return true;
    }

    /// <summary>
    /// Manage next button command (go to next page if possible, close wizard otherwise)
    /// </summary>
    private async void WizardNextCommandExecute()
    {
      if (WizardPosition < WizardPages.Count - 1)
        WizardPosition += 1;
      else
      {
        // Save to local database as done
        SelectedAdjustment.Done = true;
        SelectedAdjustment.DoneDateTime = _sysUtility.Now;
        if (await _dbService.SaveStrutAdjustment(SelectedAdjustment))
        {
          App.NavigationController.NavigateTo(NavigationController.STRUT_ADJUSTMENT_WIZARDCOMPLETED);
        }
      }
    }

    private void WizardCloseCommandExecute()
    {
      // Re-enable sync on strut adjustment completed
      App.SetDoNotDisturbMode(false);
      App.NavigationController.CanNavigateBack = true;
      App.NavigationController.NavigateTo(NavigationController.STRUT_ADJUSTMENT_RECAP);
    }

    private void OpenDetailCommandCommandExecute()
    {
      if (SelectedPrescription != null && !SelectedPrescription.AllDone)
      {
        SelectedAdjustment = SelectedPrescription.ExpiredStrutAdjustments.OrderBy(s => s.DateOfAdjustment).First();
        DetailTitle = string.Concat(Localization.LocalizationManager.GetText("LblFrameId"), " ", SelectedPrescription.FrameID);
        DetailDateTime = SelectedAdjustment.DateOfAdjustment.ToString("dd MMM - hh tt");
        App.NavigationController.NavigateTo(NavigationController.STRUT_ADJUSTMENT_DETAIL);
      }
      else
      {
        // Cancel selection
        SelectedPrescription = null;
      }
    }

    private void StartWizardCommandExecute()
    {
      if (SelectedStrut == null)
        SelectedStrut = new StrutDetail();

      InitWizard();
      // Disable sync and alerts during strut adjustment
      App.SetDoNotDisturbMode(true);
      App.NavigationController.CanNavigateBack = false;
      App.NavigationController.NavigateTo(NavigationController.STRUT_ADJUSTMENT_WIZARD);
    }

    private async void PostponeCommandExecute()
    {
      // Ask for postpone confirmation
      var result = await App.Current.MainPage.DisplayAlert(Resources.PatientApp.LblStrutAdjustment, Resources.PatientApp.BtnConfirmPostpone, Resources.PatientApp.BtnOK, Resources.PatientApp.BtnCancel);
      if (result)
      {
        // Add new item in log history
        var historyItem = new LogHistoryItem()
        {
          ExpectedDateTime = SelectedAdjustment.DateOfAdjustment,
          EventDateTime = _sysUtility.Now,
          ItemType = LogHistoryItem.ItemTypeEnum.StrutAdjustmentPostponed,
          Description = string.Format("Strut Adjustment Postponed on Frame ID {0}", SelectedPrescription.FrameID),
          LocalEntityId = SelectedAdjustment.Id,
          ServerEntityId = SelectedAdjustment.TreatmentStepNumber
        };
        await _dbService.SaveHistoryLogItem(historyItem);

        App.NavigationController.NavigateBack();
      }
    }

    private void PlayVideoCommandExecute()
    {
      App.NavigationController.NavigateTo(NavigationController.STRUT_ADJUSTMENT_VIDEO);
    }

    /// <summary>
    ///  Build the list of prescriptions with expired adjustments
    /// </summary>
    private async Task LoadRecap()
    {
      var prescriptions = await _dbService.GetPrescriptions();

      var recapList = new List<PrescriptionDetail>();

      foreach (var p in prescriptions.OrderBy(p => p.FrameID))
      {
        var struts = await _dbService.GetPrescriptionStrutAdjustments(p.Id, true);
        var adjItems = struts.OrderBy(a => a.DateOfAdjustment);
        var expiredItems = adjItems.Where(a => a.DateOfAdjustment < _sysUtility.Now && !a.Done);

        var boneName = p.BoneTypeSegment.HasValue ? Localization.LocalizationManager.GetText(((BoneTypeSegmentEnum)p.BoneTypeSegment.Value).ToString()) : "";
        string anamotomyTypeName = p.AnatomiesType.HasValue ? Localization.LocalizationManager.GetText(((AnatomiesTypeEnum)p.AnatomiesType.Value).ToString()) : "";
        recapList.Add(new PrescriptionDetail()
        {
          Title = p.CaseNumber,
          FrameID = p.FrameID,
          Site = string.Concat(anamotomyTypeName, " ", boneName),
          ExpiredStrutAdjustments = expiredItems.ToList(),
          BadgeCount = expiredItems.Count(),
          AllDone = !expiredItems.Any(),
        });
      }

#if ENABLE_TEST_CLOUD
            if (App.TestModel?.TestModeOn == true) {
                App.TestModel.BadgeStrutsAdj = "";
                App.TestModel.FrameStrutsAdj = "";

                foreach (var recap in recapList) {
                    App.TestModel.BadgeStrutsAdj += recap.BadgeCount;
                    App.TestModel.FrameStrutsAdj += recap.FrameID;
                }
            }
#endif

      PrescriptionRecap = new ObservableCollection<PrescriptionDetail>(recapList);
    }

    /// <summary>
    /// Delete all strut adjusments scheduled to a date older that today
    /// </summary>
    private async Task DeleteOlderThanTodayStruts()
    {
      if (await _dbService.DeleteStrutAdjustmentsToDate(_sysUtility.Now.Date.AddDays(-1), true))
      {
        AppSettings.SetShowStrutsSkippedWarning(false);
      }
      else
      {
        AppLoggerHelper.LogEvent("StrutAdjustment", "Error deleting older struts: " + _dbService.LastException?.ToString(), TraceLevel.Error);
      }
    }

    /// <summary>
    /// Overrides standard behavior of Android back button
    /// </summary>
    protected void OnAndroidBackButtonPressed()
    {
      // During wizard we can't go back (previous page), but we move to previous wizard step if possible            
      if (!App.NavigationController.CanNavigateBack)
      {
        if (WizardPrevCommandCanExecute())
          WizardPrevCommandExecute();
      }

    }
  }

}
