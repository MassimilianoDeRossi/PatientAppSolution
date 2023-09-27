using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHexPlanProxies.Models
{
    /// <summary>
    /// App diary log event type
    /// </summary>
    public enum PatientLogHistoryType
    {
        /// <summary>
        /// Patient goal reached
        /// </summary>
        PersonalGoalSet = 10,

        /// <summary>
        /// PinSiteCare done
        /// </summary>
        PinSiteCareDone = 20,

        /// <summary>
        /// Strut adjustment done
        /// </summary>
        StrutAdjustmentDone = 30,

        /// <summary>
        /// Strut adjustment postponed
        /// </summary>
        StrutAdjustmentPostponed = 40,

        /// <summary>
        /// Motivational message read
        /// </summary>
        MotivationalMessageRead = 50,

        /// <summary>
        /// Daily mood survey answered
        /// </summary>
        DailyMoodSurveyAnswered = 60,

        /// <summary>
        /// App state changed
        /// </summary>
        AppStateChanged = 70,

        /// <summary>
        /// Prescription updated
        /// </summary>
        PrescriptionUpdated = 80,

        /// <summary>
        /// Enable/Disable motivational message flag
        /// </summary>
        MotivationalMessageFlag = 90,
    }

    public partial class PatientDiaryEvent
    {
    }
}
