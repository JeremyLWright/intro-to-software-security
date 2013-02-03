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
using OTPHelpers;

namespace OTP_Locker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button_encrypt.Enabled = false;
            button_decrypt.Enabled = false;
           
            textBox_password.TextChanged += textBox_otp_TextChanged;
            textBox_otp.TextChanged += textBox_otp_TextChanged;
            update_nonce();
        }

        private void update_nonce()
        {
            var r = new Random();
            #if (DEBUG)
            if(textBox_nonce.Text == "1234")
                textBox_nonce.Text = "08522";
            else
                textBox_nonce.Text = "1234";
            #else
            textBox_nonce.Text = r.Next(1000, 9999).ToString();
            #endif
        }


        void textBox_otp_TextChanged(object sender, EventArgs e)
        {
            if (OTPHelpers.OTPChecker.VerifyUser(OTPChecker.UserVerificationKey, textBox_nonce.Text, textBox_otp.Text))
            {
                button_encrypt.Enabled = true;
                button_decrypt.Enabled = true;
            }
            else
            {
                button_encrypt.Enabled = false;
                button_decrypt.Enabled = false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                textBox_filename.Text = openFileDialog1.SafeFileName;
            }
            
        }
    }
}
