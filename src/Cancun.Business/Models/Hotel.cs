using System;
using System.Collections.Generic;

namespace Cancun.Business.Models
{
    public class Hotel : Entity
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public Address Address { get; set; }
        public bool Active { get; set; }

        /* EF Relations */
        public IEnumerable<Suite> Suites { get; set; }
    }
}