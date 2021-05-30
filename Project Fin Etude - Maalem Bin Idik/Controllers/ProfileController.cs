using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_Fin_Etude___Maalem_Bin_Idik.Models;
using Project_Fin_Etude___Maalem_Bin_Idik.Common_Methodes;

namespace Project_Fin_Etude___Maalem_Bin_Idik.Controllers
{
    public class ProfileController : Controller
    {
        MAALEM_BIN_IDIK_DBEntities MyDB = new MAALEM_BIN_IDIK_DBEntities();

        // GET: Profile
        public ActionResult Index()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                if (Session["UserType"].ToString() == "utilisateur")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (Session["Banni"] != null)
                    {
                        return RedirectToAction("PageDeBannissement", "Home");
                    }
                    else
                    {
                        int id = int.Parse(Session["ID"].ToString());
                        Session["nbCommande"] = MyDB.COMMANDE_SERVICE.Where(c => c.Prix_service == 0 && c.SERVICE.ID_artisan == id).ToList().Count;
                        ViewBag.nbServiceEffectuer = MyDB.COMMANDE_SERVICE.Where(c => c.Prix_service != 0 && c.SERVICE.ID_artisan == id).ToList().Count;
                        ViewBag.totalRevenue = MyDB.COMMANDE_SERVICE.Where(c => c.SERVICE.ID_artisan == id).Sum(c => c.Prix_service);
                        ViewBag.nbServicePoster = MyDB.SERVICE.Where(c => c.ID_artisan == id).ToList().Count;
                        return View();
                    }
                }
            }
        }

        public ActionResult Settings()
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                if (Session["Banni"] != null)
                {
                    return RedirectToAction("PageDeBannissement", "Home");
                }
                else
                {
                    int id = int.Parse(Session["ID"].ToString());
                    Session["nbCommande"] = MyDB.COMMANDE_SERVICE.Where(c => c.Prix_service == 0 && c.SERVICE.ID_artisan == id).ToList().Count;
                    return View();
                }
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(Update user)
        {
            if (ModelState.IsValid)
            {
                int id = int.Parse(Session["ID"].ToString());
                Session["nbCommande"] = MyDB.COMMANDE_SERVICE.Where(c => c.Prix_service == 0 && c.SERVICE.ID_artisan == id).ToList().Count;
                string ModelPassword = PasswordEncryption.GetMD5Hash(user.MotDePasse);
                var UserU = MyDB.UTILISATEUR.FirstOrDefault(m => m.Email_utilisateur == user.AdresseElectronique);
                var UserA = MyDB.ARTISAN.FirstOrDefault(m => m.Email_artisan == user.AdresseElectronique);
                if(UserA == null || UserU == null)
                {
                    if (UserA == null)
                    {
                        UserU.Nom_utilisateur = user.Nom;
                        UserU.Prenom_utilisateur = user.Prenom;
                        UserU.DateN_utilisateur = user.DateDeNaissance;
                        UserU.Genre_utilisateur = user.Genre;
                        UserU.Tele_utilisateur = user.Telephone;
                        UserU.Email_utilisateur = user.AdresseElectronique;
                        UserU.Mdp_utilisateur = ModelPassword;
                        MyDB.SaveChanges();
                        Session["ID"] = UserU.ID_utilisateur;
                        Session["Nom"] = UserU.Nom_utilisateur;
                        Session["Prenom"] = UserU.Prenom_utilisateur;
                        Session["DateN"] = UserU.DateN_utilisateur;
                        Session["Genre"] = UserU.Genre_utilisateur;
                        Session["Tele"] = UserU.Tele_utilisateur;
                        Session["Email"] = UserU.Email_utilisateur;
                        Session["Mdp"] = UserU.Mdp_utilisateur;
                        Session["Status"] = "OK";
                    }
                    else
                    {
                        UserA.Nom_artisan = user.Nom;
                        UserA.Prenom_artisan = user.Prenom;
                        UserA.DateN_artisan = user.DateDeNaissance;
                        UserA.Genre_artisan = user.Genre;
                        UserA.Tele_artisan = user.Telephone;
                        UserA.Email_artisan = user.AdresseElectronique;
                        UserA.Mdp_artisan = ModelPassword;
                        MyDB.SaveChanges();
                        Session["ID"] = UserA.ID_artisan;
                        Session["Nom"] = UserA.Nom_artisan;
                        Session["Prenom"] = UserA.Prenom_artisan;
                        Session["DateN"] = UserA.DateN_artisan;
                        Session["Genre"] = UserA.Genre_artisan;
                        Session["Tele"] = UserA.Tele_artisan;
                        Session["Email"] = UserA.Email_artisan;
                        Session["Mdp"] = UserA.Mdp_artisan;
                        Session["Status"] = "OK";
                    }
                }
                else
                {
                    Session["Status"] = null;
                }
            }
            else
            {
                Session["Status"] = null;
            }
            return View(user);
        }
    }
}