using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SUMO_INTERFACE
{
    public partial class WarningWindow : Form
    {
        string WarningMessage;
        public WarningWindow(string a_WarningMessage)
        {
            WarningMessage = a_WarningMessage;
            InitializeComponent();
        }

        private void WarningWindow_Load(object sender, EventArgs e)
        {
            textBox1.Text = WarningMessage;
            System.Media.SystemSounds.Exclamation.Play();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
