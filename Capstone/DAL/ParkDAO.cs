using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ParkDAO : IParkDAO
    {
        private string ConnectionString;
        public ParkDAO(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IList<Park> GetAllParks()
        {
            List<Park> parks = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string commandText = "Select * from park";
                    SqlCommand cmd = new SqlCommand(commandText, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park park = ConvertReaderToPark(reader);
                        parks.Add(park);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred getting the parks. Sorry.");
            }

            return parks;
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
