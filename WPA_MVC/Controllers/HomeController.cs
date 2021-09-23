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
using Microsoft.Extensions.Configuration;
using OtpNet;


namespace WPA_MVC.Controllers
{
    public class HomeController : Controller
    {
        private WebConfig _webConfig;
        private static bool _includeLowercase;
        private static bool _includeUppercase;

        public HomeController(WebConfig webConfig)
        {
            _webConfig = webConfig;
        }

        public IActionResult Index()
        {

            Dictionary<object, string> codificationDictionary = new Dictionary<object, string>();
            foreach (var item in (Constants.Codifications[])Enum.GetValues(typeof(Constants.Codifications)))
            {
                codificationDictionary.Add((int)item, Enum.GetName(item.GetType(), item));
            }

            Password passwordDefault = new Password();
            passwordDefault = _webConfig.Password;

            passwordDefault.OutputPassword = null;
            passwordDefault.MinLength = Constants.PasswordMinLength;
            passwordDefault.MaxLength = Constants.PasswordMaxLength;
            passwordDefault.CodificationList = SelectList.Build(codificationDictionary, null, false, false, false, false);
            return View(passwordDefault);
        }

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
            Password toReturn = new Password();

            int passwLength = 0;
            int passwLettersLength = 0;
            int passNumLength = 0;
            string existingPasswSymbols = String.Empty;
            bool result;

            try
            {
                toReturn = _webConfig.Password;                

                if (!passwordRequest.Equals(toReturn))
                {
                    if (!String.IsNullOrWhiteSpace(passwordRequest.InputPassword))
                    {
                        passwLength = passwordRequest.InputPassword.Length;
                    }
                    else if (!String.IsNullOrWhiteSpace(passwordRequest.Length))
                    {
                        passwLength = Int32.Parse(passwordRequest.Length);
                    }
                    else
                    {
                        passwLength = Constants.PasswordMinLength;
                    }

                    if (Boolean.TryParse(passwordRequest.Settings.IncludeSymbols, out result))
                    {
                        if (result)
                        {
                            existingPasswSymbols = GetSymbols(passwordRequest.InputPassword, passwLength);
                        }
                    }

                    if (Boolean.TryParse(passwordRequest.Settings.IncludeLowercase, out result))
                    {
                        _includeLowercase = result;
                    }
                    if (Boolean.TryParse(passwordRequest.Settings.IncludeUppercase, out result))
                    {
                        _includeUppercase = result;
                    }

                    // The number of letters will be half of the non-symbol character amount
                    passwLettersLength = (int)(Math.Round((decimal)(passwLength - existingPasswSymbols.Length) / 2, MidpointRounding.AwayFromZero));

                    // The number of number will be the difference
                    passNumLength = passwLength - existingPasswSymbols.Length - passwLettersLength;

                    string symbols = existingPasswSymbols;
                    // TODO: Buscar una forma buena de pasar bools de JS en string a Controller bool
                    string letters = GetLetters(passwLettersLength, _includeLowercase, _includeUppercase, passwordRequest.Settings.HexadecimalDigits == "true" ? true : false);
                    string numbers = GetNumbers(passNumLength);

                    toReturn.OutputPassword = String.Concat(symbols, letters, numbers);

                    toReturn.OutputPassword = RearrangeString(toReturn.OutputPassword);

                    int requestCodification = Int32.Parse(passwordRequest.Codification);
                    toReturn.OutputPassword = GetPasswordEncoded(toReturn.OutputPassword, requestCodification);

                    //if (passwordRequest.Settings.AutoCopyToClipboard == "true")
                    //{
                    //    Clipboard.SetText(toReturn.OutputPassword);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(toReturn);
        }

        private string GetPasswordEncoded(string initPassword, int codification)
        {
            string toReturn = initPassword;                        

            switch (codification)
            {
                // TODO get the values from enum Codifications
                case 0:
                    // TODO convert to Hex
                    //var plainTextBytes0 = System.Text.Encoding.Unicode.GetBytes(initPassword);
                    //toReturn = ConvertToHexString(plainTextBytes0);
                    BaseConversion baseConversion = new BaseConversion(Constants.Codifications.Hex, _includeLowercase, _includeUppercase);
                    toReturn = baseConversion.ConvertToHexString(initPassword);
                    break;

                case 1:
                    var plainTextBytes1 = System.Text.Encoding.Unicode.GetBytes(initPassword);
                    toReturn = Convert.ToBase64String(plainTextBytes1);
                    break;

                case 2:
                    var plainTextBytes2 = BaseConversion.Decode(initPassword);// Base32.FromBase32String(initPassword);
                    toReturn = BaseConversion.Encode(plainTextBytes2); //Base32.ToBase32String(plainTextBytes2);
                    break;

                case 3:                    
                    var plainTextBytes3 = System.Text.Encoding.UTF8.GetBytes(initPassword);
                    toReturn = Convert.ToBase64String(plainTextBytes3);
                    break;

                case 4:
                    // TODO convert to Dec
                    var plainTextBytes4 = System.Text.Encoding.Unicode.GetBytes(initPassword);
                    toReturn = Convert.ToBase64String(plainTextBytes4);
                    break;


                default:
                    break;
            }

            return toReturn;
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

        private static string GetLetters(int letterLength, bool includeLowercase, bool includeUppercase, bool hexadecimalDigits)
        {
            string toReturn = String.Empty;

            string str = String.Empty;
            for (int i = 0; i < letterLength; i++)
            {
                Random random = new Random();
                int a = 0;
                if (hexadecimalDigits)
                {
                    // random lowercase letter
                    a = random.Next(0, 5);
                }
                else
                {
                    // random lowercase letter
                    a = random.Next(0, 26);
                }

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

            
            if (!includeUppercase)
            {
                toReturn = toReturn.ToLower();
            }
            else if (!includeLowercase)
            {
                toReturn = toReturn.ToUpper();
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
