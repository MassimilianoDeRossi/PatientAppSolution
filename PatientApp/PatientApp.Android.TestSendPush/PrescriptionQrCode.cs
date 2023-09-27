using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp.DataModel
{
  public class PrescriptionQrCode
  {
    public class PrescriptionQrCodeInfo
    {
      [JsonProperty(PropertyName = "C")]
      public string CaseId { get; set; }

      [JsonProperty(PropertyName = "P")]
      public string PatientId { get; set; }
    }

    [JsonProperty(PropertyName = "Chk")]
    public string Check { get; set; }

    [JsonProperty(PropertyName = "Info")]
    public PrescriptionQrCodeInfo Info { get; set; }
  }

}
