using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPA_MVC.Infrastructure;

namespace WPA_MVC.Models
{
    public class Password
    {
        // TODO: hacer bien, tipos para la vista en clase diferente (esta es usada 
        //  en controller y no deben ser strings        
        public string InputPassword { get; set; }
        public string OutputPassword { get; set; }
        public string Length { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }        
        public List<SelectListItem> CodificationList { get; set; }
        public string Codification { get; set; }
        public Settings Settings { get; set; }
    }

    public class PasswordView
    {
        // TODO: hacer bien, tipos para la vista en clase diferente (esta es usada 
        //  en controller y no deben ser strings        
        public string InputPassword { get; set; }
        public string OutputPassword { get; set; }
        public string Length { get; set; }
        public string Codification { get; set; }
        public SettingsView Settings { get; set; }        
    }

    public class Settings {
        public bool SavePreferences { get; set; }
        public bool HidePassword { get; set; }
        public bool IncludeSymbols { get; set; }
        public bool IncludeNumbers { get; set; }
        public bool IncludeLowercase { get; set; }
        public bool IncludeUppercase { get; set; }
        public string HexadecimalDigits { get; set; }
        public bool ExcludeSimilar { get; set; }
        public bool ExcludeAmbiguous { get; set; }
        public bool GenerateOnDevice { get; set; }
        public bool AutoSelect { get; set; }
        public bool AutoCopyToClipboard { get; set; }
        public bool LoadSettingsAnywhere { get; set; }
    }

    public class SettingsView
    {
        public string SavePreferences { get; set; }
        public string HidePassword { get; set; }
        public string IncludeSymbols { get; set; }
        public string IncludeNumbers { get; set; }
        public string IncludeLowercase { get; set; }
        public string IncludeUppercase { get; set; }
        public string HexadecimalDigits { get; set; }
        public string ExcludeSimilar { get; set; }
        public string ExcludeAmbiguous { get; set; }
        public string GenerateOnDevice { get; set; }
        public string AutoSelect { get; set; }
        public string AutoCopyToClipboard { get; set; }
        public string LoadSettingsAnywhere { get; set; }
    }
}
