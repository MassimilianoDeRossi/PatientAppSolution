using Java.Lang;
using System;
using System.Linq;

namespace PatientApp.Droid.Utilities
{
  public static class ExecuteAsRootHelper
  {
    public static bool CanRunRootCommands()
    {
      bool retval = false;

      Process suProcess;

      try
      {
        suProcess = Runtime.GetRuntime().Exec("su");
        var os = new Java.IO.DataOutputStream(suProcess.OutputStream);
        var osRes = new Java.IO.DataInputStream(suProcess.InputStream);

        if (null != os && null != osRes)
        {
          os.WriteBytes("id\n");
          os.Flush();
          string currUid = osRes.ReadLine();
          bool exitSu = false;

          if (null == currUid)
          {
            retval = false;
            exitSu = false;
            Console.WriteLine("Can't get root access or denied by user");
          }
          else if (true == currUid.Contains("uid=0"))
          {
            retval = true;
            exitSu = true;
            Console.WriteLine("Root access granted");
          }
          else
          {
            retval = false;
            exitSu = true;
            Console.WriteLine("Root access rejected: " + currUid);
          }

          if (exitSu)
          {
            os.WriteBytes("exit\n");
            os.Flush();
          }
        }
      }

      catch (Java.Lang.Exception e)
      {
        retval = false;
        Console.WriteLine("Root access rejected [" + e.Class.Name + "] : " + e.Message);
      }

      return retval;

    }



    public static bool IsRooted()
    {
      bool IsRooted = false;
      //NLog.ILogger Logger = NLog.LogManager.GetCurrentClassLogger();

      try
      {
        var paths = new[]
        {
                    "/system/app/Superuser.apk",
                    "/sbin/su",
                    "/system/bin/su",
                    "/system/xbin/su",
                    "/data/local/xbin/su",
                    "/data/local/bin/su",
                    "/system/sd/xbin/su",
                    "/system/bin/failsafe/su",
                    "/data/local/su",
                    "/su/bin/su",
                    "/system/xbin/which"
                 };

        IsRooted = paths.Any<string>(System.IO.File.Exists);

        if (IsRooted)
        {
          //Logger.Info("Application can not run on rooted device");
        }

        return IsRooted;
      }
      catch (System.Exception e)
      {
        //Logger.Info("Can not detect system files. Considering not rooted : {0}", e.Message);
      }

      return IsRooted;

    }

  }

}