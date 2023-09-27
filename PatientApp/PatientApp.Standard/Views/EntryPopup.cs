using PatientApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PatientApp.Views
{
    public class EntryPopupClosedArgs : EventArgs
    {
        public string Text { get; set; }
        public int ButtonIndex { get; set; }
    }

    public class EntryPopup
    {
        public string Text { get; set; }
        public string Title { get; set; }
        public string DefaultValue { get; set; }
        public List<string> Buttons { get; set; }

        public string NotAllowedCharsRegex { get; set; } = "[<>%#&?]+";
        public int? MaxLength { get; set; }

        public EntryPopup(string title, string text, string defaultValue, params string[] buttons)
        {
            Title = title;
            Text = text;
            DefaultValue = defaultValue;
            Buttons = buttons.ToList();
        }

        public EntryPopup(string title, string text) : this(title, text, "", "OK", "Cancel")
        {
        }

        public event EventHandler<EntryPopupClosedArgs> PopupClosed;
        public void OnPopupClosed(EntryPopupClosedArgs e)
        {
            var handler = PopupClosed;
            if (handler != null)
                handler(this, e);
        }

        public void Show()
        {
            DependencyService.Get<IEntryPopupLoader>().ShowPopup(this);
        }
    }
}
