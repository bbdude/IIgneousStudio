using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace IIgneousStudio
{
    public partial class Form1 : Form
    {
        string name = "Project";
        string version = "0.0.2";

        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //Save
            saveToText();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //load
            toolStripStatusLabel1.Text = "Loading";
            OpenFileDialog openFile1 = new OpenFileDialog();

            openFile1.Filter = "IIgneous files (*.ii)|*.ii|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFile1.FilterIndex = 1;
            if (openFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                richTextBox2.LoadFile(openFile1.FileName, RichTextBoxStreamType.PlainText);
            name = openFile1.FileName;
            this.Text = name;
            toolStripStatusLabel1.Text = "Ready";
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //Save and Run
            toolStripStatusLabel1.Text = "Saving and Running";
            try
            {
                //string path = 
                saveToText();
                //Process p = Process.Start(name, "iigneous.exe");



                Process process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.FileName = name;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.Arguments = "iigneous.exe";
                process.Start();
                name = name.Substring(0, name.Length - 3);
                name += ".exe";

                try
                {
                    Process c = Process.Start(name);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    Console.WriteLine(ee.Message);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ee.Message);
            }
            toolStripStatusLabel1.Text = "Ready";
        }

        private void updateNumberLabel(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Updating Number Label";
            Point pos = new Point(0, 0);
            int firstIndex = richTextBox2.GetCharIndexFromPosition(pos);
            int firstLine = richTextBox2.GetLineFromCharIndex(firstIndex);

            pos.X = ClientRectangle.Width;
            pos.Y = ClientRectangle.Height;

            int lastIndex = richTextBox2.GetCharIndexFromPosition(pos);
            int lastLine = richTextBox2.GetLineFromCharIndex(lastIndex);

            pos = richTextBox2.GetPositionFromCharIndex(lastIndex);
            richTextBox1.Text = "";
            for (int i = firstLine; i <= lastLine + 1; i++)
            {
                richTextBox1.Text += i + "\n";
            }
            toolStripStatusLabel1.Text = "Ready";
        }

        private void Form1_FormClosing(System.Object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            toolStripStatusLabel1.Text = "CLOSING";
            DialogResult dialogResult = MessageBox.Show("Save before closing?", "", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                saveToText();
            }
            toolStripStatusLabel1.Text = "Ready";
        }

        private string saveToText()
        {
            
            try
            {
                toolStripStatusLabel1.Text = "Saving";
                
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "IIgneous files (*.ii)|*.ii|txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate))
                    {
                        using (TextWriter tw = new StreamWriter(fs))
                        {
                            foreach (string line in richTextBox2.Text.Split(new string[] { "\n" }, StringSplitOptions.None))
                            {
                                tw.WriteLine(line);
                            }
                        }
                    }
                }
                Form1_Load(null, null);
                name = saveFileDialog1.FileName;
                DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(saveFileDialog1.FileName));
                this.Text = name;
                return di.FullName;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(e.Message);
            }
            toolStripStatusLabel1.Text = "Ready";
            return name;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = name;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Form2 outputWindow = new Form2();
            outputWindow.Name = "Output";
            outputWindow.Show();
	 
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Form2 channelLogWindow = new Form2();
            channelLogWindow.Name = "Channel Log";
            channelLogWindow.Show();
            channelLogWindow.loadAsChannel(version);
	 
        }
    }
}