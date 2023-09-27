using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PatientApp.Droid.Services;
using PatientApp.Services;
using PatientApp.Views;
using System.Text.RegularExpressions;

[assembly: Dependency(typeof(EntryPopupLoader))]
namespace PatientApp.Droid.Services
{
    class EntryPopupLoader : IEntryPopupLoader
    {
        private string notAllowedCharsRegex = null;
        private Regex regex = null;
        private int? maxLength = null;

        public void ShowPopup(EntryPopup popup)
        {
            if (!string.IsNullOrEmpty(popup.NotAllowedCharsRegex))
            {
                notAllowedCharsRegex = popup.NotAllowedCharsRegex;
                regex = new Regex(notAllowedCharsRegex);
            }
            maxLength = popup.MaxLength;

            //var context = Forms.Context;
            var context = MainActivity.Instance;

            var alert = new AlertDialog.Builder(context);

            var edit = new EditText(context) { Text = popup.DefaultValue };
            edit.SetSingleLine();
            FrameLayout container = new FrameLayout(context);
            var layoutParams = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            layoutParams.SetMargins(60, 0, 60, 0);
            edit.LayoutParameters = layoutParams;
            container.AddView(edit);
            edit.TextChanged += TextChanged;
            edit.SetSelection(edit.Length());
            alert.SetView(container);

            alert.SetTitle(popup.Title);
            alert.SetMessage(popup.Text);

            //these 3 if can be replaced with a view with custom buttons
            if (popup.Buttons.Count > 0)
            {
                alert.SetPositiveButton(popup.Buttons[0], (senderAlert, args) =>
                {
                    popup.OnPopupClosed(new EntryPopupClosedArgs
                    {
                        Text = edit.Text,
                        ButtonIndex = 0
                    });
                });
            }

            if (popup.Buttons.Count > 1)
            {
                alert.SetNeutralButton(popup.Buttons[1], (senderAlert, args) =>
                {
                    popup.OnPopupClosed(new EntryPopupClosedArgs
                    {
                        Text = edit.Text,
                        ButtonIndex = 1
                    });
                });
            }

            if (popup.Buttons.Count > 2)
            {
                alert.SetNegativeButton(popup.Buttons[2], (senderAlert, args) =>
                {
                    popup.OnPopupClosed(new EntryPopupClosedArgs
                    {
                        Text = edit.Text,
                        ButtonIndex = 2
                    });
                });
            }

            alert.Show();
        }

        private void TextChanged(object sender, EventArgs e)
        {
            var textField = sender as EditText;

            if (regex != null && regex.IsMatch(textField.Text))
            {
                int cursorPosition = textField.SelectionStart;
                textField.Text = Regex.Replace(textField.Text, notAllowedCharsRegex, "");
                textField.SetSelection(Math.Min(20, cursorPosition));
            }
            if (maxLength.HasValue && textField.Text.Length > maxLength.Value)
            {
                int cursorPosition = textField.SelectionStart;
                textField.Text = textField.Text.Substring(0, maxLength.Value);
                textField.SetSelection(Math.Min(20, cursorPosition));
            }
        }
    }
}