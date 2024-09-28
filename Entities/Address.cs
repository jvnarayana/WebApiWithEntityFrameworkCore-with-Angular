using System;
using System.Collections.Generic;

namespace WebApplication1.Entities
{
    public partial class Address
    {
        public int AddressId { get; set; }
        public string? StreetNumber { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public int? Zipcode { get; set; }
    }
}
