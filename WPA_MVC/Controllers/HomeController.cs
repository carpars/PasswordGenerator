using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WPA_MVC.Models;

namespace WPA_MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Passwords passwords = new Passwords();
            return View(passwords);
        }

        [HttpPost]
        public IActionResult Index(Passwords passwordsRequest)
        {
            Passwords passwords = new Passwords()
            {
                InputToUse = passwordsRequest.InputToUse,
                InputPassword = passwordsRequest.InputPassword,
                OutputPassword = passwordsRequest.OutputPassword,
                Length = passwordsRequest.Length,
                Settings = passwordsRequest.Settings
            };

            // TODO: Set a User advise if InputToUse == true and Length == null
            int passwLength = (passwords.InputToUse && !String.IsNullOrWhiteSpace(passwords.Length)) ? Int32.Parse(passwords.Length) : passwords.InputPassword.Length;
            string existingPasswSymbols = GetSymbols(passwords.InputPassword);
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

            //Additional calls because the result was too dummy (too equal-type chars together)
            passwords.OutputPassword = RearrangeString(passwords.OutputPassword);
            passwords.OutputPassword = RearrangeString(passwords.OutputPassword);

            return View(passwords);
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
