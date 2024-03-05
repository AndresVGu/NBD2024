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
                static string GenerateAddress(Random random)
                {
                    // Números de calle aleatorios (1-9999)
                    int streetNumber = random.Next(1, 10000);

                    // Nombres de calle aleatorios
                    string[] streetNames = { "Main St", "First Ave", "Elm St", "Maple Ave", "Oak St" };
                    string streetName = streetNames[random.Next(streetNames.Length)];

                    // Formatear la dirección
                    string address = $"{streetNumber} {streetName}";

                    return address;
                }
                //Random Postal Code:
                static string GeneratePostalCode(Random random)
                {
                    // Generar tres letras aleatorias
                    char[] letters = new char[3];
                    for (int i = 0; i < 3; i++)
                    {
                        letters[i] = (char)random.Next('A', 'Z' + 1);
                    }

                    // Generar tres dígitos aleatorios
                    int[] digits = new int[3];
                    for (int i = 0; i < 3; i++)
                    {
                        digits[i] = random.Next(0, 10);
                    }

                    // Formatear el código postal
                    string postalCode = string.Format("{0}{1}{2} {3}{4}{5}",
                                                      letters[0], digits[0],
                                                      letters[1], digits[1],
                                                      letters[2], digits[2]);

                    return postalCode;
                }

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
                        new City { Name = "Toronto", ProvinceID="ON"},
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

                             AddressStreet = GenerateAddress(random),
                             PostalCode = GeneratePostalCode(random)
                         }
                        );
                    context.SaveChanges();
                }

                #region random Clients

                //Because we can, lets add a bunch more Customers and Functions
                string[] companyNames = new string[] { "Consolidated Credit Union", "PEI  Womens Institute", "Atlantic Psychiatric Conference", "TLC Laser Eye Centers", "Cooperators Insurance", "Department of Education", "PEI Teacher's Federation", "Credit Union Managers", "International Union of Operating Engineers", "Department of Transportation & Public Works", "Department of Fisheries & Oceans", "Summerside Tax Center", "Summerside Community Church", "Health & Social Services", "Department of Education", "Bridge Tournament Tour", "Malpeque bay Credit Union", "Atlantic provinces Chamber of  Commerce", "Child & Family Services", "Canadian mental Health assoc.", "PEI Institute if Agrologists", "Acadian Fishermans Coop", "Toronto Dominion Bank", "Highfield Construction", "Dept of Fisheries & Oceans", "Canadian Motor Veichle Arbitration Plan", "PEI Real Estate Assoc.", "Heart & Stroke Foundation", "East Prince Youth Development Centre", "PEI Assoc of Exhibitions", "PEI Potato Processing Council", "Union of Public sector Employees", "Cavendish Agri services", "PEI Pharmaceutical Assoc", "Zellers District Office", "UPEI  Faculty of Education", "College of Family Physicians-PEI chapter", "Dept of Education and Early Childhood Development", "Aerospace Industries Assoc of Canada", "Dept of Transportation", "Mark's Work Warehouse", "Cavendish Agri-Services", "Baie Acadienne Dev. Corp", "Primerica Financial Services", "Holland College-Adult Education", "Downtown  Summerside Inc.", "PEI Hairdressers Assoc", "Occupational Health & Safety Division-WCB", "BTC-Building Tennis Communities", "PEI Institute of Agrologists", "Agriculture Insurance Corp", "Island Health Training Center", "PEI Federation of Agriculture", "Cavendish Agri Services", "Public  Service Alliance", "Loyalist Lakeview Resort", "Consolidated Credit Union", "Cusource-Credit Un.Cen.of N.S.", "Callbecks  Home Hardware", "Summerside Tax Center", "Summerside Tax center", "Genworth Financial Canada", "East Prince Health board", "Agricultural Insurance Corp", "Family Resource Center", "The National Chapter of Canada IODE", "Summerside Tax Center", "Pro Trans Personal Services", "Girls & Women in Sports", "Dept of Agriculture, Fisheries &Aquaculture", "PEI Automobile Dealers Assoc", "Spreadsheet Solutions", "PC Association of PEI-22ND DISTRICT", "Can Society of Safety Engineering", "Canadian Assoc of Property & Home Inspectors", "Egg Producers Board of PEI", "Key, McKnight & Maynard", "Agriculture & Agri Food Canada", "Atlantic Canada Oppurtunities", "Premier World Tours Canadian Maritimes Pioneer" };
                string[] firstNames = new string[] { "Woodstock", "Violet", "Charlie", "Lucy", "Linus", "Franklin", "Marcie", "Schroeder" };
                string[] lastNames = new string[] { "Hightower", "Broomspun", "Jones", "Bloggs", "Brown", "Smith", "Daniel" };
                int companyNameCount = companyNames.Length;
                string[] postalLetters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", };


                //Add more clients
                if (!context.Clients.Any())
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Client c = new()
                        {
                            FirstName = firstNames[i],
                            MiddleName = lastNames[random.Next(0, companyNameCount)][1].ToString().ToUpper(),
                            LastName = lastNames[i],
                            CompanyName = companyNames[random.Next(0, companyNameCount)],
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            AddressCountry = "Canada",
                            AddressStreet = GenerateAddress(random),
                            PostalCode = GeneratePostalCode(random)
                        };
                        context.Clients.Add(c);
                    }
                    context.SaveChanges();
                }
                #endregion


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
                            CityID = 6
                            ,
                            SetupNotes = "Notes here",
                            Material = context.Materials.ToList(),
                            ClientID = context.Clients.FirstOrDefault(c => c.ID == 3).ID

                        }
                  );
                    context.SaveChanges();
                }

              

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetBaseException().Message);
            }
            Debug.WriteLine("Seed process completed.");
        }
    }
}
