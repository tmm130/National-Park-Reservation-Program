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
    public class CampgroundTests : NationalParkCapstone2TestInitialize
    {
        [TestMethod]
        public void GetCampgroundsAtParkTest()
        {
            Park park = new Park();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string commandText = $"Select * from park where name = '{parkName}'";
                SqlCommand cmd = new SqlCommand(commandText, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    park = ConvertReaderToPark(reader);
                }
            }

            IList<Campground> campgrounds = campgroundDAO.GetCampgroundsAtPark(park);

            Assert.IsTrue(campgrounds.Count > 0);
        }

        private Park ConvertReaderToPark(SqlDataReader reader)
        {
            Park parks = new Park();
            parks.ParkId = Convert.ToInt32(reader["park_id"]);
            parks.Name = Convert.ToString(reader["name"]);
            parks.Location = Convert.ToString(reader["location"]);
            parks.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
            parks.Area = Convert.ToInt32(reader["area"]);
            parks.Visitors = Convert.ToInt32(reader["visitors"]);
            parks.Description = Convert.ToString(reader["description"]);
            return parks;
        }

    }
}
