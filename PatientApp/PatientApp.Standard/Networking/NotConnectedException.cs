using System;
using System.Runtime.Serialization;

namespace PatientApp.Networking
{
    /// <summary>
    /// Exception thrown on network unavailabilty
    /// </summary>
    [DataContract]
    internal class NotConnectedException : Exception
    {
        public NotConnectedException()
        {
        }

        public NotConnectedException(string message) : base(message)
        {
        }

        public NotConnectedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}