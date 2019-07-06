namespace SUMO_INTERFACE
{
    partial class HourlyChart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Set_Value = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.HourlyTotal = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.PositionDistribution = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(16, 15);
            this.chart1.Margin = new System.Windows.Forms.Padding(4);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.IsValueShownAsLabel = true;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(1988, 292);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "HourlyGraph";
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            this.chart1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseDown);
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            this.chart1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseUp);
            // 
            // chart2
            // 
            chartArea2.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea2);
            legend2.Enabled = false;
            legend2.Name = "Legend1";
            this.chart2.Legends.Add(legend2);
            this.chart2.Location = new System.Drawing.Point(52, 350);
            this.chart2.Margin = new System.Windows.Forms.Padding(4);
            this.chart2.Name = "chart2";
            series2.ChartArea = "ChartArea1";
            series2.IsValueShownAsLabel = true;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart2.Series.Add(series2);
            this.chart2.Size = new System.Drawing.Size(605, 369);
            this.chart2.TabIndex = 1;
            this.chart2.Text = "chart2";
            this.chart2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chart2_MouseDown);
            this.chart2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart2_MouseMove);
            this.chart2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart2_MouseUp);
            // 
            // Set_Value
            // 
            this.Set_Value.Location = new System.Drawing.Point(1681, 657);
            this.Set_Value.Margin = new System.Windows.Forms.Padding(4);
            this.Set_Value.Name = "Set_Value";
            this.Set_Value.Size = new System.Drawing.Size(323, 102);
            this.Set_Value.TabIndex = 2;
            this.Set_Value.Text = "Set Value";
            this.Set_Value.UseVisualStyleBackColor = true;
            this.Set_Value.Click += new System.EventHandler(this.Set_Value_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(907, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(276, 29);
            this.label1.TabIndex = 3;
            this.label1.Text = "Hourly Car Distribution";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(209, 337);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(308, 29);
            this.label2.TabIndex = 4;
            this.label2.Text = "Car Quadrant Distribution";
            // 
            // HourlyTotal
            // 
            this.HourlyTotal.AutoSize = true;
            this.HourlyTotal.Location = new System.Drawing.Point(1079, 644);
            this.HourlyTotal.Name = "HourlyTotal";
            this.HourlyTotal.Size = new System.Drawing.Size(46, 17);
            this.HourlyTotal.TabIndex = 5;
            this.HourlyTotal.Text = "label3";
            this.HourlyTotal.Click += new System.EventHandler(this.HourlyTotal_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(972, 615);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(284, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Vehicle Hourly Distribution Total";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(1323, 615);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(297, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Vehicle Position Distribution Total";
            // 
            // PositionDistribution
            // 
            this.PositionDistribution.AutoSize = true;
            this.PositionDistribution.Location = new System.Drawing.Point(1435, 644);
            this.PositionDistribution.Name = "PositionDistribution";
            this.PositionDistribution.Size = new System.Drawing.Size(46, 17);
            this.PositionDistribution.TabIndex = 8;
            this.PositionDistribution.Text = "label5";
            // 
            // HourlyChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1914, 774);
            this.Controls.Add(this.PositionDistribution);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.HourlyTotal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Set_Value);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.chart1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "HourlyChart";
            this.Text = "HourlyChart";
            this.Load += new System.EventHandler(this.HourlyChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.Button Set_Value;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label HourlyTotal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label PositionDistribution;
    }
}