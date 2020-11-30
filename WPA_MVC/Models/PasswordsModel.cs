using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPA_MVC.Models
{
    public class Passwords
    {
        public bool InputToUse { get; set; }
        public string InputPassword { get; set; }
        public string OutputPassword { get; set; }
        public string Length { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public string Settings { get; set; }
    }
}
