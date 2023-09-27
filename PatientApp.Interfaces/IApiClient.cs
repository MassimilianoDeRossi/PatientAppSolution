using MyHexPlanProxies.Models;
using PatientApp.DataModel.Networking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientApp.Interfaces
{
    /// <summary>
    /// HttpRestClient for Dentonet mobile
    /// </summary>
    public interface IApiClient
    {
        Task<bool> IsServerReachable();

        // GET TOKEN - POST (/token) : username, password, grant_type = password
        Task<BaseResponse<bool>> Login(string usernName, string password);

        /// <summary>
        /// Request to associate the current device to the current logged patient
        /// </summary>
        /// <returns></returns>
        Task<AssociateDeviceResponse> AssociateDevice();

        /// <summary>
        /// Request to update the device - patient association
        /// </summary>
        /// <returns></returns>
        Task<AssociateDeviceResponse> ChangeAssociatedDevice();


        /// <summary>
        /// Invoke webapi method for getting the prescription based on caseId and patientId
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        Task<GetPrescriptionResponse> GetPrescription(string caseId, string patientId);



        /// <summary>
        /// Invoke webapi method for downloading the package update for logged patient
        /// </summary>
        /// <returns></returns>
        Task<UpdatePackageResponse> DownloadUpdatePackage();


        Task<GetSettingsResponse> GetSettings();

        Task<SetSettingsResponse> UploadSettings(List<PatientDiaryEvent> events, TimeSpan pinSiteCareTime, bool isGoalEnabled, bool isMotivationalMessageEnabled, TimeSpan motivationaMessageTime, bool isPushEnabled,string appVersion);

        Task<SetSyncCompletedResponse> SetSyncCompleted();

        /// <summary>
        /// Invoke REST API method for downloading the signing certificate if newer exists
        /// </summary>
        /// <returns></returns>
        Task<GetSigningCertificateResponse> GetSigningCertificate(DateTime? refDate);
    }
}