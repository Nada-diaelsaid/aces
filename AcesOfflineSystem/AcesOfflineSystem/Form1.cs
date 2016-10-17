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

namespace AcesOfflineSystem
{
    public partial class Form1 : Form
    {
        Dictionary<string, string> map = new Dictionary<string, string>();
        List<string> templist = new List<string>();
        string mobile;
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (warning.Visible == true) {
                warning.Visible = false;
            }
            mobile = textBox1.Text;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 11)
            {
                warning.Visible = true;
            }
            else {
                string password = generateRandom(7);
                //string pass = password.ToString();
                addToDict(mobile, password);
                
                
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
                MessageBox.Show(password, "Your password");
                label6.Text = mobile;

                CSV();
            }
            else
            {
                //mobile is the key
                foreach (KeyValuePair<string, string> entry in map)
                    {
                        if (map.ContainsKey(mobile))
                        {
                            //get password of the already entered mobile number
                            var myValue = map.FirstOrDefault(x => x.Key == mobile).Value;
                            MessageBox.Show("This mobile number has already registerd\n"+"Password is "+myValue, "Warning");
                            return;
                        }
                        
                        ok = true;
                        //map.Add(mobile, password);

                        templist.Add(mobile);
                        templist.Add(password);
                        label6.Text = mobile;
     
                        MessageBox.Show(password, "Your password");
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
            //readCSV();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(this, new EventArgs());
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            readCSV();
        }
    }
}
