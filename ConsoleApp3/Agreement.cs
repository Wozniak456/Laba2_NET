using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ConsoleApp3
{
    internal class Agreement 
    {
        public int AgreementId { get; set; }
        public DateTime AgreementIssueDate { get; set; }
        public DateTime AgreementReturnDate { get; set; }
        public int AgreementClientId { get; set; }
        public int AgreementCarId { get; set; }
        
        public Agreement(int AgreementId, DateTime AgreementIssueDate, int AgreementClientId, int AgreementCarId, DateTime AgreementReturnDate)
        {
            this.AgreementId = AgreementId;
            this.AgreementIssueDate = AgreementIssueDate;
            this.AgreementClientId = AgreementClientId;
            this.AgreementCarId = AgreementCarId;
            this.AgreementReturnDate = AgreementReturnDate;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"\n\t[Info about agreement]\nThe agreement with id {this.AgreementId} was concluded in {this.AgreementIssueDate}" +
                $"\nWith clientID: {AgreementClientId}\n");
            sb.Append($"With carID: {AgreementCarId}\n");
            sb.Append($"The agreement ends in {this.AgreementReturnDate}");
            return sb.ToString();
        }
        public static void FormingAgreementNodeIntoAgreementsXml(int agreementIdentifier, DateTime dateOfRentBegin, int clientIdentifier, int carIdentifier, DateTime dateOfRentEnd)
        {
            XmlDocument document = new XmlDocument();
            document.Load("agreements.xml");

            XmlElement? xRoot = document.DocumentElement;
            XmlElement agreem = document.CreateElement("agreement");

            XmlElement agrId = document.CreateElement("id");
            XmlElement agrIssueD = document.CreateElement("issueDate");
            XmlElement agrClientID = document.CreateElement("clientID");
            XmlElement agrCarID = document.CreateElement("carID");
            XmlElement agrReturnDate = document.CreateElement("returnDate");

            XmlText idText = document.CreateTextNode(agreementIdentifier.ToString());
            XmlText IssueDateText = document.CreateTextNode(dateOfRentBegin.ToString());
            XmlText clientIdText = document.CreateTextNode(clientIdentifier.ToString());
            XmlText carIdText = document.CreateTextNode(carIdentifier.ToString());
            XmlText returnDateText = document.CreateTextNode(dateOfRentEnd.ToString());

            agrId.AppendChild(idText);
            agrIssueD.AppendChild(IssueDateText);
            agrClientID.AppendChild(clientIdText);
            agrCarID.AppendChild(carIdText);
            agrReturnDate.AppendChild(returnDateText);

            agreem.AppendChild(agrId);
            agreem.AppendChild(agrIssueD);
            agreem.AppendChild(agrClientID);
            agreem.AppendChild(agrCarID);
            agreem.AppendChild(agrReturnDate);

            xRoot.AppendChild(agreem);

            document.Save("agreements.xml");
        }
        public static List<Agreement> WriteFromXmlFile(string file)
        {
            List<Agreement> agreementsCollection = new List<Agreement>();
            XmlDocument document = new XmlDocument();
            document.Load(file);
            XmlElement? xRooti = document.DocumentElement;

            int AgreementId = 0;
            DateTime AgreementIssueDate = new DateTime(2000, 1, 1);
            int AgreementClientId = 0;
            int AgreementCarId = 0;
            DateTime AgreementReturnDate = new DateTime(2000, 1, 1);

            if (xRooti != null)
            {
                foreach (XmlElement xnode in xRooti)
                {
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        if (childnode.Name == "id")
                            AgreementId = Convert.ToInt32(childnode.InnerText);
                        else if (childnode.Name == "issueDate")
                            AgreementIssueDate = Convert.ToDateTime(childnode.InnerText);
                        else if (childnode.Name == "clientID")
                            AgreementClientId = Convert.ToInt32(childnode.InnerText);
                        else if (childnode.Name == "carID")
                            AgreementCarId = Convert.ToInt32(childnode.InnerText);
                        else if (childnode.Name == "returnDate")
                            AgreementReturnDate = Convert.ToDateTime(childnode.InnerText);
                    }
                    Agreement agreementInstance = new Agreement(AgreementId, AgreementIssueDate, AgreementClientId, AgreementCarId, AgreementReturnDate);
                    agreementsCollection.Add(agreementInstance);
                }
            }
            return agreementsCollection;
        }
    }
}
