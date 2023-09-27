namespace PatientApp.Services
{
    /// <summary>
    /// Dependency Service used to manage device orientation
    /// </summary>
    public interface IOrientationManager
    {
        /// <summary>
        /// Force the orientation to landscape
        /// </summary>
        void ForceLandscape();

        /// <summary>
        /// Force the orientation to portrait
        /// </summary>
        void ForcePortrait();

        /// <summary>
        /// Disable previously forced orientation and enable automatic mode (device sensor based)
        /// </summary>
        void DisableForcedOrientation();
    }
}
