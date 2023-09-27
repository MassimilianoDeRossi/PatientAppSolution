using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace PatientApp.Behaviors
{
    /// <summary>
    /// Behavior implementing validation on an entry
    /// You can specify max number of chars or a regex expression
    /// </summary>
    public class EntryValidationBehavior : Behavior<Entry>
    {
        private Entry attachedEntry = null;
        //private string specialCharsRegex = "[^a-zA-Z0-9_.]+";
        private string notAllowedCharsRegex = "[<>%#&?]+";
        private Regex regex;

        /// <summary>
        /// Max number of permitted chars
        /// </summary>
        public int? MaxLength { get; set; } = null;

        /// <summary>
        /// Permit special chars typing
        /// </summary>
        public bool AllowSpecialChars { get; set; } = false;

        public EntryValidationBehavior()
        {
            regex = new Regex(notAllowedCharsRegex);
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            attachedEntry = bindable;
            attachedEntry.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
            attachedEntry = null;
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            attachedEntry.TextChanged -= OnEntryTextChanged;

            var entry = (Entry)sender;

            if (entry.Text != null)
            {
                // if Entry text is longer then valid length
                if (this.MaxLength.HasValue && entry.Text.Length > this.MaxLength)
                {
                    string entryText = entry.Text;
                    entryText = entryText.Remove(entryText.Length - 1); // remove last char
                    entry.Text = entryText;
                }
                else if (!AllowSpecialChars)
                {
                    if (regex.IsMatch(entry.Text))
                    {
                        entry.Text = Regex.Replace(entry.Text, notAllowedCharsRegex, "");
                    }
                }
            }

            attachedEntry.TextChanged += OnEntryTextChanged;
        }
    }
}

