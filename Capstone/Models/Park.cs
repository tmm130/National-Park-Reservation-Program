using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Park
    {
        /// <summary>
        /// The park id.
        /// </summary>
        public int ParkId { get; set; }

        /// <summary>
        /// The name of the park.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The park's location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The park's date of establishment.
        /// </summary>
        public DateTime EstablishDate { get; set; }

        /// <summary>
        /// The area of the park
        /// </summary>
        public int Area { get; set; }
        
        /// <summary>
        /// The annual number of visitors to the park.
        /// </summary>
        public int Visitors { get; set; }

        /// <summary>
        /// A short description about the park.
        /// </summary>
        public string Description { get; set; }
    }
}
