using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace SUMO_INTERFACE
{
    class SaveManager
    {
        public static string SavedFileName;
        public static string m_DirectoryPath = Directory.GetCurrentDirectory() + "\\" + "Projects";
        //utility function loads all files from given directory
        public static bool LoadFile()
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Title = "Save File";
            OFD.InitialDirectory = m_DirectoryPath;
            OFD.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            OFD.FilterIndex = 2;
            OFD.RestoreDirectory = true;
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                SavedFileName = OFD.FileName;
                string[] FileBuffer = File.ReadAllLines(SavedFileName);
                UtilityHelper.quad1Percent = double.Parse(FileBuffer[0]);
                UtilityHelper.quad2Percent = double.Parse(FileBuffer[1]);
                UtilityHelper.quad3Percent = double.Parse(FileBuffer[2]);
                UtilityHelper.quad4Percent = double.Parse(FileBuffer[3]);
                UtilityHelper.CARS_IN_SIMULATION = int.Parse(FileBuffer[4]);
                for (int i = 0; i < 24; i++)
                {
                    UtilityHelper.HourlyCarDistribution[i] = int.Parse(FileBuffer[5 + i]);
                }
                return true;
            }
            return false;

        }
        //SetsTransformerFolder

        //saves project
        public static void SaveProject()
        {

            StreamWriter writer = new StreamWriter(SavedFileName);
            writer.WriteLine(UtilityHelper.quad1Percent);
            writer.WriteLine(UtilityHelper.quad2Percent);
            writer.WriteLine(UtilityHelper.quad3Percent);
            writer.WriteLine(UtilityHelper.quad4Percent);

            writer.WriteLine(UtilityHelper.CARS_IN_SIMULATION);
            for (int i = 0; i < 24; i++)
            {
                writer.WriteLine(UtilityHelper.HourlyCarDistribution[i]);
            }

            writer.Close();
        }

    }
}
