using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;

namespace WindowsFormsApplication1
{

    

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            currentinput = new StringWithEvent();
            currentinput.PropertyChanged += currentinput_PropertyChanged;
        }

        void currentinput_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Hash")
            {
                var tempFont = textBox1.Font;
                textBox1.Font = new Font(tempFont, FontStyle.Italic);
                textBox1.Text = currentinput.Hash;
            }
            else if (e.PropertyName == "MyString" || currentinput.MyString != null)
            {
                var tempFont = textBox1.Font;
                textBox1.Font = new Font(tempFont, FontStyle.Regular);
                textBox1.Text = currentinput.MyString;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentinput.MyString += "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentinput.MyString += "2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            currentinput.MyString += "3";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            currentinput.MyString += "4";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            currentinput.MyString += "5";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            currentinput.MyString += "6";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            currentinput.MyString += "7";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            currentinput.MyString += "8";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            currentinput.MyString += "9";
        }

        private void buttonA_Click(object sender, EventArgs e)
        {
            currentinput.MyString += "0";
        }

        private void button_calculate_Click(object sender, EventArgs e)
        {
            if (currentinput.MyString == null)
                return;
                        
            HMACSHA1 hmac_sha1 = new HMACSHA1(Encoding.ASCII.GetBytes("VjXb38Zn7H6wnvBlczkY"));
            var salt = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(currentinput.MyString));
            hmac_sha1.Initialize();

            var hashBytes = hmac_sha1.ComputeHash(salt);
            
            // Use a bitwise operation to get a representative binary code from the hash
            // Refer section 5.4 at http://tools.ietf.org/html/rfc4226#page-7            
            int offset = hashBytes[19] & 0xf;
            int binaryCode = (hashBytes[offset] & 0x7f) << 24
                | (hashBytes[offset + 1] & 0xff) << 16
                | (hashBytes[offset + 2] & 0xff) << 8
                | (hashBytes[offset + 3] & 0xff);
            
            int otp = binaryCode % (int)Math.Pow(10, 8); // where 8 is the password length

            currentinput.Hash = otp.ToString().PadLeft(8, '0');
        }

        private StringWithEvent currentinput;

        private void button10_Click(object sender, EventArgs e)
        {
            currentinput.MyString = null;   
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Hello.");
        }

       
    }

    public class StringWithEvent : INotifyPropertyChanged
    {
        private string myString;

        public string MyString
        {
            get
            { return myString; }
            set
            {
                myString = value;
                OnPropertyChanged("MyString");
            }
        }

        public string Hash {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
                myString = null; //Clear the private string.
                OnPropertyChanged("Hash");
            }
        }
        private string hash;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            { handler(this, new PropertyChangedEventArgs(propertyName)); }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
