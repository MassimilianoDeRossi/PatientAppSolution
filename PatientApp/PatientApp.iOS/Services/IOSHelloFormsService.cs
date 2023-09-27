using PatientApp.Interfaces;

namespace IoCDemo.iOS
{
	public class IOSHelloFormsService : IHelloFormsService
	{
        //public SQLiteConnection GetDBPath(string sqliteFilename)
        //{
        //    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
        //    string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder instead
        //    var path = Path.Combine(libraryPath, sqliteFilename);
        //    return new SQLiteConnection(path);
        //}

        public string GetHelloFormsText ()
		{
			return "Hello from iOS!";
		}	
	}
}