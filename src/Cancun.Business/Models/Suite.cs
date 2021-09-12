using System;
using System.Collections.Generic;

namespace Cancun.Business.Models
{
    public class Suite : Entity
    {
        public Guid HotelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool Active { get; set; }

        /* EF Relations */
        public Hotel Hotel { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }
    }
}