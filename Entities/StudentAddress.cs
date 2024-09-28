using System;
using System.Collections.Generic;

namespace WebApplication1.Entities
{
    public partial class StudentAddress
    {
        public int Id { get; set; }
        public int? StudentId { get; set; }
        public int? AddressId { get; set; }

        public virtual Address? Address { get; set; }
        public virtual Student? Student { get; set; }
    }
}
