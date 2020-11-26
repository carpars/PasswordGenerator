using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private void txtExistingPassw_TextChanged(object sender, EventArgs e)
        {
            string existingPassw = this.txtExistingPassw.Text;

            string outputPassw = String.Empty;

            int passwLength = existingPassw.Length;
            string existingPasswSymbols = GetSymbols(existingPassw);
            int passwAlphanumLength = passwLength - existingPasswSymbols.Length;
            // The number of letters will be half of the non-symbol character amount
            int passwAlphaLength = (int)Math.Round((decimal)(passwAlphanumLength / 2));
            // The number of number will be half of the non-symbol character amount
            int passNumLength = passwAlphanumLength - passwAlphaLength;

            //Console.WriteLine("initialPasswSymbols: " + initialPasswSymbols);
            //Console.WriteLine("initialPasswSymbols.Length: " + initialPasswSymbols.Length);
            //Console.WriteLine("passwAlphaLength: " + passwAlphaLength);
            //Console.WriteLine("passNumLength: " + passNumLength);


            string symbols = existingPasswSymbols;
            string letters = String.Empty;
            string numbers = String.Empty;

            // Generate letters
            for (int i = 0; i < passwAlphaLength; i++)
            {
                Random random = new Random();
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
                Random random = new Random();
                // random number
                int a = random.Next(0, 9);
                string str = a.ToString();
                numbers += str;
            }

            outputPassw = String.Concat(symbols, letters, numbers);
            //Console.WriteLine("  outputPassw: " + outputPassw);
            outputPassw = RearrangeString(outputPassw);

            //Additional calls because the result was too dummy (too equal-type chars together)
            outputPassw = RearrangeString(outputPassw);
            outputPassw = RearrangeString(outputPassw);

            this.txtNewPassword.Text = outputPassw;
        }

        private static string RearrangeString(string inputString)
        {
            Random random = new Random();
            string rand = new string(inputString.
                OrderBy(s => (random.Next(2) % 2) == 0).ToArray());
            return rand;
        }

        private static string GetSymbols(string inputString)
        {
            string toReturn = String.Empty;
            foreach (char inputChar in inputString)
            {
                if (!Char.IsLetter(inputChar) & !Char.IsNumber(inputChar))
                {
                    toReturn += inputChar.ToString();
                }
            }

            //Console.WriteLine("Total simbols:  " + counter + ", y son: " + toReturn);
            return toReturn;
        }
    }
}
