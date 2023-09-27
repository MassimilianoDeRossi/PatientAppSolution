using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PatientApp.Utilities;

namespace PatientApp.ViewModels
{
    public class LanguageItem : ObservableObject
    {
        private string _code = null;
        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }

        private string _Name = null;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private string _iconName = null;
        public string IconName
        {
            get { return _iconName; }
            set { SetProperty(ref _iconName, value); }
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set 
            { 
                SetProperty(ref _isSelected, value);
                OnPropertyChanged(StateImage);
            }
        }

        /// <summary>
        /// Change image in listview if item is selected
        /// </summary>
        public string StateImage
        {
            get
            {
                return IsSelected ? "ico_checkon" : "ico_checkoff";
            }
            set { }
        }


        public LanguageItem()
        {

        }

        public LanguageItem(string code, string name, string iconName)
        {
            this.Code = code;
            this.Name = name;
            this.IconName = iconName;
        }

    }
}
