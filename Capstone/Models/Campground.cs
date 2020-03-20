using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Campground
    {
        /// <summary>
        /// The campground id.
        /// </summary>
        public int CampgroundId { get; set; }

        /// <summary>
        /// The park id.
        /// </summary>
        public int ParkId { get; set; }

        /// <summary>
        /// The name of the campground.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The numerical month the campground is open for reservation.
        /// </summary>
        public int OpenFromMonth { get; set; }

        /// <summary>
        /// The numberical month the campground is closed for reservation.
        /// </summary>
        public int OpenUntilMonth { get; set; }

        /// <summary>
        /// The daily fee for booking a campsite at this campground.
        /// </summary>
        public decimal DailyFee { get; set; }
    }
}
