using PatientApp.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using PatientApp.Utilities;
using PatientApp.DataModel;
using PatientApp.DataModel.Networking;

namespace GCMSendPush
{
    class Program
    {
        static string APIKey = "AIzaSyB6EEb8_dYdd3lVDjqxPQfsjT9cnnmitQg";

        private static string SendGCMNotification(string key, string postData, string postDataContentType = "application/json")
        {
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

            //
            //  MESSAGE CONTENT
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            var baseAddress = "https://gcm-http.googleapis.com";
            var endpoint = new Uri(baseAddress + "/gcm/send");
            //
            //  CREATE REQUEST
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(endpoint);
            Request.Method = "POST";
            Request.KeepAlive = false;
            Request.ContentType = postDataContentType;
            Request.Headers.Add(string.Format("Authorization: key={0}", key));
            Request.ContentLength = byteArray.Length;

            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //
            //  SEND MESSAGE
            try
            {
                WebResponse Response = Request.GetResponse();
                HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                string statusCode = string.Empty;
                if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                {
                    statusCode = "Unauthorized - need new token";
                }
                else if (!ResponseCode.Equals(HttpStatusCode.OK))
                {
                    statusCode = "Response from web service isn't OK";
                }
                Console.WriteLine("STATUS CODE: " + statusCode);
                StreamReader Reader = new StreamReader(Response.GetResponseStream());
                string responseLine = Reader.ReadToEnd();
                Console.WriteLine("RESPONSE: " + responseLine);
                Reader.Close();

                return responseLine;
            }
            catch (Exception)
            {
            }
            return "error";
        }

        private static string SendFCMNotification(string key, string postData, string postDataContentType = "application/json")
        {
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

            //
            //  MESSAGE CONTENT
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            var baseAddress = "https://fcm.googleapis.com";
            var endpoint = new Uri(baseAddress + "/fcm/send");
            //
            //  CREATE REQUEST
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(endpoint);
            Request.Method = "POST";
            Request.KeepAlive = false;
            Request.ContentType = postDataContentType;
            Request.Headers.Add(string.Format("Authorization: key={0}", key));
            Request.ContentLength = byteArray.Length;

            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //
            //  SEND MESSAGE
            try
            {
                WebResponse Response = Request.GetResponse();
                HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                string statusCode = string.Empty;
                if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                {
                    statusCode = "Unauthorized - need new token";
                }
                else if (!ResponseCode.Equals(HttpStatusCode.OK))
                {
                    statusCode = "Response from web service isn't OK";
                }
                Console.WriteLine("STATUS CODE: " + statusCode);
                StreamReader Reader = new StreamReader(Response.GetResponseStream());
                string responseLine = Reader.ReadToEnd();
                Console.WriteLine("RESPONSE: " + responseLine);
                Reader.Close();

                return responseLine;
            }
            catch (Exception)
            {
            }
            return "error";
        }

        public static bool ValidateServerCertificate(
                                                    object sender,
                                                    X509Certificate certificate,
                                                    X509Chain chain,
                                                    SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        static void Main(string[] args)
        {
            //string deviceId = "cASDAj8AnOY:APA91bGWZyxNfv3BcLAvRwVXW7lvmPkwC0kQWagOpkuwUKi3-HdXT0t3g2O6FquW7LPPZAXvNiT2D9JaJFNE1BD02R14DZc8MSRcgh_VPvHLCHdYPeW7ssJZ1C7NudsfLpjeydLOUZqd";

            //if (args.Length > 0)
            //{
            //    deviceId = args[0];
            //}

            //TestFCM(deviceId);

            TestSign();

        }

        private static void TestSign()
        {
            var code = System.IO.File.ReadAllText(@"d:\temp\Orthofix\testQrCode.txt");

            var request = WebRequest.Create("https://myhexplanver.tlhex.com/api/Mobile/LastCertificate");

            var response = request.GetResponse();

            string json;
            using (Stream stream = response.GetResponseStream())   
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                json = reader.ReadToEnd();
            }

            var certResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<GetSigningCertificateResponse>(json);

            SigningCertHelper.SetCertificate(certResponse.Data.Certificate, "gabnicpwd1230");
            
            var content = Newtonsoft.Json.JsonConvert.DeserializeObject<PrescriptionQrCode>(code);

            var check = SigningCertHelper.Verify(content.Info, content.Check);

        }

        private static void TestGCM(string deviceId)
        {
            // var data = new { to = "/topics/global", data = new { message = "Test myHEXPlan", OtherData = new { Code = "123", Description = "Test myHEXPlan Description" } } };

            Console.WriteLine("Sending notification to device id {0}", deviceId);

            int notificationId = 123456;

            var data = new
            {
                registration_ids = new string[] { deviceId },
                collapse_key = "KEY_123",
                time_to_live = 180,
                data = new
                {
                    id = notificationId,
                    message = "Test da console delle " + DateTime.Now.ToLongTimeString()
                }
            };

            var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            string result = SendGCMNotification(APIKey, jsonData);
        }

        private static void TestFCM(string deviceId)
        {
            // var data = new { to = "/topics/global", data = new { message = "Test myHEXPlan", OtherData = new { Code = "123", Description = "Test myHEXPlan Description" } } };

            Console.WriteLine("Sending notification to device id {0}", deviceId);

            int notificationId = 123456;

            var data = new
            {
                registration_ids = new string[] { deviceId },
                collapse_key = "KEY_123",
                time_to_live = 180,
                data = new
                {
                    forceStart = "1",
                    id = notificationId,
                    message = "Test da console delle " + DateTime.Now.ToLongTimeString()
                }
            };

            var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            string result = SendFCMNotification(APIKey, jsonData);
        }
    }
}
