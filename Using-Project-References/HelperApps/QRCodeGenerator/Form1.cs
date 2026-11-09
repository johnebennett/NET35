using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OtpNet;
using Net.Codecrete.QrCodeGenerator;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private int counter = 30;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void btnSha1_Click(object sender, EventArgs e)
        {
            try
            {
                var CurrentTime = DateTime.UtcNow;
                var correction = new TimeCorrection(CurrentTime);
                //byte[] securitykey = Encoding.ASCII.GetBytes(txtSecret.Text);
                byte[] securitykey = Base32Encoding.ToBytes(txtSecret.Text + txtUniqueId.Text);

                var totp = new Totp(securitykey, mode: OtpHashMode.Sha1, step: 30, totpSize: 6, timeCorrection: correction);
                //var totpCode = totp.ComputeTotp(DateTime.UtcNow);
                var totpCode = totp.ComputeTotp(CurrentTime);
                counter = totp.RemainingSeconds();
                timer1.Start();
                txtSha1.Text = totpCode;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void btnSha256_Click(object sender, EventArgs e)
        {
            //byte[] securitykey = Encoding.ASCII.GetBytes(txtSecret.Text);
            var Secret = txtSecret.Text;
            byte[] securitykey = Base32Encoding.ToBytes(AsciiToHexStr(txtSecret.Text));

            var totp = new Totp(securitykey, mode: OtpHashMode.Sha256, step: 30, totpSize: 6);
            var totpCode = totp.ComputeTotp(DateTime.UtcNow);
            txtSha256.Text = totpCode;
        }

        private void btnSha512_Click(object sender, EventArgs e)
        {
            //byte[] securitykey = Encoding.ASCII.GetBytes(txtSecret.Text);
            byte[] securitykey = Base32Encoding.ToBytes(AsciiToHexStr(txtSecret.Text));

            var totp = new Totp(securitykey, mode: OtpHashMode.Sha512, step: 30, totpSize: 6);
            var totpCode = totp.ComputeTotp(DateTime.UtcNow);
            txtSha512.Text = totpCode;
        }

        private void btnGenerateAll_Click(object sender, EventArgs e)
        {
            btnSha1_Click(sender, e);
            //btnSha256_Click(sender, e);
            //btnSha512_Click(sender, e);
        }

        private string AsciiToHexStr(string AsciiStr)
        {
            char[] charValues = AsciiStr.ToCharArray();
            string Result = "";
            foreach (char _eachChar in charValues)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(_eachChar);
                // Convert the decimal value to a hexadecimal value in string form.
                Result += String.Format("{0:X}", value);
                // to make output as your eg 
                //  hexOutput +=" "+ String.Format("{0:X}", value);

            }
            return Result;
        }

        private string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach(byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        private byte[] HexStringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            txtUniqueId.Text = Base32Encoding.GenerateRandomCode(12);
        }

        private void btnMakeEntry_Click(object sender, EventArgs e)
        {
            var Entry = BuildOTPEntry();
            var qr = QrCode.EncodeText(Entry, QrCode.Ecc.High);
            using (var bitmap = qr.ToBitmap(4, 10))
            {
                bitmap.Save("qr-code.png", System.Drawing.Imaging.ImageFormat.Png);
                pictureBox1.Image = Image.FromFile("qr-code.png");
            }
            MessageBox.Show(Entry);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter--;
            lblRemaining.Text = counter.ToString();
            if (counter == 0)
            {
                counter = 30;
                timer1.Stop();
                btnSha1_Click(sender, e);
            }
        }

        private string BuildOTPEntry()
        {
            return "otpauth://totp/GOAT" + "?secret=" + txtSecret.Text + txtUniqueId.Text;
        }
    }
}
