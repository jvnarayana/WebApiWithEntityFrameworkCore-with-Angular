using System;
using System.Collections.Generic;

namespace WebApplication1.Entities
{
    public partial class Address
    {
        public Address()
        {
            StudentAddresses = new HashSet<StudentAddress>();
        }

        public string? StreetNumber { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public int? Zipcode { get; set; }
        public int AddressId { get; set; }

        public virtual ICollection<StudentAddress> StudentAddresses { get; set; }
    }
}
