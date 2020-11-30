using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WPA.Models;

namespace WPA.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Passwords Passwords
        {
            get
            {
                //Passwords toReturn = new Passwords();
                string inputValue = this.Passwords.InputPassword;
                //return new Passwords(inputValue);
                return this.Passwords;
            }
            set { }
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Passwords.OutputPassword = "lll";
            //await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
    public class Passwords
    {
        public bool InputToUse { get; set; }
        public string InputPassword { get; set; }
        public string OutputPassword { get; set; }
        public string Length { get; set; }
        public string Settings { get; set; }
    }
}

