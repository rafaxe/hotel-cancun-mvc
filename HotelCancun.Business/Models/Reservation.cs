using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelCancun.Business.Models
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

        //It is not in the constructor because of a limitation of the EF
        private void RecalculateDays()
        {
            Days = (int)(CheckOut - CheckIn).TotalDays;
        }

        public void RecalculatePrice()
        {
            RecalculateDays();
            PriceTotal = Suite?.Price * Days ?? 0;
        }
    }
}