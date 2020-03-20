using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        /// <summary>
        /// Finds all of the reservations that have been made at a given site.
        /// </summary>
        /// <returns></returns>
        IList<Reservation> FindAllReservations(Site site);

        /// <summary>
        /// Finds the total cost of the stay using the amount of days wanted to be booked at the campground chosen.
        /// </summary>
        /// <param name="daysStayed"></param>
        /// <param name="campgroundId"></param>
        /// <returns></returns>
        decimal FindTotalCost(int daysStayed, Campground campground);

        /// <summary>
        /// Book the reservation for the customer with their name and date's to and from booked and returns confirmation number.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        int BookReservation(Campground campground, Site site, string nameOnReservation, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Finds all of the sites at a given campground.
        /// </summary>
        /// <param name="campground"></param>
        /// <returns></returns>
        IList<Site> FindAllSites(Campground campground);

        /// <summary>
        /// Returns top 5 sites available for reservation with a given arrival date, departure date, and campground.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="campground"></param>
        /// <returns></returns>
        IList<Site> FindAvailableSitesForReservations(DateTime startDate, DateTime endDate, Campground campground);
    }
}
