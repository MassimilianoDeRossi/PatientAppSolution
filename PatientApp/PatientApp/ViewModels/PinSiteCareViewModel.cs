using System.Threading.Tasks;
using System.Linq;

using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Essentials;

using Plugin.Media;
using Plugin.Media.Abstractions;

using PatientApp.DataModel.SqlEntities;
using PatientApp.Interfaces;
using PatientApp.Services;
using PatientApp.Views;
using PatientApp.Utilities;

namespace PatientApp.ViewModels
{
    /// <summary>
    /// Handling pin site care wizard
    /// </summary>
    public class PinSiteCareViewModel : BaseViewModel
    {
        public Command OpenShoppingListCommand { get; set; }

        public Command OpenGetAnInfectionSurveyCommand { get; set; }

        public Command TakeOrPickAPhotoCommand { get; set; }

        public Command ShareImageCommand { get; set; }

        private const string TAKE_PHOTO = "Take new photo";
        private const string PICK_PHOTO = "Select from gallery";

        /// <summary>
        /// The absolute path of selected user profile image in the device storage
        /// </summary>
        string _profileImagePath;
        public string ProfileImagePath
        {
            get { return _profileImagePath; }
            set { SetProperty(ref _profileImagePath, value); }
        }

        /// <summary>
        /// Flag that track Pin Site button are active
        /// </summary>
        bool _pinSiteCareButtonsEnabled;
        public bool PinSiteCareButtonsEnabled
        {
            get { return _pinSiteCareButtonsEnabled; }
            set { SetProperty(ref _pinSiteCareButtonsEnabled, value); }
        }

        public Command PlayVideoCommand { get; set; }
        
        public Command DoneCommand { get; set; }

        public PinSiteCareViewModel(ILocalDatabaseService dbService, ISystemUtility sysUtility) : base(dbService, null, sysUtility)
        {
            OpenShoppingListCommand = new Command(OpenShoppingListCommandExecute);
            OpenGetAnInfectionSurveyCommand = new Command(OpenGetAnInfectionSurveyExecute);
            TakeOrPickAPhotoCommand = new Command(TakeOrPickAPhotoCommandExecute);
            ShareImageCommand = new Command(ShareImageCommandExecute);
            PlayVideoCommand = new Command(PlayVideoCommandExecute);
            DoneCommand = new Command(DoneCommandExecute);

            MessagingCenter.Subscribe<BaseContentPage>(this, Messaging.Messages.VIEW_APPEARING_MESSAGE, (page) =>
            {
                if (page is PinSiteCarePage)
                {
                    RefreshDone();
                }
                else if (page is PinSiteCareVideoPage)
                {
                    // Give use the option to rotate device (will be disbled on back in BaseViewModel)
                    DependencyService.Get<IOrientationManager>().DisableForcedOrientation();
                }

            });          
        }

        /// <summary>
        /// Set the pinsitecare done flag based if pin site care reminders have been found
        /// </summary>
        private void RefreshDone()
        {
            var reminder = _dbService.GetRemindersAtDate(_sysUtility.Now, Reminder.ReminderType.PinSiteCareReminder).FirstOrDefault();
            PinSiteCareButtonsEnabled = reminder != null && reminder.DateTime < _sysUtility.Now;
        }

        private void OpenShoppingListCommandExecute()
        {
            App.NavigationController.NavigateTo(NavigationController.SHOPPING_LIST);
        }

        private void OpenGetAnInfectionSurveyExecute()
        {
            App.NavigationController.NavigateTo(NavigationController.GET_AN_INFECTION_SURVEY);
        }

        private async void TakeOrPickAPhotoCommandExecute()
        {
            var result = await Application.Current.MainPage.DisplayActionSheet(Resources.PatientApp.LblAddYourPhoto_PinSiteCare, Resources.PatientApp.BtnCancel_PinSiteCareViewModel, null, TAKE_PHOTO, PICK_PHOTO);

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
                App.NavigationController.NavigateTo(NavigationController.SIGNS_OF_INFECTION);
            }
        }

