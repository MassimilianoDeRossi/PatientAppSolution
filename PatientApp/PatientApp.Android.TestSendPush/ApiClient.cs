using MyHexPlanProxies.Models;
using Newtonsoft.Json;
using PatientApp.DataModel.Networking;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace PatientApp.Networking
{
    /// <summary>
    /// Client for REST API consuming
    /// </summary>
    public class ApiClient 
    {
        private string BaseAddress;
        private HttpRestClient _client;
        private string authenticationToken = null;

        public ApiClient()
        {
            _client = new HttpRestClient();
            BaseAddress = "https://myhexplanver.tlhex.com/";
            //if (string.IsNullOrEmpty(BaseAddress))
            //    BaseAddress = "https://myhexplandev.tlhex.com/";
        }
      

        /// <summary>
        /// Try to login using credentials
        /// </summary>
        /// <param name="usernName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<BaseResponse<bool>> Login(string usernName, string password)
        {
            var result = new BaseResponse<bool>();
            result.Success = false;

            try
            {
                var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", usernName),
                    new KeyValuePair<string, string>("password", password)
                };
                var content = new FormUrlEncodedContent(pairs);

                HttpResponseMessage response = null;
                string responseContent = null;
                using (var client = new HttpClient())
                {
                    var tokenEndpoint = new Uri(new Uri(BaseAddress), "Token");
                    response = await client.PostAsync(tokenEndpoint, content);
                    responseContent = await response.Content.ReadAsStringAsync();
                }

                if (response != null && !string.IsNullOrEmpty(responseContent))
                {
                    Dictionary<string, string> responseDictionary = null;
                    responseDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

                    if (response.IsSuccessStatusCode)
                    {
                        authenticationToken = responseDictionary["access_token"];
                        if (!string.IsNullOrEmpty(authenticationToken))
                        {
                            result.Success = true;
                        }
                        else
                        {
                            result.ErrorMessage = "Login Error. Please Contact service support. (ERROR 1)";
                        }
                    }
                    else if (responseDictionary != null)
                    {
                        if (responseDictionary.ContainsKey("error"))
                        {
                            if (responseDictionary["error"] == "invalid_grant")
                            {
                                result.ErrorCode = 1; // Not authorized
                            }
                            else
                            {
                                result.ErrorMessage = responseDictionary["error"];
                            }
                        }
                        else if (responseDictionary.ContainsKey("error_description"))
                        {
                            result.ErrorMessage = responseDictionary["error_description"];
                        }
                        else
                        {
                            result.ErrorMessage = "Login Error. Please Contact service support. (ERROR 2)";
                        }
                    }
                    else
                    {
                        result.ErrorMessage = "Login Error. Please Contact service support. (ERROR 3)";
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.ToString();
                result.Success = false;
                result.ErrorCode = -1;
                return result;
            }
            return result;
        }

      
        /// <summary>
        /// Invoke REST API method for downloading the signing certificate if newer exists
        /// </summary>
        /// <returns></returns>
        public async Task<GetSigningCertificateResponse> GetSigningCertificate(DateTime? refDate)
        {
            string url;

            if (refDate != null && refDate.HasValue)
                url = string.Format("/api/Mobile/LastCertificate?lastUpdate={0}", refDate.Value.ToString("yyyy-MM-dd"));
            else
                url = "/api/Mobile/LastCertificate";

            return await GetHttpAsyncWithCheckNull<GetSigningCertificateResponse, CertificateBinDTO>(_client, BaseAddress, url, authenticationToken);
        }

        private async Task<TResult> PostHttpAsyncWithCheckNull<TDataRequest, TResult, TDataResponse>(HttpRestClient client, string baseAddress, string requestUri, TDataRequest data = default(TDataRequest), string token = null) where TDataRequest : class, new() where TResult : class, new()
        {
            TResult result;
            try
            {
                result = await _client.PostHttpAsync<TDataRequest, TResult>(BaseAddress, requestUri, data, authenticationToken);
            }
            catch
            {
                result = new BaseResponse<TDataResponse>()
                {
                    Success = false,
                    ErrorCode = -1,
                    ErrorMessage = "Request failed",
                    Data = default(TDataResponse)
                } as TResult;
            }
            return result;
        }

        private async Task<TResult> GetHttpAsyncWithCheckNull<TResult, TDataResponse>(HttpRestClient client, string baseAddress, string requestUri, string token = null) where TResult : class, new()
        {
            TResult result;
            try
            {
                result = await _client.GetHttpAsync<TResult>(BaseAddress, requestUri, authenticationToken);
            }
            catch
            {
                result = new BaseResponse<TDataResponse>()
                {
                    Success = false,
                    ErrorCode = -1,
                    ErrorMessage = "Request failed",
                    Data = default(TDataResponse)
                } as TResult;
            }
            return result;
        }

    }
}