using System;
using Xamarin.Forms;
using System.IO;
using PatientApp.Services;
using SQLite;
using System.Threading.Tasks;

[assembly: Dependency(typeof(PatientApp.iOS.Services.SQLite))]

namespace PatientApp.iOS.Services
{
  public class SQLite : ISQLite
  {
    public SQLite()
    {
    }

    #region ISQLite implementation
    public global::SQLite.SQLiteConnection GetConnection()
    {
      var sqliteFilename = "PatientApp.db3";
      string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
      string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
      var path = Path.Combine(libraryPath, sqliteFilename);

      var conn = new global::SQLite.SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache | SQLiteOpenFlags.FullMutex);
      return conn;
    }

    public SQLiteAsyncConnection GetAsyncConnection()
    {
      var sqliteFilename = "PatientApp.db3";
      string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
      var path = Path.Combine(documentsPath, sqliteFilename);
      var conn = new SQLiteAsyncConnection(path, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex | SQLiteOpenFlags.SharedCache);
      return conn;
    }

    #endregion
  }
}
