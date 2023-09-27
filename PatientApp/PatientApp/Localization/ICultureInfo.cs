using System.Globalization;

namespace PatientApp.Localization
{
    /// <summary>
    /// It constains property to get and set the localization of  
    /// </summary>
    public interface ICultureInfo
    {
        /// <summary>
        /// Get/Set the current culture
        /// </summary>
        CultureInfo CurrentCulture { get; set; }
        /// <summary>
        /// Get/Set the current UI culture
        /// </summary>
        CultureInfo CurrentUICulture { get; set; }

        /// <summary>
        /// Get the SIM (and so, the carrier operator) country code in ISO format
        /// </summary>
        string SIMCountryIso {get;}     
    }

}
