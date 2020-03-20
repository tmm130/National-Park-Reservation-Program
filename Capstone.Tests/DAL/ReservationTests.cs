using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.DAL;
using Microsoft.Extensions.Configuration;
using System.IO;
using Capstone.Models;

namespace Capstone.Tests.DAL
{
    [TestClass]
    public class ReservationTests : NationalParkCapstone2TestInitialize
    {
        [TestMethod]
        public void FindAllSitesTest()
        {
            Campground campground = new Campground();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string commandText = $"Select * from campground where name = '{campName}'";
                SqlCommand cmd = new SqlCommand(commandText, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    campground = ConvertReaderToCampground(reader);
                }
            }

            IList<Site> sites = reservationDAO.FindAllSites(campground);

            Assert.IsTrue(sites.Count > 0);
        }

        [TestMethod]
        public void FindAllReservationsTest()
        {
            Site site = new Site();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string commandText = $"Select * from site where site_number = '{siteNumber}'";
                SqlCommand cmd = new SqlCommand(commandText, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    site = ConvertReaderToSite(reader);
                }
            }

            IList<Reservation> reservations = reservationDAO.FindAllReservations(site);

            Assert.IsTrue(reservations.Count > 0);
        }

        [TestMethod]
        public void FindAvailableSitesForReservationsTest()
        {
            Campground campground = new Campground();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string commandText = $"Select * from campground where name = '{campName}'";
                SqlCommand cmd = new SqlCommand(commandText, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    campground = ConvertReaderToCampground(reader);
                }
            }

            IList<Site> sites = reservationDAO.FindAvailableSitesForReservations(Convert.ToDateTime("2/26/3030"), Convert.ToDateTime("2/28/3030"), campground);

            Assert.IsTrue(sites.Count > 0);
        }

        [TestMethod]
        public void FindTotalCostTest()
        {
            DateTime arrival = Convert.ToDateTime("2/21/2020");
            DateTime departure = Convert.ToDateTime("2/28/2020");
            decimal expectedCost = 175M;

            Campground campground = new Campground();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string commandText = $"Select * from campground where name = '{campName}'";
                SqlCommand cmd = new SqlCommand(commandText, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    campground = ConvertReaderToCampground(reader);
                }
            }

            Assert.AreEqual(expectedCost, reservationDAO.FindTotalCost((departure - arrival).Days, campground));
        }

        [TestMethod]
        public void BookReservationTest()
        {
            Campground campground = new Campground();
            Site site = new Site();
            int lastReservationIdInSystem = 0;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string commandText = $"Select * from campground where name = '{campName}'";
                SqlCommand cmd = new SqlCommand(commandText, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    campground = ConvertReaderToCampground(reader);
                }
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string commandText2 = $"select * from site where site_number = '{siteNumber}'";
                SqlCommand cmd2 = new SqlCommand(commandText2, conn);
                SqlDataReader reader2 = cmd2.ExecuteReader();

                while (reader2.Read())
                {
                    site = ConvertReaderToSite(reader2);
                }
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string commandText3 = $"select reservation_id from reservation order by reservation_id desc;";
                SqlCommand cmd3 = new SqlCommand(commandText3, conn);
                lastReservationIdInSystem = Convert.ToInt32(cmd3.ExecuteScalar());
            }

            /* Increase last reservation Id by one because that's what we know the newly booked reservation should have as it's
                reservation Id */
            int expected = lastReservationIdInSystem + 1;
            string familyName = "Testing Family";
            DateTime arrivalDate = Convert.ToDateTime("5/22/4040");
            DateTime departureDate = Convert.ToDateTime("5/29/4040");

            Assert.AreEqual(expected, reservationDAO.BookReservation(campground, site, familyName, arrivalDate, departureDate));
        }

        private Campground ConvertReaderToCampground(SqlDataReader reader)
        {
            Campground campgrounds = new Campground();
            campgrounds.CampgroundId = Convert.ToInt32(reader["campground_id"]);
            campgrounds.ParkId = Convert.ToInt32(reader["park_id"]);
            campgrounds.Name = Convert.ToString(reader["name"]);
            campgrounds.OpenFromMonth = Convert.ToInt32(reader["open_from_mm"]);
            campgrounds.OpenUntilMonth = Convert.ToInt32(reader["open_to_mm"]);
            campgrounds.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
            return campgrounds;
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
