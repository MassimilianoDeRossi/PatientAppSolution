using SQLite;

namespace PatientApp.Interfaces
{
    public interface IAppSettings
    {
        SQLiteConnection CreateSqLiteConnection();
        string DatabaseFilename { get; }
    }
}
