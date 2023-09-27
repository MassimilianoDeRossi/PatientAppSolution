using Xamarin.Forms;

using PatientApp.Services;
using PatientApp.Droid.Utilities;

[assembly: Dependency(typeof(PatientApp.Droid.Services.DeviceIntegrityServiceImplementation))]
namespace PatientApp.Droid.Services
{
  public class DeviceIntegrityServiceImplementation : IDeviceIntegrityService
  {
    public bool IsSafe()
    {
      // detect rooted devices

      bool rootaccess = (ExecuteAsRootHelper.CanRunRootCommands() && ExecuteAsRootHelper.IsRooted());

      if (rootaccess)
      {
        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
      }

      return true;
    }
  }
}