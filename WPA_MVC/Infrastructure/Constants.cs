using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPA_MVC.Infrastructure
{
    public class Constants
    {
        public static readonly int PasswordMinLength = 8;
        public static readonly int PasswordMaxLength = 256;
        public static readonly string[] Symbols = { "&", "‘", "*", "@", "`", "\\", "{", "}", "[", "]", "^", "}", "]", ")", ":", ",", "$", "=", "!", ">", "<", "–", "{", "[", "(", "(", ")", "%", "|", "+", "#", "“", ";", "/", "~", "_", "?", "." };

        public enum Codifications
        {
            Hex,
            Base64,
            Base32,
            UTF8,
            Dec
        }
    }
}
