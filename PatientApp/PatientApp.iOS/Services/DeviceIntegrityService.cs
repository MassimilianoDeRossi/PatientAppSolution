using System.Linq;

using Xamarin.Forms;
using Foundation;
using PatientApp.Services;
using UIKit;

[assembly: Dependency(typeof(PatientApp.iOS.Services.DeviceIntegrityServiceImplementation))]
namespace PatientApp.iOS.Services
{
  public class DeviceIntegrityServiceImplementation : IDeviceIntegrityService
  {
    public bool IsSafe()
    {
      Foundation.NSUrl url = new NSUrl("cydia://");
            
      bool canopenurl = UIApplication.SharedApplication.CanOpenUrl(url);
      bool isSimulator = false;

      if (ObjCRuntime.Runtime.Arch == ObjCRuntime.Arch.SIMULATOR)
      {
        isSimulator = true;
      }

      if (IsJailBroken() || canopenurl || isSimulator)
      {
        return false;
      }

      return true;
    }

    private bool IsJailBroken()
    {
      //NLog.ILogger Logger = NLog.LogManager.GetCurrentClassLogger();

      bool IsJailBroken = false;

      try
      {
        var paths = new[]
        {
                     "/Applications/Checkra1n.app",
                     "/Applications/blackra1n.app",
                     "/Applications/Cydia.app",
                     "/Applications/Icy.app",
                     "/Applications/IntelliScreen.app",
                     "/Applications/MxTube.app",
                     "/Applications/RockApp.app",
                     "/Applications/FakeCarrier.app",
                     "/Applications/SBSettings.app",
                     "/Applications/WinterBoard.app",
                     "/private/var/lib/cydia",
                     "/private/var/tmp/cydia.log",
                     "/private/var/lib/apt",
                     "/private/var/lib/apt/",
                     "/private/var/stash",
                     "/private/var/mobile/Library",
                     "/private/var/mobile/Library/SBSettings/Themes",
                     "/System/Library/LaunchDaemons/",
                     "/System/Library/LaunchDaemons/com.saurik.Cydia.Startup.plist",
                     "/System/Library/LaunchDaemons/com.ikey.bbot.plist",
                     "/Application/Preferences.app/General.plist",
                     "/usr/libexec/sftp-server",
                     "/usr/bin/sshd",
                     "/usr/sbin/sshd",
                     "/Library/MobileSubstrate/MobileSubstrate.dylib",
                     "/Library/MobileSubstrate/DynamicLibraries",
                     "/Library/MobileSubstrate/DynamicLibraries/LiveClock.plist",
                     "/Library/MobileSubstrate/DynamicLibraries/Veency.plist",
                     "/bin/hash",
                     "/var/cache/apt",
                     "/var/lib/apt",
                     "/var/lib/cydia",
                     "/var/log/syslog",
                     "/var/tmp/cydia.log",
                     "/bin/bash",
                     "/bin/sh",
                     "/usr/libexec/ssh-keysign",
                     "/usr/sbin/sshd",
                     "/usr/bin/sshd",
                     "/etc/ssh/sshd_config",
                     "/etc/apt",
                     "/jb"
                 };

        IsJailBroken = paths.Any(System.IO.File.Exists);
        return IsJailBroken;

      }

      catch (System.Exception e)
      {
      }

      return false;

    }
  }
}