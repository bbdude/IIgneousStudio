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
        string version = "0.0.5";
        int checkLength = 2;
        string lastText = "";

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
            RichTextBox richTextBox = new RichTextBox();
            if (openFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                richTextBox.LoadFile(openFile1.FileName, RichTextBoxStreamType.PlainText);
                addTabs(richTextBox, System.IO.Path.GetFileName(openFile1.FileName));
            }
            name = System.IO.Path.GetFileName(openFile1.FileName);
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

            var richTextBox = (RichTextBox)tabControl1.SelectedTab.Controls[0];

            int firstIndex = richTextBox.GetCharIndexFromPosition(pos);
            int firstLine = richTextBox.GetLineFromCharIndex(firstIndex);

            pos.X = ClientRectangle.Width;
            pos.Y = ClientRectangle.Height;

            int lastIndex = richTextBox.GetCharIndexFromPosition(pos);
            int lastLine = richTextBox.GetLineFromCharIndex(lastIndex);

            pos = richTextBox.GetPositionFromCharIndex(lastIndex);
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
                            //foreach (string line in richTextBox2.Text.Split(new string[] { "\n" }, StringSplitOptions.None))
                            foreach (string line in tabControl1.SelectedTab.Controls[0].Text.Split(new string[] { "\n" }, StringSplitOptions.None))
                            {
                                tw.WriteLine(line);
                            }
                        }
                    }
                }
                Form1_Load(null, null);
                name = System.IO.Path.GetFileName(saveFileDialog1.FileName);
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

        private void addTabs(RichTextBox richTextBox,string name)
        {
            //Add Tabs
            try
            {
                if (tabControl1.TabPages.Count <= 8)
                {
                    TabPage tabPage = new TabPage();
                    tabPage.Controls.Add(richTextBox);
                    if (name == null)
                    {
                        tabPage.Text = "Page: " + (tabControl1.TabCount + 1).ToString();
                        tabPage.Tag = "Page: " + (tabControl1.TabCount + 1).ToString();
                    }
                    else
                    {
                        tabPage.Text = name;
                        tabPage.Tag = name;
                    }
                    richTextBox.Dock = DockStyle.Fill;

                    tabControl1.TabPages.Add(tabPage);
                }
                else
                    MessageBox.Show("You have reached the max threshold for tabs.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(e.Message);
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            //Add Tabs
            RichTextBox richTextBox = new RichTextBox();
            addTabs(richTextBox,null);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            //Remove Tabs
            try
            {
                if (tabControl1.TabPages.Count > 1)
                {
                    tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ee.Message);
            }
        }

        private void tabControl1_KeyPress(object sender, KeyPressEventArgs e)
        {
            updateNumberLabel(sender, e);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(toolStripTextBox1.Text) > 0 && Convert.ToInt32(toolStripTextBox1.Text) < 5)
                {
                    try
                    {
                        checkLength = Convert.ToInt32(toolStripTextBox1.Text);
                        MessageBox.Show("Check Length Box was changed", "Check Box", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine(ee.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Check Length Box was not used correctly", "Check Box", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    toolStripTextBox1.Text = checkLength.ToString();
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ee.Message);
            }
        }

        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
            {
                try
                {
                    lastText = lastText.Substring(0, lastText.Length - 1);
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }
            else  if (e.KeyChar == '\r' || e.KeyChar == ' ')
            {
                lastText = "";
            }
            else
                lastText += e.KeyChar;

            if (lastText.Length >= checkLength)
            {
                IWin32Window win = this;
                string toSay = "";
                if (lastText == "check".Substring(0, 5 - checkLength))
                {
                    toSay += "check?\n";
                }
                else if (lastText == "pause".Substring(0, 5 - checkLength))
                {
                    toSay += "pause?\n";
                }
                else if (lastText == "var  ".Substring(0, 5 - checkLength))
                {
                    toSay += "var?\n";
                }
                else if (lastText == "string".Substring(0, 6 - checkLength))
                {
                    toSay += "string?\n";
                }
                else if (lastText == "int  ".Substring(0, 5 - checkLength))
                {
                    toSay += "int?\n";
                }
                else if (lastText == "print".Substring(0, 5 - checkLength))
                {
                    toSay += "print?\n";
                }
                else if (lastText == "foreach  ".Substring(0, 7 - checkLength))
                {
                    toSay += "foreach?\n";
                }
                else if (lastText == "for  ".Substring(0, 5 - checkLength))
                {
                    toSay += "for?\n";
                }
                else if (lastText == "end  ".Substring(0, 5 - checkLength))
                {
                    toSay += "end?\n";
                }
                else if (lastText == "CLR  ".Substring(0, 5 - checkLength))
                {
                    toSay += "CLR?\n";
                }
                else if (lastText == "read_int".Substring(0, 8 - checkLength))
                {
                    toSay += "read_int?\n";
                }
                if (toSay != "")
                    toolTip1.Show(toSay, win, MousePosition);
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            string folderName = null;
            string note = Microsoft.VisualBasic.Interaction.InputBox("Input note text:", "Note for project", "");
            DialogResult dr = MessageBox.Show("Make a project folder with that?", "Project Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                //Make a folder then place it inside
                toolStripStatusLabel1.Text = "Saving Project Folder";
                folderName = Microsoft.VisualBasic.Interaction.InputBox("Folder name:", "Name for project", "");
            }
            try
            {
                toolStripStatusLabel1.Text = "Saving Note";

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = false;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate))
                    {
                        if (folderName != null)
                        {
                            string path = saveFileDialog1.FileName;
                            path.Substring(0, path.Length - Path.GetFileName(path).Length);

                            System.IO.Directory.CreateDirectory(folderName);
                        }
                        using (TextWriter tw = new StreamWriter(fs))
                        {
                            //foreach (string line in richTextBox2.Text.Split(new string[] { "\n" }, StringSplitOptions.None))
                            foreach (string line in note.Split(new string[] { "\n" }, StringSplitOptions.None))
                            {
                                tw.WriteLine(line);
                            }
                        }
                    }
                }
                Form1_Load(null, null);
                name = System.IO.Path.GetFileName(saveFileDialog1.FileName);
                DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(saveFileDialog1.FileName));
                this.Text = name;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ee.Message);
            }
            toolStripStatusLabel1.Text = "Ready";
        }
    }
}