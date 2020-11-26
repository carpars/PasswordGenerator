using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WPA.Pages
{
    public class PasswordModel : PageModel
    {
        //private int _length { get; set; }
        //private int _symbolsLength { get; set; }
        //private int _alphaLength { get; set; }
        //private int _numbersLength { get; set; }
        //private string _symbols { get; set; }

        public PasswordModel()
        {
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public int Length { get; set; }
            public string ValueInput { get; set; }
            public string ValueOutput { get; set; }
            public int[] Settings { get; set; } // TODO: associate this numbers to 2 enums

            public string GetPassword()
            {
                string toReturn = String.Empty;

                int length, symbolsLength;
                string symbols = null;

                //There is no existing password and it's needed
                //  to fetch all settings to generate one
                if (String.IsNullOrWhiteSpace(this.ValueInput))
                {
                    // Fetch the select field
                    length = 1;
                    // Not user setting properties:
                    symbolsLength = (int)Math.Round((decimal)(length / 2));
                }
                else
                {
                    length = this.ValueInput.Length;
                    symbols = GetSymbols(this.ValueInput);
                    symbolsLength = symbols.Length;
                }

                int alphaLength = GetAlphaLength(length, symbolsLength);
                int numbersLength = length - alphaLength;

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
        }

        public IActionResult OnPost()
        {
            //InputModel inputModel = new InputModel();

            Input.ValueInput = Request.Form["Input.ValueInput"];
            //Input.ValueOutput = Input.GetPassword();

            ViewData["Input.ValueOutput"] = Input.GetPassword();
            PageResult pageResult = new PageResult();
            var keyValuePair = KeyValuePair.Create<string, string>("ad", Input.GetPassword());
            //var viewDataDictionary =


            //pageResult.ViewData = new ViewDataDictionary(
            //    keyValuePair
            //    );
            
            //pageResult.Page.Response.Body =
            return pageResult;
            //return pageResult.Page();
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

