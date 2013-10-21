using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IIgneousStudio
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        public void loadAsChannel(string version)
        {
            var contents = new System.Net.WebClient().DownloadString("http://iigniteus.com/Logs/Studio/Log.txt");
            richTextBox1.Text = contents.ToString();
            contents = new System.Net.WebClient().DownloadString("http://iigniteus.com/Logs/Studio/Version.txt");
            if (version != contents.ToString())
                MessageBox.Show("Current Version: " + contents.ToString() + ".\nYour version: " + version + ".\n\nPlease update to ensure the best experience.", "Need to update", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void loadAsOutput()
        {

        }
    }
}
