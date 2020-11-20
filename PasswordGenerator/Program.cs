using System;
using System.Linq;

namespace PasswordGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PASSWORD GENERATOR");

            // generates a new password 
            //   - from a current one (from whitch the app will take the symbols, in order
            //        to preserve validity for the login purpose (since some apps-logins doesn't allow 
            //        certain specific symbols)
            //   - from chosen types of characters (uppercase letters, lowercase letters, numbers and symbols)

            //TODO: Implement generation of password from chosen types of characters 
            string initialPassw = "5B6,h+FdD6!4?;dWy_Qy>BxyQvC2^+2r7#%SzM=;Gz~j[cBge)kU@&72KW<?LmyNu8nzYpX]>$SXau)BCV$GP~xVz8";
            string outputPassw = String.Empty;

            int passwLength = initialPassw.Length;
            string initialPasswSymbols = GetSymbols(initialPassw);
            int passwAlphanumLength = passwLength - initialPasswSymbols.Length;
            int passwAlphaLength = (int)Math.Round((decimal)(passwAlphanumLength / 2));
            int passNumLength = passwAlphanumLength - passwAlphaLength;

            string symbols = GetSymbols(initialPassw);
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

                // If i is even`, return Uppercase
                System.Math.DivRem(i, 2, out int outvalue);
                if (outvalue == 0)
                    str = str.ToUpper();

                letters += str;
            }

            // Generate numbers
            for (int i = 0; i < passNumLength; i++)
            {
                Random random = new Random();
                // random lowercase letter
                int a = random.Next(0, 9);
                string str = a.ToString();
                numbers += str;
            }

            outputPassw = String.Concat(symbols, letters, numbers);
            outputPassw = RearrangeString(outputPassw);

            //Additional calls because the result was too dummy (too equal-type chars together)
            outputPassw = RearrangeString(outputPassw);
            outputPassw = RearrangeString(outputPassw);

            
            Console.WriteLine("RESULTADO: ");
            Console.WriteLine(outputPassw);

            Console.Read();

            //do
            //{
            //    if (!Console.KeyAvailable)
            //    {
            //        // Do something
            //        if (Console.ReadKey(true).Key == ConsoleKey.A)
            //        {
            //            Console.WriteLine("Has presionado la Primera tecla del Alfabeto: " + ConsoleKey.A);
            //        }
            //        else
            //        {
            //            Console.WriteLine("Has presionado  " + Console.ReadKey(true).Key);
            //        }                   
            //    }
            //} while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            //Environment.Exit(0);
        }

        private static string RearrangeString(string inputString)
        {
            Random num = new Random();
            string rand = new string(inputString.
                OrderBy(s => (num.Next(2) % 2) == 0).ToArray());
            return rand;
        }

        private static string GetSymbols(string inputString)
        {
            string toReturn = String.Empty;

            foreach (char inputChar in inputString)
            {
                if (!Char.IsLetter(inputChar) & !Char.IsNumber(inputChar))
                {
                    //Console.WriteLine(inputChar + " is a Symbol");
                    toReturn += inputChar.ToString();
                }
                    
                //else
                    //Console.WriteLine(inputChar + " is not a Symbol");
            }           

            return toReturn;
        }
}

public static class ArrayExtensions
{
    public static int Push<T>(this T[] source, T value)
    {
        var index = Array.IndexOf(source, default(T));

        if (index != -1)
        {
            source[index] = value;
        }

        return index;
    }
}
}
