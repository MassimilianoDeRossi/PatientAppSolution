using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

using PatientApp.DataModel.SqlEntities;
using PatientApp.Interfaces;
using PatientApp.Services;
using PatientApp.Views;

namespace PatientApp.ViewModels
{
  /// <summary>
  /// ViewModel Handling daily task divided in todolist and donelist
  /// </summary>
  public class AllMyDailyTasksViewModel : BaseViewModel
  {
    private DateTime _date;

    private DateTime Date
    {
      get { return _date; }
      set { SetProperty(ref _date, value); }
    }

    public ObservableCollection<ViewDailyTask> TodoTaskList { get; set; } = new ObservableCollection<ViewDailyTask>();
    public ObservableCollection<ViewDailyTask> DoneTaskList { get; set; } = new ObservableCollection<ViewDailyTask>();
    public string MonthAndDay => Date.ToString("M");
    public string DayOfWeekName => Date.DayOfWeek.ToString();

    public bool ShowTodoList => TodoTaskList.Count > 0;
    public bool ShowDoneList => DoneTaskList.Count > 0;

    public AllMyDailyTasksViewModel(ILocalDatabaseService dbService, ISystemUtility sysUtility) : base(dbService, null, sysUtility)
    {
      Task.Run(async () =>
      {
        await InitOrRefreshView();
      });
      
      MessagingCenter.Subscribe<BaseContentPage>(this, Messaging.Messages.VIEW_APPEARING_MESSAGE, ViewAppeared);
      MessagingCenter.Subscribe<App>(this, Messaging.Messages.APP_RESUMED, AppResume);
    }


    /// <summary>
    /// Used to show/hide the "all my daily tasks" button on the view
    /// The button is visible if user is logged in and the current time is after 10AM
    /// </summary>
    public bool CanSetMood => IsLoggedIn && _sysUtility.Now.Hour >= 10;

    public string MoodText => MoodManager.GetMoodAtDateTime(_sysUtility.Now);

    private async void ViewAppeared(BaseContentPage page)
    {
      if (page is AllMyDailyTasksPage)
      {
        // Update visibility flag
        OnPropertyChanged(nameof(CanSetMood));
        await InitOrRefreshView();
      }
    }

    private async void AppResume(App page)
    {
      if (page != null)
      {
        await InitOrRefreshView();
      }
    }

    /// <summary>
    /// Used to init o refresh context after viewappearing or app resume.
    /// </summary>
    private async Task InitOrRefreshView()
    {
      Date = _sysUtility.Now;
      await ReloadTodoTaskList(Date);
      await ReloadDoneTaskList(Date);
      OnPropertyChanged(nameof(MoodText));
      OnPropertyChanged(nameof(TodoTaskList));
      OnPropertyChanged(nameof(ShowTodoList));
      OnPropertyChanged(nameof(DoneTaskList));
      OnPropertyChanged(nameof(ShowDoneList));
    }

    /// <summary>
    /// Load TODO list from database and populates view items
    /// </summary>
    /// <param name="when"></param>
    private async Task ReloadTodoTaskList(DateTime when)
    {
      var todoActivities = await _dbService.GetRemindersAtDate(when);
      TodoTaskList.Clear();

      DateTime? moodDateTime = null;
      int moodItemIndex = 0;

      // Create a dummy Mood Assestment item at 10.00am if not set
      var moodSet = (await _dbService.GetHistoryLogItemsAtDate(when, LogHistoryItem.ItemTypeEnum.MoodSelfAssessment)).Any();
      if (!moodSet)
      {
        moodDateTime = when.Date.AddHours(10);
        TodoTaskList.Add(new ViewDailyTask()
        {
          Description = Resources.PatientApp.LblMoodSelfAssessmentActivityDone,
          Time = moodDateTime.Value.ToString("h:mm tt")
        });
      }

      foreach (var curActivity in todoActivities.OrderBy(r => r.DateTime))
      {
        var vdt = new ViewDailyTask()
        {
          Time = curActivity.DateTime.ToString("h:mm tt")
        };

        if (curActivity.Type == Reminder.ReminderType.StrutAdjustmentReminder)
        {
          var frameid = (await _dbService.GetPrescritionById(curActivity.PrescriptionId)).FrameID;
          vdt.Description = string.Format("{0} {1}", Resources.PatientApp.AllMyDailyTask_StrutAdjDesc, frameid);
        }
        else if (curActivity.Type == Reminder.ReminderType.PinSiteCareReminder)
        {
          vdt.Description = Resources.PatientApp.AllMyDailyTask_PinSiteCareDesc;
        }

        if (moodDateTime.HasValue && curActivity.DateTime < moodDateTime)
        {
          TodoTaskList.Insert(moodItemIndex, vdt);
          moodItemIndex++;
        }
        else
        {
          TodoTaskList.Add(vdt);
        }

      }

#if ENABLE_TEST_CLOUD
            string toDoListXBackdoor = "";
            foreach (var item in TodoTaskList)
            {
                toDoListXBackdoor += item.Description == "Mood self assessment" ? 'M' : item.Description.Last();
            }
            App.TestModel.ToDoList = toDoListXBackdoor;
#endif
    }

    /// <summary>
    /// Load Tone list from database and populates view items
    /// </summary>
    /// <param name="when"></param>
    private async Task ReloadDoneTaskList(DateTime when)
    {
      //var doneActivities = from a in _dbService.GetHistoryLogItemsAtDate(when)
      //                     where a.ItemType == LogHistoryItem.ItemTypeEnum.MoodSelfAssessment ||
      //                           a.ItemType == LogHistoryItem.ItemTypeEnum.PinSiteCareDone ||
      //                           a.ItemType == LogHistoryItem.ItemTypeEnum.StrutAdjustmentDone
      //                     select a;

      var doneActivities = await _dbService.GetHistoryLogItemsDoneActivitesAtDate(when);

      DoneTaskList.Clear();
      foreach (var curActivity in doneActivities.OrderBy(a => a.EventDateTime))
      {
        var vdt = new ViewDailyTask()
        {
          Time = curActivity.EventDateTime.ToString("h:mm tt")
        };
        switch (curActivity.ItemType)
        {
          case LogHistoryItem.ItemTypeEnum.MoodSelfAssessment:
            vdt.Description = Resources.PatientApp.AllMyDailyTask_MoodSelfDesc;
            break;
          case LogHistoryItem.ItemTypeEnum.PinSiteCareDone:
            vdt.Description = Resources.PatientApp.AllMyDailyTask_PinSiteCareDesc;
            break;
          case LogHistoryItem.ItemTypeEnum.StrutAdjustmentDone:
            var strut = await _dbService.GetStrutAdjustmentById(curActivity.LocalEntityId.Value);
            if (strut != null)
            {
              var prescription = await _dbService.GetPrescritionById(strut.PrescriptionId);
              var frameid = prescription.FrameID;
              vdt.Description = string.Format("{0} {1}", Resources.PatientApp.AllMyDailyTask_StrutAdjDesc, frameid);
            }
            else
            {
              vdt.Description = "UNKWOWN";
            }
            break;
        }
        DoneTaskList.Add(vdt);
      }

#if ENABLE_TEST_CLOUD
            string doneListXBackdoor = "";
            foreach (var item in DoneTaskList)
            {
                doneListXBackdoor += item.Description == "Mood self assessment" ? 'M' : item.Description.Last();
            }
            App.TestModel.DoneList = doneListXBackdoor;
#endif
    }

  }

}
