using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using Plugin.Media;
using Plugin.Connectivity;
using Newtonsoft.Json;

using Xamarin.Forms;

using PatientApp.Interfaces;
using PatientApp.Settings;
using PatientApp.DataModel;
using PatientApp.Utilities;
using Xamarin.Essentials;

namespace PatientApp.ViewModels
{
    /// <summary>
    /// ViewModel used from prescription scan 
    /// </summary>
    public class PrescriptionViewModel : BaseViewModel
    {
        public string ScannedCode { get; set; }

        public Command ScanCommand { get; set; }
        public Command ShowHelpCommand { get; set; }
        public Command SkipScanCommand { get; set; }

        public PrescriptionViewModel(ILocalDatabaseService dbService, IApiClient apiClient, ISystemUtility sysUtility) : base(dbService, apiClient, sysUtility)
        {
            ScanCommand = new Command(ScanCommandExecute);
            ShowHelpCommand = new Command(ShowHelpCommandExecute);
            SkipScanCommand = new Command(SkipScanCommandExecute, SkipScanCommandCanExecute);

            MessagingCenter.Subscribe<string>(this, Messaging.Messages.PRESCRIPTION_CODE_SCANNED, async (code) =>
            {
                // A qrcode has been scanned, go back
                await App.Current.MainPage.Navigation.PopAsync();
                PrescriptionCodeScanned(code);
            });
        }

        private void ShowHelpCommandExecute()
        {
            App.NavigationController.NavigateTo(NavigationController.PRESCRIPTION_HELP_POPUP, true);
        }

        private async void ScanCommandExecute()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (!App.IsCertificateChecked)
                {
                    // Check again for signing certificate
                    await UpdateSigningCertificate(_apiClient);
                }

                if (App.IsCertificateChecked)
                {
                    var status = await Permissions.RequestAsync<Permissions.Camera>();
                    if (status == PermissionStatus.Granted)
                        App.NavigationController.NavigateTo(NavigationController.QRCODE_SCAN);
                    else
                        ShowErrorMessage(Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Title, Resources.PatientApp.LblTakePhoto_PrescriptionViewModel_Message);
                }

