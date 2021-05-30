using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_Fin_Etude___Maalem_Bin_Idik.Models
{
    public class Update
    {
        public int ID_user { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime DateDeNaissance { get; set; }
        public string Genre { get; set; }
        public int Telephone { get; set; }
        public string AdresseElectronique { get; set; }
        public string MotDePasse { get; set; }
    }
}