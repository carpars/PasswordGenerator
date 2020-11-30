using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WPA.Pages
{
    public class IndexModel : PageModel
    {
        public string InputPassword { get; set; }
        public string OutPassword { get; set; }        
        public string Length { get; set; }
        public string Settings { get; set; }
        public void OnGet()
        {
            throw new Exception("Entra on get");
        }

        public void Page_Load()
        {
            throw new Exception("Entra LOad");
        }

    }
}

