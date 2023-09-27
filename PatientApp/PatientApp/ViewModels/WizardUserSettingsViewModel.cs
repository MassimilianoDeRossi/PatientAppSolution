using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Essentials;

using Plugin.Media;

using PatientApp.Interfaces;
using PatientApp.Views;
using PatientApp.Settings;

namespace PatientApp.ViewModels
{
    /// <summary>
    /// User profile wizard carousel view model
    /// </summary>
    public class WizardUserSettingsViewModel : BaseViewModel
    {

        private bool _wizardInProgress = false;

        private string _nickname;
        public string Nickname
        {
            get { return _nickname; }
            set { SetProperty(ref _nickname, value); }
        }

        private string _profileImagePath;
        public string ProfileImagePath
        {
            get { return _profileImagePath; }
            set { SetProperty(ref _profileImagePath, value); }
        }

        private bool _isInsightEnabled;
        public bool IsInsightEnabled
        {
            get { return _isInsightEnabled; }
            set { SetProperty(ref _isInsightEnabled, value); }
        }

        private TimeSpan _insightTime;
        public TimeSpan InsightTime
        {
            get { return _insightTime; }
            set { SetProperty(ref _insightTime, value); }
        }

        private bool _isGoalEnabled;
        public bool IsGoalEnabled
        {
            get { return _isGoalEnabled; }
            set { SetProperty(ref _isGoalEnabled, value); }
        }

        private string _personalGoal;
        public string PersonalGoal
        {
            get { return _personalGoal; }
            set { SetProperty(ref _personalGoal, value); }
        }

        private TimeSpan _pinSiteCareTime;
        public TimeSpan PinSiteCareTime
        {
            get { return _pinSiteCareTime; }
            set { SetProperty(ref _pinSiteCareTime, value); }
        }


        public Command WizardPrevCommand { get; set; }
        public Command WizardNextCommand { get; set; }
        public Command SkipWizardCommand { get; set; }
        public Command SelectPhotoCommand { get; set; }

        WizardUserSettingsBaseTemplateSelector _wizardTemplateSelector = null;
        public WizardUserSettingsBaseTemplateSelector WizardTemplateSelector
        {
            get
            {
                return _wizardTemplateSelector;
            }
            set
            {
                SetProperty(ref _wizardTemplateSelector, value);
            }
        }

        /// <summary>
        /// The list of pages to show in carousel wizard
        /// </summary>
        public ObservableCollection<int> WizardPages { get; set; } = new ObservableCollection<int>();

