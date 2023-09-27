using System;

namespace PatientApp.DataModel.SqlEntities
{

    public class Prescription : BaseSqlEntity
    {
        public Guid CaseId { get; set; }
        public string CaseName { get; set; }
        public string CaseNumber { get; set; }
        public int? AnatomiesType { get; set; }
        public int? BoneTypeSegment { get; set; }
        public string FrameID { get; set; }
        public DateTime? SurgeryDate { get; set; }
        public DateTime? StartOfTreatment { get; set; }
        public DateTime? EndOfTreatment { get; set; }
        public DateTime? RemovalDate { get; set; }
        public DateTime? LastSyncDate { get; set; }
        public string Notes { get; set; }
    }
}
