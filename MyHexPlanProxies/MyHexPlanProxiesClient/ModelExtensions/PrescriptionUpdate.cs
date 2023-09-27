using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyHexPlanProxies.Models
{
    public enum PrescriptionState
    {
        Unchanged = 0,
        Added = 10,
        Revoked = 20,
        Updated = 30
    }

    public partial class PrescriptionUpdate
    {
        [JsonIgnore]
        public int AddedCount
        {
            get
            {

                return this.Changes != null ? this.Changes.Count(c => c.State == (int)PrescriptionState.Added) : 0;
            }
        }

        [JsonIgnore]
        public int UpdatedCount
        {
            get
            {

                return this.Changes != null ? this.Changes.Count(c => c.State == (int)PrescriptionState.Updated) : 0;
            }
        }

        [JsonIgnore]
        public int RevokedCount
        {
            get
            {

                return this.Changes != null ? this.Changes.Count(c => c.State == (int)PrescriptionState.Revoked) : 0;
            }
        }

        [JsonIgnore]
        public bool HasChanges
        {
            get
            {

                return this.AddedCount > 0 || this.UpdatedCount > 0 || this.RevokedCount > 0;
            }
        }
    }
}
