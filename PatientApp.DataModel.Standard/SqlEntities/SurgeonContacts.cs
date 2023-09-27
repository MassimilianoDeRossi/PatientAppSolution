namespace PatientApp.DataModel.SqlEntities
{

    public class SurgeonContacts : BaseSqlEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string StateProvince { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Hospital { get; set; }
        public string MobilePhone { get; set; }
        public string OfficePhone { get; set; }
    }
}
