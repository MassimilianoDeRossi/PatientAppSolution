using PatientApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp.Services
{
    /// <summary>
    /// Dependency service used to show a native EntryPopup
    /// </summary>
    public interface IEntryPopupLoader
    {
        void ShowPopup(EntryPopup reference);
    }
}
