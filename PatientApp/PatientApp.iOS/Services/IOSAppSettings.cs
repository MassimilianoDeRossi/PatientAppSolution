using System;
using Xamarin.Forms;
using PatientApp.iOS.Services;
using PatientApp.Interfaces;
using SQLite;
using System.IO;

[assembly: Dependency(typeof(IOSAppSettings))]
namespace PatientApp.iOS.Services
{
    public class IOSAppSettings :AppSettingsBase
    {
        public override SQLiteConnection CreateSqLiteConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, DatabaseFilename);            
            var connection = new SQLiteConnection(path);
            return connection;
        }
    }
}