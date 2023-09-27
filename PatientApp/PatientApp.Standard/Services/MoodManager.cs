using PatientApp.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp.Services
{
    /// <summary>
    /// Mood images and labels manager
    /// </summary>
    public class MoodManager
    {
        // An item containing texts and images for a mood 
        public class MoodItem
        {

            /// <summary>
            /// Text description associated with mood.
            /// </summary>
            public string MoodText { get; set; }
            /// <summary>
            /// Image Icon associated with mood.
            /// </summary>
            public string MoodIcon { get; set; }
            /// <summary>
            /// Label associated with mood.
            /// </summary>
            public string MoodLabel { get; set; }
            /// <summary>
            /// Emoticon associated with mood.
            /// </summary>
            public string EmojiCode { get; set; }

            public override string ToString()
            {
                return string.Format(Localization.LocalizationManager.GetText("FormatTodayIFeel"), Localization.LocalizationManager.GetText(EmojiCode));
            }
        }

        private static List<MoodItem> _moodList = new List<MoodItem>(9)
        {
            new MoodItem()
            {
                MoodIcon = "mood_01_annoyed",
                MoodLabel = "LblMoodAnnoyed_HowDoYouFeel",
                MoodText = "LblMoodAnnoyedText_HowDoYouFeel",
                EmojiCode = "LblMoodAnnoyedEmoji_HowDoYouFeel"

            },
            new MoodItem()
            {
                MoodIcon = "mood_02_nervous",
                MoodLabel = "LblMoodNervous_HowDoYouFeel",
                MoodText = "LblMoodNervousText_HowDoYouFeel",
                EmojiCode = "LblMoodNervousEmoji_HowDoYouFeel"
            },
            new MoodItem()
            {
                MoodIcon = "mood_03_sad",
                MoodLabel = "LblMoodSad_HowDoYouFeel",
                MoodText = "LblMoodSadText_HowDoYouFeel",
                EmojiCode = "LblMoodSadEmoji_HowDoYouFeel"
            },
            new MoodItem()
            {
                MoodIcon = "mood_04_bored",
                MoodLabel = "LblMoodBored_HowDoYouFeel",
                MoodText = "LblMoodBoredText_HowDoYouFeel",
                EmojiCode = "LblMoodBoredEmoji_HowDoYouFeel"
            },
            new MoodItem()
            {
                MoodIcon = "mood_05_neutral",
                MoodLabel = "LblMoodNeutral_HowDoYouFeel",
                MoodText = "LblMoodNeutralText_HowDoYouFeel",
                EmojiCode = "LblMoodNeutralEmoji_HowDoYouFeel"
            },
            new MoodItem()
            {
                MoodIcon = "mood_06_calm",
                MoodLabel = "LblMoodCalm_HowDoYouFeel",
                MoodText = "LblMoodCalmText_HowDoYouFeel",
                EmojiCode = "LblMoodCalmEmoji_HowDoYouFeel"
            },
            new MoodItem()
            {
                MoodIcon = "mood_07_satisfied",
                MoodLabel = "LblMoodSatisfied_HowDoYouFeel",
                MoodText = "LblMoodSatisfiedText_HowDoYouFeel",
                EmojiCode =  "LblMoodSatisfiedEmoji_HowDoYouFeel"
            },
            new MoodItem()
            {
                MoodIcon = "mood_08_happy",
                MoodLabel = "LblMoodHappy_HowDoYouFeel",
                MoodText = "LblMoodHappyText_HowDoYouFeel",
                EmojiCode = "LblMoodHappyEmoji_HowDoYouFeel"
            },
            new MoodItem()
            {
                MoodIcon = "mood_09_excited",
                MoodLabel = "LblMoodExcited_HowDoYouFeel",
                MoodText = "LblMoodExcitedText_HowDoYouFeel",
                EmojiCode = "LblMoodExcitedEmoji_HowDoYouFeel"
            }
        };

        /// <summary>
        /// The list of mood items
        /// </summary>
        public static List<MoodItem> MoodList
        {
            get
            {
                return _moodList;
            }
        }

        /// <summary>
        /// Get saved mood index on specific date
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int? GetMoodIndexAtDateTime(DateTime dt)
        {
            var settings = AppSettings.Instance;

            if (settings.LastMoodDateTime.HasValue && settings.LastMoodDateTime.Value.Date == dt.Date && settings.MoodIndex.HasValue)
            {
                return settings.MoodIndex.Value;
            }
            return null;
        }

        /// <summary>
        /// Get saved mood item on specific date
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetMoodAtDateTime(DateTime dt)
        {
            var index = GetMoodIndexAtDateTime(dt);
            if (index.HasValue)
                return MoodList[index.Value].ToString();
            else
                return Localization.LocalizationManager.GetText("BtnHowDoYouFeelToday");
        }

        /// <summary>
        ///  Get saved mood text on specific date
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetMoodTextAtDateTime(DateTime dt)
        {
            var index = GetMoodIndexAtDateTime(dt);
            if (index.HasValue)
                return MoodList[index.Value].MoodText;
            else
                return null;
        }

        /// <summary>
        /// Get text of a mood referred by its index in the list
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetMoodTextByIndex(int? index)
        {
            if (index.HasValue && index.Value >= 0)
                return MoodList[index.Value].MoodText;
            else
                return null;
        }

    }
}
