using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mesoBoard.Framework.Models
{
    public class ResendActivationCodeViewModel
    {
        [Required]
        [Display(Name = "Username or email")]
        public string UsernameOrEmail { get; set; }
    }
}