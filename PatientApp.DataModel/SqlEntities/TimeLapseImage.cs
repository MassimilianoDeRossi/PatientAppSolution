namespace PatientApp.DataModel.SqlEntities
{
  public class TimeLapseImage : BaseSqlEntity
  {
    public System.DateTime DateTime { get; set; }
    public string ImagePath { get; set; }
    public bool Deleted { get; set; }
  }
}
