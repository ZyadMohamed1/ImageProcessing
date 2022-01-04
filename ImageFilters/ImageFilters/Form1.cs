using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZGraphTools;

namespace ImageFilters
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte[,] ImageMatrix;

        private void btnOpen_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
        }

        private static int  getSortType(string type)
        {
            switch (type)
            {
                case "Quick Sort":
                    return 1;
                case "Counting Sort":
                    return 2;
                case "Select K":
                    return 3;
                default:
                    return 1;
            }
        }

        private static int getMaxWindowSize(string size)
        {
            return Convert.ToInt32(size);
        }

        private static int getAlpha(string size)
        {
            return Convert.ToInt32(size);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label5.Text = "";
            string type = comboBox1.SelectedItem.ToString();
            int sortType = getSortType(type);
            string size = comboBox2.SelectedItem.ToString();
            int maxsize = getMaxWindowSize(size);

            int Start = System.Environment.TickCount;
            byte[,] img = AdaptiveMedianFilter.adaptiveFilter(ImageMatrix, 3, maxsize, sortType);
            int End = System.Environment.TickCount;
            ImageOperations.DisplayImage(img, pictureBox2);

            double Time = End - Start;
            Time /= 1000;
            label5.Text = (Time).ToString() + " S";
        }

        private void button2_Click(object sender, EventArgs e)
        {

            label5.Text = "";

            string type = comboBox1.SelectedItem.ToString();
            int sortType = getSortType(type);
            string size = comboBox2.SelectedItem.ToString();
            int maxsize = getMaxWindowSize(size);
            string trim = comboBox3.SelectedItem.ToString();
            int alpha = getAlpha(trim);


            int Start = System.Environment.TickCount;
            byte[,] img = AlphaTrim.AlphaFilter(ImageMatrix, maxsize, alpha, sortType);
            int End = System.Environment.TickCount;
            ImageOperations.DisplayImage(img, pictureBox2);
            double Time = End - Start;
            Time /= 1000;
            label5.Text = (Time).ToString();
            label5.Text += " s";
        }

        private void button3_Click(object sender, EventArgs e)
        {

            string size = comboBox4.SelectedItem.ToString();
            int maxsize = getMaxWindowSize(size);
            //Graph arrays
            int Msize = (maxsize - 3) / 2 + 1;
            double[] x_axis = new double[Msize];
            double[] y_axis_quick = new double[Msize];
            double[] y_axis_count = new double[Msize];

            int xCounter = 0;
            for (int WS = 3; WS <= maxsize; WS += 2)
            {
                x_axis[xCounter] = WS;
                double StartQ = System.Environment.TickCount;
                AdaptiveMedianFilter.adaptiveFilter(ImageMatrix, 3, WS, 1);
                double EndQ = System.Environment.TickCount;
                double Time_adaptiveQ = EndQ - StartQ;
                Time_adaptiveQ /= 1000;
                y_axis_quick[xCounter] = Time_adaptiveQ;
                double Start = System.Environment.TickCount;
                AdaptiveMedianFilter.adaptiveFilter(ImageMatrix, 3, WS, 2);
                double End = System.Environment.TickCount;
                Double Time_adaptive = End - Start;
                Time_adaptive /= 1000;
                y_axis_count[xCounter] = Time_adaptive;
                xCounter++;
            }
            //Create a graph and add two curves to it
            ZGraphForm AlGF = new ZGraphForm("Adaptive Filter", "Window Size", "Time");
            AlGF.add_curve("Quick Sort", x_axis, y_axis_quick, Color.Red);
            AlGF.add_curve("Counting Sort", x_axis, y_axis_count, Color.Blue);
            AlGF.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string size = comboBox4.SelectedItem.ToString();
            int maxsize = getMaxWindowSize(size);
            string trim = comboBox3.SelectedItem.ToString();
            int alpha = getAlpha(trim);
            //Graph arrays
            int Msize = (maxsize - 3) / 2 + 1;
            double[] x_axis = new double[Msize];
            double[] y_axis_count = new double[Msize];
            double[] y_axis_selectK = new double[Msize];

            int xCounter = 0;
            for (int WS = 3; WS <= maxsize; WS += 2)
            {
                x_axis[xCounter] = WS;
                double Start = System.Environment.TickCount;
                AlphaTrim.AlphaFilter(ImageMatrix, WS, alpha, 2);
                double End = System.Environment.TickCount;
                double Time_Alpha = End - Start;
                Time_Alpha /= 1000;
                y_axis_count[xCounter] = Time_Alpha;
                Start = System.Environment.TickCount;
                AlphaTrim.AlphaFilter(ImageMatrix, WS, alpha, 3);
                End = System.Environment.TickCount;
                Time_Alpha = End - Start;
                Time_Alpha /= 1000;
                y_axis_selectK[xCounter] = Time_Alpha;
                xCounter++;
            }
            //Create a graph and add two curves to it
            ZGraphForm AlGF = new ZGraphForm("Alpha Filter", "Window Size", "Time");
            AlGF.add_curve("Counting Sort", x_axis, y_axis_count, Color.Red);
            AlGF.add_curve("select K Sort", x_axis, y_axis_selectK, Color.Blue);
            AlGF.Show();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            label5.Text = "";
            string type = comboBox1.SelectedItem.ToString();
            int sortType = getSortType(type);
            string size = comboBox2.SelectedItem.ToString();
            int maxsize = getMaxWindowSize(size);

            int Start = System.Environment.TickCount;
            byte[,]img = FastMedian.FastFilter(ImageMatrix, maxsize);
            int End = System.Environment.TickCount;
            ImageOperations.DisplayImage(img, pictureBox2);


            double Time = End - Start;
            Time /= 1000;
            label5.Text = (Time).ToString();
            label5.Text += " s";
        }
    }
}