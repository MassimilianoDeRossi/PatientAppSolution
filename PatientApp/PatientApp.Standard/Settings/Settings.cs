using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace PatientApp.Settings
{
  /// <summary>
  /// Settings static class
  /// Contains method for loading and saving application settings
  /// </summary>
  public static class Settings
  {
    public const string USER_SETTINGS_KEY = "USER_SETTINGS";

    private static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }

    /// <summary>
    /// Get a serialized object stored in settings 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T GetSerializedObject<T>(string key)
    {
      var stringValue = AppSettings.GetValueOrDefault(key, null);
      if (!string.IsNullOrEmpty(stringValue))
      {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(stringValue);
      }
      else
      {
        return default(T);
      }
    }

    /// <summary>
    /// Serialize and save and object in settings
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public static void SaveSerializedObject(string key, object obj)
    {
      var stringValue = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
      AppSettings.AddOrUpdateValue(key, stringValue);
    }

  }
}