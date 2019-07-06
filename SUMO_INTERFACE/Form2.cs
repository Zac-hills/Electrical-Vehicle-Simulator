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
namespace SUMO_INTERFACE
{
    public partial class Form2 : Form
    {
        public static List<TreeNode> OutputList = new List<TreeNode>();

        public Form2()
        {
            InitializeComponent();
            treeView1.Scrollable = true;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public void CreateNewViewNode(String a_TransformerID, String a_MaxCapacity, String a_CurrentCapacity, String a_Time)
        {
            //TreeNode tn = new TreeNode(a_TransformerID);
            //tn.Nodes.Add("Max Capacity: " + a_MaxCapacity);
            //tn.Nodes.Add("Current Load: " + a_CurrentCapacity);
            //tn.Nodes.Add("Current Hour: " + a_Time);
            ////OutputList.Add(tn);
            Invoke((MethodInvoker)delegate 
            {
                if (treeView1.Nodes.ContainsKey(a_TransformerID))
                {
                    treeView1.BeginUpdate();
                    var CurrentNode = treeView1.Nodes.Find(a_TransformerID, true).First();
                    CurrentNode.Nodes.Add("Current Load: " + a_CurrentCapacity);
                    CurrentNode.Nodes.Add("Current Hour: " + a_Time);
                    treeView1.EndUpdate();
                }
                else
                {
                    treeView1.BeginUpdate();
                    treeView1.Nodes.Add(a_TransformerID, a_TransformerID);
                    var CurrentNode = treeView1.Nodes.Find(a_TransformerID,true).First();
                    CurrentNode.Nodes.Add("Max Capacity: " + a_MaxCapacity);
                    CurrentNode.Nodes.Add("Current Load: " + a_CurrentCapacity);
                    CurrentNode.Nodes.Add("Current Hour: " + a_Time);
                    treeView1.EndUpdate();
                }
            });

        }
        public void SaveTreeNodes()
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Title = "Save File";
            SFD.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\" + "TransformerData";
            SFD.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            SFD.FilterIndex = 2;
            SFD.RestoreDirectory = true;
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                StreamWriter l_StreamWriter = new StreamWriter(SFD.FileName);
                foreach (TreeNode Node in treeView1.Nodes)
                {
                    string CurrentLine = Node.Text;
                    //l_StreamWriter.Write(Node.Text);

                    foreach (TreeNode ChildNode in Node.Nodes)
                    {
                        CurrentLine += ",";
                        CurrentLine += ChildNode.Text;
                    }
                    l_StreamWriter.WriteLine(CurrentLine);
                }
                l_StreamWriter.Close();
            }
        }
    }
}
