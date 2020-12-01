using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WPA_MVC.Models;
using WPA_MVC.Infrastructure;

namespace WPA_MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Passwords passwords = new Passwords()
            {
                InputToUse = false,
                MinLength = Constants.PasswordMinLength,
                MaxLength = Constants.PasswordMaxLength
            };
            return View(passwords);
        }

        [HttpPost]
        public IActionResult Index(Passwords passwordsRequest)
        {
            string inputToUse = Request.Form["existingPassw"];

            Passwords passwords = new Passwords()
            {
                InputToUse = String.Equals(inputToUse, "1") ? passwordsRequest.InputToUse = true : passwordsRequest.InputToUse = false,
                InputPassword = passwordsRequest.InputPassword,
                OutputPassword = passwordsRequest.OutputPassword,
                Length = passwordsRequest.Length,
                MinLength = Constants.PasswordMinLength,
                MaxLength = Constants.PasswordMaxLength,
                Settings = passwordsRequest.Settings
            };

            int passwLength = 0;
            // TODO: Set a User advise if InputToUse == true and InputPassword == null
            if (passwords.InputPassword != null)
            {
                passwLength = passwords.InputPassword.Length;
            }
            else
            {
                passwLength = Int32.Parse(passwords.Length);
            }                           
            
            string existingPasswSymbols = GetSymbols(passwords.InputPassword, Int32.Parse(passwords.Length));
            int passwAlphanumLength = passwLength - existingPasswSymbols.Length;
            // The number of letters will be half of the non-symbol character amount
            int passwAlphaLength = (int)Math.Round((decimal)(passwAlphanumLength / 2));
            // The number of number will be half of the non-symbol character amount
            int passNumLength = passwAlphanumLength - passwAlphaLength;           

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

            passwords.OutputPassword = String.Concat(symbols, letters, numbers);            
            passwords.OutputPassword = RearrangeString(passwords.OutputPassword);

            // TODO: fix the problem that chars are not placed ramdomly

            //Additional calls because the result was too dummy (too equal-type chars together)
            //passwords.OutputPassword = RearrangeString(passwords.OutputPassword);
            //passwords.OutputPassword = RearrangeString(passwords.OutputPassword);

            return View(passwords);
        }

        private static string RearrangeString(string inputString)
        {
            Random random = new Random();
            string rand = new string(inputString.
                OrderBy(s => (random.Next(2) % 2) == 0).ToArray());
            return rand;
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
