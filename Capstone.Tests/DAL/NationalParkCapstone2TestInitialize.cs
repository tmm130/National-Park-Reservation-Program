using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.DAL;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Capstone.Tests.DAL
{
    [TestClass]
    public class NationalParkCapstone2TestInitialize
    {
        protected TransactionScope transaction;
        protected ParkDAO parkDAO;
        protected CampgroundDAO campgroundDAO;
        protected ReservationDAO reservationDAO;

        protected string ConnectionString
        {
            get
            {
                return "Server=.\\SQLEXPRESS;Database=npcampground;Trusted_Connection=True;";
            }
        }

        protected const string parkName = "New Park";
        protected const string campName = "New Camp";
        protected const int siteNumber = 12312;
        protected const string reservationName = "Made up test name";

        [TestInitialize]
        public void SetUp()
        {
            transaction = new TransactionScope();
            parkDAO = new ParkDAO(ConnectionString);
            campgroundDAO = new CampgroundDAO(ConnectionString);
            reservationDAO = new ReservationDAO(ConnectionString);

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"select count(*) from park where name = '{parkName}';", conn);
                SqlCommand cmd2 = new SqlCommand($"select count(*) from campground where park_id = (select park_id from park where name = '{parkName}');", conn);
                SqlCommand cmd3 = new SqlCommand($"select count(*) from site where campground_id = (select campground_id from campground where name = '{campName}');", conn);
                SqlCommand cmd4 = new SqlCommand($"select count(*) from reservation where site_id = (select site_id from site where site_number = '{siteNumber}');", conn);

                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                {
                    cmd = new SqlCommand($"insert into park values ('{parkName}', 'USA', '2/12/2000', 300, 2500, 'Brand new test park');", conn);
                    cmd.ExecuteNonQuery();
                }

                if (Convert.ToInt32(cmd2.ExecuteScalar()) == 0)
                {
                    cmd2 = new SqlCommand($"insert into campground values ((select park_id from park where name = '{parkName}'), '{campName}', 1, 12, 25);", conn);
                    cmd2.ExecuteNonQuery();
                }

                if (Convert.ToInt32(cmd3.ExecuteScalar()) == 0)
                {
                    cmd3 = new SqlCommand($"insert into site values ((select campground_id from campground where name = '{campName}'), {siteNumber}, 15, 0, 12, 1);", conn);
                    cmd3.ExecuteNonQuery();
                }

                if (Convert.ToInt32(cmd4.ExecuteScalar()) == 0)
                {
                    cmd4 = new SqlCommand($"insert into reservation values ((select site_id from site where site_number = '{siteNumber}'), '{reservationName}', '2/21/3030', '2/25/3030', '2/10/2020');", conn);
                    cmd4.ExecuteNonQuery();
                }
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            transaction.Dispose();
        }
    }
}
