using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

using PatientApp.DataModel.SqlEntities;
using PatientApp.Interfaces;
using PatientApp.Views;

namespace PatientApp.ViewModels
{
  /// <summary>
  /// ViewModel for user personal diary management
  /// </summary>
  public class MyDiaryViewModel : BaseViewModel
  {
    private DateTime _currentDate;
    public DateTime CurrentDate
    {
      get { return _currentDate; }
      set
      {
        SetProperty(ref _currentDate, value);
        OnPropertyChanged(nameof(CurrentDateDayName));
        OnPropertyChanged(nameof(CurrentDateDayNumber));
      }
    }

    public string CurrentDateDayName
    {
      get
      {
        return _currentDate.ToString("ddd").ToUpper();
      }
    }

    public string CurrentDateDayNumber
    {
      get
      {
        return _currentDate.ToString("dd");
      }
    }


    private DateTime _todayDate;

    public DateTime TodayDate
    {
      get { return _todayDate; }
      set { SetProperty(ref _todayDate, value); }
    }

    public bool HasItems
    {
      get { return DiaryItems != null && DiaryItems.Any(); }
    }

    public ObservableCollection<ViewDiaryItem> DiaryItems { get; set; }

    public MyDiaryViewModel(ILocalDatabaseService dbService, ISystemUtility sysUtility) : base(dbService, null, sysUtility)
    {
      this.TodayDate = _sysUtility.Now.Date;

      DiaryItems = new ObservableCollection<ViewDiaryItem>();

      this.PropertyChanged += MyDiaryViewModel_PropertyChanged;

      MessagingCenter.Subscribe<BaseContentPage>(this, Messaging.Messages.VIEW_APPEARING_MESSAGE, ViewAppearing);
      MessagingCenter.Subscribe<App>(this, Messaging.Messages.APP_RESUMED, AppResume);
    }

    private async void MyDiaryViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(this.CurrentDate))
      {
        await LoadMyDiaryItems();
      }
    }

    private async void ViewAppearing(BaseContentPage page)
    {
      if (page is MyDiaryPage)
      {
        // Refresh data on view appearing
        this.CurrentDate = _sysUtility.Now.Date;
        this.TodayDate = _sysUtility.Now.Date;
        await LoadMyDiaryItems();
      }
    }

    private void AppResume(App page)
    {
      this.TodayDate = _sysUtility.Now.Date;
    }

    private async Task LoadMyDiaryItems()
    {
      DiaryItems.Clear();

      // Load LogHistory items from database and populates diary items
      var logItems = await _dbService.GetHistoryLogItemsAtDate(this.CurrentDate);
      if (logItems != null)
      {
        foreach (var logItem in logItems.ToList().OrderBy(i => i.EventDateTime))
        {
          switch (logItem.ItemType)
          {
            case LogHistoryItem.ItemTypeEnum.StrutAdjustmentDone:
              var strut = await _dbService.GetStrutAdjustmentById(logItem.LocalEntityId.Value);
              if (strut != null)
              {
                var prescription = await _dbService.GetPrescritionById(strut.PrescriptionId);
                if (prescription != null)
                {
                  DiaryItems.Add(new ViewDiaryItem()
                  {
                    Time = logItem.EventDateTime.ToString("h:mm tt"),
                    Description = string.Format("{0} - {1} {2} - {3}",
                                                  Resources.PatientApp.LblStrutAdjustmentDone,
                                                  Resources.PatientApp.LblFrameId,
                                                  prescription.FrameID,
                                                  logItem.ExpectedDateTime.Value.ToString("dd MMM h:mm tt"))
                  });
                }
              }
              break;
            case LogHistoryItem.ItemTypeEnum.StrutAdjustmentPostponed:
              var strutPostponed = await _dbService.GetStrutAdjustmentById(logItem.LocalEntityId.Value);
              if (strutPostponed != null)
              {
                var prescription = await _dbService.GetPrescritionById(strutPostponed.PrescriptionId);
                if (prescription != null)
                {
                  DiaryItems.Add(new ViewDiaryItem()
                  {
                    Time = logItem.EventDateTime.ToString("h:mm tt"),
                    Description = string.Format("{0} - {1} {2} - {3}",
                                                  Resources.PatientApp.LblStrutAdjustmentPostponed,
                                                  Resources.PatientApp.LblFrameId,
                                                  prescription.FrameID,
                                                  logItem.ExpectedDateTime.Value.ToString("dd MMM h:mm tt"))
                  });
                }
              }
              break;
            case LogHistoryItem.ItemTypeEnum.PinSiteCareDone:
              DiaryItems.Add(new ViewDiaryItem()
              {
                Time = logItem.EventDateTime.ToString("h:mm tt"),
                Description = Resources.PatientApp.LblPinSiteCareTitle,
              });
              break;
            case LogHistoryItem.ItemTypeEnum.MotivationalMesssageRead:
              DiaryItems.Add(new ViewDiaryItem()
              {
                Time = logItem.EventDateTime.ToString("h:mm tt"),
                Description = string.Concat(Resources.PatientApp.LblMotivationalMessageRead, " - ", logItem.Description),
              });
              break;
          }
        }
      }

      OnPropertyChanged(nameof(HasItems));
    }


  }

}
