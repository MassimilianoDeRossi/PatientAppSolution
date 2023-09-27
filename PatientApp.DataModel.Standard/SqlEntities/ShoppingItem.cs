namespace PatientApp.DataModel.SqlEntities
{

  public class ShoppingItem : BaseSqlEntity
  {
    public string Description { get; set; }
    public bool IsChecked { get; set; }
  }
}
