using MyHexPlanProxies.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PatientApp.DataModel.Networking
{
    #region Request
    public class AuthenticationRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

    }

    public class BaseRequest
    {
        public string AuthenticationToken { get; set; }
    }

    public class GetPrescriptionRequest : BaseRequest
    {
        public Guid CaseId { get; set; }
        public string DeviceId { get; set; }
    }
    #endregion

    #region Response


    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }

    public class AssociateDeviceResponse : BaseResponse<bool>
    {

    }


    public class GetPrescriptionResponse : BaseResponse<IList<PrescriptionDTO>>
    {

    }

    public class UpdatePackageResponse : BaseResponse<PackageUpdate>
    {

    }

    public class GetSettingsResponse : BaseResponse<PortalSettingsDTO>
    {

    }

    public class SetSettingsResponse : BaseResponse<object>
    {

    }

    public class SetSyncCompletedResponse : BaseResponse<object>
    {

    }

    public class GetSigningCertificateResponse : BaseResponse<CertificateBinDTO>
    {

    }

    #endregion
}
