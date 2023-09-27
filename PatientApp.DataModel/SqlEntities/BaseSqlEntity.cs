using SQLite;
using System;

namespace PatientApp.DataModel.SqlEntities
{
  public partial class BaseSqlEntity
  {
        [PrimaryKey]
        public Guid Id { get; set; }
  }
}
