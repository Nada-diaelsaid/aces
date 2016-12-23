using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace AcesOfflineSystem
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            //Thread t = new Thread(new ThreadStart(startForm));
            //t.Start();
            //Thread.Sleep(1000);
            InitializeComponent();
            //t.Abort();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.progressBar1.Increment(5);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.timer1.Start();

        }
        //public void startForm() { 
        //    Application.Run(new Form1());
        //}
    }
}
