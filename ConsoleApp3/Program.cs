using ConsoleApp3;
using System.Xml;
using System.Xml.Linq;

namespace Lab2
{
    public class Program 
    {
        static void Main(string[] args)
        {
            List<Car> carsList = Car.WriteFromXmlFile("cars.xml");
            List<Client> clientList = Client.WriteFromXmlFile("clients.xml");
            List<Agreement> agreementList = Agreement.WriteFromXmlFile("agreements.xml");

            
            FormingMainXmlFile(clientList, carsList, agreementList);


            Console.WriteLine("Choose an action: \n1 - I want to take a car\n" +
                "2 - To show LINQ requests\n3 - To show all elements in file.xml");

            int choice = 0;
            try
            {
                bool result = int.TryParse(Console.ReadLine(), out choice);
                if (!result)
                    throw new Exception("Thats not a number !");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            
            switch (choice)
            {
                case 1:
                    try
                    {
                        OrderForming(carsList, clientList, agreementList);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                    }
                    break;
                case 2:
                    LinqToXmlRequests();
                    break;
                case 3:
                    ShowAllElementsFromTheMainXmlFile();
                    break;
                default:
                    Console.WriteLine("Please input 1,2 or 3 !");
                    break;
            }

        }
        static void OrderForming(List<Car> carsList, List<Client> clientList, List<Agreement> agreementList)
        {
            Console.Write($"id of agreement: ");
            int agreementIdentifier;
            bool result = int.TryParse(Console.ReadLine(), out agreementIdentifier);
            if (!result || agreementIdentifier <=0 )
                throw new ArgumentException(" [not valid value inputed] ");

            var idAlreadyExist = from x in agreementList
                        where x.AgreementId == agreementIdentifier
                        select x;

            if (idAlreadyExist != null && idAlreadyExist.Count() != 0)
                throw new ArgumentException(" [already exist] ");


            Console.Write("Who are you? \nInput your first name: ");
            string clientFirstName = Console.ReadLine();
            if (!clientFirstName.All(char.IsLetter) || string.IsNullOrEmpty(clientFirstName))
                throw new ArgumentException(" [not valid value inputed] ");


            Console.Write("Input your last name: ");
            string clientLastName = Console.ReadLine();
            if (!clientLastName.All(char.IsLetter) || string.IsNullOrEmpty(clientLastName))
                throw new ArgumentException(" [not valid value inputed] ");


            int clientIdentifier = 0;
            Console.Write("Which car do you want to take?\nInput brand: ");
            string carBrand = Console.ReadLine();
            if (string.IsNullOrEmpty(carBrand))
                throw new ArgumentException(" [PLEASE INPUT A BRAND NAME] ");


            Console.Write("Input a year of production: ");
            int carYearOfProduction;
            bool result1 = int.TryParse(Console.ReadLine(), out carYearOfProduction);
            
            if (!result || carYearOfProduction == 0)
                throw new ArgumentException(" [not valid value inputed] ");
            if (carYearOfProduction > 2023 || carYearOfProduction < 1900)
                throw new ArgumentException("Sorry, what? :))");

            int carIdentifier = 0;

            Console.Write("How long you want to use it (in weeks): ");
            int numOfRentWeeks;
            bool result2 = int.TryParse(Console.ReadLine(), out numOfRentWeeks);
            if (!result || numOfRentWeeks == 0)
                throw new ArgumentException(" [not valid value inputed] ");
            DateTime dateOfRentBegin = DateTime.Now;
            DateTime dateOfRentEnd;
            TimeSpan durationOfRest = new TimeSpan(numOfRentWeeks * 7, 0, 0, 0);
            dateOfRentEnd = dateOfRentBegin + durationOfRest;

            for (int i = 0; i < clientList.Count; i++)
            {
                if (clientList[i].ClientFirstName == clientFirstName && clientList[i].ClientLastName.ToString() == clientLastName)
                {
                    clientIdentifier = i + 1;
                    i = clientList.Count;
                }
            }
            if (clientIdentifier == 0)
                throw new ArgumentException("Client not found");

            for (int i = 0; i < carsList.Count; i++)
            {
                if (carsList[i].CarBrand == carBrand && carsList[i].CarYear == carYearOfProduction)
                {
                    carIdentifier = i + 1;
                    i = carsList.Count;
                }
            }
            if (clientIdentifier == 0)
                throw new ArgumentException("Car not found");

            Agreement? agreementInstance = null;
            try
            {
                Agreement.FormingAgreementNodeIntoAgreementsXml(agreementIdentifier, dateOfRentBegin, clientIdentifier, carIdentifier, dateOfRentEnd);
            }
            catch (Exception)
            {
                Console.WriteLine("Agreement was not formed");
            }
            

            //try to form an agreement instance with inputed dates
            try{agreementInstance = new Agreement(agreementIdentifier, dateOfRentBegin, clientIdentifier, carIdentifier, dateOfRentEnd);}
            catch (Exception)
            {Console.WriteLine("Forming Order ERROR");}


            //try to add agreement instance to collection and to update the main xml file
            try{agreementList.Add(agreementInstance);
                FormingMainXmlFile(clientList, carsList, agreementList);}
            catch (Exception)
            {Console.WriteLine("Order not added");}
        }
        static void ShowAllElementsFromTheMainXmlFile()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Clients:");
            Console.ResetColor();

            XDocument xDocShow = XDocument.Load("labka2.xml");

            foreach (XElement clientelement in xDocShow.Element("CarPark").Element("Clients").Elements("client"))
            {
                XElement clientID = clientelement.Element("id");
                XElement clFirstName = clientelement.Element("firstName");
                XElement clLastName = clientelement.Element("lastName");
                XElement clAdress = clientelement.Element("adress");
                XElement clPhoneNum = clientelement.Element("phoneNumber");

                if (clientID != null && clFirstName.Value != null && clLastName.Value != null && clAdress != null && clPhoneNum != null)
                {
                    Console.WriteLine($"ID: {clientID.Value}, Name: {clFirstName.Value} {clLastName.Value}\nAdress: {clAdress.Value}\nPhone number: {clPhoneNum.Value}\n");
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Cars:");
            Console.ResetColor();

            foreach (XElement carElement in xDocShow.Element("CarPark").Element("Cars").Elements("car"))
            {
                XElement cID = carElement.Element("id");
                XElement cYear = carElement.Element("year");
                XElement cBrand = carElement.Element("brand");
                XElement cType = carElement.Element("type");
                XElement cCost1 = carElement.Element("cost1");
                XElement cCost3 = carElement.Element("cost3");
                XElement cPlAmount = carElement.Element("pledgeAmount");

                if (cID != null && cYear != null && cBrand != null && cType != null && cCost1 != null && cCost3 != null && cPlAmount != null)
                {
                    Console.WriteLine($"ID: {cID.Value}\nBrand: {cBrand.Value}, Year of production: {cYear.Value}\n" +
                        $"Pledge amount: {cPlAmount.Value}$, Usual cost for 1 week: {cCost1.Value}$,\nDiscounted cost for 1 week: {cCost3.Value}$\n");
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Agreements:");
            Console.ResetColor();

            foreach (XElement carElement in xDocShow.Element("CarPark").Element("Agreements").Elements("agreement"))
            {
                XElement aID = carElement.Element("id");
                XElement aIssue = carElement.Element("issueDate");
                XElement acliId = carElement.Element("clientID");
                XElement acarId = carElement.Element("carID");
                XElement aReturn = carElement.Element("returnDate");

                if (aID != null && aIssue != null && acliId != null && acarId != null && aReturn != null)
                {
                    Console.WriteLine($"ID: {aID.Value}\nIssue date: {aIssue.Value},\nClient id: {acliId.Value} " +
                        $"Car id: {acarId.Value},\nThe end of egreement: {aReturn.Value}\n");
                }
            }
        }
        static void LinqToXmlRequests()
        {
            XDocument xDocR = XDocument.Load("Labka2.xml");
            IEnumerable<XElement> clientElements = xDocR.Element("CarPark").Element("Clients").Elements("client");
            IEnumerable<XElement> carElements = xDocR.Element("CarPark").Element("Cars").Elements("car");
            IEnumerable<XElement> agree = xDocR.Element("CarPark").Element("Agreements").Elements("agreement");


            Console.WriteLine("1. Перелiк зареєстрованих клiєнтів, вiдсортованi за алфавiтом");
            var querySorted = xDocR.Descendants("client").Select(p =>
            p.Element("firstName").Value).OrderBy(p => p.Trim());
            foreach (var s in querySorted)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("2. Фiльтр за роком. (вивести марку автомобiля, випущеного 2017 року)");
            var queryYear =
                from b in carElements
                where (int)b.Element("year") == 2017
                select b;
            Console.WriteLine(queryYear.FirstOrDefault().Element("brand").Value);
            
            
            Console.WriteLine("3. Чиє прiзвище - Vozniak");
            var items = from xe in clientElements
                        where xe.Element("lastName").Value == "Vozniak"
                        select new Client
                        {
                            ClientId = int.Parse(xe.Element("id").Value),
                            ClientFirstName = xe.Element("firstName").Value,
                            ClientLastName = xe.Element("lastName").Value,
                            ClientAdress = xe.Element("adress").Value,
                            ClientPhoneNumber = xe.Element("phoneNumber").Value
                        };
            Console.WriteLine();
            foreach (var item in items)
                Console.WriteLine($"ID: {item.ClientId}, Name: {item.ClientFirstName} {item.ClientLastName}");
            Console.WriteLine();


            Console.WriteLine("4. Чиє прiзвище - Vozniak, але чий id > 3");
            var q1 = from x in clientElements
                     where x.Element("lastName").Value == "Vozniak" && ((int)x.Element("id") > 3)
                     select x;

            foreach (var x in q1)
                Console.WriteLine($"ID: {x.Element("id").Value} Name: {x.Element("firstName").Value} {x.Element("lastName").Value}");
            Console.WriteLine();
            

            Console.WriteLine("5. Сортування. ID i ClientID угод, якi пiдписали клiєнти з ID 2 або 5 i бiльше нiж 1, вiдсортоване за спаданням id угоди");
            var q2 = from x in agree
                     where (int)x.Element("id") > 1 && (x.Element("clientID").Value == "5" || x.Element("clientID").Value == "2")
                     orderby x.Element("id").Value descending
                     select x;

            foreach (var x in q2)
                Console.WriteLine($"ID: {x.Element("id").Value}, ClientID: {x.Element("clientID").Value}");
            Console.WriteLine();


            Console.WriteLine("6. Сортування з використанням розширюючих методiв. Машини, чий id > 1 i (чий бренд - BMW або рiк випуску < 2018)");
            var q2_1 = carElements.Where((x) =>
            {
                return (int)x.Element("id") > 1 && (x.Element("brand").Value == "BMW" || (int)x.Element("year") < 2018);
            }).OrderByDescending(x => (int)x.Element("id"));
            foreach (var x in q2_1) Console.WriteLine($"ID: {x.Element("id").Value}, Brand: {x.Element("brand").Value}: Year: {x.Element("year").Value}");
            Console.WriteLine();


            Console.WriteLine("7. Inner Join з використанням Where. Виведення iмен клiєнтiв i iх дати початку угоди");
            var q3 = from x in clientElements
                     from y in agree
                     where y.Element("clientID").Value == x.Element("id").Value
                     select new { ID = y.Element("id").Value, Name = x.Element("firstName").Value + " " + x.Element("lastName").Value, IssueDate = y.Element("issueDate").Value };
            foreach (var x in q3)
                Console.WriteLine(x);


            Console.WriteLine("\n8. Inner Join з використанням Join");
            var q4 = from x in carElements
                     join y in agree on x.Element("id").Value equals y.Element("carID").Value
                     select new { ID = y.Element("id").Value, Brand = x.Element("brand").Value, Year = x.Element("year").Value, ReturnDate = y.Element("returnDate").Value };
            foreach (var x in q4)
                Console.WriteLine(x);


            Console.WriteLine("\n9. Cross Join и Group Join");
            var q11 = from x in clientElements
                      join y in agree on x.Element("id").Value equals y.Element("clientID").Value into temp
                      from t in temp
                      select new { v1 = x.Element("firstName").Value, v2 = t.Element("clientID").Value, cnt = temp.Count() };
            foreach (var x in q11)
                Console.WriteLine(x);


            Console.WriteLine("\n10. Outer Join");
            var q12 = from x in clientElements
                      join y in agree on x.Element("id").Value equals y.Element("clientID").Value into temp
                      from t in temp.DefaultIfEmpty()
                      select new { Name = x.Element("firstName").Value, clientID = ((t == null) ? "null" : t.Element("clientID").Value) };
            foreach (var x in q12)
                Console.WriteLine(x);


            Console.WriteLine("\n11. Групування");
            var q16 = from x in agree group x by x.Element("carID").Value into g select new { Key = g.Key, Values = g };
            foreach (var x in q16)
            {
                Console.WriteLine("Car " + x.Key + " chose");
                foreach (var y in x.Values)
                    Console.WriteLine(" ClientID: " + y.Element("id").Value);
            }


            Console.WriteLine("\n12. Результат перетворюється в масив");
            Console.Write($"Було: {clientElements.GetType().Name}\nСтало: ");
            var e3 = (from x in clientElements select x).ToArray();
            Console.WriteLine(e3.GetType().Name);


            Console.WriteLine("\n13. Перший елемент з вибiрки з прiзвищем Vozniak");
            var f1 = (from x in clientElements select x).First(x => x.Element("lastName").Value == "Vozniak");
            Console.WriteLine($"ID: {f1.Element("id").Value} Name: {f1.Element("firstName").Value} {f1.Element("lastName").Value}");


            Console.WriteLine("\n14. Перший елемент з вибiрки або значення за замовчанням");
            var f2 = (from x in carElements select x).FirstOrDefault(x => x.Element("brand").Value == "Ferrari");
            Console.WriteLine(f2 == null ? "null" : f2.ToString());


            Console.WriteLine("\n15. Елемент в заданiй позицiйi (0)");
            var f3 = (from x in agree select x).ElementAt(0);
            Console.WriteLine($"ID: {f3.Element("id").Value}, ClientID: {f3.Element("clientID").Value}");

        }
        static void FormingMainXmlFile(List<Client> clients, List<Car> cars, List<Agreement> agreements)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;


            using (XmlWriter writer = XmlWriter.Create("labka2.xml", settings))
            {
                writer.WriteStartElement("CarPark");
                writer.WriteStartElement("Cars");
                foreach (Car car in cars)
                {
                    writer.WriteStartElement("car");
                    writer.WriteElementString("id", car.СarId.ToString());
                    writer.WriteElementString("brand", car.CarBrand);
                    writer.WriteElementString("type", car.CarType);
                    writer.WriteElementString("year", car.CarYear.ToString());
                    writer.WriteElementString("cost1", car.CarCostForLessThen1Mounth.ToString());
                    writer.WriteElementString("cost3", car.CarCostForMoreThen1Mounth.ToString());
                    writer.WriteElementString("pledgeAmount", car.PledgeAmount.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteStartElement("Clients");

                foreach (Client client1 in clients)
                {
                    writer.WriteStartElement("client");
                    writer.WriteElementString("id", client1.ClientId.ToString());
                    writer.WriteElementString("firstName", client1.ClientFirstName.ToString());
                    writer.WriteElementString("lastName", client1.ClientLastName.ToString());
                    writer.WriteElementString("adress", client1.ClientAdress.ToString());
                    writer.WriteElementString("phoneNumber", client1.ClientPhoneNumber.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteStartElement("Agreements");

                foreach (Agreement agr in agreements)
                {
                    writer.WriteStartElement("agreement");
                    writer.WriteElementString("id", agr.AgreementId.ToString());
                    writer.WriteElementString("issueDate", agr.AgreementIssueDate.ToString());
                    writer.WriteElementString("clientID", agr.AgreementClientId.ToString());
                    writer.WriteElementString("carID", agr.AgreementCarId.ToString());
                    writer.WriteElementString("returnDate", agr.AgreementReturnDate.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.Close();
            }
        }
    }
}