using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mesoBoard.Framework.Models
{
    public class MailInstallViewModel
    {
        [Required(ErrorMessage = "Enter mail server address")]
        [Display(Name = "Mail server address")]
        public string MailServerAddress { get; set; }

        [Required(ErrorMessage = "Enter mail server port number")]
        [Display(Name = "Port number")]
        [DefaultValue(25)]
        public int PortNumber { get; set; }
        
        [DefaultValue(false)]
        [Display(Name = "Use default credentials")]
        public bool MailUseDefaultCredentials { get; set; }

        [Required(ErrorMessage = "Enter mail server login")]
        [Display(Name = "Login")]
        public string MailLogin { get; set; }

        [Required(ErrorMessage = "Enter mail server login password")]
        [Display(Name = "Password")]
        public string MailPassword { get; set; }
    }
}