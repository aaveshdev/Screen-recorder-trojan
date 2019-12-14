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
using System.Threading;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace Remote_view
{
    public partial class Form2 : Form
    {
        
        public readonly int port;
        private TcpClient client;
        private TcpListener server;
        private NetworkStream mainstream;

        private readonly Thread Listening;
        private readonly Thread GetImages;

        public Form2(int Port)
        {
          
            port = Port;
            client = new TcpClient();
			//calling methods of realtime images from port
            Listening = new Thread(StartListening);
            GetImages = new Thread(ReceiveImages);

            InitializeComponent();
        }
        private void StartListening()
        {
            while (!client.Connected)
            {
				//starting the server connection to get the data.
                server.Start();
                client = server.AcceptTcpClient();
            
            }
            GetImages.Start();
        
        }

        private void StopListening()
        {
            //stoping the server.
            server.Stop();
            client = null;
            if (Listening.IsAlive) Listening.Abort();
            if (GetImages.IsAlive) GetImages.Abort();
        }

        private void ReceiveImages()
        {
            BinaryFormatter binary = new BinaryFormatter();

            while (client.Connected)
            {
                mainstream = client.GetStream();
				//converting binary data into bitmap images.
                pictureBox1.Image = (Image)binary.Deserialize(mainstream);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //listening from specific IP and port.
            server = new TcpListener(IPAddress.Any, port);
            Listening.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
			//listening service stop on form close.
            StopListening();
        }
        

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
