using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace saugumas2
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string orginalas = textBox1.Text;
            // byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(textBox2.Text);
            HashAlgorithm hash = MD5.Create();
            byte[] passwordBytes = hash.ComputeHash(Encoding.Unicode.GetBytes(textBox2.Text));
            using (Aes myAes = Aes.Create())
            {

                // Encrypt the string to an array of bytes.
                // byte[] encrypted = EncryptStringToBytes_Aes(orginalas, passwordBytes, pakeistasIV);

                var random = new Random();
                byte[] IV = new byte[16];
                random.NextBytes(IV);
               Encoding encoding = Encoding.GetEncoding("437");
                textBox6.Text = encoding.GetString(IV);

                byte[] encrypted;

                // Create an Aes object
                // with the specified key and IV.
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = passwordBytes;
                    aesAlg.IV = IV;
                   // aesAlg.BlockSize = BlockSize;

                    // Create an encryptor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                //Write all data to the stream.
                                swEncrypt.Write(orginalas);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }







                // Decrypt the bytes to a string.
                //string roundtrip = DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);
                // Encoding encoding = Encoding.GetEncoding("437");
                //Display the original data and the decrypted data.
                // textBox3.Text = encoding.GetString(encrypted);
                textBox3.Text = Convert.ToBase64String(encrypted);
            }




        }
 
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] pakeistasIV)
        {
            // Check arguments.
             
             var random = new Random();
            byte[] IV = new byte[16];
            random.NextBytes(IV);

            
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                //   aesAlg.Key = Key;
                // aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {


            if (textBox3.Text != "")
            {
                /* Encoding encoding = Encoding.GetEncoding("437");
                 string pakeistasIV = encoding.GetString(IV);*/
              // string IV = getIV();

                File.WriteAllText("C:\\Users\\Otter\\source\\repos\\saugumas2\\saugumas2\\Encrypted.txt", textBox3.Text + Environment.NewLine + textBox6.Text);
                MessageBox.Show("Saved");

            }
            else MessageBox.Show("reikia uzsifruoti pirma");

            /* Stream myStream;
             SaveFileDialog saveFileDialog1 = new SaveFileDialog();

             saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
             saveFileDialog1.FilterIndex = 2;
             saveFileDialog1.RestoreDirectory = true;

             if (saveFileDialog1.ShowDialog() == DialogResult.OK)
             {
                 if ((myStream = saveFileDialog1.OpenFile()) != null)
                 {
                     // Code to write the stream goes here.
                     myStream.Write(saveFileDialog1.FileName, textBox3.Text);
                     myStream.Close();
                 }

             }
         }*/



        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] orginalas = Convert.FromBase64String(textBox4.Text);
            HashAlgorithm hash = MD5.Create();
            byte[] passwordBytes = hash.ComputeHash(Encoding.Unicode.GetBytes(textBox2.Text));
            Encoding encoding = Encoding.GetEncoding("437");
            byte[] IV = encoding.GetBytes(textBox7.Text);
            // Declare the string used to hold
            // the decrypted text.

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = passwordBytes;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(orginalas))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            textBox3.Text = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "File selection";
            fdlg.InitialDirectory = @"C:\Users\Otter\source\repos\saugumas2\saugumas2";
            fdlg.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK) {
                string[] lines = System.IO.File.ReadAllLines(fdlg.FileName);
                textBox4.Text = lines[0];
                textBox7.Text = lines[1];
            }
        }
    }

            
 }
