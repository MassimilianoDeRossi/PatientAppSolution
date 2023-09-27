using System;

namespace PatientApp.Interfaces
{
    /// <summary>
    /// Manage device system data. Created mainly for uitests to mock system time
    /// </summary>
    public interface ISystemUtility
    {
        /// <summary>
        /// Get system time (equals to DateTime.Now)
        /// </summary>
        DateTime Now { get; }
    }
}
