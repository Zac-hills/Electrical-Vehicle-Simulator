using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;



namespace SUMO_INTERFACE
{
    class UtilityHelper
    {
        static private int NumOfSecondsInHour = 3600;
        public static int GasElectricRatio=100;
        public static Random random = new Random();
        public static int tripID = 0;
        public static int MAX_PERCENTAGE = 100;
        static public int CARS_IN_SIMULATION = 0;
        static public double quad1Percent = 25,
            quad2Percent = 25,
            quad3Percent = 25,
            quad4Percent = 25;
        public static int[] HourlyCarDistribution = new int[24];
        public static List<int> VehicleType = new List<int>();
        private static List<roadContainer> quad1 = new List<roadContainer>();
        private static List<roadContainer> quad2 = new List<roadContainer>();
        private static List<roadContainer> quad3 = new List<roadContainer>();
        private static List<roadContainer> quad4 = new List<roadContainer>();

        private static double X_INTERSECTION;
        private static double Y_INTERSECTION;
        private static String TXT_EXTENSION = ".txt";
        static public List<TransformerData> TransformerContainer = new List<TransformerData>();
        public static void CalculateVehicleTypeDistribution()
        {
            for (int i = 0; i < CARS_IN_SIMULATION; i++)
            {
                int CurrentRandom=random.Next(1, 101);
                if (CurrentRandom <= GasElectricRatio)
                {
                    VehicleType.Add(i);
                }
            }
        }
        public static double[] CalculateCenterPoint(string FileName)
        {
            int Count = 0;
            double[] f_CenterPoint = new double[2];
            char[] splitCharacters = { ' ', ',' };
            XmlReader reader = XmlReader.Create(FileName);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "edge" && reader.GetAttribute("shape") != null)
                        {
                            string[] hold = (reader.GetAttribute("shape")).Split(splitCharacters);
                            for (int i = 0; i < hold.Length; i += 2)
                            {
                                roadContainer temp = new roadContainer();
                                double.TryParse(hold[i], out temp.x);
                                double.TryParse(hold[i + 1], out temp.y);
                                temp.edgeID = reader.GetAttribute("id");
                                f_CenterPoint[0] += temp.x;
                                f_CenterPoint[1] += temp.y;
                                Count++;
                            }
                        }
                        break;
                }
            }

            f_CenterPoint[0] = f_CenterPoint[0] / Count;
            f_CenterPoint[1] = f_CenterPoint[1] / Count;
            X_INTERSECTION = f_CenterPoint[0];
            Y_INTERSECTION = f_CenterPoint[1];
            return f_CenterPoint;
        }

        public static bool WriteTrip(string FileName)
        {
            //default is test.xml
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };

            XmlWriter _Writer = XmlWriter.Create(FileName, settings);

            _Writer.WriteStartDocument();
            _Writer.WriteStartElement("trips");
            _Writer.WriteStartElement("vType");
            _Writer.WriteAttributeString("id", "ElectricCar");
            _Writer.WriteAttributeString("accel", "1.0");
            _Writer.WriteAttributeString("decel", "1.0");
            _Writer.WriteAttributeString("length", "4");
            _Writer.WriteAttributeString("maxSpeed", "100");
            _Writer.WriteAttributeString("sigma", "0.0");
            _Writer.WriteAttributeString("minGap", "2.5");
            _Writer.WriteAttributeString("color", "1,0,0");
            _Writer.WriteStartElement("param");
            _Writer.WriteAttributeString("key", "MaxBatKap");
            _Writer.WriteAttributeString("value", "85");
            _Writer.WriteEndElement();

            _Writer.WriteStartElement("param");
            _Writer.WriteAttributeString("key", "actualBatteryCapacity");
            _Writer.WriteAttributeString("value", "2000");
            _Writer.WriteEndElement();

            _Writer.WriteStartElement("param");
            _Writer.WriteAttributeString("key", "vehicleMass");
            _Writer.WriteAttributeString("value", "216");
            _Writer.WriteEndElement();

            _Writer.WriteStartElement("param");
            _Writer.WriteAttributeString("key", "frontSurfaceArea");
            _Writer.WriteAttributeString("value", "4");
            _Writer.WriteEndElement();

            _Writer.WriteStartElement("param");
            _Writer.WriteAttributeString("key", "airDragCoefficient");
            _Writer.WriteAttributeString("value", "0.6");
            _Writer.WriteEndElement();

            _Writer.WriteStartElement("param");
            _Writer.WriteAttributeString("key", "internalMomentOfInertia");
            _Writer.WriteAttributeString("value", "0.01");
            _Writer.WriteEndElement();

            _Writer.WriteStartElement("param");
            _Writer.WriteAttributeString("key", "radialDragCoefficient");
            _Writer.WriteAttributeString("value", "0.5");
            _Writer.WriteEndElement();

            _Writer.WriteStartElement("param");
            _Writer.WriteAttributeString("key", "constantPowerIntake");
            _Writer.WriteAttributeString("value", "0.00327");
            _Writer.WriteEndElement();

            _Writer.WriteStartElement("param");
            _Writer.WriteAttributeString("key", "propulsionEfficiency");
            _Writer.WriteAttributeString("value", "0.9");
            _Writer.WriteEndElement();

            _Writer.WriteStartElement("param");
            _Writer.WriteAttributeString("key", "recuperationEfficiency");
            _Writer.WriteAttributeString("value", "0.9");
            _Writer.WriteEndElement();

            _Writer.WriteEndElement();
            for (int j = 0; j < 24; j++)
            {
                if (tripID < j * NumOfSecondsInHour)
                {
                    tripID = j * NumOfSecondsInHour;
                }
                int TripIncrement = (int)(CARS_IN_SIMULATION * ((quad1Percent / 100f) * ((double)HourlyCarDistribution[j] / 100f)));
                TripIncrement += (int)(CARS_IN_SIMULATION * ((quad2Percent / 100f) * ((double)HourlyCarDistribution[j] / 100f)));
                TripIncrement += (int)(CARS_IN_SIMULATION * ((quad3Percent / 100f) * ((double)HourlyCarDistribution[j] / 100f)));
                TripIncrement += (int)(CARS_IN_SIMULATION * ((quad4Percent / 100f) * ((double)HourlyCarDistribution[j] / 100f)));
                if (TripIncrement != 0)
                {
                    TripIncrement = (NumOfSecondsInHour / TripIncrement);
                    if (TripIncrement == 0)
                    {
                        WarningWindow WW = new WarningWindow("At Hour: " + j.ToString() + "Vehicle limit is exceeded(3600 per hour).");
                        WW.Show();
                        return false;
                    }
                }
                for (int i = 0; i < (int)(CARS_IN_SIMULATION * ((quad1Percent / 100f) * ((double)HourlyCarDistribution[j] / 100f))); i++)
                {
                    if (random.Next(1, 101) <= UtilityHelper.GasElectricRatio)
                    {
                        VehicleType.Add(tripID);
                    }
                    _Writer.WriteStartElement("trip");
                    _Writer.WriteAttributeString("id", tripID.ToString());
                    _Writer.WriteAttributeString("depart", (tripID.ToString() + ".00"));
                    _Writer.WriteAttributeString("from", quad1[random.Next(0, quad1.Count)].edgeID.ToString());
                    switch (random.Next(0, 3))
                    {
                        case 0:
                            _Writer.WriteAttributeString("to", quad1[random.Next(0, quad1.Count)].edgeID.ToString());
                            break;
                        case 1:
                            _Writer.WriteAttributeString("to", quad2[random.Next(0, quad2.Count)].edgeID.ToString());
                            break;
                        case 2:
                            _Writer.WriteAttributeString("to", quad3[random.Next(0, quad3.Count)].edgeID.ToString());
                            break;
                        case 3:
                            _Writer.WriteAttributeString("to", quad4[random.Next(0, quad4.Count)].edgeID.ToString());
                            break;
                        default:
                            break;
                    }
                    _Writer.WriteStartElement("param");
                    _Writer.WriteAttributeString("key", "ActBatKap");
                    _Writer.WriteAttributeString("value", "85");
                    _Writer.WriteEndElement();
                    _Writer.WriteEndElement();
                    tripID++;
                }
                for (int i = 0; i < (int)(CARS_IN_SIMULATION * ((quad2Percent / 100f) * ((double)HourlyCarDistribution[j] / 100f))); i++)
                {
                    if (random.Next(1, 101) <= UtilityHelper.GasElectricRatio)
                    {
                        VehicleType.Add(tripID);
                    }
                    _Writer.WriteStartElement("trip");
                    _Writer.WriteAttributeString("id", tripID.ToString());
                    _Writer.WriteAttributeString("depart", (tripID.ToString() + ".00"));
                    _Writer.WriteAttributeString("from", quad2[random.Next(0, quad2.Count)].edgeID.ToString());
                    switch (random.Next(0, 3))
                    {
                        case 0:
                            _Writer.WriteAttributeString("to", quad1[random.Next(0, quad1.Count)].edgeID.ToString());
                            break;
                        case 1:
                            _Writer.WriteAttributeString("to", quad2[random.Next(0, quad2.Count)].edgeID.ToString());
                            break;
                        case 2:
                            _Writer.WriteAttributeString("to", quad3[random.Next(0, quad3.Count)].edgeID.ToString());
                            break;
                        case 3:
                            _Writer.WriteAttributeString("to", quad4[random.Next(0, quad4.Count)].edgeID.ToString());
                            break;
                        default:
                            break;
                    }
                    _Writer.WriteStartElement("param");
                    _Writer.WriteAttributeString("key", "ActBatKap");
                    _Writer.WriteAttributeString("value", "85");
                    _Writer.WriteEndElement();
                    _Writer.WriteEndElement();
                    tripID++;
                }
                for (int i = 0; i < (int)(CARS_IN_SIMULATION * ((quad3Percent / 100f) * ((double)HourlyCarDistribution[j] / 100f))); i++)
                {
                    if (random.Next(1, 101) <= UtilityHelper.GasElectricRatio)
                    {
                        VehicleType.Add(tripID);
                    }
                    _Writer.WriteStartElement("trip");
                    _Writer.WriteAttributeString("id", tripID.ToString());
                    _Writer.WriteAttributeString("depart", (tripID.ToString() + ".00"));
                    _Writer.WriteAttributeString("from", quad3[random.Next(0, quad3.Count)].edgeID.ToString());
                    switch (random.Next(0, 3))
                    {
                        case 0:
                            _Writer.WriteAttributeString("to", quad1[random.Next(0, quad1.Count)].edgeID.ToString());
                            break;
                        case 1:
                            _Writer.WriteAttributeString("to", quad2[random.Next(0, quad2.Count)].edgeID.ToString());
                            break;
                        case 2:
                            _Writer.WriteAttributeString("to", quad3[random.Next(0, quad3.Count)].edgeID.ToString());
                            break;
                        case 3:
                            _Writer.WriteAttributeString("to", quad4[random.Next(0, quad4.Count)].edgeID.ToString());
                            break;
                        default:
                            break;
                    }
                    _Writer.WriteStartElement("param");
                    _Writer.WriteAttributeString("key", "ActBatKap");
                    _Writer.WriteAttributeString("value", "85");
                    _Writer.WriteEndElement();
                    _Writer.WriteEndElement();
                    tripID++;
                }
                for (int i = 0; i < (int)(CARS_IN_SIMULATION * ((quad4Percent / 100f) * ((double)HourlyCarDistribution[j] / 100f))); i++)
                {
                    if (random.Next(1, 101) <= UtilityHelper.GasElectricRatio)
                    {
                        VehicleType.Add(tripID);
                    }
                    _Writer.WriteStartElement("trip");
                    _Writer.WriteAttributeString("id", tripID.ToString());
                    _Writer.WriteAttributeString("depart", (tripID.ToString() + ".00"));
                    _Writer.WriteAttributeString("from", quad4[random.Next(0, quad4.Count)].edgeID.ToString());
                    switch (random.Next(0, 3))
                    {
                        case 0:
                            _Writer.WriteAttributeString("to", quad1[random.Next(0, quad1.Count)].edgeID.ToString());
                            break;
                        case 1:
                            _Writer.WriteAttributeString("to", quad2[random.Next(0, quad2.Count)].edgeID.ToString());
                            break;
                        case 2:
                            _Writer.WriteAttributeString("to", quad3[random.Next(0, quad3.Count)].edgeID.ToString());
                            break;
                        case 3:
                            _Writer.WriteAttributeString("to", quad4[random.Next(0, quad4.Count)].edgeID.ToString());
                            break;
                        default:
                            break;
                    }
                    _Writer.WriteStartElement("param");
                    _Writer.WriteAttributeString("key", "ActBatKap");
                    _Writer.WriteAttributeString("value", "85");
                    _Writer.WriteEndElement();
                    _Writer.WriteEndElement();
                    tripID++;
                }
            }

             _Writer.WriteEndDocument();
             _Writer.Close();
            return true;
        }

        static public void ReadCSV(string TransformerPositionFiles)
        {
            var reader = new StreamReader(File.OpenRead(TransformerPositionFiles + TXT_EXTENSION));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                TransformerData temp;
                temp.transformerID = values[0];
                temp.x = float.Parse(values[3]);
                temp.y = float.Parse(values[2]);
                TransformerContainer.Add(temp);

            }
        }

        static public void parseNetFiles(string FileName)
        {
            //fileName = ajax.net.xml
            char[] splitCharacters = { ' ', ',' };
            XmlReader reader = XmlReader.Create(FileName);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "edge" && reader.GetAttribute("shape") != null)
                        {
                            string[] hold = (reader.GetAttribute("shape")).Split(splitCharacters);
                            for (int i = 0; i < hold.Length; i += 2)
                            {
                                roadContainer temp = new roadContainer();
                                double.TryParse(hold[i], out temp.x);
                                double.TryParse(hold[i + 1], out temp.y);
                                temp.edgeID = reader.GetAttribute("id");
                                if (temp.x < X_INTERSECTION && temp.y > Y_INTERSECTION)
                                {
                                    quad1.Add(temp);
                                }
                                else if (temp.x > X_INTERSECTION && temp.y > Y_INTERSECTION)
                                {
                                    quad2.Add(temp);
                                }
                                else if (temp.x < X_INTERSECTION && temp.y < Y_INTERSECTION)
                                {
                                    quad3.Add(temp);
                                }
                                else if (temp.x > X_INTERSECTION && temp.y < Y_INTERSECTION)
                                {
                                    quad4.Add(temp);
                                }
                            }
                        }
                        break;
                }
            }


        }
      
    }
}
