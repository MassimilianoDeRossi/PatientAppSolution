namespace PatientApp.DataModel.SqlEntities
{

    public class UserPreferences : BaseSqlEntity
    {
        public bool GoalDisabled { get; set; }
        public bool InsightDisabled { get; set; }
        public int? MoodIndex { get; set; }
        public System.DateTime? LastMoodDateTime { get; set; }
    }
}
