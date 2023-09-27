using PatientApp.Interfaces;
using System;
using SQLite;

namespace PatientApp.Services
{
    public class AppSettings : IAppSettings
    {
        public string DatabaseFilename => throw new NotImplementedException();

        public SQLiteConnection CreateSqLiteConnection()
        {
            throw new NotImplementedException();
        }
    }
}
