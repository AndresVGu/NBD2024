using NBD2024.Models;
using System.Diagnostics;

namespace NBD2024.Data
{
    public static class NBDInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            NBDContext context = applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<NBDContext>();

            try
            {
                //Delete and recreate the Database with every restart
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                //context.Database.Migrate();

                //To randomly generate data
                Random random = new Random();

                #region Generate random Records
                //Random Phone number:
                /*/ static string PhoneRd(string[] args)
                 {
                     for (int i = 0;i <10;i++)
                     {
                         string phoneNumber = GeneratePhoneNumber(random);
                         return phoneNumber;
                     }
                 }*/
                static string GeneratePhoneNumber(Random random)
                {
                    //random area code
                    int areaCode = random.Next(100, 1000);
                    //random exchange code 
                    int exchangeCode = random.Next(100, 1000);
                    //random susbscriber number
                    int subscriberNumber = random.Next(100, 1000);
                    string phonerd = string.Format("{0:000}{1:000}{2:0000}", areaCode, exchangeCode, subscriberNumber);
                    return phonerd;
                }
                //Random Address:
                //Random Postal Code:

                #endregion

                //Provinces, Cities
                if (!context.Provinces.Any())
                {
                    var provinces = new List<Province>
                    {
                        new Province { ID = "ON", Name = "Ontario"},
                        new Province { ID = "PE", Name = "Prince Edward Island"},
                        new Province { ID = "NB", Name = "New Brunswick"},
                        new Province { ID = "BC", Name = "British Columbia"},
                        new Province { ID = "NL", Name = "Newfoundland and Labrador"},
                        new Province { ID = "SK", Name = "Saskatchewan"},
                        new Province { ID = "NS", Name = "Nova Scotia"},
                        new Province { ID = "MB", Name = "Manitoba"},
                        new Province { ID = "QC", Name = "Quebec"},
                        new Province { ID = "YT", Name = "Yukon"},
                        new Province { ID = "NU", Name = "Nunavut"},
                        new Province { ID = "NT", Name = "Northwest Territories"},
                        new Province { ID = "AB", Name = "Alberta"}
                    };
                    context.Provinces.AddRange(provinces);
                    context.SaveChanges();
                }
                if (!context.Cities.Any())
                {
                    var cities = new List<City>
                    {
                        new City { Name = "Toronto", ProvinceID="ON" },
                        new City { Name = "Halifax", ProvinceID="NS" },
                        new City { Name = "Calgary", ProvinceID="AB" },
                        new City { Name = "Winnipeg", ProvinceID="MB", },
                        new City { Name = "Stratford", ProvinceID="ON" },
                        new City { Name = "St. Catharines", ProvinceID="ON" },
                        new City { Name = "Welland", ProvinceID="ON" },
                        new City { Name = "Stratford", ProvinceID="PE" },
                        new City { Name = "Ancaster", ProvinceID="ON" },
                        new City { Name = "Vancouver", ProvinceID="BC" },
                    };
                    context.Cities.AddRange(cities);
                    context.SaveChanges();
                }

                //Look for any client. Since we can't have Projects without Clients.
                if (!context.Clients.Any())
                {
                    context.Clients.AddRange(
                        new Client
                        {
                            FirstName = "Amy",
                            LastName = "Benson",
                            CompanyName = "London Sq. Mall",
                            Phone = "4088945603",
                            AddressCountry = "Canada",
                            AddressStreet = "Scott Valley",
                            PostalCode = "LC34N7"
                        },
                         new Client
                         {
                             FirstName = "Bob",
                             MiddleName ="Jones",
                             LastName = "Kellet",
                             CompanyName = "Quantum Innovations Co.",
                             Phone = GeneratePhoneNumber(random),
                             AddressCountry = "Canada",

                             AddressStreet = "21 example Rd.",
                             PostalCode = "LC34N7"
                         },
                         new Client
                         {
                             FirstName = "Jim",

                             LastName = "Cruz",
                             CompanyName = "Fusion Enterprices International",
                             Phone = GeneratePhoneNumber(random),
                             AddressCountry = "Canada",

                             AddressStreet = "21 example Rd.",
                             PostalCode = "LC34N7"
                         },
                         new Client
                         {
                             FirstName = "Joe",

                             LastName = "Smith",
                             CompanyName = "Stellar Solutions Co.",
                             Phone = GeneratePhoneNumber(random),
                             AddressCountry = "Canada",

                             AddressStreet = "21 example Rd.",
                             PostalCode = "LC34N7"
                         },
                         new Client
                         {
                             FirstName = "Jack",

                             LastName = "Jones",
                             CompanyName = "Nexus Dynamics Inc.",
                             Phone = GeneratePhoneNumber(random),
                             AddressCountry = "Canada",

                             AddressStreet = "21 example Rd.",
                             PostalCode = "LC34N7"
                         }
                        );
                    context.SaveChanges();
                }


                // Seed Projects if there aren't any.
                if (!context.Projects.Any())
                {
                    context.Projects.AddRange(
                    new Project
                    {
                        ProjectName = "Project 1",
                        BidDate = DateTime.Parse("2023-06-08"),
                        StartTime = DateTime.Parse("2023-06-15"),
                        EndTime = DateTime.Parse("2023-07-08"),
                        ProjectSite = "Back deck 1",
                        SetupNotes = "Notes here",
                        CityID = 2,
                        Material = context.Materials.ToList(),
                        ClientID = context.Clients.FirstOrDefault(c => c.FirstName == "Amy" && c.LastName == "Benson").ID

                    },
                     new Project
                     {
                         ProjectName = "Project 2",
                         BidDate = DateTime.Parse("2023-06-08"),
                         StartTime = DateTime.Parse("2023-06-15"),
                         EndTime = DateTime.Parse("2023-07-08"),
                         ProjectSite = "Back deck 1",
                         CityID = 1,
                         SetupNotes = "Notes here",
                         Material = context.Materials.ToList(),
                         ClientID = context.Clients.FirstOrDefault(c => c.ID == 2).ID

                     },
                      new Project
                      {
                          ProjectName = "Project 3",
                          BidDate = DateTime.Parse("2023-06-08"),
                          StartTime = DateTime.Parse("2023-06-15"),
                          EndTime = DateTime.Parse("2023-07-08"),
                          ProjectSite = "Back deck 1",
                          CityID = 3,
                          SetupNotes = "Notes here",
                          Material = context.Materials.ToList(),
                          ClientID = context.Clients.FirstOrDefault(c => c.ID == 1).ID

                      },
                       new Project
                       {
                           ProjectName = "Project 4",
                           BidDate = DateTime.Parse("2023-06-08"),
                           StartTime = DateTime.Parse("2023-06-15"),
                           EndTime = DateTime.Parse("2023-07-08"),
                           ProjectSite = "Back deck 4",
                           CityID = 4,
                           SetupNotes = "Notes here",
                           Material = context.Materials.ToList(),
                           ClientID = context.Clients.FirstOrDefault(c => c.ID == 2).ID

                       },
                        new Project
                        {
                            ProjectName = "Project 5",
                            BidDate = DateTime.Parse("2023-06-08"),
                            StartTime = DateTime.Parse("2023-06-15"),
                            EndTime = DateTime.Parse("2023-07-08"),
                            ProjectSite = "Back deck 5",
                            CityID = 6,
                            SetupNotes = "Notes here",
                            Material = context.Materials.ToList(),
                            ClientID = context.Clients.FirstOrDefault(c => c.ID == 3).ID

                        }
                  );
                    context.SaveChanges();
                }

                #region random data
                //Because we can, lets add a bunch more Customers and Functions
                string[] companyNames = new string[] { "Consolidated Credit Union", "PEI  Womens Institute", "Atlantic Psychiatric Conference", "TLC Laser Eye Centers", "Cooperators Insurance", "Department of Education", "PEI Teacher's Federation", "Credit Union Managers", "International Union of Operating Engineers", "Department of Transportation & Public Works", "Department of Fisheries & Oceans", "Summerside Tax Center", "Summerside Community Church", "Health & Social Services", "Department of Education", "Bridge Tournament Tour", "Malpeque bay Credit Union", "Atlantic provinces Chamber of  Commerce", "Child & Family Services", "Canadian mental Health assoc.", "PEI Institute if Agrologists", "Acadian Fishermans Coop", "Toronto Dominion Bank", "Highfield Construction", "Dept of Fisheries & Oceans", "Canadian Motor Veichle Arbitration Plan", "PEI Real Estate Assoc.", "Heart & Stroke Foundation", "East Prince Youth Development Centre", "PEI Assoc of Exhibitions", "PEI Potato Processing Council", "Union of Public sector Employees", "Cavendish Agri services", "PEI Pharmaceutical Assoc", "Zellers District Office", "UPEI  Faculty of Education", "College of Family Physicians-PEI chapter", "Dept of Education and Early Childhood Development", "Aerospace Industries Assoc of Canada", "Dept of Transportation", "Mark's Work Warehouse", "Cavendish Agri-Services", "Baie Acadienne Dev. Corp", "Primerica Financial Services", "Holland College-Adult Education", "Downtown  Summerside Inc.", "PEI Hairdressers Assoc", "Occupational Health & Safety Division-WCB", "BTC-Building Tennis Communities", "PEI Institute of Agrologists", "Agriculture Insurance Corp", "Island Health Training Center", "PEI Federation of Agriculture", "Cavendish Agri Services", "Public  Service Alliance", "Loyalist Lakeview Resort", "Consolidated Credit Union", "Cusource-Credit Un.Cen.of N.S.", "Callbecks  Home Hardware", "Summerside Tax Center", "Summerside Tax center", "Genworth Financial Canada", "East Prince Health board", "Agricultural Insurance Corp", "Family Resource Center", "The National Chapter of Canada IODE", "Summerside Tax Center", "Pro Trans Personal Services", "Girls & Women in Sports", "Dept of Agriculture, Fisheries &Aquaculture", "PEI Automobile Dealers Assoc", "Spreadsheet Solutions", "PC Association of PEI-22ND DISTRICT", "Can Society of Safety Engineering", "Canadian Assoc of Property & Home Inspectors", "Egg Producers Board of PEI", "Key, McKnight & Maynard", "Agriculture & Agri Food Canada", "Atlantic Canada Oppurtunities", "Premier World Tours Canadian Maritimes Pioneer" };
                //Adding all students as customers.  
                string[] firstNames = new string[] { "Hamza", "Ben", "Vinicius", "Doaa", "Daniel", "Alex", "Sumiran", "Josh Rydzpol Adlhaie", "Femarie Vien", "Devin", "Elijah", "Sophie", "Donaven", "Jorge Eliecer", "Andrey", "Jacob", "Ryan", "Aidan", "Cheryl", "David Chikamso", "Jason", "Jordan", "Michael", "Divyansh", "Ben", "Grayson", "Nathan", "Nuran", "Olubusiyi Olorungbemi", "Aanson", "Jorge Ernesto", "Jemima", "Sumandeep Kaur", "Miguel Angel", "Meet Rajeshkumar", "Rostyslav", "Chris", "Kyle", "Raul Guillermo", "Nadav", "Nikola", "Nafisat Oiza", "Juan Francisco", "Lakshay", "Emmanuel Kelechukwu", "Mathew", "Farooq", "Yiyang", "Sean", "Sawyer", "Leon", "Markus", "Mohammad", "Jiyoung", "Emily", "Victor Hugo", "Jordan", "Diana Marcela", "Josh", "Julian Andres", "Fernand", "Quintin", "Iago", "Cristina", "Jinil", "Anthony", "Evan", "Mai Phuong", "Edward Rey", "Uju Amanda Rodrigues", "Kenechukwu Kenneth", "Teslim", "Dorian", "Hetav Vijaybhai", "Rishi", "Julia", "Bhavik Hiteshkumar", "Smit Rakeshbhai", "Vansh Mineshkumar", "Subit", "Soumyajit", "Ny Avo Manjaka", "Esmael Mazen Esmael", "Ria", "Gagan", "Gagandeep", "Gurmehar", "Gurwinder", "Harshdeep", "Christian", "Philip Luis", "Catalin", "Caleb", "David", "Dhaval", "George", "Nigel", "Joe", "Ashly", "Pablo David", "Andres Arturo", "Curtis", "Cole", "Amron", "Leo", "Keegan", "Kyle" };
                string[] lastNames = new string[] { "Albashatweh", "Allen", "Araujo Guedes", "Awan", "Baptiste", "Baxter", "Bhandari", "Binalay", "Briones", "Briscall", "Bugiardini", "Cabrera", "Carmichael", "Castano Mejia", "Chepurnov", "Clayton", "Cote", "Dagan", "Dautermann", "Davidson", "De Guzman", "Di Marcantonio", "Dionne", "Divyansh", "Durocher", "Dyer", "Dykstra", "Erbek", "Famobiwo", "Findlater", "Fuentes Gonzalez", "Gill", "Gill", "Gonzalez Gonzalez", "Hadvaidya", "Havryshkiv", "Head", "Henderson", "Herrera Aguilar", "Hilu", "Hrnjez", "Isa", "Jacobo Rodriguez", "Jakhar", "James", "Jean", "Jidelola", "Jin", "Johnston", "Johnston-Jeffrey", "Kammegne Kamdem", "Kari", "Khan", "Kim", "Little", "Lopez Chavez", "Lucciola", "Maldonado Burgos", "Martin", "Martinez Avila", "Michel", "Mohammed", "Moreira Romao", "Murguia Cozar", "Murikkanolikkal Johny", "Muro", "Myers", "Nguyen", "Norris", "Nzekwesi", "Obeta", "Ogunlola", "Orozco", "Panchal", "Pancholi", "Parson", "Patel", "Patel", "Patel", "Paudyal", "Paul", "Rajaonalison", "Sandoqa", "Sharma", "Shrestha", "Singh", "Singh", "Singh", "Singh", "Smallwood", "Soriano", "Spirleanu", "Srigley", "Sychevskyi", "Tailor", "Tatulea", "Temple", "Terdik", "Thompson", "Torres Moreno", "Villarreal Gutierrez", "Wachowiak", "Warner", "Weeber", "Williston", "Wood", "Zizian" };

                #endregion



            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetBaseException().Message);
            }
            Debug.WriteLine("Seed process completed.");
        }
    }
}
