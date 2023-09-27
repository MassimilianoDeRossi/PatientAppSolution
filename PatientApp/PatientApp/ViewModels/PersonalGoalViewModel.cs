using System.Collections.ObjectModel;
using PatientApp.Settings;
using PatientApp.Views;
using Xamarin.Forms;

namespace PatientApp.ViewModels
{
    /// <summary>
    /// Handling personal goal set up by the patient.
    /// </summary>
    public class PersonalGoalViewModel : BaseViewModel
    {

        public string PersonalGoal { get; set; }   

        private bool _isEnabled;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        public PersonalGoalViewModel() : base (null, null, null)
        {
            PersonalGoal = "Personal Goal";
            IsEnabled = true;

        }

     
    }
    
}
