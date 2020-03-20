using Capstone.DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("NationalParksConnection");

            IParkDAO parkDAO = new ParkDAO(connectionString);
            IReservationDAO reservationDAO = new ReservationDAO(connectionString);
            ICampgroundDAO campgroundDAO = new CampgroundDAO(connectionString);

            NationalParkCLI application = new NationalParkCLI(parkDAO, reservationDAO, campgroundDAO);
            application.RunCLI();
        }
    }
}
