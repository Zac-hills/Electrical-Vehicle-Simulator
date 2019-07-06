using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;


/*6900 Y value  7950 X value*/


public struct roadContainer
{
    public double x, y;
    public string edgeID;
}

public struct TransformerData
{
    public float x, y;
    public string transformerID;
}

public struct Cars
{
    public float Battery;
    public int VehicleID;
    public int HourIndex;
}

public struct TransformerLoad
{
    public String transformerID;
    public float TotalLoad;
}


namespace SUMO_INTERFACE
{
    public partial class Form1 : Form
    {
        //CONSTANTS:
        const double X_INTERSECTION = 7950;
        const double Y_INTERSECTION = 6900;
        const float Y_GEOMID = -78.7923f;
        const float X_GEOMID = 44.0358124f;
        //CONTAINERS:
        public static float[] HourlyDistribution = new float[24];
        public static Dictionary<String, bool[]> Checked = new Dictionary<String, bool[]>();
        public static Dictionary<string, bool> CheckedList = new Dictionary<string, bool>();
        public static Dictionary<string, float>[] TransformerLoads = new Dictionary<string, float>[]
            {
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),
                new Dictionary<string, float>(),

            };
       // public static List<TransformerData> UtilityHelper.TransformerContainer = new List<TransformerData>();
        public static Dictionary<string, Cars> CarInfo = new Dictionary<string, Cars>();
        public static Dictionary<string, float> TransformerCapacities = new Dictionary<string, float>();
       
        //STRINGS        
        public static String TransformerFileLoad = "TransformerLoad";
        public static String TXT_EXTENSION = ".txt";
        public static String CSV_EXTENSION = ".csv";
        public static String TransformerPositionFiles = "TransformerPositions";
        //ASSORTED VARIABLES
        public static int Counter = 0;
        //output Window
        public static Form2 OutputWindow = new Form2();
        //import Window API for window parenting
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public static Process p;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        private const UInt32 WM_CLOSE = 0x0010;

        public Form1()
        {
            
            InitializeComponent();
            //get SUMO Process ID
            p = System.Diagnostics.Process.Start("sumo-gui.exe");
            Thread.Sleep(500);
            //Set Sumo Window Inside Main Window
            SetParent(p.MainWindowHandle, panel1.Handle);
            ShowWindow(p.MainWindowHandle, 3);
            OutputWindow.Show();
            ShowWindow(GetConsoleWindow(), 0);
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            webBrowser1.Navigate("https://www.openstreetmap.org/#map=12/43.8642/-79.0260", null, null, "User-Agent: Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Win64; x64;)\r\n");
            //webBrowser1.FileDownload
            
        }

        public void MapDownload(Object sender, EventArgs e)
        {
            
        }
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown5.Maximum = 100000;

