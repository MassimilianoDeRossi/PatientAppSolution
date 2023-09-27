using System.IO;
using System.Threading.Tasks;

using Xamarin.Forms;
using SQLite;
using PatientApp.Droid.Services;
using PatientApp.Services;

[assembly: Dependency(typeof(SQLiteImplementation))]

namespace PatientApp.Droid.Services
{
  public class SQLiteImplementation : ISQLite
  {    

    public SQLiteImplementation()
    {
    }

    #region ISQLite implementation
    public global::SQLite.SQLiteConnection GetConnection()
    {
      var sqliteFilename = "PatientApp.db3";
      string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
      var path = Path.Combine(documentsPath, sqliteFilename);
      var conn = new SQLiteConnection(path, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex);
      // Return the database connection 
      return conn;
    }

    public SQLiteAsyncConnection GetAsyncConnection()
    {
      var sqliteFilename = "PatientApp.db3";
      string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
      var path = Path.Combine(documentsPath, sqliteFilename);
      var conn = new SQLiteAsyncConnection(path, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex | SQLiteOpenFlags.SharedCache);      
      // Return the async database connection 
      return conn;
    }
    #endregion

  }
}