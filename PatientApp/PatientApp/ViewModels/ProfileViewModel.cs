using System;
using System.Threading.Tasks;
using Xamarin.Forms;

using Xamarin.Essentials;
using Plugin.Media;

using PatientApp.Interfaces;
using PatientApp.Views;
using PatientApp.DataModel.SqlEntities;
using PatientApp.Settings;

namespace PatientApp.ViewModels
{
    /// <summary>
    /// ViewModel for user profile management
    /// </summary>
    public class ProfileViewModel : BaseViewModel
    {
        public Command EditNicknameCommand { get; set; }
        public Command SelectPhotoCommand { get; set; }
        public Command SurgeonInfoCommand { get; set; }
        public Command SaveSettingsCommand { get; set; }
        public Command<string> PlaceCallCommand { get; set; }
        public Command<string> SendSmsCommand { get; set; }

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
            set
            {
                SetProperty(ref _profileImagePath, value);
                OnPropertyChanged(nameof(SelectPhotoMessage));
            }
        }

        public string SelectPhotoMessage => (string.IsNullOrEmpty(ProfileImagePath)) ? Resources.PatientApp.LblAddYourPhoto_WizardUserProfile : Resources.PatientApp.LblChangeYourPhoto_WizardUserProfile;

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

        private SurgeonContacts _surgeonContactsInfo;
        public SurgeonContacts SurgeonContactsInfo
        {
            get { return _surgeonContactsInfo; }
            set
            {
                SetProperty(ref _surgeonContactsInfo, value);
                SurgeonInfoCommand.ChangeCanExecute();
                OnPropertyChanged(nameof(HasSurgeonInfo));
                OnPropertyChanged(nameof(CanCallMobile));
                OnPropertyChanged(nameof(CanCallOffice));
                OnPropertyChanged(nameof(CanSendMessage));
            }
        }

        public bool HasSurgeonInfo
        {
            get { return this.SurgeonContactsInfo != null; }
        }


        public bool CanCallOffice
        {
            get { return this.SurgeonContactsInfo != null && !string.IsNullOrEmpty(this.SurgeonContactsInfo.OfficePhone); }
        }

        public bool CanCallMobile
        {
            get { return this.SurgeonContactsInfo != null && !string.IsNullOrEmpty(this.SurgeonContactsInfo.MobilePhone); }
        }

        public bool CanSendMessage
        {
            get { return this.SurgeonContactsInfo != null && !string.IsNullOrEmpty(this.SurgeonContactsInfo.MobilePhone); }
        }

        public ProfileViewModel(ILocalDatabaseService dbService, IApiClient apiClient, ISystemUtility sysUtility) : base(dbService, apiClient, sysUtility)
        {
            EditNicknameCommand = new Command(EditNicknameCommandExecute);
            SelectPhotoCommand = new Command(SelectPhotoCommandExecute, SelectPhotoCommandCanExecute);
            SaveSettingsCommand = new Command(SaveSettingsCommandExecute);
            SurgeonInfoCommand = new Command(SurgeonInfoCommandExecute, () => { return this.HasSurgeonInfo; });
            PlaceCallCommand = new Command<string>(PlaceCallCommandExecute, (number) => { return !string.IsNullOrEmpty(number); });
            SendSmsCommand = new Command<string>(SendSmsCommandExecute, (number) => { return !string.IsNullOrEmpty(number); });

            MessagingCenter.Subscribe<BaseContentPage>(this, Messaging.Messages.VIEW_APPEARING_MESSAGE, (page) =>
            {
                if (page is ProfilePage)
                {
                    // Refresh data on view appearing
                    var settings = AppSettings.Instance;
                    IsGoalEnabled = settings.IsGoalEnabled;
                    PersonalGoal = settings.PersonalGoal;
                    IsInsightEnabled = settings.IsInsightEnabled;
                    PinSiteCareTime = settings.PinSiteCareTime;
                    Nickname = settings.Nickname ?? "Nickname";
                    ProfileImagePath = settings.ProfileImagePath;
                    this.SurgeonContactsInfo = _dbService.GetSurgeonContacts();
                    OnPropertyChanged(nameof(IsLoggedIn));
                }
                if (page is SurgeonContactPage)
                {
                }
            });

            MessagingCenter.Subscribe<BaseViewModel>(this, Messaging.Messages.SURGEON_CONTACTS_UPDATED, (vm) =>
            {
                // SurgeonContact Info has been updated: refresh data
                this.SurgeonContactsInfo = _dbService.GetSurgeonContacts();
            });
        }

        private void EditNicknameCommandExecute()
        {
            EntryPopup popup = new EntryPopup(Localization.LocalizationManager.GetText("LblNickname"),
            Localization.LocalizationManager.GetText("LblEnterNickname"),
            this.Nickname,
            Localization.LocalizationManager.GetText("BtnOK"),
            Localization.LocalizationManager.GetText("BtnCancel"))
            {
                MaxLength = 20
            };

            popup.PopupClosed += (o, closedArgs) =>
            {
                if (closedArgs.ButtonIndex == 0)
                {
                    this.Nickname = closedArgs.Text;
                    AppSettings.SetUserProfile(this.Nickname, this.ProfileImagePath);
                }
            };
            popup.Show();
        }