            UtilityHelper.CARS_IN_SIMULATION = (int)numericUpDown5.Value;
        }
        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            UtilityHelper.GasElectricRatio = (int)numericUpDown6.Value;
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            UtilityHelper.CalculateCenterPoint("ajax.net.xml");
            UtilityHelper.ReadCSV(TransformerPositionFiles);
            UtilityHelper.parseNetFiles("ajax.net.xml");
            //UtilityHelper.CalculateVehicleTypeDistribution();
            if (!UtilityHelper.WriteTrip("test.xml"))
            {
                return;
            }
            RunBatch();
            FileSystemWatcher Watch = new FileSystemWatcher();

            Watch.Path = Directory.GetCurrentDirectory() + "/";
            Watch.Filter = "Trip.out.xml";
            Watch.Changed += new FileSystemEventHandler(onChanged);
            Watch.EnableRaisingEvents = true;
        }

        public static void onChanged(object source, FileSystemEventArgs e)
        {
            Counter++;
            if (Counter == 2)
            {
                XmlDocument xDoc = new XmlDocument();
                XmlNodeList xmlNodes;
                FileStream fs = new FileStream("Trip.out.xml", FileMode.Open, FileAccess.Read);
                xDoc.Load(fs);
                xmlNodes = xDoc.GetElementsByTagName("tripinfo");
                for (int i = 0; i < xmlNodes.Count; i++)
                {
                    Cars temp;

                    temp.VehicleID = int.Parse(xmlNodes[i].Attributes[0].Value);
                    temp.Battery = 100 - (((float.Parse(xmlNodes[i].Attributes[11].Value) / 1000f) / 330f) * 100);
                    double normalizedTime = (double.Parse(xmlNodes[i].Attributes[6].Value)) / (86400);
                    temp.HourIndex = (int)(normalizedTime * 23);
                    if(UtilityHelper.VehicleType.Contains(temp.VehicleID))
                    CarInfo[xmlNodes[i].Attributes[0].Value] = temp;
                }

                fs.Close();
                XmlReader _reader = XmlReader.Create("Trip.geo.xml");
                StreamWriter _Writer = new StreamWriter("GeoPositions.txt");
                String lineFill = "";
                while (_reader.Read())
                {
                    switch (_reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (_reader.Name == "vehicle")
                            {
                                lineFill = _reader.GetAttribute("id");
                                lineFill += "," + _reader.GetAttribute("x");
                                lineFill += "," + _reader.GetAttribute("y");
                                _Writer.WriteLine(lineFill);
                                lineFill = "";
                            }
                            break;
                        default:
                            break;
                    }
                }
                _Writer.Flush();
                _Writer.Close();
                var reverse = File.ReadAllLines("GeoPositions.txt").Reverse();
                File.WriteAllLines("GeoPositions.txt", reverse);
                reverse = null;
                StreamReader finalPositions = new StreamReader("GeoPositions.txt");
                while (!finalPositions.EndOfStream)
                {
                    String[] s_Temp = finalPositions.ReadLine().Split(',');
                    if (!CarInfo.ContainsKey(s_Temp[0]))
                    {
                        continue;
                    }
                    if (CheckedList.ContainsKey(s_Temp[0]))
                    {

                    }
                    else
                    {
                        float CarX = float.Parse(s_Temp[2]);
                        float CarY = float.Parse(s_Temp[1]);
                        CheckedList[s_Temp[0]] = true;
                        for (int k = 0; k < UtilityHelper.TransformerContainer.Count; k++)
                        {

                            float newX = UtilityHelper.TransformerContainer[k].x - CarX;
                            float newY = UtilityHelper.TransformerContainer[k].y - CarY;

                            if ((float)Math.Sqrt(((newX * newX) + (newY * newY))) < 0.005f)
                            {
                                if (TransformerLoads[CarInfo[s_Temp[0]].HourIndex].ContainsKey(UtilityHelper.TransformerContainer[k].transformerID))
                                {
                                    TransformerLoads[CarInfo[s_Temp[0]].HourIndex][UtilityHelper.TransformerContainer[k].transformerID] += (5 - ((CarInfo[s_Temp[0]].Battery / 100) * 5)) * 17;
                                    HourlyDistribution[CarInfo[s_Temp[0]].HourIndex] += (5 - ((CarInfo[s_Temp[0]].Battery / 100) * 5)) * 17;
                                }
                                else
                                {
                                    TransformerLoads[CarInfo[s_Temp[0]].HourIndex][UtilityHelper.TransformerContainer[k].transformerID] = (5 - ((CarInfo[s_Temp[0]].Battery / 100) * 5)) * 17;
                                    HourlyDistribution[CarInfo[s_Temp[0]].HourIndex] += (5 - ((CarInfo[s_Temp[0]].Battery / 100) * 5)) * 17;
                                }
                                break;
                            }

                        }
                    }
                }

                /*
                 Write Hourly Distribution to file
                */
                for (int i = 0; i < HourlyDistribution.Length; i++)
                {
                    File.AppendAllText("HourlyDistribution.txt", i.ToString() + "," + HourlyDistribution[i].ToString() + Environment.NewLine);
                }
                ReadTransformerLoadMax("MaximumCapacity.txt");
                for (int loopFiles = 0; loopFiles < 24; loopFiles++)
                {
                    String[] file = File.ReadAllLines(TransformerFileLoad + loopFiles.ToString() + ".txt");
                    StreamWriter fileout = new StreamWriter("Added_Loads" + loopFiles.ToString() + ".txt");
                    //OutputWindow.Invoke((MethodInvoker)delegate { OutputWindow.BringToFront(); });
                    //OutputWindow.BringToFront();
                    foreach (var item in TransformerLoads[loopFiles])
                        for (int u = 0; u < file.Length; u++)
                        {
                            if (file[u].Contains(item.Key))
                            {
                                String[] temp = file[u].Split(',');
                                String[] TempTransformerID = temp[0].Split('_', '-');
                                if (!Checked.ContainsKey(item.Key))
                                {
                                    bool[] tempCheck = new bool[3];
                                    tempCheck[0] = false;
                                    tempCheck[1] = false;
                                    tempCheck[2] = false;
                                    Checked[item.Key] = tempCheck;
                                }
                                if (temp[12] == "A" && Checked[item.Key][0] == false)
                                {
                                    float currentLoad = float.Parse(temp[16]);
                                    float AddedLoad = ((item.Value) * 10) + currentLoad;
                                    if (TransformerCapacities.ContainsKey(TempTransformerID[1]))
                                    {
                                        if (TransformerCapacities[TempTransformerID[1]] < AddedLoad)
                                        {
                                            Console.WriteLine(TempTransformerID[1] + " is overloaded at " + loopFiles + ":00");
                                            OutputWindow.CreateNewViewNode(TempTransformerID[1], TransformerCapacities[TempTransformerID[1]].ToString(), AddedLoad.ToString(), loopFiles + ":00");
                                            
                                        }
                                    }
                                    double percentage = AddedLoad / currentLoad;
                                    temp[16] = (float.Parse(temp[16]) + ((item.Value) * 10)).ToString();
                                    String holdNewLine;
                                    holdNewLine = temp[0];
                                    for (int size = 1; size < temp.Length; size++)
                                    {
                                        holdNewLine += "," + temp[size];
                                    }
                                    holdNewLine += ",,,,,,";
                                    file[u] = holdNewLine;
                                    Checked[item.Key][0] = true;
                                    fileout.WriteLine(temp[0] + ", " + currentLoad.ToString() + ", " + ((item.Value) * 10).ToString() + ", %" + percentage.ToString());
                                }
                                else if (temp[12] == "B" && Checked[item.Key][1] == false)
                                {
                                    float currentLoad = float.Parse(temp[16]);
                                    float AddedLoad = ((item.Value) * 10) + currentLoad;
                                    if (TransformerCapacities.ContainsKey(TempTransformerID[1]))
                                    {
                                        if (TransformerCapacities[TempTransformerID[1]] < AddedLoad)
                                        {
                                            Console.WriteLine(TempTransformerID[1] + " is overloaded at " + loopFiles + ":00");
                                            OutputWindow.CreateNewViewNode(TempTransformerID[1], TransformerCapacities[TempTransformerID[1]].ToString(), AddedLoad.ToString(), loopFiles + ":00");
                                            
                                        }
                                    }
                                    double percentage = AddedLoad / currentLoad;
                                    temp[16] = (float.Parse(temp[16]) + ((item.Value) * 10)).ToString();
                                    String holdNewLine;
                                    holdNewLine = temp[0];
                                    for (int size = 1; size < temp.Length; size++)
                                    {
                                        holdNewLine += "," + temp[size];
                                    }
                                    holdNewLine += ",,,,,,";
                                    file[u] = holdNewLine;
                                    Checked[item.Key][1] = true;
                                    fileout.WriteLine(temp[0] + ", " + currentLoad.ToString() + ", " + ((item.Value) * 10).ToString() + ", %" + percentage.ToString());
                                }
                                else if (temp[12] == "C" && Checked[item.Key][2] == false)
                                {
                                    float currentLoad = float.Parse(temp[16]);
                                    float AddedLoad = ((item.Value) * 10) + currentLoad;
                                    if (TransformerCapacities.ContainsKey(TempTransformerID[1]))
                                    {
                                        if (TransformerCapacities[TempTransformerID[1]] < AddedLoad)
                                        {
                                            Console.WriteLine(TempTransformerID[1] + " is overloaded at " + loopFiles + ":00");
                                            OutputWindow.CreateNewViewNode(TempTransformerID[1], TransformerCapacities[TempTransformerID[1]].ToString(), AddedLoad.ToString(), loopFiles + ":00");
                 
                                        }
                                    }
                                    double percentage = AddedLoad / currentLoad;
                                    temp[16] = (float.Parse(temp[16]) + ((item.Value) * 10)).ToString();
                                    String holdNewLine;
                                    holdNewLine = temp[0];
                                    for (int size = 1; size < temp.Length; size++)
                                    {
                                        holdNewLine += "," + temp[size];
                                    }
                                    holdNewLine += ",,,,,,";
                                    file[u] = holdNewLine;
                                    Checked[item.Key][2] = true;
                                    fileout.WriteLine(temp[0] + ", " + currentLoad.ToString() + ", " + ((item.Value) * 10).ToString() + ", %" + percentage.ToString());
                                }
                            }
                        }
                    String[] TopFile = File.ReadAllLines("TopLoad.txt");
                    File.WriteAllLines("LoadTransformerOutput" + loopFiles.ToString() + ".txt", TopFile);
                    File.AppendAllLines("LoadTransformerOutput" + loopFiles.ToString() + ".txt", file);
                    foreach (KeyValuePair<String, bool[]> entry in Checked)
                    {
                        entry.Value[0] = false;
                        entry.Value[1] = false;
                        entry.Value[2] = false;
                    }
                }
            OutputWindow.Invoke((MethodInvoker)delegate { OutputWindow.SaveTreeNodes(); });
            }

        }

        public void RunBatch()
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "start-command-line.bat";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.OutputDataReceived += CheckForCompletion;
            cmd.Start();


            cmd.StandardInput.WriteLine(("cd /d " + System.IO.Directory.GetCurrentDirectory()));
            cmd.StandardInput.WriteLine("duarouter.exe -n ajax.net.xml -t test.xml -o ajax.rou.xml --ignore-errors=true");
            cmd.BeginOutputReadLine();
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            cmd.WaitForExit();
        }
        public void CheckForCompletion(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == "Success.")
            {
                this.BeginInvoke(new MethodInvoker(delegate () 
                {
                    WarningWindow ww = new WarningWindow("Success. You may now Load the config File in Sumo.");
                    ww.Show();
                }));
            }
        }
    

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public static void ReadTransformerLoadMax(string FilePath)
        {
            StreamReader f_SR = new StreamReader(FilePath);
            String Catch = "#N/A";
            while (!f_SR.EndOfStream)
            {
                String[] Temp = f_SR.ReadLine().Split(',');
                if (Temp[1] == Catch)
                {
                    continue;
                }
                else
                {
                    float TempMAXCapacity = 0;
                    float.TryParse(Temp[1], out TempMAXCapacity);
                    TransformerCapacities.Add(Temp[0], TempMAXCapacity);
                }
            }
            f_SR.Close();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingWindow settingWindows = new SettingWindow();
            settingWindows.Show();
        }

        private void Form1_FormClosed(object sender, EventArgs e)
        {
            SendMessage(p.MainWindowHandle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AutoScroll = true;
        }

        private void profileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HourlyChart hourlyChart = new HourlyChart();
            hourlyChart.Show();
        }

        private void vehiclesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VehicleCreation vehicleCreation = new VehicleCreation();
            vehicleCreation.Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveManager.SavedFileName == null)
            {
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Title = "Save File";
                SFD.InitialDirectory = Directory.GetCurrentDirectory() + "\\" + "Projects";
                SFD.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
                SFD.FilterIndex = 2;
                SFD.RestoreDirectory = true;
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    SaveManager.SavedFileName = SFD.FileName;
                }
            }
            SaveManager.SaveProject();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveManager.LoadFile())
            {
                numericUpDown5.Value = UtilityHelper.CARS_IN_SIMULATION;
            }
        }
    }
}
