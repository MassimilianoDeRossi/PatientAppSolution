using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp.DataModel
{
    [Table("Setting")]
    public class SettingsModel
    {
        [PrimaryKey]
        public string Id { get; set; }
        [MaxLength(200)]
        public string Value { get; set; }
    }
}
