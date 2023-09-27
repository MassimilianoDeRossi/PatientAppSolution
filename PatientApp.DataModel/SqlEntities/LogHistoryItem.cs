using System;

namespace PatientApp.DataModel.SqlEntities
{
    public class LogHistoryItem : BaseSqlEntity
    {
        public enum ItemTypeEnum
        {
            StrutAdjustmentDone = 1,
            PinSiteCareDone = 2,
            PersonalGoalDisabledEnabled = 3,
            MotivationalMesssageRead = 4,
            AppStateChanged = 5,
            StrutAdjustmentPostponed = 6,
            MoodSelfAssessment = 7,
            PrescriptionUpdated = 8,
            MotivationalMesssageDisabledEnabled = 9,

            DebugTest = 100,
        }
        public Guid? LocalEntityId { get; set; }
        public System.DateTime? ExpectedDateTime { get; set; }
        public System.DateTime EventDateTime { get; set; }
        public ItemTypeEnum ItemType { get; set; }
        public string Description { get; set; }
        public int? ServerEntityId { get; set; }
        public bool Synced { get; set; }
    }
}
