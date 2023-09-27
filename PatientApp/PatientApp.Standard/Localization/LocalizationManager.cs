using System.Collections.Generic;
using System.Resources;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;
using PatientApp.ViewModels;
using System.Linq;

namespace PatientApp.Localization
{
  /// <summary>
  /// Application scoped Localization Manager based on embedded Resource files (resx)
  /// </summary>
  public class LocalizationManager
  {
    private static Dictionary<string, CultureInfo> _ciCache;
    private static string _currentLangCode;

    private static CultureInfo _currentCultureInfo;
    public static CultureInfo CurrentCultureInfo
    {
      get
      {
        return _currentCultureInfo;
      }
    }

    private static List<LanguageItem> _availableLanguages = null;
    public static List<LanguageItem> AvailableLanguages
    {
      get
      {
        if (_availableLanguages == null)
        {
          _availableLanguages = new List<LanguageItem>();
          _availableLanguages.Add(new LanguageItem("en-US", "English", "flag_uk.png"));
          _availableLanguages.Add(new LanguageItem("it-IT", "Italiano", "flag_ita.png"));
          _availableLanguages.Add(new LanguageItem("de-DE", "Deutsch", "flag_ger.png"));
          _availableLanguages.Add(new LanguageItem("fr-FR", "Français", "flag_fra.png"));
          _availableLanguages.Add(new LanguageItem("es-ES", "Español", "flag_spa.png"));
        }
        return _availableLanguages;
      }
    }

    static LocalizationManager()
    {
      _ciCache = new Dictionary<string, CultureInfo>();
    }

    public static string GetCurrentLanguageCode()
    {
      return _currentLangCode;
    }

    /// <summary>
    /// Set the selected culture for returned localized values
    /// </summary>
    /// <param name="isoCode"></param>
    public static void SetCurrentLanguage(string isoCode)
    {
      var isLanguageSelectionEnabled = PCLAppConfig.ConfigurationManager.AppSettings["IsLanguageSelectionEnabled"] == "1";
      if (string.IsNullOrEmpty(isoCode))
      {
        if (isLanguageSelectionEnabled)
        {
          // Get device OS language if none has been specified
          //isoCode = System.Globalization.CultureInfo.CurrentUICulture.Name;
          isoCode = DependencyService.Get<ICultureInfo>().DeviceOSCultureIsoCode;
        }
        else
        {
          // Language selection disabled: english forced
          isoCode = "en-US";
        }
      }

      // If the language is not supported, use english as default 
      if (!AvailableLanguages.Any(l => l.Code == isoCode))
        _currentLangCode = "en-US";
      else
        _currentLangCode = isoCode;

      if (!_ciCache.ContainsKey(_currentLangCode))
      {
        _currentCultureInfo = new CultureInfo(_currentLangCode);
        _ciCache.Add(_currentLangCode, _currentCultureInfo);
      }
      else
        _currentCultureInfo = _ciCache[_currentLangCode];

      CultureInfo.DefaultThreadCurrentCulture = _currentCultureInfo;
      CultureInfo.DefaultThreadCurrentUICulture = _currentCultureInfo;
      DependencyService.Get<ICultureInfo>().CurrentCulture = _currentCultureInfo;
      DependencyService.Get<ICultureInfo>().CurrentUICulture = _currentCultureInfo;

      Translator.Instance.Invalidate();
      //ImageTranslator.Instance.Invalidate();
    }


    /// <summary>
    /// Get a localized text by its reasource key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetText(string key)
    {
      ResourceManager temp = new ResourceManager("PatientApp.Resources.PatientApp", typeof(LocalizationManager).GetTypeInfo().Assembly);

      string result = null;
      try
      {
        result = temp.GetString(key, CurrentCultureInfo);
      }
      catch
      {

      }

      return result ?? key;
    }

    /// <summary>
    /// Get a localized text by its reasource key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetImage(string key)
    {
      ResourceManager temp = new ResourceManager("PatientApp.Resources.Images", typeof(LocalizationManager).GetTypeInfo().Assembly);

      string result = null;
      try
      {
        result = temp.GetString(key, CurrentCultureInfo);
      }
      catch
      {

      }

      return result ?? key;
    }

    /// <summary>
    /// Get the country iso code taken from the sim provider
    /// </summary>
    /// <returns></returns>
    public static string GetSIMCountry()
    {
      return DependencyService.Get<ICultureInfo>().SIMCountryIso;
    }




  }
}
