﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cancun.Business.Models
{
    public class Reservation : Entity
    {
        public Guid SuiteId { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Days { get; set; }
        public decimal PriceTotal { get; set; }

        /* EF Relations */
        [ForeignKey("SuiteId")]
        public virtual Suite Suite { get; set; }
    }
}