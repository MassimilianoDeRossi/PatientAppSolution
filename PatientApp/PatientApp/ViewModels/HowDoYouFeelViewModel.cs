using System.Collections.Generic;
using PatientApp.DataModel.SqlEntities;
using Xamarin.Forms;
using PatientApp.Interfaces;
using PatientApp.Views;
using static PatientApp.Services.MoodManager;
using PatientApp.Services;
using PatientApp.Settings;
using PatientApp.Utilities;
using Newtonsoft.Json;

namespace PatientApp.ViewModels
{

    /// <summary>
    /// ViewModel that stores information of different mood selectable by the patients.  Used both in howdoyoufeelpage and relative popup.
    /// </summary>
    public class HowDoYouFeelViewModel : BaseViewModel
    {
        public Command UserConfirmMoodCommand { get; set; }
        public Command UserCancelMoodCommand { get; set; }

        public Command ClickOnEmoticonForSettingCommand { get; set; }

        public string UnselectedIcon { get; set; }

        int _currentMoodIndex = -1;
        public int CurrentMoodIndex
        {
            get { return _currentMoodIndex; }
            set
            {
                SetProperty(ref _currentMoodIndex, value);
                OnPropertyChanged(nameof(CurrentSelection));
                OnPropertyChanged(nameof(IsMoodSelected));                
            }
        }

        int _editMoodIndex = -1;
        public int EditMoodIndex
        {
            get { return _editMoodIndex; }
            set
            {
                if (value != _editMoodIndex)
                {
                    SetProperty(ref _editMoodIndex, value);
                    OnPropertyChanged(nameof(EditSelection));
                }
            }
        }

        //private int _savedMoodIndex;

        public MoodItem CurrentSelection => CurrentMoodIndex >= 0 ? MoodList[CurrentMoodIndex] : null;
        public MoodItem EditSelection => EditMoodIndex >= 0 ? MoodList[EditMoodIndex] : null;

        public string PersonalGoal { get; set; }
        public bool IsPersonalGoalEnabled { get; set; }

        public HowDoYouFeelViewModel(ILocalDatabaseService dbService, ISystemUtility sysUtility) : base(dbService, null, sysUtility)
        {
            UnselectedIcon = "mood_00_unselected";
            UserConfirmMoodCommand = new Command(UserConfirmMoodCommandExecute);

            UserCancelMoodCommand = new Command(UserCancelMoodCommandExecute);

            ClickOnEmoticonForSettingCommand = new Command(ClickOnEmoticonForSettingCommandExecute);

            MessagingCenter.Subscribe<BaseContentPage>(this, Messaging.Messages.VIEW_APPEARING_MESSAGE, (page) =>
            {
                if (page is HowDoYouFeelPage)
                {
                    CurrentMoodIndex = AppSettings.Instance.MoodIndex ?? -1;
                    PersonalGoal = AppSettings.Instance.PersonalGoal;
                    IsPersonalGoalEnabled = AppSettings.Instance.IsGoalEnabled;
                    OnPropertyChanged(nameof(PersonalGoal));
                    OnPropertyChanged(nameof(IsPersonalGoalEnabled));
                }
            });
        }

        private void ClickOnEmoticonForSettingCommandExecute(object sender)
        {
            if (CurrentMoodIndex == -1)
                EditMoodIndex = 4;
            else
                EditMoodIndex = CurrentMoodIndex;

            App.NavigationController.NavigateTo(NavigationController.SELECT_MOOD_POPUP_PAGE, true);
        }


        private async void UserCancelMoodCommandExecute()
        {
            await App.NavigationController.ClosePopupAsync();
        }

        private async void UserConfirmMoodCommandExecute()
        {
            CurrentMoodIndex = EditMoodIndex;
            AppSettings.SetMoodIndex(CurrentMoodIndex);
            var historyItem = new LogHistoryItem()
            {
                EventDateTime = _sysUtility.Now,
                ItemType = LogHistoryItem.ItemTypeEnum.MoodSelfAssessment,
                Description = Resources.PatientApp.LblMoodSelfAssessmentActivityDone
            };
            if (_dbService.SaveHistoryLogItem(historyItem))
            {
                await App.NavigationController.ClosePopupAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(PatientApp.Resources.PatientApp.ErrorTitle, PatientApp.Resources.PatientApp.ErrorModeSaving, PatientApp.Resources.PatientApp.BtnOK);
                AppLoggerHelper.LogEvent("Mood Confirm", "Error while saving mood", TraceLevel.Error);
            }
              

        }

        public bool IsMoodSelected => CurrentMoodIndex >= 0;

    }

}
