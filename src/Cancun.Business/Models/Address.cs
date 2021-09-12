using System;

namespace Cancun.Business.Models
{
    public class Address : Entity
    {
        public Guid HotelId { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string ZipCode { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string Province { get; set; }

        /* EF Relation */
        public Hotel Hotel { get; set; }
    }
}