using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PasswordGeneratorWinForm.Infrastructure;

namespace PasswordGeneratorWinForm
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }       

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string inputPassword = this.txtExistingPassw.Text;

            string outputPassword = this.txtNewPassword.Text;

            //string length = this.cmbLength.Text;
            string length = "20";

            int minLength = 8;

            int maxLength = 128;

            int[] settings = { };
         

            int passwLength = 0;
            // TODO: Set a User advise if InputToUse == true and InputPassword == null
            if (!String.IsNullOrWhiteSpace(inputPassword))
            {
                passwLength = inputPassword.Length;
            }
            else
            {
                passwLength = Int32.Parse(length);
            }

            string existingPasswSymbols = GetSymbols(inputPassword, passwLength);
            int passwAlphanumLength = passwLength - existingPasswSymbols.Length;
            // The number of letters will be half of the non-symbol character amount
            int passwAlphaLength = (int)Math.Round((decimal)(passwAlphanumLength / 2));
            // The number of number will be half of the non-symbol character amount
            int passNumLength = passwAlphanumLength - passwAlphaLength;

            string symbols = existingPasswSymbols;
            string letters = String.Empty;
            string numbers = String.Empty;

            Random random = new Random();

            // Generate letters
            for (int i = 0; i < passwAlphaLength; i++)
            {
                
                // random lowercase letter
                int a = random.Next(0, 26);
                char ch = (char)('a' + a);
                string str = ch.ToString();

                // If i is even, return Uppercase
                System.Math.DivRem(i, 2, out int outvalue);
                if (outvalue == 0)
                    str = str.ToUpper();

                letters += str;
            }

            // Generate numbers
            for (int i = 0; i < passNumLength; i++)
            {                
                // random number
                int a = random.Next(0, 9);
                string str = a.ToString();
                numbers += str;
            }

            outputPassword = String.Concat(symbols, letters, numbers);
            outputPassword = RearrangeString(outputPassword);           

            this.txtNewPassword.Text = outputPassword;
        }

        private static string RearrangeString(string inputString)
        {
            string toReturn = null;
            Random random = new Random();

            toReturn = new string(inputString.
                OrderBy(s => (random.Next(inputString.Length))).ToArray());
      
            return toReturn;
        }

        private static string GetSymbols(string inputString, int length)
        {
            string toReturn = String.Empty;
            if (!String.IsNullOrWhiteSpace(inputString))
            {
                foreach (char inputChar in inputString)
                {
                    if (!Char.IsLetter(inputChar) & !Char.IsNumber(inputChar))
                    {
                        toReturn += inputChar.ToString();
                    }
                }
            }
            else
            {
                string[] symbols = Constants.Symbols;
                int symbolsLength = (int)Math.Round((decimal)(length / 3));

                for (int i = 0; i < symbolsLength; i++)
                {
                    Random random = new Random();
                    // random number
                    int a = random.Next(0, symbols.Length);
                    string str = symbols[a];
                    toReturn += str;
                }
            }

            return toReturn;
        }
    }
}
