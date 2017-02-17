using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Linq;
using System.Threading;
using System.Net.Mail;

namespace AcesOfflineSystem
{
    public partial class Form1 : Form
    {
        public void startForm()
        {
            Application.Run(new Form2());
        }
        Dictionary<string, string> map = new Dictionary<string, string>();
        List<string> templist = new List<string>();
        string mobile;
        public Form1()
        {
            Thread t = new Thread(new ThreadStart(startForm));
            t.Start();
            Thread.Sleep(3000);
            InitializeComponent();
            t.Abort();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (warning.Visible == true) {
                warning.Visible = false;
            }
            else if (textBox1.TextLength == 11)
                mobile = textBox1.Text;
        }

        //private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        //(e.KeyChar != '.'))
        //    {
        //        e.Handled = true;
        //    }


        //}

        private void button1_Click(object sender, EventArgs e)
        {
            if (mobileToolStripMenuItem1.Checked == true)
            {
                if (textBox1.TextLength < 11)
                {
                    warning.Visible = true;
                }
                else if (textBox1.TextLength == 11)
                {
                    warning.Visible = false;
                    mobile = textBox1.Text;
                    string password = generateRandom(7);
                    //string pass = password.ToString();
                    addToDict(mobile, password);
                    //empty the textbox after submitting
                    textBox1.Text = "";

                }
            }
            else if (emailToolStripMenuItem1.Checked == true) {
                //check email regular expression 
                try
                {
                    emailWarning.Visible = false;
                    var eMailValidator = new System.Net.Mail.MailAddress(textBox2.Text);
                    mobile = textBox2.Text;
                    string password = generateRandom(7);
                    //string pass = password.ToString();
                    addToDict(mobile, password);
                    //empty the textbox after submitting
                    textBox2.Text = "";
                }
                catch (FormatException ex)
                {
                    emailWarning.Visible = true;
                }
            }
        }

        ///my methods....
        private string generateRandom(int size) {
            Random random = new Random();
            string input = "0123456789";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = input[random.Next(0, input.Length)];
                builder.Append(ch);
            }

            return builder.ToString();

        }
        private void addToDict(string mobile, string password) {
            bool ok = false;
            if (map.Count == 0)
            {
                ok = true;
                try
                {
                    map.Add(mobile, password);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error");

                }
                //MessageBox.Show(password, "Your password");
                label5.Visible = true;
                label8.Visible = true;
                label6.Text = mobile;
                label8.Location = new Point(label6.Right, label8.Top);
                label9.Text = password;
                label9.Location = new Point(label8.Right, label9.Top);
               
                CSV();
            }
            else
            {
                //mobile is the key
                foreach (KeyValuePair<string, string> entry in map)
                    {
                    try
                        {

                            if (map.ContainsKey(mobile))
                            {
                                //get password of the already entered mobile number
                                var myValue = map.FirstOrDefault(x => x.Key == mobile).Value;
                                MessageBox.Show("This mobile number has already registerd\n"+"Password is "+myValue, "Warning");
                                return;
                            }
                          }
                    catch(Exception ex){
                        MessageBox.Show(ex.ToString(), "Error");
                    }
                        
                        ok = true;
                        //map.Add(mobile, password);

                        templist.Add(mobile);
                        templist.Add(password);
                        label5.Visible = true;
                        label8.Visible = true;
                        label6.Text = mobile;
                        label8.Location = new Point(label6.Right, label8.Top);
                        label9.Text = password;
                        label9.Location = new Point(label8.Right, label9.Top);
                 
                        //MessageBox.Show(password, "Your password");
                        break;
                        
                    }
                if (ok == true)
                {
                    try
                    {
                        map.Add(templist[0], templist[1]);
                        templist.Clear();
                        ok = false;
                    }
                    catch(Exception ex) {
                        MessageBox.Show(ex.ToString(), "Error");

                    }
                    CSV();
                }

            }
            
            //label2.Text = map.Keys;
            //foreach (KeyValuePair<string, string> entry in map)
            //{
            //    // do something with entry.Value or entry.Key
                
            //    label2.Text = entry.Key;
            //    label3.Text = entry.Value;
            //}
        }

        private void CSV() {
            StringBuilder csvcontent = new StringBuilder();
            //foreach (KeyValuePair<string, string> entry in map)
            //{
            //    // do something with entry.Value or entry.Key  
            //    csvcontent.AppendLine(entry.Key + "," + entry.Value);

            //}
            csvcontent.AppendLine(map.Keys.Last() + "," + map.Values.Last());
            string path = @"passwords.csv";
            try
            {
                File.AppendAllText(path, csvcontent.ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.ToString(), "Error");
            }
        }
        private void readCSV() {
            using (TextFieldParser parser = new TextFieldParser(@"passwords.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();

                    //takes fields as an argument to convert it to list to be able to iterate through it
                    List<string> mylist = new List<string> (fields);
                    for (int i = 0; i < mylist.Count; i=i+2 )
                    {
                        //TODO: Process field
                        //map.Clear();
                        try
                        {
                            map.Add(mylist[0], mylist[1]);
                        }
                        catch (Exception ex) {
                            MessageBox.Show(ex.ToString(), "Error");
                        }
                    }
                }
            }

            //for debugging issues..........
            //foreach (KeyValuePair<string, string> entry in map)
            //{
            //    // do something with entry.Value or entry.Key

            //    label2.Text = entry.Key;
            //    label3.Text = entry.Value;
            //}
        }

        private void updateCSVFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            readCSV();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            readCSV();
        }

        //private void textBox1_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        if (textBox1.TextLength < 11) { }
        //        else if (textBox1.TextLength == 11)
        //        {
        //            button1_Click(this, new EventArgs());
        //        }

               
        //    }
        //}

        
        private void button2_Click(object sender, EventArgs e)
        {
            readCSV();
        }

        

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(Char.IsNumber(e.KeyChar) || e.KeyChar == 8);

        }

        

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(this, new EventArgs());
            }
        }

      

      
        //Choose entry options whether email or mobile:
        private void mobileToolStripMenuItem1_CheckStateChanged(object sender, EventArgs e)
        {
            if (mobileToolStripMenuItem1.Checked == true) {
                label1.Text = "Enter Mobile Number";
                textBox2.Visible = false;
                textBox1.Visible = true;
            }
            else if (mobileToolStripMenuItem1.Checked == false) {
                label1.Text = "Enter Email";
                textBox2.Visible = true;
                textBox1.Visible = false;
            }
        }

        private void mobileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            mobileToolStripMenuItem1.Checked = true;
            emailToolStripMenuItem1.Checked = false;

        }

        private void emailToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            mobileToolStripMenuItem1.Checked = false;
            emailToolStripMenuItem1.Checked = true;

        }

       

        //private void toolStrip1_MouseHover(object sender, EventArgs e)
        //{
        //    if (toolStrip1.Visible == true) {
        //        toolStrip1.Visible = false;
        //    }
        //    else if (toolStrip1.Visible == false)
        //    {
        //        toolStrip1.Visible = true;
        //    }
        //}
    }
}
