namespace ASP.NET.DEMO.API.Models.DTOS.SystemConfig.AddressBook
{
    public class DTOAddressBook
    {
        public int address_id { get; set; }
        public int? parent_id { get; set; }
        public string? address_name { get; set; }
        public string? alias_name { get; set; }
        public string? secondary_name { get; set; }
        public string? code { get; set; }
        public string? remarks { get; set; }
        public string? type { get; set; }
        public string? link { get; set; }
        public bool? is_active { get; set; }
        public DateTime? created_date { get; set; }
        public string? parent_name { get; set; }
    }
}
