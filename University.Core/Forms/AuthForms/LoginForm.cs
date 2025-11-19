using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Core.Forms.AuthForms
{
    public class LoginForm
    {
        [Required]
        [EmailAddress]
        public  string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
