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
    public class ParkTests : NationalParkCapstone2TestInitialize
    {
        [TestMethod]
        public void GetAllParksTest()
        {
            IList<Park> parks = parkDAO.GetAllParks();

            Assert.IsTrue(parks.Count > 0);
        }
    }
}
