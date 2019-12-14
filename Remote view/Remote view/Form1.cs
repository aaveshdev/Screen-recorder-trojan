using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remote_view
{
    public partial class Form1 : Form
    {
    /* 
     Written by 
     Aavesh Jilani
     */
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
			//getting port from textBox2
            int ip = int.Parse(textBox2.Text);
            new Form2(ip).Show();
        }
    }
}
