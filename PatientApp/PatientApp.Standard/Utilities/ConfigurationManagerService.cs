using System;
using System.IO;
using System.Reflection;
using PCLAppConfig;

namespace PatientApp.Utilities
{
    /// <summary>
    /// Service used to read from application settings file (app.config)
    /// </summary>
    public class ConfigurationManagerService : IConfigurationManager
    {
        private static readonly string _connectionStringPlain = "connectionString";


        static ConfigurationManagerService()
        {


            //Initialize app.config reader.
            var assembly = typeof(App).GetTypeInfo().Assembly;
            Stream certStream;
            using (certStream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".app.config"))
            {
                ConfigurationManager.Initialise(certStream);
            }

        }


        public static string GetconnectionString => ReadProperty(_connectionStringPlain);


        /// <summary>
        /// Method used to read app.config settings file through PCLApp Plugin.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string ReadProperty(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }


    }
}