        private bool SelectPhotoCommandCanExecute()
        {
            return !IsBusy;
        }

        /// <summary>
        /// Take photo or pick from device gallery
        /// </summary>
        private async void SelectPhotoCommandExecute()
        {
            var optionTake = Localization.LocalizationManager.GetText("LblTakePhoto");
            var optionPick = Localization.LocalizationManager.GetText("LblPickPhoto");

            var result = await Application.Current.MainPage.DisplayActionSheet(
                Resources.PatientApp.LblAddYourPhoto_WizardUserProfile,
                Resources.PatientApp.BtnCancel,
                null,
                new[]
                {
                    optionTake,
                    optionPick
                });

            string filePath = null;
            if (result == optionTake)
                filePath = await TakePhoto();
            else if (result == optionPick)
                filePath = await PickPhoto();

            if (!string.IsNullOrEmpty(filePath))
            {
                // Assign selected photo to use profile and save in settings
                ProfileImagePath = filePath;
                AppSettings.SetUserProfile(this.Nickname, this.ProfileImagePath);
                OnPropertyChanged(nameof(SelectPhotoMessage));
            }
        }


        private async Task<string> PickPhoto()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert(Resources.PatientApp.LblPickPhoto_PrescriptionViewModel_Title, Resources.PatientApp.LblPickPhoto_PrescriptionViewModel_Message, Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_OK);
                return null;
            }

            var status = await Permissions.RequestAsync<Permissions.StorageRead>();
            if (status != PermissionStatus.Granted)
            {
                ShowErrorMessage(Resources.PatientApp.LblPickPhoto_PrescriptionViewModel_Title, Resources.PatientApp.LblPickPhoto_PrescriptionViewModel_Message);
                return null;
            }

            status = await Permissions.RequestAsync<Permissions.Photos>();
            if (status != PermissionStatus.Granted)
            {
                ShowErrorMessage(Resources.PatientApp.LblPickPhoto_PrescriptionViewModel_Title, Resources.PatientApp.LblPickPhoto_PrescriptionViewModel_Message);
                return null;
            }                

            IsBusy = true;
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions()
            {
                //RotateImage = true,
            });

            IsBusy = false;

            return file?.Path;

        }

        private async Task<string> TakePhoto()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert(Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Title, Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Message, Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_OK);
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
                ShowErrorMessage(Resources.PatientApp.LblPickPhoto_PrescriptionViewModel_Title, Resources.PatientApp.LblPickPhoto_PrescriptionViewModel_Message);
                return null;
            }


            status = await Permissions.RequestAsync<Permissions.Photos>();
            if (status != PermissionStatus.Granted)
            {
                ShowErrorMessage(Resources.PatientApp.LblPickPhoto_PrescriptionViewModel_Title, Resources.PatientApp.LblPickPhoto_PrescriptionViewModel_Message);
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

        /// <summary>
        /// Place a call to the number passed as argument
        /// </summary>
        /// <param name="number"></param>
        private void PlaceCallCommandExecute(string number)
        {
            if (string.IsNullOrEmpty(number))
                return;

            //var phoneDialer = CrossMessaging.Current.PhoneDialer;
            //if (phoneDialer.CanMakePhoneCall)
            //{
            //    phoneDialer.MakePhoneCall(number);
            //}
            //else
            //    App.Current.MainPage.DisplayAlert("Phone", Localization.LocalizationManager.GetText("ErrorCannotPlaceCall"), "OK");

            try
            {
                PhoneDialer.Open(number);
            }
            catch (ArgumentNullException ex)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                App.Current.MainPage.DisplayAlert("Phone", Localization.LocalizationManager.GetText("ErrorCannotPlaceCall"), "OK");
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Phone", Localization.LocalizationManager.GetText("ErrorCannotPlaceCall"), "OK");
            }
        }

        /// <summary>
        /// Send a SMS to the number passed as argument
        /// </summary>
        /// <param name="number"></param>
        private async void SendSmsCommandExecute(string number)
        {
            if (string.IsNullOrEmpty(number))
                return;

            //var smsMessenger = CrossMessaging.Current.SmsMessenger;
            //if (smsMessenger.CanSendSms)
            //    smsMessenger.SendSms(number);
            //else
            //    App.Current.MainPage.DisplayAlert("Sms", Localization.LocalizationManager.GetText("ErrorCannotSendSms"), "OK");

            try
            {
                var message = new SmsMessage(string.Empty, new[] { number });
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                await App.Current.MainPage.DisplayAlert("Sms", Localization.LocalizationManager.GetText("ErrorCannotSendSms"), "OK");                
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sms", Localization.LocalizationManager.GetText("ErrorCannotSendSms"), "OK");
            }

        }


        private void SaveSettingsCommandExecute()
        {
            AppSettings.SetAllSettings(Nickname, ProfileImagePath, PinSiteCareTime, IsInsightEnabled, InsightTime, IsGoalEnabled, PersonalGoal, _sysUtility.Now);
        }

        private void SurgeonInfoCommandExecute()
        {
            if (this.SurgeonContactsInfo != null)
            {
                App.NavigationController.NavigateTo(NavigationController.SURGEON_CONTACT);
            }
        }
    }
}
