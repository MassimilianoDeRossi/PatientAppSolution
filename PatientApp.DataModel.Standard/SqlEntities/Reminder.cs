using System;

namespace PatientApp.DataModel.SqlEntities
{

    public class Reminder : BaseSqlEntity
    {
        public enum ReminderType
        {
            StrutAdjustmentReminder,
            PinSiteCareReminder
        }

        public System.DateTime DateTime { get; set; }
        public ReminderType Type { get; set; }
        public bool IsChecked { get; set; }
        public Guid PrescriptionId { get; set; }
        public Guid EntityId { get; set; }
    }
}
