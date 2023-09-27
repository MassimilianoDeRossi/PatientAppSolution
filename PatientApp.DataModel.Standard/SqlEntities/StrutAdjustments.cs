using System;
using System.Collections.Generic;

namespace PatientApp.DataModel.SqlEntities
{

    public class StrutAdjustment : BaseSqlEntity
    {
        public Guid PrescriptionId { get; set; }
        public DateTime DateOfAdjustment { get; set; }
        public int TreatmentStepNumber { get; set; }

        public int? Click1 { get; set; }
        public int? Click2 { get; set; }
        public int? Click3 { get; set; }
        public int? Click4 { get; set; }
        public int? Click5 { get; set; }
        public int? Click6 { get; set; }

        public int? Length1 { get; set; }
        public int? Length2 { get; set; }
        public int? Length3 { get; set; }
        public int? Length4 { get; set; }
        public int? Length5 { get; set; }
        public int? Length6 { get; set; }

        public bool Done { get; set; }
        public DateTime? DoneDateTime { get; set; }

    }
}
