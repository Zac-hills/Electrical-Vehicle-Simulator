using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

public struct CarInfo
{
    public float BatteryCapacity;
    public float AverageDistancePerCharge;
    public string Name;
}
namespace SUMO_INTERFACE
{
    public partial class VehicleCreation : Form
    {
        public static string VehicleClassFolder = "SupportedVehicles";
        public static List<CarInfo> SupportVehicles;
        private static int MaximumFieldSize = 10000;
        public VehicleCreation()
        {
            InitializeComponent();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void VehicleCreation_Load(object sender, EventArgs e)
        {
            numericUpDown1.Maximum = MaximumFieldSize;
            numericUpDown2.Maximum = MaximumFieldSize;
            string[] FileNames = Directory.GetFiles(Directory.GetCurrentDirectory() + "/" + VehicleClassFolder);
            foreach (string VehicleFile in FileNames)
            {
                string[] f_TempFileBuffer = File.ReadAllLines(VehicleFile);
                CarInfo temp;
                temp.Name = f_TempFileBuffer[0];
                temp.AverageDistancePerCharge = float.Parse(f_TempFileBuffer[1]);
                temp.BatteryCapacity = float.Parse(f_TempFileBuffer[2]);
                TreeNode tempNode = new TreeNode(f_TempFileBuffer[0]);
                tempNode.Nodes.Add(f_TempFileBuffer[1]);
                tempNode.Nodes.Add(f_TempFileBuffer[2]);
                treeView1.Nodes.Add(tempNode);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamWriter SW = new StreamWriter(Directory.GetCurrentDirectory() + "/" + VehicleClassFolder + "/" + textBox1.Text + ".txt");
            SW.WriteLine(textBox1.Text);
            SW.WriteLine(numericUpDown1.Value);
            SW.WriteLine(numericUpDown2.Value);
            SW.Close();
        }
    }
}
