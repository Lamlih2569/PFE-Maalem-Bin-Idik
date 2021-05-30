using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_Fin_Etude___Maalem_Bin_Idik.Models
{
    public class Registration
    {
        public string Nom_utilisateur { get; set; }
        public string Prenom_utilisateur { get; set; }
        public DateTime DateN_utilisateur { get; set; }
        public string Genre_utilisateur { get; set; }
        public Nullable<int> Tele_utilisateur { get; set; }
        public string Email_utilisateur { get; set; }
        public string Mdp_utilisateur { get; set; }
        public string type_utilisateur { get; set; }
    }
}