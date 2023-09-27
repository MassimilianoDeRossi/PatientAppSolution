using System;
using System.Linq;

using UIKit;

using PatientApp.Services;
using PatientApp.Views;
using PatientApp.iOS.Services;
using System.Text.RegularExpressions;

[assembly: Xamarin.Forms.Dependency(typeof(EntryPopupLoader))]
namespace PatientApp.iOS.Services
{
    public class EntryPopupLoader : IEntryPopupLoader
    {
        private string notAllowedCharsRegex = null;
        private Regex regex = null;
        private int? maxLength = null;

        public EntryPopupLoader()
        {

        }

        public void ShowPopup(EntryPopup popup)
        {
            if (!string.IsNullOrEmpty(popup.NotAllowedCharsRegex))
            {
                notAllowedCharsRegex = popup.NotAllowedCharsRegex;
                regex = new Regex(notAllowedCharsRegex);
            }
            maxLength = popup.MaxLength;

            var alert = new UIAlertView
            {
                Title = popup.Title,
                Message = popup.Text,
                AlertViewStyle = UIAlertViewStyle.PlainTextInput                 
            };

            var textField = alert.GetTextField(0);
            textField.Text = popup.DefaultValue;
            textField.AddTarget(TextChanged, UIControlEvent.EditingChanged);            

            foreach (var b in popup.Buttons)
                alert.AddButton(b);

            alert.Clicked += (s, args) =>
            {
                popup.OnPopupClosed(new EntryPopupClosedArgs
                {
                    ButtonIndex = Convert.ToInt32(args.ButtonIndex),
                    Text = textField.Text
                });
            };
            alert.Show();
        }

        private void TextChanged(object sender, EventArgs e)
        {
            var textField = sender as UITextField;
            if (regex != null && regex.IsMatch(textField.Text))
            {
                textField.Text = Regex.Replace(textField.Text, notAllowedCharsRegex, "");
            }
            if (maxLength.HasValue && textField.Text.Length > maxLength.Value)
            {
                textField.Text = textField.Text.Substring(0, maxLength.Value);
            }
        }
    }
}
