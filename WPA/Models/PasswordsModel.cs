using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPA.Models
{

    public class Passwords
    {
        public string InputValue { get; set; }
        public string OutputValue { get; set; }
        public int Length { get; set; }
    
        private string _value { get; set; }
        private int _length { get; set; }
        private int _symbolsLength { get; set; }
        private int _alphaLength { get; set; }
        private int _numbersLength { get; set; }
        private string _symbols { get; set; }

        public Passwords(string value)
        { 
            //There is no existing password and it's needed
            //  to fetch all settings to generate one
            if (String.IsNullOrWhiteSpace(value))
            {
                // Fetch the select field
                _length = 1;
                // Not user setting properties:
                _symbolsLength = (int)Math.Round((decimal)(_length / 2));
            }
            else
            {
                _value = value;
                _length = value.Length;                                               
                _symbols = GetSymbols(_value);
                _symbolsLength = _symbols.Length;                
            }

            _alphaLength = GetAlphaLength(_length, _symbolsLength);
            _numbersLength = _length - _alphaLength;
        }

        public string Value
        {
            get
            {
                return GetValue(_symbols, _symbolsLength, _alphaLength, _numbersLength);
            }
        }

        private string GetValue(string symbols, int symbolsLength, int alphaLength, int numbersLength)
        {
            string toReturn = String.Empty;

            string toReturnSymbols = String.Empty;
            string toReturnLetters = String.Empty;
            string toReturnNumbers = String.Empty;

            Random random = new Random();
            if (String.IsNullOrWhiteSpace(symbols))
            {
                char[] allSymbols = { "\\"[0], "º"[0], "ª"[0], "|"[0], "!"[0], "@"[0],
                    "\""[0],"#"[0],"·"[0],"~"[0],"$"[0],"€"[0],"%"[0],"¬"[0],
                    "&"[0],"/"[0],"("[0],")"[0],"="[0],"?"[0],"¿"[0],"]"[0],"*"[0],"+"[0],
                    "["[0],"^"[0],"`"[0],"}"[0],"Ç"[0],"´"[0],"{"[0],"¨"[0],"´"[0],
                    "_"[0],"-"[0],":"[0],"."[0],";"[0],","[0] ,"<"[0] ,">"[0] };

                // Generate symbols                
                for (int i = 0; i < symbolsLength; i++)
                {
                    int randomOffset = random.Next(0, 26);
                    char letter = (char)allSymbols[randomOffset];
                    toReturnSymbols += letter.ToString();
                }                
            }
            // Generate letters
            random = new Random();
            for (int i = 0; i < alphaLength; i++)
            {
                int randomOffset = random.Next(0, alphaLength);
                char letter = (char)('a' + randomOffset);
                toReturnLetters += letter.ToString();
            }
            // Generate numbers
            random = new Random();
            for (int i = 0; i < alphaLength; i++)
            {
                int randomNumber = random.Next(0, 9);                
                toReturnNumbers += randomNumber.ToString();
            }           

            toReturn = String.Concat(toReturnSymbols, toReturnLetters, toReturnNumbers);

            // Rearrange
            toReturn = RearrangeString(toReturn);
            toReturn = RearrangeString(toReturn);
            toReturn = RearrangeString(toReturn);

            return toReturn;
        }

        private static string RearrangeString(string inputString)
        {
            Random random = new Random();
            string rand = new string(inputString.
                OrderBy(s => (random.Next(2) % 2) == 0).ToArray());
            return rand;
        }

        private static int GetAlphaLength(int length, int symbolsLength)
        {
            return (int)Math.Round((decimal)((length - symbolsLength) / 2));
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