        /// <summary>
        /// Open camera and take a photo
        /// </summary>
        /// <returns></returns>
        private async Task<string> TakePhoto()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert(Resources.PatientApp.LblTakePhoto_PinSiteCare_Title, Resources.PatientApp.LblTakePhoto_PinSiteCare_Message, Resources.PatientApp.LblTakePhoto_PinSiteCare_OK);
                return null;
            }

            var status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                ShowErrorMessage(Resources.PatientApp.LblTakePhoto_PinSiteCare_Title, Resources.PatientApp.LblTakePhoto_PinSiteCare_Message);
                return null;
            }

            status = await Permissions.RequestAsync<Permissions.Photos>();
            if (status != PermissionStatus.Granted)
            {
                ShowErrorMessage(Resources.PatientApp.LblPickPhoto_PinSiteCare_Title, Resources.PatientApp.LblPickPhoto_PinSiteCare_Message);
                return null;
            }

            status = await Permissions.RequestAsync<Permissions.StorageRead>();
            if (status != PermissionStatus.Granted)
            {
                ShowErrorMessage(Resources.PatientApp.LblPickPhoto_PinSiteCare_Title, Resources.PatientApp.LblPickPhoto_PinSiteCare_Message);
                return null;
            }

            IsBusy = true;

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                // use rear camera as default
                DefaultCamera = CameraDevice.Rear,
                SaveToAlbum = true,
                Name = "pinsitecare",
                RotateImage = true

            });

            IsBusy = false;

            return file?.Path;
        }

        /// <summary>
        /// Select a photo from the device gallery
        /// </summary>
        /// <returns></returns>
        private async Task<string> PickPhoto()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert(Resources.PatientApp.LblPickPhoto_PinSiteCare_Title, Resources.PatientApp.LblPickPhoto_PinSiteCare_Message, Resources.PatientApp.LblPickPhoto_PinSiteCare_OK);
                return null;
            }

            var status = await Permissions.RequestAsync<Permissions.StorageRead>();
            if (status != PermissionStatus.Granted)
            {
                ShowErrorMessage(Resources.PatientApp.LblPickPhoto_PinSiteCare_Title, Resources.PatientApp.LblPickPhoto_PinSiteCare_Message);
                return null;
            }

            status = await Permissions.RequestAsync<Permissions.Photos>();
            if (status != PermissionStatus.Granted)
            {
                ShowErrorMessage(Resources.PatientApp.LblPickPhoto_PinSiteCare_Title, Resources.PatientApp.LblPickPhoto_PinSiteCare_Message);
                return null;
            }

            IsBusy = true;
            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
            {
                RotateImage = true
            });

            IsBusy = false;

            return file?.Path;

        }

        private void ShareImageCommandExecute()
        {
            DependencyService.Get<IShareMediaService>().ShareImage(ProfileImagePath);
        }

        private void PlayVideoCommandExecute()
        {
            App.NavigationController.NavigateTo(NavigationController.PIN_SITE_CARE_VIDEO);
        }

        private void DoneCommandExecute()
        {
            var reminder = _dbService.GetRemindersAtDate(_sysUtility.Now, Reminder.ReminderType.PinSiteCareReminder).FirstOrDefault();
            var expiredReminders = _dbService.GetRemindersToDate(_sysUtility.Now, Reminder.ReminderType.PinSiteCareReminder).ToList();

            // Delete all expired reminders (we can have reminder not set in previously days)
            if (_dbService.DeleteReminders(expiredReminders))
            {
                // Add new item in log history to mark this pin site care done by user.
                var historyItem = new LogHistoryItem()
                {
                    LocalEntityId = null,
                    ExpectedDateTime = reminder.DateTime,
                    EventDateTime = _sysUtility.Now,
                    ItemType = LogHistoryItem.ItemTypeEnum.PinSiteCareDone,
                    Description = Resources.PatientApp.LblMessagePinSiteCareDone,
                    ServerEntityId = null,
                };
                _dbService.SaveHistoryLogItem(historyItem);
                RefreshDone();
                App.NavigationController.NavigateToHome();
            }
            else
            {
                AppLoggerHelper.LogEvent("PinSiteCare", "Error deleting PinSiteCare Reminder:"+_dbService.LastException?.Message, TraceLevel.Error);
            }

        }

    }

}
