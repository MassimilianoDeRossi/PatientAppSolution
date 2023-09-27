using PatientApp.Utilities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PatientApp.ViewModels
{

    /// <summary>
    ///  A class used to display a daily task description and time
    /// </summary>
    public class ViewDailyTask : ObservableObject
    {      
        private string _description = null;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private string _time = null;
        public string Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }
             
    }
}