        int _wizardPosition = 0;
        /// <summary>
        /// The index of the view displayed in the carousel 
        /// </summary>
        public int WizardPosition
        {
            get { return _wizardPosition; }
            set
            {
                if (value != _wizardPosition)
                {
                    if (CanMoveToPosition(value))
                    {
                        SetProperty(ref _wizardPosition, value);
                        WizardNextCommand.ChangeCanExecute();
                        WizardPrevCommand.ChangeCanExecute();
                        OnPropertyChanged(nameof(WizardPrevButtonText));
                        OnPropertyChanged(nameof(WizardNextButtonText));
                        CanGoBack = _wizardPosition > 0;
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            App.Current.MainPage.DisplayAlert(Localization.LocalizationManager.GetText("ErrorTitle"), Localization.LocalizationManager.GetText("ErrorPersonalGoalRequired"), "OK");
                            var oldValue = _wizardPosition;
                            SetProperty(ref _wizardPosition, value);
                            // User has swiped on next step but can't move: restore previous value
                            WizardPosition = oldValue;
                        });

                    }
                }
                
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
                if (WizardTemplateSelector != null && WizardPosition < WizardTemplateSelector.ItemsCount - 1)
                    return Resources.PatientApp.BtnNext;
                else
                    return Resources.PatientApp.BtnDone;
            }
        }

        public string SelectPhotoMessage => (string.IsNullOrEmpty(ProfileImagePath)) ? Resources.PatientApp.LblAddYourPhoto_WizardUserProfile : Resources.PatientApp.LblChangeYourPhoto_WizardUserProfile;


        bool _cangoBack = false;
        public bool CanGoBack
        {
            get { return _cangoBack; }
            set
            {
                SetProperty(ref _cangoBack, value);
            }
        }

        public WizardUserSettingsViewModel(ILocalDatabaseService dbService, ISystemUtility sysUtility) : base(dbService, null, sysUtility)
        {
            WizardPrevCommand = new Command(WizardPrevCommandExecute, WizardPrevCommandCanExecute);
            WizardNextCommand = new Command(WizardNextCommandExecute, WizardNextCommandCanExecute);
            SkipWizardCommand = new Command(SkipWizardCommandExecute);
            SelectPhotoCommand = new Command(SelectPhotoCommandExecute, SelectPhotoCommandCanExecute);

            MessagingCenter.Subscribe<BaseContentPage>(this, Messaging.Messages.VIEW_APPEARING_MESSAGE,
                (page) =>
                {
                    if (page is WizardUserSettingsPage)
                    {
                        // Avoid unexpected restart
                        if (!_wizardInProgress)
                            InitWizard(IsLoggedIn);
                    }
                });

            MessagingCenter.Instance.Subscribe<BaseContentPage>(this, Messaging.Messages.ANDROID_BACKBUTTON_PRESSED,
                (page) =>
                {
                    if (page is WizardUserSettingsPage)
                        OnAndroidBackButtonPressed();
                });

            InitWizard(IsLoggedIn);
        }

        /// <summary>
        /// Initialize the wizard template selector with the corrent panels 
        /// depending on user logging state
        /// </summary>
        /// <param name="loggedIn"></param>
        private void InitWizard(bool loggedIn)
        {
            var settings = AppSettings.Instance;
            ProfileImagePath = settings.ProfileImagePath;
            Nickname = settings.Nickname;
            PinSiteCareTime = settings.PinSiteCareTime;
            IsGoalEnabled = true; // settings.IsGoalEnabled;
            PersonalGoal = settings.PersonalGoal;
            InsightTime = settings.InsightTime;
            IsInsightEnabled = true; // settings.IsInsightEnabled;

            OnPropertyChanged(nameof(SelectPhotoMessage));

            WizardPosition = -1;
            if (Device.OS == TargetPlatform.Android)
            {
                WizardPages = new ObservableCollection<int>();
            }
            else
            {
                WizardPages.Clear();
            }

            if (loggedIn)
            {
                WizardTemplateSelector = new WizardUserSettingsNormalTemplateSelector();
            }
            else
            {
                WizardTemplateSelector = new WizardUserSettingsAnonymousTemplateSelector();
            }

            for (int i = 0; i < WizardTemplateSelector.ItemsCount; i++)
                WizardPages.Add(i);


            OnPropertyChanged(nameof(WizardPages));
            WizardPosition = 0;
            App.NavigationController.CanNavigateBack = false;
            _wizardInProgress = true;
            App.IsWizardUserSettingsActive = true;
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
            {
                WizardPosition = newPos;
            }
        }

        private bool WizardNextCommandCanExecute()
        {
            return true;
        }

        /// <summary>
        /// Manage next button command (go to next page if possible, close wizard otherwise)
        /// </summary>
        private void WizardNextCommandExecute()
        {
            if (WizardPosition < WizardPages.Count - 1)
            {
                WizardPosition = WizardPosition+1;
            }
            else
            {
                App.IsWizardUserSettingsActive = false;
                if (IsLoggedIn)
                {
                    AppSettings.SetAllSettings(this.Nickname, this.ProfileImagePath, this.PinSiteCareTime, this.IsInsightEnabled, this.InsightTime, this.IsGoalEnabled, this.PersonalGoal, _sysUtility.Now);
                    // Force sync settings 
                    MessagingCenter.Send<BaseViewModel>(this, Messaging.Messages.SYNC_SETTINGS_REQUEST);
                }
                else
                {
                    AppSettings.SetUserProfile(this.Nickname, this.ProfileImagePath);
                    AppSettings.SetPinSiteCare(true, this.PinSiteCareTime, _sysUtility.Now);

                    // Rebuild and reschedule pin site care reminders and notifications
                    RebuildPinSiteCareCalendarAndReminders(AppSettings.Instance.PinSiteCareTime);
                }
                _wizardInProgress = false;
                App.NavigationController.CanNavigateBack = true;
                App.NavigationController.NavigateToHome();
            }
        }

        private async void SkipWizardCommandExecute()
        {
            var result = await App.Current.MainPage.DisplayAlert(Resources.PatientApp.AlertMsgTitle_PrescriptionViewModel,
                                                                 Resources.PatientApp.AlertConfirmSkipPrescriptionWizard,
                                                                 Resources.PatientApp.AlertConfirmSkipPrescriptionWizardYes,
                                                                 Resources.PatientApp.AlertConfirmSkipPrescriptionWizardNo);
            if (result)
            {
                _wizardInProgress = false;
                App.IsWizardUserSettingsActive = false;
                App.NavigationController.CanNavigateBack = true;
                App.NavigationController.NavigateToHome();
                // Force sync settings 
                MessagingCenter.Send<BaseViewModel>(this, Messaging.Messages.SYNC_SETTINGS_REQUEST);
            }
        }

        private bool SelectPhotoCommandCanExecute()
        {
            return !IsBusy;
        }

        private const string TAKE_PHOTO = "Take new photo";
        private const string PICK_PHOTO = "Select from gallery";
        private async void SelectPhotoCommandExecute()
        {
            var result = await Application.Current.MainPage.DisplayActionSheet(Resources.PatientApp.LblAddYourPhoto_WizardUserProfile, Resources.PatientApp.BtnCancel, null, new[] { TAKE_PHOTO, PICK_PHOTO });

            string filePath = null;
            switch (result)
            {
                case TAKE_PHOTO:
                    filePath = await TakePhoto();
                    break;
                case PICK_PHOTO:
                    filePath = await PickPhoto();
                    break;
            }

            if (!string.IsNullOrEmpty(filePath))
            {
                ProfileImagePath = filePath;
                OnPropertyChanged(nameof(SelectPhotoMessage));
            }
        }

        private bool CanMoveToPosition(int position)
        {
            // Validate step
            switch (position)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 4:
                    if (IsLoggedIn && this.IsGoalEnabled && string.IsNullOrEmpty(this.PersonalGoal))
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }

        private async Task<string> PickPhoto()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert(Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Title, Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Message, Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_OK);
                return null;
            }

            var status = await Permissions.RequestAsync<Permissions.StorageRead>();
            if (status != PermissionStatus.Granted)
            {
                ShowErrorMessage(Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Title, Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Message);
                return null;
            }

            status = await Permissions.RequestAsync<Permissions.Photos>();
            if (status != PermissionStatus.Granted)
            {
                ShowErrorMessage(Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Title, Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Message);
                return null;
            }

            IsBusy = true;
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions()
            {
            });

            IsBusy = false;

            return file?.Path;

        }

        private async Task<string> TakePhoto()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert(Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Title, Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Message, Resources.PatientApp.LblPickPhoto_PrescriptionViewModel_OK);
                return null;
            }

            var status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                ShowErrorMessage(Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Title, Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Message);
                return null;
            }

            status = await Permissions.RequestAsync<Permissions.StorageRead>();
            if (status != PermissionStatus.Granted)
            {
                ShowErrorMessage(Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Title, Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Message);
                return null;
            }

            status = await Permissions.RequestAsync<Permissions.Photos>();
            if (status != PermissionStatus.Granted)
            {
                ShowErrorMessage(Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Title, Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Message);
                return null;
            }

            IsBusy = true;

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
                SaveToAlbum = true,
                RotateImage = false
            });

            IsBusy = false;
            return file?.Path;

        }

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