using System;

namespace PatientApp.DataModel.SqlEntities
{
    public enum SyncResultStateEnum
    {
        Unchanged,
        Added,
        Updated,
        Revoked
    }

    public class SyncResult : BaseSqlEntity
    {
        public Guid? CaseId { get; set; }
        public string FrameId { get; set; }
        public SyncResultStateEnum State { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
