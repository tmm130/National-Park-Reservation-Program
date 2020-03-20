using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Site
    {
        /// <summary>
        /// The site id.
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// The campground the park belongs to.
        /// </summary>
        public int CampgroundId { get; set; }

        /// <summary>
        /// The arbitrary campsite number.
        /// </summary>
        public int SiteNumber { get; set; }

        /// <summary>
        /// Max occupancy at the campsite.
        /// </summary>
        public int MaxOccupancy { get; set; }

        /// <summary>
        /// Indicates whether or not the campsite is handicap accessible.
        /// </summary>
        public bool Accessible { get; set; }

        /// <summary>
        /// The max rv length that the campsite can fit. 0 indicates that no rv will fit at this campsite.
        /// </summary>
        public int MaxRvLength { get; set; }

        /// <summary>
        /// Indicates whether or not the campsite provides access to utility hookup.
        /// </summary>
        public bool Utilities { get; set; }
    }
}
