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
    public partial class HourlyChart : Form
    {
        public HourlyChart()
        {
            InitializeComponent();
        }
        int IndexClicked = -1;
        int IndexClicked2 = -1;

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var results = chart1.HitTest(e.X, e.Y, false, System.Windows.Forms.DataVisualization.Charting.ChartElementType.DataPoint);
                foreach(var result in results)
                if (result.ChartElementType == System.Windows.Forms.DataVisualization.Charting.ChartElementType.DataPoint)
                {
                    IndexClicked = result.PointIndex;
                }

            }
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y > -1 && e.Y < chart1.Size.Height)
            {
                if (IndexClicked != -1)
                {

                    int PosY = Math.Max(Math.Min((int)chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y), 100), 0);
                    chart1.Series[0].Points[IndexClicked].SetValueY(PosY);
                    UtilityHelper.HourlyCarDistribution[IndexClicked] = PosY;
                    Console.WriteLine(PosY);
                    int total = 0;
                    for (int i = 0; i < 24; i++)
                    {
                        total += UtilityHelper.HourlyCarDistribution[i];
                    }
                    if (total > 100)
                    {
                        chart1.Series[0].Points[IndexClicked].SetValueY(PosY-(total-100));
                        UtilityHelper.HourlyCarDistribution[IndexClicked] = PosY-(total-100);
                        total = 100;
                    }
                    chart1.Refresh();
                    PositionDistribution.Text = total.ToString();
                }

            }
        }

        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IndexClicked = -1;
            }
        }

        private void chart2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var result = chart2.HitTest(e.X, e.Y);
                Console.WriteLine(result.ChartElementType);
                if (result.ChartElementType == System.Windows.Forms.DataVisualization.Charting.ChartElementType.DataPoint)
                {
                    IndexClicked2 = result.PointIndex;
                }
            }
        }

        private void chart2_MouseMove(object sender, MouseEventArgs e)
        {
            
            if(e.Y < chart2.Size.Height && e.Y > -1)
            if (IndexClicked2 != -1)
            {
                int PosY = Math.Max(Math.Min((int)chart2.ChartAreas[0].AxisY.PixelPositionToValue(e.Y), 100), 0);
                chart2.Series[0].Points[IndexClicked2].SetValueY(PosY);
                Console.WriteLine(PosY);
                chart2.Refresh();
                switch (IndexClicked2)
                {
                    case 0:
                        UtilityHelper.quad1Percent = chart2.Series[0].Points[IndexClicked2].YValues[0];
                        break;
                    case 1:
                        UtilityHelper.quad2Percent = chart2.Series[0].Points[IndexClicked2].YValues[0];
                        break;
                    case 2:
                        UtilityHelper.quad3Percent = chart2.Series[0].Points[IndexClicked2].YValues[0];
                        break;
                    case 3:
                        UtilityHelper.quad4Percent = chart2.Series[0].Points[IndexClicked2].YValues[0];
                        break;
                }
            }

            HourlyTotal.Text = (UtilityHelper.quad1Percent + UtilityHelper.quad2Percent + UtilityHelper.quad3Percent + UtilityHelper.quad4Percent).ToString();
        }

        private void chart2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IndexClicked2 = -1;
            }
        }

        private void HourlyChart_Load(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false; ;
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 100;
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 23;
            chart1.Series[0].BorderWidth = 3;
            for (int i = 0; i < 24; i++)
            {
                chart1.Series[0].Points.AddXY(i, UtilityHelper.HourlyCarDistribution[i]);
            }
            chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart2.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart2.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart2.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            chart2.ChartAreas[0].AxisY.Minimum = -1;
            chart2.ChartAreas[0].AxisY.Maximum = 100;
            chart2.ChartAreas[0].AxisX.Interval = 1;
            chart2.ChartAreas[0].AxisX.Minimum = 1;
            chart2.ChartAreas[0].AxisX.Maximum = 4;
            chart2.Series[0].Points.AddXY(1, UtilityHelper.quad1Percent);
            chart2.Series[0].Points.AddXY(2, UtilityHelper.quad2Percent);
            chart2.Series[0].Points.AddXY(3, UtilityHelper.quad3Percent);
            chart2.Series[0].Points.AddXY(4, UtilityHelper.quad4Percent);
            chart2.Series[0].BorderWidth = 3;
            HourlyTotal.Text = "100";
            PositionDistribution.Text = "0";
            AutoScroll = true;
        }

        private void Set_Value_Click(object sender, EventArgs e)
        {
            int Sum = 0;
            int Sum2 = 0;
            for (int i = 0; i < 4; i++)
            {
                Sum += (int)chart2.Series[0].Points[i].YValues[0];
            }
            for (int i = 0; i < 24; i++)
            {
                Sum2 += (int)chart1.Series[0].Points[i].YValues[0];
            }

            if (Sum != 100 && Sum2 != 100)
            {
                WarningWindow WW = new WarningWindow("The quadrant and hourly distribution graphs need to add up to 100. Both graphs are incorrect.");
                WW.Show();
            }
            else if (Sum != 100)
            {
                WarningWindow WW = new WarningWindow("The quadrant distribution graph needs to add up to 100.");
                WW.Show();
            }
            else if (Sum2 != 100)
            {
                WarningWindow WW = new WarningWindow("The hourly distribution graph needs to add up to 100.");
                WW.Show();
            }
            else
            {
                this.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void HourlyTotal_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
