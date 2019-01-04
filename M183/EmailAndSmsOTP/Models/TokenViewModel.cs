using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmailAndSmsOTP.Models
{
    public class TokenViewModel
    {
        [Required]
        public string Token { get; set; }
    }
}