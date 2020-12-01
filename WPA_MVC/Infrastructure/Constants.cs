using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPA_MVC.Infrastructure
{
    public class Constants
    {
        public static readonly int PasswordMinLength = 8;
        public static readonly int PasswordMaxLength = 128;
        public static readonly string[] Symbols = { "&", "‘", "*", "@", "`", "\\", "{", "}", "[", "]", "^", "}", "]", ")", ":", ",", "$", "=", "!", ">", "<", "–", "{", "[", "(", "(", ")", "%", "|", "+", "#", "“", ";", "/", "~", "_", "?", "." };
    }
}
