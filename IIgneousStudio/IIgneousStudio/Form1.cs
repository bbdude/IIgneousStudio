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
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //Save and Run
            string path = saveToText();
            Process p = Process.Start(name, "iigneous.exe");
            name = name.Substring(0, name.Length - 3);
            //path = path.Substring(0, path.Length);
            path += name  + ".exe";
            name += ".exe";
            Process c = Process.Start(name);
            //Thread.Sleep(600);  // Allow the process to open it's window
            //SetParent(p.MainWindowHandle, panel1.Handle);
        }

        private void updateNumberLabel(object sender, EventArgs e)
        {
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
        }

        private void Form1_FormClosing(System.Object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Save before closing?", "", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                saveToText();
            }
        }

        private string saveToText()
        {
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
            return di.FullName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = name;
        }
    }
}