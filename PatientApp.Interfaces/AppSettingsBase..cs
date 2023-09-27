using SQLite;

namespace PatientApp.Interfaces
{
    public abstract class AppSettingsBase : IAppSettings
    {
        public abstract SQLiteConnection CreateSqLiteConnection();
        public string DatabaseFilename { get; } = "Database.db3";
    }

}
