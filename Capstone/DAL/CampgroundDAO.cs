using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class CampgroundDAO : ICampgroundDAO
    {
        private string ConnectionString;
        public CampgroundDAO(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public IList<Campground> GetCampgroundsAtPark(Park park)
        {
            List<Campground> campgrounds = new List<Campground>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string commandText = "select * from campground where park_id = @parkId order by name;";
                    SqlCommand cmd = new SqlCommand(commandText, conn);
                    cmd.Parameters.AddWithValue("@parkId", park.ParkId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground campground = ConvertReaderToCampground(reader);
                        campgrounds.Add(campground);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred getting the campgrounds. Sorry.");
            }

            return campgrounds;
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
    }
}
