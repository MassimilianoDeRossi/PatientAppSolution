using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PatientApp.ViewModels
{
    /// <summary>
    /// Class that describe Shopping Item
    /// </summary>
    public class ViewShoppingItem : DataModel.SqlEntities.ShoppingItem, INotifyPropertyChanged
    {
       /// <summary>
        /// Change image in listview if 
        /// </summary>
        public string StateImage
        {
            get
            {
                return IsChecked ? "ico_checkon" :
                "ico_checkoff";
            }
            set { }
        }

        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            var changed = PropertyChanged;

            changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ToggleChecked()
        {
            IsChecked = !IsChecked;
            OnPropertyChanged(nameof(StateImage));
        }
    }
}