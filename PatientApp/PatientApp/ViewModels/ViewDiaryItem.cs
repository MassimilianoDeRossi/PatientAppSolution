using PatientApp.Utilities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PatientApp.ViewModels
{
    /// <summary>
    /// Class that describe Shopping Item
    /// </summary>
    public class ViewDiaryItem : ObservableObject
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