using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PatientApp.Networking
{
    /// <summary>
    /// Base Client for REST API Consuming
    /// </summary>
    public class HttpRestClient
    {
        public bool IsConnected
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Check for network connection availability
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsInternetAvailable()
        {
            bool check = false;

            string CheckUrl = "http://google.com";

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(3);
                try
                {
                    var response = await client.GetAsync(CheckUrl);
                    response.EnsureSuccessStatusCode();
                    check = response.IsSuccessStatusCode;
                }
                catch
                {

                }
            }
            return check;

        }

        /// <summary>
        /// Perform a GET request
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="baseAddress"></param>
        /// <param name="requestUri"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TResult> GetHttpAsync<TResult>(string baseAddress, string requestUri, string token = null)
        {
            if (IsConnected)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    try
                    {
                        var response = await client.GetAsync(requestUri);
                        response.EnsureSuccessStatusCode();
                        if (response.StatusCode == HttpStatusCode.OK && response.Content != null)
                        {
                            var value = await response.Content.ReadAsStringAsync();
                            if (value != null)
                            {
                                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<TResult>(value);
                                return result;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                }
            }
            else
                throw new Exception();

            return default(TResult);
        }

        /// <summary>
        /// Perform a POST request
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="baseAddress"></param>
        /// <param name="requestUri"></param>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TResult> PostHttpAsync<TData, TResult>(string baseAddress, string requestUri, TData data = default(TData), string token = null) where TData : class, new()
        {
            if (IsConnected)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string sData = data != null ? Newtonsoft.Json.JsonConvert.SerializeObject(data) : string.Empty;
                    HttpContent content = new StringContent(sData, System.Text.Encoding.UTF8, "application/json");
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    try
                    {
                        var response = await client.PostAsync(requestUri, content);
                        response.EnsureSuccessStatusCode();
                        var value = await response.Content.ReadAsStringAsync();
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<TResult>(value);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                        return default(TResult);
                    }
                }
            }
            else
                throw new Exception();
        }

    }
}
