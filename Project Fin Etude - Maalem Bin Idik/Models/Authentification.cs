using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Project_Fin_Etude___Maalem_Bin_Idik.Models
{
    public class Authentification
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}