using PatientApp.Interfaces;
using System;

namespace PatientApp.Utilities
{
    public class SystemUtilityFake : ISystemUtility
    {
        private DateTime ConvertStringDataToDateTime(string data)
        {
            var dates = data.Split(':');
            var hours = int.Parse(dates[0]);
            var minutes = int.Parse(dates[1]);
            var today = DateTime.Now;
            return new DateTime(today.Year, today.Month, today.Day, hours, minutes, 0);
        }

        public DateTime Now => App.TestModel.TimeToReturn != null ? ConvertStringDataToDateTime(App.TestModel.TimeToReturn) : DateTime.Now;
    }
}
