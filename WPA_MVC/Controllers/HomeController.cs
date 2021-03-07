using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WPA_MVC.Models;
using WPA_MVC.Infrastructure;
using System.Windows.Forms;

namespace WPA_MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Password password = new Password()
            {                
                MinLength = Constants.PasswordMinLength,
                MaxLength = Constants.PasswordMaxLength
            };
            return View(password);
        }

        //[HttpPost]
        //public IActionResult Index(Passwords passwordsRequest)
        //{
        //    // TODO: hacer el retorno de la password en otro metodo, en Ajax            

        //    Passwords passwords = new Passwords()
        //    {
        //        InputPassword = passwordsRequest.InputPassword,
        //        OutputPassword = passwordsRequest.OutputPassword,
        //        Length = passwordsRequest.Length,
        //        MinLength = Constants.PasswordMinLength,
        //        MaxLength = Constants.PasswordMaxLength,
        //        Settings = passwordsRequest.Settings
        //    };

        //    int passwLength = 0;

        //    if (!String.IsNullOrWhiteSpace(passwords.InputPassword))
        //    {
        //        passwLength = passwords.InputPassword.Length;
        //    }
        //    else
        //    {
        //        passwLength = Int32.Parse(passwords.Length);
        //    }

        //    string existingPasswSymbols = GetSymbols(passwords.InputPassword, Int32.Parse(passwords.Length));
        //    //// The number of letters will be half of the non-symbol character amount
        //    //int passwLetterLength = GetLetters(passwords.InputPassword, Int32.Parse(passwords.Length)); // passwLength - existingPasswSymbols.Length;

        //    // The number of number will be half of the non-symbol character amount
        //    int passNumLength = passwLength - existingPasswSymbols.Length - passwLetterLength;// == 0 ? - passwAlphaLength;           

        //    string symbols = existingPasswSymbols;
        //    string letters = String.Empty;
        //    string numbers = String.Empty;

        //    // Generate letters
        //    for (int i = 0; i < passwLetterLength; i++)
        //    {
        //        Random random = new Random();
        //        // random lowercase letter
        //        int a = random.Next(0, 26);
        //        char ch = (char)('a' + a);
        //        string str = ch.ToString();

        //        // If 'Include UPPERCASE' is checked, If i is even, return Uppercase
        //        if (true)
        //        {

        //        }
        //        if (passwordsRequest.Form[""] == true)
        //        {

        //        }

        //        System.Math.DivRem(i, 2, out int outvalue);
        //        if (outvalue == 0)
        //            str = str.ToUpper();

        //        letters += str;
        //    }

        //    // Generate numbers
        //    for (int i = 0; i < passNumLength; i++)
        //    {
        //        Random random = new Random();
        //        // random number
        //        int a = random.Next(0, 9);
        //        string str = a.ToString();
        //        numbers += str;
        //    }

        //    passwords.OutputPassword = String.Concat(symbols, letters, numbers);
        //    passwords.OutputPassword = RearrangeString(passwords.OutputPassword);

        //    return View(passwords);
        //}

        /// <summary>
        ///     
        /// </summary>
        /// <param name="passwordsRequest"></param>
        /// <returns>Ajax response; the returned value is got from the input password 
        /// if any (its length and the exact same symbols if any to avoid some 
        /// incompatibilities for the use of the new password), or build from scratch 
        /// with the same proportion of symbols, letters and numbers (if the corresponding 
        /// option is checked); the letters are UPPERCASE and lowercase in the same 
        /// proportion (if both options are checked)</returns>
        [HttpPost]
        public JsonResult GetResult(PasswordView passwordRequest)
        {
            PasswordView toReturn = new PasswordView();
            
            int passwLength = 0;
            int passwLettersLength = 0;
            int passNumLength = 0;            
            string existingPasswSymbols = String.Empty;

            try
            {
                if (!String.IsNullOrWhiteSpace(passwordRequest.InputPassword))
                {
                    passwLength = passwordRequest.InputPassword.Length;
                    existingPasswSymbols = GetSymbols(passwordRequest.InputPassword, passwLength);
                }
                else if (!String.IsNullOrWhiteSpace(passwordRequest.Length))
                {
                    passwLength = Int32.Parse(passwordRequest.Length);
                }
                else
                {
                    passwLength = Constants.PasswordMinLength;
                }

                // The number of letters will be half of the non-symbol character amount
                passwLettersLength = (int)(Math.Round((decimal)(passwLength - existingPasswSymbols.Length) / 2, MidpointRounding.AwayFromZero));

                // The number of number will be the difference
                passNumLength = passwLength - existingPasswSymbols.Length - passwLettersLength;

                string symbols = existingPasswSymbols;
                // TODO: Buscar una forma buena de pasar bools de JS en string a Controller bool
                string letters = GetLetters(passwLettersLength, passwordRequest.Settings.IncludeLowercase == "true" ? true : false, passwordRequest.Settings.IncludeUppercase == "true" ? true : false);
                string numbers = GetNumbers(passNumLength);

                toReturn.OutputPassword = String.Concat(symbols, letters, numbers);

                toReturn.OutputPassword = RearrangeString(toReturn.OutputPassword);
                //if (passwordRequest.Settings.AutoCopyToClipboard == "true")
                //{
                //    Clipboard.SetText(toReturn.OutputPassword);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }            

            return Json(toReturn);
        }

        private static string RearrangeString(string inputString)
        {
            string toReturn = null;
            Random random = new Random();

            toReturn = new string(inputString.
                OrderBy(s => (random.Next(inputString.Length))).ToArray());

            return toReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="length"></param>
        /// <returns>If there's an input password, the same symbols it includes; 
        /// if there isn't an input password, a set of sympols stablished by the 
        /// app of inputpassword/3 in length </returns>
        private static string GetSymbols(string inputString, int passwLength)
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

                int symbolsLength = (int)Math.Round((decimal)passwLength / 3);
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

        private static string GetLetters(int letterLength, bool includeLowercase, bool includeUppercase)
        {
            string toReturn = String.Empty;

            string str = String.Empty;
            for (int i = 0; i < letterLength; i++)
            {
                Random random = new Random();
                // random lowercase letter
                int a = random.Next(0, 26);
                char ch = (char)('a' + a);
                str = ch.ToString();

                // If 'Include UPPERCASE' is checked
                if (includeUppercase)
                {
                    if (!includeLowercase)
                    {
                        str = str.ToUpper();
                    }
                    else
                    {
                        // For even i, it converts the char to UPPERCASE
                        System.Math.DivRem(i, 2, out int outvalue);
                        if (outvalue == 0)
                            str = str.ToUpper();
                    }
                }

                toReturn += str;
            }

            return toReturn;
        }

        private string GetNumbers(int numberLength)
        {
            string toReturn = String.Empty;
            for (int i = 0; i < numberLength; i++)
            {
                Random random = new Random();
                // random number
                int a = random.Next(0, 9);
                string str = a.ToString();
                toReturn += str;
            }

            return toReturn;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
