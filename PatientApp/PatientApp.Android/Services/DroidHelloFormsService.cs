using PatientApp.Interfaces;


namespace PatientApp.Droid.Services
{
	public class DroidHelloFormsService : IHelloFormsService
	{
        //public SQLite.SQLiteConnection GetDBPath(string sqlLiteFileName)
        //{
        //    return new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), sqlLiteFileName)); 
        //}

        public string GetHelloFormsText ()
		{
			return "Hello from Android!";
		}
	}
}