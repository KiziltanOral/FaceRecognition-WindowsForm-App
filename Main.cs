using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Windows.Forms;

namespace FaceRecognition
{
    public partial class Main : Form
    {
        private VideoCapture capture;
        private CascadeClassifier faceCascade;
        private Mat frame;
        private System.Windows.Forms.Timer timer;
        public Main()
        {
            InitializeComponent();

            capture = new VideoCapture(0);
            frame = new Mat();
            faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000 / 30;
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (capture.Read(frame))
            {
                var faces = faceCascade.DetectMultiScale(frame, scaleFactor: 1.1, minNeighbors: 4, minSize: new OpenCvSharp.Size(30, 30));

                foreach (var face in faces)
                {
                    Cv2.Rectangle(frame, face, Scalar.Red, 2);
                }

                var image = BitmapConverter.ToBitmap(frame);
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = image;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            capture.Release();
            frame.Dispose();
            faceCascade.Dispose();
            timer.Dispose();
        }
    }
}
