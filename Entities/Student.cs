using System;
using System.Collections.Generic;

namespace WebApplication1.Entities
{
    public partial class Student
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
    }
}
