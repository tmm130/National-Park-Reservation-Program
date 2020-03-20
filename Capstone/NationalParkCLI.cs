using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Capstone
{
    public class NationalParkCLI
    {
        private const string Command_DisplayAllParks = "1";
        private const string Command_Quit = "q";
        private const int Command_ViewCampgrounds = 1;
        private const int Command_SearchForReservation = 2;
        private const int Command_PreviousScreen = 3;
        private const int Command_FindAvailableReservation = 1;
        private const int Command_ReturnToParkMenu = 2;

        private IReservationDAO reservationDAO;
        private ICampgroundDAO campgroundDAO;
        private IParkDAO parkDAO;

        protected IList<Park> parks
        {
            get
            {
                return parkDAO.GetAllParks();
            }
        }

        public NationalParkCLI(IParkDAO parkDAO, IReservationDAO reservationDAO, ICampgroundDAO campgroundDAO)
        {
            this.parkDAO = parkDAO;
            this.reservationDAO = reservationDAO;
            this.campgroundDAO = campgroundDAO;
        }

        public void RunCLI()
        {
            PrintTitle();
            DisplayMainMenu();
            bool didTheyQuit = false;

            while (!didTheyQuit)
            {
                string input = Console.ReadLine().Trim();

                switch (input.ToLower().Trim())
                {
                    case Command_DisplayAllParks:
                        DisplayParkMenu();
                        break;

                    case Command_Quit:
                        Console.WriteLine("Thanks for visiting the National Park Campsite Reservation Service. Have a great day!");
                        didTheyQuit = true;
                        return;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("The command given is not a valid command, please try again using one of the options.");
                        DisplayMainMenu();
                        break;
                }
            }

        }
        private void PrintTitle()
        {
            Console.WriteLine("Welcome to the National Park Campsite Reservation Service");
        }

        private void DisplayMainMenu()
        {
            Console.WriteLine();
            Console.WriteLine("(1) Display all parks");
            Console.WriteLine("(Q) Quit");
        }

        private void DisplayParkMenu()
        {
            if (parks.Count > 0)
            {
                Console.WriteLine("Select a Park for Further Details");
                for (int i = 0; i < parks.Count; i++)
                {
                    Console.WriteLine("(" + (i + 1) + ") " + parks[i].Name);
                }
                Console.WriteLine("(Q) quit");
                UserParkChoice();
            }
            else
            {
                Console.WriteLine("NO RESULTS AVAILABLE!");
            }
        }

        private int UserInputForPark;

        private void UserParkChoice()
        {
            string input = Console.ReadLine();
            bool correctEntry = int.TryParse(input, out UserInputForPark);

            while ((!correctEntry || UserInputForPark < 1 || UserInputForPark > parks.Count) ^ input.ToLower().Trim() == "q")
            {
                Console.WriteLine("That is not a valid entry. Please try again.");
                input = Console.ReadLine();
                correctEntry = int.TryParse(input, out UserInputForPark);
            }

            if (input.ToLower().Trim() == "q")
            {
                Console.Clear();
                PrintTitle();
                DisplayMainMenu();
                return;
            }
            else
            {
                UserInputForPark--;
                DisplayParkInformation();
                CampgroundMenu();
                return;
            }
        }

        private void DisplayParkInformation()
        {
            Console.Clear();
            Console.WriteLine(parks[UserInputForPark].Name + " National Park");
            Console.WriteLine("Location:".PadRight(18) + parks[UserInputForPark].Location);
            Console.WriteLine("Established:".PadRight(18) + $"{parks[UserInputForPark].EstablishDate.ToString("MM/dd/yyyy")}");
            Console.WriteLine("Area:".PadRight(18) + parks[UserInputForPark].Area);
            Console.WriteLine("Annual Visitors:".PadRight(18) + parks[UserInputForPark].Visitors);
            Console.WriteLine();
            Console.WriteLine(parks[UserInputForPark].Description);
        }

        protected IList<Campground> campgrounds
        {
            get
            {
                return campgroundDAO.GetCampgroundsAtPark(parks[UserInputForPark]);
            }
        }

        private void DisplayCampgroundsInChosenPark()
        {
            if (campgrounds.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Campgrounds");
                Console.WriteLine("_".PadRight(83, '_'));
                Console.WriteLine("".PadRight(5) + "|" + "Name".PadRight(40) + "|" + "Open".PadRight(12) + "|" + "Close".PadRight(12) + "|" + "Daily Fee");
                Console.WriteLine("_".PadRight(83, '_'));

                for (int i = 0; i < campgrounds.Count; i++)
                {
                    DateTime needOpenMonthName = new DateTime(2000, campgrounds[i].OpenFromMonth, 1);
                    DateTime needCloseMonthName = new DateTime(2000, campgrounds[i].OpenUntilMonth, 1);
                    string openMonth = needOpenMonthName.ToString("MMMM");
                    string closeMonth = needCloseMonthName.ToString("MMMM");
                    Console.WriteLine(("#" + (i + 1) + ") ").PadRight(5) + "|" + campgrounds[i].Name.PadRight(40) + "|" + $"{openMonth}".PadRight(12) + "|" + $"{closeMonth}".PadRight(12) + "|" + $"{campgrounds[i].DailyFee:C2}");
                }

                AskIfReservationWanted();

            }
            else
            {
                Console.WriteLine("NO RESULTS AVAILABLE!");
            }
        }

        private void AskIfReservationWanted()
        {
            int userChoice = 0;
            bool isItParseable = false;

            do
            {
                Console.WriteLine();
                Console.WriteLine("Select a command");
                Console.WriteLine("(1) Search for Available Reservation".PadLeft(5));
                Console.WriteLine("(2) Return to Previous Screen".PadLeft(5));
                string choice = Console.ReadLine();
                isItParseable = int.TryParse(choice, out userChoice);

                if (!isItParseable || (userChoice != 1 && userChoice != 2))
                {
                    Console.WriteLine("Invalid Selection. Try again.");
                }

            } while (!isItParseable || (userChoice != 1 && userChoice != 2));

            while (true)
            {
                switch (userChoice)
                {
                    case Command_FindAvailableReservation:
                        CampgroundReservationSearchEntry();
                        return;

                    case Command_ReturnToParkMenu:
                        DisplayParkInformation();
                        DisplayCampgroundMenu();
                        return;
                }
            }
        }

        private void DisplayCampgroundMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Select a Command");
            Console.WriteLine("(1) View Campgrounds".PadLeft(5));
            Console.WriteLine("(2) Search for Reservation".PadLeft(5));
            Console.WriteLine("(3) Return to Previous Screen".PadLeft(5));
        }

        private void CampgroundMenu()
        {
            bool isItParseable = false;
            int userChoice = 0;
            DisplayParkInformation();
            DisplayCampgroundMenu();

            while (true)
            {
                string choice = Console.ReadLine();
                isItParseable = int.TryParse(choice, out userChoice);

                switch (userChoice)
                {
                    case Command_ViewCampgrounds:
                        DisplayCampgroundsInChosenPark();
                        break;

                    case Command_SearchForReservation:
                        DisplayCampgroundsInChosenPark();
                        break;

                    case Command_PreviousScreen:
                        DisplayParkMenu();
                        return;

                    default:
                        Console.WriteLine("Invalid Selection. Try Again.");
                        DisplayParkInformation();
                        DisplayCampgroundMenu();
                        break;
                }
            }
        }

        private int UserInputForCampground;

        private void CampgroundReservationSearchEntry()
        {
            bool isItParseable = false;
            do
            {
                UserInputForCampground = -1;
                Console.Write("Which Campground (enter 0 to cancel)? ");
                string userCampgroundChoice = Console.ReadLine();
                isItParseable = int.TryParse(userCampgroundChoice, out UserInputForCampground);

                if (!isItParseable)
                {
                    Console.WriteLine("Invalid Selection. Try Again.");
                }

            } while (!isItParseable || UserInputForCampground < 0 || UserInputForCampground > campgrounds.Count);

            if (UserInputForCampground == 0)
            {
                DisplayParkInformation();
                DisplayCampgroundsInChosenPark();
            }
            else
            {
                UserInputForCampground--;
                DateReservationSearchEntry();
            }
        }

        private DateTime ArrivalDate;
        private DateTime DepartureDate;

        private void DateReservationSearchEntry()
        {
            bool isItParseableArrival = false;
            bool isItParseableDeparture = false;

            do
            {
                Console.Write("What is the arrival date? ");
                string arrivalInput = Console.ReadLine();
                isItParseableArrival = DateTime.TryParse(arrivalInput, out ArrivalDate);

                if (!isItParseableArrival)
                {
                    Console.WriteLine("Invalid Selection. Try Again.");
                }
                else if (ArrivalDate < DateTime.Now)
                {
                    Console.WriteLine("Not a valid date. Please choose a date after today's date.");
                }

            } while (!isItParseableArrival || ArrivalDate < DateTime.Now);

            do
            {
                Console.Write("What is the departure date? ");
                string departureInput = Console.ReadLine();
                isItParseableDeparture = DateTime.TryParse(departureInput, out DepartureDate);

                if (!isItParseableDeparture)
                {
                    Console.WriteLine("Invalid Selection. Try Again.");
                }
                else if (DepartureDate <= ArrivalDate)
                {
                    Console.WriteLine("Not a valid date. Must be after arrival date. Please try again.");
                }
                else if ((DepartureDate - ArrivalDate).Days > 365)
                {
                    Console.WriteLine("Sorry we do not accept reservations longer than 365 days.");
                }

            } while (!isItParseableDeparture || DepartureDate <= ArrivalDate || (DepartureDate - ArrivalDate).Days > 365);

            IsThereAvailableReservations();
            return;
        }

        protected IList<Site> sites
        {
            get
            {
                return reservationDAO.FindAllSites(campgrounds[UserInputForCampground]);
            }
        }

        protected IList<Reservation> reservations
        {
            get
            {
                return reservationDAO.FindAllReservations(sites[UserInputForSite]);
            }
        }

        protected IList<Site> SitesAvailableForReservation
        {
            get
            {
                return reservationDAO.FindAvailableSitesForReservations(ArrivalDate, DepartureDate, campgrounds[UserInputForCampground]);
            }
        }

        private int UserInputForSite;
        private void IsThereAvailableReservations()
        {
            if (SitesAvailableForReservation.Count == 0)
            {
                string tryAgain = "";
                do
                {
                    Console.WriteLine("No campsites are available for the dates you requested. Would you like to enter an alternate date?");
                    tryAgain = Console.ReadLine();

                    if (tryAgain != "y" && tryAgain != "yes" && tryAgain != "n" && tryAgain != "no")
                    {
                        Console.WriteLine("That is not a valid input. Please enter (Y)es or (N)o.");
                    }

                } while (tryAgain != "y" && tryAgain != "yes" && tryAgain != "n" && tryAgain != "no");

                Console.Clear();
                DisplayCampgroundsInChosenPark();
                return;
            }
            else
            {
                DisplayAvailableReservations();
                return;
            }
        }

        private decimal TotalCost
        {
            get
            {
                return reservationDAO.FindTotalCost((DepartureDate - ArrivalDate).Days, campgrounds[UserInputForCampground]);
            }
        }

        protected Dictionary<string, Site> SiteSelector = new Dictionary<string, Site>();

        private void DisplayAvailableReservations()
        {
            Console.WriteLine();
            Console.WriteLine("Results Matching Your Search Criteria");
            Console.WriteLine("Site No.".PadRight(10) + "Max Occup.".PadRight(12) + "Accessible?".PadRight(14) + "RV Len.".PadRight(9) + "Utilities".PadRight(12) + "Cost");

            foreach (Site site in SitesAvailableForReservation)
            {
                SiteSelector.Add(site.SiteNumber.ToString(), site);
            }

            foreach (KeyValuePair<string, Site> kvp in SiteSelector)
            {
                string isItAccessible = kvp.Value.Accessible ? "Yes" : "No";
                string doRvsFit = kvp.Value.MaxRvLength == 0 ? "N/A" : kvp.Value.MaxRvLength.ToString();
                string areThereUtilities = kvp.Value.Utilities ? "Yes" : "No";
                Console.WriteLine($"{kvp.Key}".PadRight(10) + $"{kvp.Value.MaxOccupancy}".PadRight(12) + isItAccessible.PadRight(14) + doRvsFit.PadRight(9) + areThereUtilities.PadRight(12) + $"{TotalCost:C2}");
            }

            AskForReservationSiteAndName();
            return;
        }

        private string NameForReservation;

        private void AskForReservationSiteAndName()
        {
            bool isItParseable = false;
            string input = "";
            do
            {
                Console.Write("Which site should be reserved (enter 0 to cancel)? ");
                input = Console.ReadLine();
                isItParseable = int.TryParse(input, out UserInputForSite);

            } while ((!SiteSelector.ContainsKey(input) && UserInputForSite != 0) || !isItParseable);

            if (UserInputForSite == 0)
            {
                SiteSelector.Clear();
                DisplayParkInformation();
                DisplayCampgroundMenu();
                return;
            }
            else
            {
                Console.Write("What name should the reservation be made under? ");
                NameForReservation = Console.ReadLine();

                Console.WriteLine();
                int reservationId = reservationDAO.BookReservation(campgrounds[UserInputForCampground], SiteSelector.GetValueOrDefault(input), NameForReservation, ArrivalDate, DepartureDate);
                Console.WriteLine($"The reservation has been made and the confirmation id is {reservationId}. Your price was {TotalCost:C2}. Thank you.");
                Thread.Sleep(7000);
                SiteSelector.Clear();
                DisplayParkInformation();
                DisplayCampgroundMenu();
                return;
            }
        }
    }
}
