using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision;
using AForge.Vision.Motion;
using System.IO;
using System.Collections;
using System.IO.Ports;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int ld;
        int onceki;

        private FilterInfoCollection ardiuno;
        private VideoCaptureDevice cam;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ardiuno = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo cihaz in ardiuno)
            {
                comboBox1.Items.Add(cihaz.Name);
            }
            cam = new VideoCaptureDevice();
            button4.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cam.IsRunning)
            {
                cam.Stop();
                pictureBox1.Image = null;
                pictureBox1.Invalidate();
                button1.Text = "Başla";
            }
            else
            {
                cam = new VideoCaptureDevice(ardiuno[comboBox1.SelectedIndex].MonikerString);
                cam.NewFrame += kamera_NewFrame;
                cam.Start();
                button1.Text = "Durdur";
            }
        }

        private void kamera_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap res = (Bitmap)eventArgs.Frame.Clone();
            Bitmap ger = (Bitmap)eventArgs.Frame.Clone();
            res.RotateFlip(RotateFlipType.Rotate180FlipY);
            ger.RotateFlip(RotateFlipType.Rotate180FlipY);

            EuclideanColorFiltering filtre = new EuclideanColorFiltering();
            filtre.CenterColor = new RGB(Color.FromArgb(200, 0, 0));
            filtre.Radius = 100;
            filtre.ApplyInPlace(res);

            ortalama(res);

            pictureBox2.Image = ger;
            pictureBox1.Image = res;



        }

        private void ortalama(Bitmap res)
        {
            BlobCounter blop = new BlobCounter();
            blop.MinWidth = 20;
            blop.MinHeight = 20;
            blop.FilterBlobs = true;
            blop.ObjectsOrder = ObjectsOrder.Size;

            Grayscale grı = new Grayscale(0.2125, 0.7154, 0.0721);
            Bitmap grıres = grı.Apply(res);

            blop.ProcessImage(grıres);
            Rectangle[] rects = blop.GetObjectsRectangles();

            foreach (Rectangle recs in rects)
            {
                if (rects.Length > 0)
                {
                    Rectangle nesneRect = rects[0];
                    Graphics b = Graphics.FromImage(res);
                    using (Pen pe = new Pen(Color.FromArgb(240, 3, 25), 2))
                    {
                        b.DrawRectangle(pe, nesneRect);
                    }
                    int nesneX = nesneRect.X + (nesneRect.Width / 2);
                    int nesneY = nesneRect.Y + (nesneRect.Height / 2);
                    b.DrawString(nesneX.ToString() + "X" + nesneY.ToString(), new Font("Times New Roman", 50), Brushes.White, new System.Drawing.Point(0, 0));
                    b.Dispose();

                    if (nesneX > 0 && nesneX <= 426)
                    {
                        ld = 1;
                    }
                    else if (nesneX > 426 && nesneX <= 852)
                    {
                        ld = 2;
                    }
                    else if (nesneX > 852 && nesneX <= 1280)
                    {
                        ld = 3;
                    }
                    if (onceki!= ld)
                    {
                        onceki = ld;
                        if(serialPort1.IsOpen)
                        {
                            serialPort1.Write(ld.ToString());
                        }

                    
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] port = SerialPort.GetPortNames();
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(port);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                serialPort1.PortName = comboBox2.Text;
                serialPort1.BaudRate = 9600;
                serialPort1.Parity = Parity.None;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Open();
                button3.Enabled = false;
                button4.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            button3.Enabled = true;
            button4.Enabled = false;
        }


        public Parity SerialPortParity { get; set; }
    }
}