                else
                    ShowErrorMessage(Resources.PatientApp.LoginTitleError, /*Resources.PatientApp.ErrorConnectionMissing*/"Signing certificate has not been downloaded. Please check your connection and retry.");
            }
            else
            {
                ShowErrorMessage(Resources.PatientApp.LoginTitleError, Resources.PatientApp.ErrorConnectionMissing);
            }
        }

        private bool SkipScanCommandCanExecute()
        {
            return true;
        }

        private void SkipScanCommandExecute()
        {
            App.NavigationController.NavigateTo(NavigationController.WIZARD_USER_SETTINGS);
        }

        /// <summary>
        /// Analize scanned code, login, associate device and download prescription 
        /// </summary>
        /// <param name="code"></param>
        private async void PrescriptionCodeScanned(string code)
        {
            ScannedCode = code;

            if (!string.IsNullOrEmpty(ScannedCode))
            {
                PrescriptionQrCode content = null;
                // Deserialized scanned json 
                try
                {
                    content = Newtonsoft.Json.JsonConvert.DeserializeObject<PrescriptionQrCode>(code);
                }
                catch (Exception ex)
                {
                    AppLoggerHelper.LogException(ex, "Failed to Deserialize scanned prescription qrcode", TraceLevel.Error);
                }

                if (content != null && content.Info != null)
                {
                    // Check signing
                    if (VerifySign(content.Info, content.Check))
                    {
                        // Store values in UserSettings
                        AppSettings.SetPatientInfo(Guid.Parse(content.Info.PatientId), Guid.Parse(content.Info.CaseId));
                        if (CrossConnectivity.Current.IsConnected)
                        {
                            IsBusy = true;
                            var loginResult = await _apiClient.Login(AppSettings.Instance.GetApiUserName(), AppSettings.Instance.GetApiPassword());
                            //ShowErrorMessage("error", loginResult.ErrorMessage); // DA RIMUOVERE
                            //ShowErrorMessage("core", loginResult.ErrorCode.ToString()); // DA RIMUOVERE
                            IsBusy = false;
                            if (loginResult.Success)
                            {

                                IsBusy = true;
                                // Try to associate device to current user
                                var associateResult = await _apiClient.AssociateDevice();
                                IsBusy = false;
                                if (associateResult.Success)
                                {
                                    var associated = true;
                                    if (!associateResult.Data)
                                    {
                                        // Association failed: Another device has already been associate to the patient
                                        // Ask user if he want to use this new device
                                        var useNewDevice = await App.Current.MainPage.DisplayAlert(Resources.PatientApp.AlertMsgTitle_PrescriptionViewModel, Resources.PatientApp.AlertAlreadyRegisteredDevice_PrescriptionViewModel, Resources.PatientApp.AlertAlreadyRegisteredDeviceYes_PrescriptionViewModel, Resources.PatientApp.AlertAlreadyRegisteredDeviceNo_PrescriptionViewModel);
                                        if (useNewDevice)
                                        {
                                            IsBusy = true;
                                            associateResult = await _apiClient.ChangeAssociatedDevice();
                                            IsBusy = false;
                                            if (associateResult.Success)
                                            {
                                                associated = associateResult.Data;
                                            }
                                            else
                                            {
                                                AppLoggerHelper.LogEvent("ChangeAssociatedDevice", "Error changing associated device:: " + associateResult.ErrorMessage ?? "(unknown)", TraceLevel.Error);
                                                associated = false;
                                            }
                                        }
                                        else
                                        {
                                            // Aborted
                                            await App.Current.MainPage.DisplayAlert(Resources.PatientApp.AlertMsgTitle_PrescriptionViewModel, Resources.PatientApp.AlertDeviceRegistrationCancelled, Resources.PatientApp.BtnOK);
                                            return;
                                        }
                                    }
                                    if (associated)
                                    {
                                        // Device associated: we can go on
                                        IsBusy = true;
                                        var updateResult = await _apiClient.DownloadUpdatePackage();
                                        IsBusy = false;
                                        if (updateResult.Success)
                                        {
                                            // Check sign
                                            if (VerifySign(updateResult.Data.Updates, updateResult.Data.Sign))
                                            {
                                                var count = 0;
                                                if (updateResult.Data.Updates.Prescriptions != null)
                                                {
                                                    count = updateResult.Data.Updates.Prescriptions.Count;
                                                    if (await _dbService.SaveDownloadedPrescriptionsUpdate(updateResult.Data.Updates))
                                                    {
                                                        var syncCompletedResult = await _apiClient.SetSyncCompleted();
                                                        if (syncCompletedResult.Success)
                                                        {

                                                            // set a flag if the prescription has struts older then today
                                                            // it will be used in strut adjustment recap for asking user to skip or not                                                            
                                                            if (await _dbService.ExistStrutAdjustmentsToDate(_sysUtility.Now.Date.AddDays(-1)))
                                                            {
                                                                AppSettings.SetShowStrutsSkippedWarning(true);
                                                            }
                                                            AppSettings.SetSyncLastDateTime(_sysUtility.Now);

                                                            // Force reminder calendar rebuilding/rescheduling 
                                                            AppSettings.SetReminderLastUpdate(_sysUtility.Now);
                                                        }
                                                        else
                                                        {
                                                            AppLoggerHelper.LogEvent("Prescription Scan Error", "Error sending sync completed: " + syncCompletedResult.ErrorMessage ?? "(unknown)", TraceLevel.Error);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        AppLoggerHelper.LogEvent("Prescription Save Error", "Error saving downloaded prescription: " + _dbService.LastException.Message, TraceLevel.Error);
                                                    }
                                                }

                                                if (!(App.TestModel != null && App.TestModel.TestModeOn && App.TestModel.ForceSkipQrScan)) //added to skip in test mode
                                                {
                                                    await App.Current.MainPage.DisplayAlert(Resources.PatientApp.PrescriptionScanInfoTitle, Resources.PatientApp.DownloadedPrescriptionPrefix + count, PatientApp.Resources.PatientApp.BtnOK);
                                                }

                                                if (count > 0)
                                                {
                                                    // if at least one prescription has been downloaded, set user as logged in 
                                                    AppSettings.SetLoginState(true);
                                                    NotifyLoginState(true);

                                                    if (!(App.TestModel != null && App.TestModel.TestModeOn && App.TestModel.ForceSkipQrScan)) //added to skip in test mode
                                                    {
                                                        App.NavigationController.NavigateTo(NavigationController.WIZARD_USER_SETTINGS);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                AppLoggerHelper.LogEvent("SignError", "Downloaded update has wrong sign", TraceLevel.Error);
                                                ShowErrorMessage(Resources.PatientApp.ErrorPrescriptionScanTitle, Resources.PatientApp.ErrorPrescriptionInvalidSignedPackage);
                                            }
                                        }
                                        else
                                        {
                                            AppLoggerHelper.LogEvent("ApiError", "Error downloading update package:" + (updateResult.ErrorMessage ?? "(unknown error)"), TraceLevel.Error);
                                            ShowErrorMessage(Resources.PatientApp.ErrorPrescriptionScanTitle, Resources.PatientApp.ErrorPrescriptionGeneric);
                                        }
                                    }
                                    else
                                    {
                                        AppLoggerHelper.LogEvent("DeviceActivation", "Failed device activation", TraceLevel.Error);
                                        ShowErrorMessage(Resources.PatientApp.ErrorPrescriptionScanTitle, Resources.PatientApp.DeviceActivationFailedError);
                                    }
                                }
                                else
                                {
                                    AppLoggerHelper.LogEvent("DeviceActivation", "Failed device association", TraceLevel.Error);
                                    ShowErrorMessage(Resources.PatientApp.ErrorPrescriptionScanTitle, Resources.PatientApp.DeviceActivationFailedRetryError);
                                }
                            }
                            else
                            {
                                AppLoggerHelper.LogEvent("Login", "Login failed on prescription qrcode scan:" + loginResult.ErrorMessage, TraceLevel.Error);
                                ShowErrorMessage(Resources.PatientApp.LoginTitleError, Resources.PatientApp.ErrorPrescriptionLogin);
                            }
                        }
                        else
                        {
                            ShowErrorMessage(Resources.PatientApp.LoginTitleError, Resources.PatientApp.ErrorConnectionMissing);
                        }
                    }
                    else
                    {
                        AppLoggerHelper.LogEvent("SignError", "Scanned prescription qrcode has wrong sign", TraceLevel.Warning);
                        ShowErrorMessage(Resources.PatientApp.ErrorPrescriptionScanTitle, Resources.PatientApp.ErrorPrescriptionInvalidSignedQrCode);
                    }
                }
                else
                {
                    ShowErrorMessage(Resources.PatientApp.ErrorPrescriptionScanTitle, Resources.PatientApp.ErrorPrescriptionInvalidQrCodeContent);
                    return;
                }

            }
        }


    }
}
