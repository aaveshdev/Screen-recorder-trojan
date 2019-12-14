using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Drawing.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.IO;

namespace service_remote
{
    public partial class Form1 : Form
    {
        private readonly TcpClient client = new TcpClient();
        private NetworkStream mainstream;
        private int port;
        private string ips;
        private static Image GrabDesktop()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            try
            {
                //Realtime Screenshot capture
                Graphics graphic = Graphics.FromImage(screenshot);
                graphic.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
            }
            catch (Exception op)
            {
				//In case application crash then automatically restarts.
                Application.Restart();
            
            }
            return screenshot;

        
        }


        private void sendDesktopImage()
        {
            BinaryFormatter binary = new BinaryFormatter();
            while (client.Connected)
            {
                mainstream = client.GetStream();
                try
                {
					//sending the Captured data.
                    binary.Serialize(mainstream, GrabDesktop());
                }
                catch (Exception eo)
                {

                    Application.Restart();
                }
            }
           

        
        
        }

        public Form1()
        {
            InitializeComponent();
          
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conc();
            button2.Text = "Stop Sharing";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            conc();
        }
        private void jonc()
        {
            timer1.Stop();
            conc();
        
        }

        private void conc()
        {
          //  port = int.Parse(textBox2.Text);
            try
            {
                string ap = Assembly.GetExecutingAssembly().Location;
                string ico = "";
				//Reading the IP and Port from configuration file.
                ico = System.Text.RegularExpressions.Regex.Replace(ap, @"service remote.exe$", "conf.txt");
                string source1 = File.ReadAllText(@ico);
                string[] arr = source1.Split(' ');
                ips = arr[0];
                port = int.Parse(arr[1]);
                client.Connect(ips, port);
               // MessageBox.Show("Connected");
                timer1.Start();
            }
            catch (Exception ec)
            {
               // MessageBox.Show("Something went wrong");

                jonc();
            }
        
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
                timer1.Start();
                button2.Text = "Stop Sharing";
            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
			//sending image in time event.
            sendDesktopImage();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            button2.Text = "Share my screen";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }



    }
}
