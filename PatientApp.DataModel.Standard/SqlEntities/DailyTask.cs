using System;

namespace PatientApp.DataModel.SqlEntities
{
    public class DailyTask
    {
        public DateTime? When { get; set; }

        public string Instruction { get; set; }

    }
}