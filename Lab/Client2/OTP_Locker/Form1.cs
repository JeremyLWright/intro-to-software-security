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
            textBox_nonce.TextChanged += textBox_otp_TextChanged;
            update_nonce();
        }

        private void update_nonce()
        {
            
            #if (DEBUG)
            if(textBox_nonce.Text == "1234")
                textBox_nonce.Text = "08522";
            else
                textBox_nonce.Text = "1234";
            #else
                var r = new Random();
                textBox_nonce.Text = r.Next(1000, 9999).ToString();
            #endif
                textBox_otp.Text = "";
        }


        void textBox_otp_TextChanged(object sender, EventArgs e)
        {
            if (openFileDialog1.CheckFileExists)
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
                if (openFileDialog1.CheckFileExists)
                    update_nonce();
            }
            
        }

        private void button_encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                var file = openFileDialog1.OpenFile();
                byte[] file_data = new byte[file.Length];
                file.Read(file_data, 0, (int)file.Length);
                using(var encryptedResult = new FileStream(openFileDialog1.FileName+".secure", FileMode.CreateNew))
                using (var algorithm = new RijndaelManaged())
                using (var key = new Rfc2898DeriveBytes(textBox_password.Text, Encoding.ASCII.GetBytes(OTPChecker.UserVerificationKey)))
                {

                    algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
                    algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);


                    using (var encryptedStream = new CryptoStream(encryptedResult, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        encryptedStream.Write(file_data, 0, file_data.Length);
                    }
                }

                update_nonce();
                MessageBox.Show("Encryption Complete.");
            } 
            catch(Exception exp)
            {
#if (DEBUG)
                MessageBox.Show("Please choose a valid file.\nError: "+exp.Message);
#else
                MessageBox.Show("Please choose a valid file.");
#endif
                update_nonce();
            }
        }

        private void button_decrypt_Click(object sender, EventArgs e)
        {
            try
            {
                var file = openFileDialog1.OpenFile();
                byte[] file_data = new byte[file.Length];
                file.Read(file_data, 0, (int)file.Length);

                using(var decryptedResult = new FileStream(openFileDialog1.FileName.Replace(".secure", ""), FileMode.CreateNew))
                using (var algorithm = new RijndaelManaged())
                using (var key = new Rfc2898DeriveBytes(textBox_password.Text, Encoding.ASCII.GetBytes(OTPChecker.UserVerificationKey)))
                {

                    algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
                    algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);

                    using (var decryptedStream = new CryptoStream(decryptedResult, algorithm.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        decryptedStream.Write(file_data, 0, file_data.Length);
                    }
                }
                update_nonce();
                MessageBox.Show("Decryption Complete.");
            }
            catch (Exception exp)
            {
#if (DEBUG)
                MessageBox.Show("Please choose a valid file.\nError: "+exp.Message);
#else
                MessageBox.Show("Please choose a valid file.");
#endif

                update_nonce();
            }
        }
    }
}
