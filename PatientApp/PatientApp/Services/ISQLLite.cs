using SQLite;

namespace PatientApp.Services
{
    /// <summary>
    /// Dependency service used to implement SqlLite connection creation
    /// </summary>
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
