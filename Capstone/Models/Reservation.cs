using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {
        /// <summary>
        /// The reservation id.
        /// </summary>
        public int ReservationId { get; set; }

        /// <summary>
        /// The campsite the reservation is for.
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// The name for the reservation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The starting date of the reservation.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The ending date of the reservation.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The date that the reservation was made.
        /// </summary>
        public DateTime? CreateDate { get; set; }
    }
}
