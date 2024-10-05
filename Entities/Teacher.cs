using System;
using System.Collections.Generic;

namespace WebApplication1.Entities
{
    public partial class Teacher
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? AddressId { get; set; }
        public string? City { get; set; }
        public Address? Address { get; set; }
    }
}
