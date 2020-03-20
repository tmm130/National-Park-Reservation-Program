using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationDAO : IReservationDAO
    {
        private string ConnectionString;
        public ReservationDAO(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IList<Site> FindAvailableSitesForReservations(DateTime startDate, DateTime endDate, Campground campground)
        {
            List<Site> sites = new List<Site>();
            //IList<Reservation> reservations = new List<Reservation>();

            //foreach (Site getSite in sites)
            //{
            //    reservations = FindAllReservations(getSite);
            //}

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string commandText = "select distinct top 5 * from site where campground_id = @campgroundId and site_id not in (select site_id from reservation where @startDate <= to_date and @endDate >= from_date); ";
                    SqlCommand cmd = new SqlCommand(commandText, conn);
                    cmd.Parameters.AddWithValue("@campgroundId", campground.CampgroundId);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    cmd.Parameters.AddWithValue("@startMonth", startDate.Month);
                    cmd.Parameters.AddWithValue("@endMonth", endDate.Month);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site site = ConvertReaderToSite(reader);
                        sites.Add(site);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("We had trouble connecting you with a list of sites. Sorry.");
            }

            return sites;
        }

        public int BookReservation(Campground campground, Site site, string nameOnReservation, DateTime startDate, DateTime endDate)
        {
            int reservationId = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string commandText = $"insert into reservation values (@siteId, @name, @startDate, @endDate, @createDate); select Scope_Identity();";
                    SqlCommand cmd = new SqlCommand(commandText, conn);
                    cmd.Parameters.AddWithValue("@siteId", site.SiteId);
                    cmd.Parameters.AddWithValue("@name", nameOnReservation);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    cmd.Parameters.AddWithValue("@createDate", DateTime.Now);
                    reservationId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch
            {
                Console.WriteLine("We had trouble making your reservation. Sorry.");
            }

            return reservationId;
        }

        public IList<Site> FindAllSites(Campground campground)
        {
            List<Site> sites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string commandText = "select * from site where campground_id = @campgroundId";
                    SqlCommand cmd = new SqlCommand(commandText, conn);
                    cmd.Parameters.AddWithValue("@campgroundId", campground.CampgroundId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site site = ConvertReaderToSite(reader);
                        sites.Add(site);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Sorry there was a problem getting the available sites.");
            }

            return sites;
        }

        public decimal FindTotalCost(int daysStayed, Campground campground)
        {
            decimal totalCost = campground.DailyFee * daysStayed;
            return totalCost;
        }


        public IList<Reservation> FindAllReservations(Site site)
        {
            List<Reservation> reservations = new List<Reservation>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string commandText = "select * from reservation where site_id = @siteId";
                    SqlCommand cmd = new SqlCommand(commandText, conn);
                    cmd.Parameters.AddWithValue("@siteId", site.SiteId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation reservation = ConvertReaderToReservation(reader);
                        reservations.Add(reservation);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Sorry there was a problem getting the available sites.");
            }

            return reservations;
        }

        private Site ConvertReaderToSite(SqlDataReader reader)
        {
            Site site = new Site();
            site.SiteId = Convert.ToInt32(reader["site_id"]);
            site.CampgroundId = Convert.ToInt32(reader["campground_id"]);
            site.SiteNumber = Convert.ToInt32(reader["site_number"]);
            site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
            site.Accessible = Convert.ToBoolean(reader["accessible"]);
            site.MaxRvLength = Convert.ToInt32(reader["max_rv_length"]);
            site.Utilities = Convert.ToBoolean(reader["utilities"]);
            return site;
        }

        private Reservation ConvertReaderToReservation(SqlDataReader reader)
        {
            Reservation reservation = new Reservation();
            reservation.ReservationId = Convert.ToInt32(reader["reservation_id"]);
            reservation.SiteId = Convert.ToInt32(reader["site_id"]);
            reservation.Name = Convert.ToString(reader["name"]);
            reservation.StartDate = Convert.ToDateTime(reader["from_date"]);
            reservation.EndDate = Convert.ToDateTime(reader["to_date"]);
            reservation.CreateDate = Convert.ToDateTime(reader["create_date"]);
            return reservation;
        }


    }
}
