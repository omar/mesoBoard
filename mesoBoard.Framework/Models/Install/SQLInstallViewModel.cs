using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mesoBoard.Framework.Models
{
    public class SQLInstallViewModel
    {
        [Required(ErrorMessage = "Enter a SQL server address")]
        [Display(Name = "SQL server address")]
        public string DatabaseServer { get; set; }

        [Required(ErrorMessage = "Enter a database name")]
        [Display(Name = "Database name")]
        public string DatabaseName { get; set; }

        [DefaultValue(false)]
        [Display(Name = "Use integrated security")]
        public bool UseIntegratedSecurity { get; set; }

        [Required(ErrorMessage = "Enter a server login")]
        [Display(Name = "Login")]
        public string DatabaseLogin { get; set; }

        [Required(ErrorMessage = "Enter SQL server login password")]
        [Display(Name = "Password")]
        public string DatabasePassword { get; set; }
    }
}