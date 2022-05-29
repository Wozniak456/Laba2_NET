using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApp3
{
    internal class Client 
    {
        public int ClientId { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientAdress { get; set; }
        public string ClientPhoneNumber { get; set; }
        public Client()
        {}
        public Client(int ClientId, string ClientFirstName, string ClientLastName, string ClientAdress, string ClientPhoneNumber)
        {
            this.ClientId = ClientId;
            this.ClientFirstName = (string)ClientFirstName;
            this.ClientLastName = (string)ClientLastName;
            this.ClientAdress = ClientAdress;
            this.ClientPhoneNumber = ClientPhoneNumber;
        }
        public override string ToString()
        {
            return "\n\t[Info about client]\n"+"Id: " +this.ClientId+"\nFirst name: "+this.ClientFirstName+"\nLast name: "+this.ClientLastName+"\n" +
                "Adress: "+this.ClientAdress+"\nPhone number: "+this.ClientPhoneNumber;
        }

        public static List<Client> WriteFromXmlFile(string file)
        {
            List<Client> clientsCollection = new List<Client>();
            XmlDocument document = new XmlDocument();
            document.Load(file);
            XmlElement? xRooti = document.DocumentElement;

            int ClientId = 0;
            string ClientFirstName = "";
            string ClientLastName = "";
            string ClientAdress = "";
            string ClientPhoneNumber = "";

            if (xRooti != null)
            {
                foreach (XmlElement xnode in xRooti)
                {
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        if (childnode.Name == "id")
                            ClientId = Convert.ToInt32(childnode.InnerText);
                        else if (childnode.Name == "firstName")
                            ClientFirstName = childnode.InnerText;
                        else if (childnode.Name == "lastName")
                            ClientLastName = childnode.InnerText;
                        else if (childnode.Name == "adress")
                            ClientAdress = childnode.InnerText;
                        else if (childnode.Name == "phoneNumber")
                            ClientPhoneNumber = childnode.InnerText;
                    }
                    Client clientInstance = new Client(ClientId, ClientFirstName, ClientLastName, ClientAdress, ClientPhoneNumber);
                    clientsCollection.Add(clientInstance);
                }
            }
            return clientsCollection;
        }
    }
}
