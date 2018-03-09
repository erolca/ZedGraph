using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;


namespace graph_real_time
{
    public partial class Form1 : Form
    {
        // Starting time in milliseconds
        int tickStart = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Sample at 50ms intervals
            timer1.Interval = 100;
            timer1.Enabled = true;
            timer1.Start();
        }


        int intmode=1;

        private void Form1_Load(object sender, EventArgs e)
        {
            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.Title.Text = "MOTOR POSITION";
            myPane.XAxis.Title.Text = "Time, Seconds";
            myPane.YAxis.Title.Text = "Angle, Deg";

            // Save 1200 points.  At 50 ms sample rate, this is one minute
            // The RollingPointPairList is an efficient storage class that always
            // keeps a rolling set of point data without needing to shift any data values
            RollingPointPairList list = new RollingPointPairList(60000);
            RollingPointPairList list1 = new RollingPointPairList(60000);

            // Initially, a curve is added with no data points (list is empty)
            // Color is blue, and there will be no symbols
            LineItem curve = myPane.AddCurve("Set Value", list, Color.Red, SymbolType.None);
            LineItem curve1 = myPane.AddCurve("Current Value", list1, Color.Blue, SymbolType.None);

            

            // Just manually control the X axis range so it scrolls continuously
            // instead of discrete step-sized jumps
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = 30;
            myPane.XAxis.Scale.MinorStep = 1;
            myPane.XAxis.Scale.MajorStep = 5;

            // Scale the axes
            zedGraphControl1.AxisChange();

            // Save the beginning time for reference
            tickStart = Environment.TickCount;
        }

      
        

        // Set the size and location of the ZedGraphControl
        private void SetSize()
        {
            // Control is always 10 pixels inset from the client rectangle of the form
            Rectangle formRect = this.ClientRectangle;
            formRect.Inflate(-100, -100);

            if (zedGraphControl1.Size != formRect.Size)
            {
                zedGraphControl1.Location = formRect.Location;
                zedGraphControl1.Size = formRect.Size;
            }
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            SetSize();
        }


        static Random r = new Random();

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            string s;
            int n = r.Next(12);
            s = n.ToString();


            Draw("10",s);
            
            //// Make sure that the curvelist has at least one curve
            //if (zedGraphControl1.GraphPane.CurveList.Count <= 0)
            //    return;

            //// Get the first CurveItem in the graph
            //LineItem curve = zedGraphControl1.GraphPane.CurveList[0] as LineItem;
            //if (curve == null)
            //    return;

            //// Get the PointPairList
            //IPointListEdit list = curve.Points as IPointListEdit;
            //// If this is null, it means the reference at curve.Points does not
            //// support IPointListEdit, so we won't be able to modify it
            //if (list == null)
            //    return;

            //// Time is measured in seconds
            //double time = (Environment.TickCount - tickStart) / 1000.0;

            //// 3 seconds per cycle
            //list.Add(time, Math.Sin(2.0 * Math.PI * time / 3.0));

            //// Keep the X scale at a rolling 30 second interval, with one
            //// major step between the max X value and the end of the axis
            //Scale xScale = zedGraphControl1.GraphPane.XAxis.Scale;
            //if (time > xScale.Max - xScale.MajorStep)
            //{
            //    xScale.Max = time + xScale.MajorStep;
            //    xScale.Min = xScale.Max - 30.0;
            //}

            //// Make sure the Y axis is rescaled to accommodate actual data
            //zedGraphControl1.AxisChange();
            //// Force a redraw
            //zedGraphControl1.Invalidate();
        }




        private void Draw(string setpoint, string current)
            {
                double intsetpoint;
                double intcurrent;
                double.TryParse(setpoint, out intsetpoint);
                double.TryParse(current, out intcurrent);
                // Make sure that the curvelist has at least one curve
                if (zedGraphControl1.GraphPane.CurveList.Count <= 0)
                    return;
                // Get the first CurveItem in the graph
                LineItem curve = zedGraphControl1.GraphPane.CurveList[0] as LineItem;
                LineItem curve1 = zedGraphControl1.GraphPane.CurveList[1] as LineItem;
                if (curve == null)
                    return;
                if (curve1 == null)
                    return;
                // Get the PointPairList
                IPointListEdit list = curve.Points as IPointListEdit;
                IPointListEdit list1 = curve1.Points as IPointListEdit;
                // If this is null, it means the reference at curve.Points does not
                // support IPointListEdit, so we won't be able to modify it
                if (list == null)
                    return;
                if (list1 == null)
                    return;
                // Time is measured in seconds
                double time = (Environment.TickCount - tickStart) / 1000;

                // 3 seconds per cycle
                list.Add(time, intsetpoint);
                list1.Add(time,intcurrent);
                // Keep the X scale at a rolling 30 second interval, with one
                // major step between the max X value and the end of the axis

                Scale xScale = zedGraphControl1.GraphPane.XAxis.Scale;
                if (time > xScale.Max - xScale.MajorStep)
                {
                    if (intmode == 1)
                    {
                        xScale.Max = time + xScale.MajorStep;
                        xScale.Min = xScale.Max - 30.0;
                    }
                    else
                    {
                        xScale.Max = time + xScale.MajorStep;
                        xScale.Min =  0;
                    }
                }

                // Make sure the Y axis is rescaled to accommodate actual data
                zedGraphControl1.AxisChange();
                // Force a redraw
                zedGraphControl1.Invalidate();
               
               
                    
                
                
               

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Sample at 50ms intervals
            timer1.Interval = 1;
            timer1.Enabled = false;
            timer1.Stop();
        }

        private void Pbmode_Click(object sender, EventArgs e)
        {
            if (Pbmode.Text == "Scroll")
            {
                intmode = 1;
            Pbmode.Text="Compact";     
            
            
            }
            else

            {
            intmode=0;
        Pbmode.Text="Scroll"; 
            
                
            }
        }




    }
}
