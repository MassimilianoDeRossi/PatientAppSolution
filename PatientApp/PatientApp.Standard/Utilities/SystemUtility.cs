using PatientApp.Interfaces;
using System;

namespace PatientApp.Utilities
{
    public class SystemUtility : ISystemUtility
    {
        public DateTime Now => DateTime.Now;
    }
}
