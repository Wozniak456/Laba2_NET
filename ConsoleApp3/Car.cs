using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApp3
{
    internal class Car 
    {
        public int СarId { get; set; }
        public string CarBrand { get; set; }
        public string CarType { get; set; }
        public int CarYear { get; set; }
        public double CarCostForLessThen1Mounth { get; set; }
        public double PledgeAmount { get; set; }
        public double CarCostForMoreThen1Mounth { get; set; }
        public Car(int carId, string carBrand, string carType, int carYear, double carCostForLessThen1Mounth, double pledgeAmount, double carCostForMoreThen1Mounth)
        {
            this.СarId = carId;
            this.CarBrand = carBrand;
            this.CarType = carType;
            this.CarYear = carYear;
            this.CarCostForLessThen1Mounth = carCostForLessThen1Mounth;
            this.PledgeAmount = pledgeAmount;
            this.CarCostForMoreThen1Mounth = carCostForMoreThen1Mounth;
        }
        public Car()
        {
        }
        public override string ToString()
        {
            return "\n\t[Info about car]\n" + "Id: " + this.СarId + "\nBrand: " + this.CarBrand + "\ntType: " + this.CarType +
                "\nThe year of priduction: " + this.CarYear + "\nPledge amount: " + this.PledgeAmount + " $\nUsual payment(for less then 1 month): " + this.CarCostForLessThen1Mounth + " $ " +
                "\nDiscounted payment: " + this.CarCostForMoreThen1Mounth + " $";
        } 
        public static List<Car> WriteFromXmlFile(string file)
        {
            List<Car> carsCollection = new List<Car>();
            XmlDocument document = new XmlDocument();
            document.Load(file);
            XmlElement? xRooti = document.DocumentElement;

            int СarId = 0; 
            string CarBrand = ""; 
            string CarType = ""; 
            int CarYear = 0; 
            double CarCostForLessThen1Mounth = 0;
            double CarCostForMoreThen1Mounth = 0; 
            double PledgeAmount = 0;

            if (xRooti != null)
            {
                foreach (XmlElement xnode in xRooti)
                {
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        if (childnode.Name == "id")
                            СarId = Convert.ToInt32(childnode.InnerText);
                        else if (childnode.Name == "brand")
                            CarBrand = childnode.InnerText;
                        else if (childnode.Name == "type")
                            CarType = childnode.InnerText;
                        else if (childnode.Name == "year")
                            CarYear = Convert.ToInt32(childnode.InnerText);
                        else if (childnode.Name == "cost1")
                            CarCostForLessThen1Mounth = Convert.ToDouble(childnode.InnerText);
                        else if (childnode.Name == "cost3")
                            CarCostForMoreThen1Mounth = Convert.ToDouble(childnode.InnerText);
                        else if (childnode.Name == "pledgeAmount")
                            PledgeAmount = Convert.ToDouble(childnode.InnerText);
                    }
                    Car carInstance = new Car(СarId, CarBrand, CarType, CarYear, CarCostForLessThen1Mounth, PledgeAmount, CarCostForMoreThen1Mounth);
                    carsCollection.Add(carInstance);
                }
            }
            return carsCollection;
        }
    }
}

        
  
