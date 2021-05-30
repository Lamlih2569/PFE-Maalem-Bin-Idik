using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_Fin_Etude___Maalem_Bin_Idik.Models;

namespace Project_Fin_Etude___Maalem_Bin_Idik.Controllers
{
    public class AdminController : Controller
    {
        MAALEM_BIN_IDIK_DBEntities MyDB = new MAALEM_BIN_IDIK_DBEntities();

        public ActionResult Index()
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                if(Session["UserType"].ToString() == "admin")
                {
                    ViewBag.NbUsers = MyDB.UTILISATEUR.ToList().Count;
                    ViewBag.NbArtisans = MyDB.ARTISAN.ToList().Count;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult ListeServices()
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                if (Session["UserType"].ToString() == "admin")
                {
                    List<SERVICE> listeService = MyDB.SERVICE.ToList();
                    return View(listeService);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult SupprimerService(int id)
        {
            try
            {
                SERVICE service_enr = MyDB.SERVICE.Where(s => s.ID_service == id).FirstOrDefault();
                List<COMMANDE_SERVICE> commandes = MyDB.COMMANDE_SERVICE.Where(c => c.SERVICE.ID_service == id).ToList();
                if (commandes.Count != 0)
                {
                    foreach (COMMANDE_SERVICE commande in commandes)
                    {
                        MyDB.COMMANDE_SERVICE.Remove(commande);
                    }
                }
                MyDB.SERVICE.Remove(service_enr);
                MyDB.SaveChanges();
                return RedirectToAction("ListeServices");
            }
            catch
            {
                return RedirectToAction("ListeServices");
            }
        }

        public ActionResult ListeUsers()
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                if (Session["UserType"].ToString() == "admin")
                {
                    List<UTILISATEUR> listeUsers = MyDB.UTILISATEUR.ToList();
                    return View(listeUsers);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult BannirUtilisateur(int id)
        {
            try
            {
                UTILISATEUR user = MyDB.UTILISATEUR.FirstOrDefault(u => u.ID_utilisateur == id);
                user.Utilisateur_Banni = true;
                MyDB.SaveChanges();
                return RedirectToAction("ListeUsers");
            }
            catch
            {
                return RedirectToAction("ListeUsers");
            }
        }

        public ActionResult AnnulerLeBanUtilisateur(int id)
        {
            try
            {
                UTILISATEUR user = MyDB.UTILISATEUR.FirstOrDefault(u => u.ID_utilisateur == id);
                user.Utilisateur_Banni = false;
                MyDB.SaveChanges();
                return RedirectToAction("ListeUsers");
            }
            catch
            {
                return RedirectToAction("ListeUsers");
            }
        }

        public ActionResult ListeArtisans()
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                if (Session["UserType"].ToString() == "admin")
                {
                    List<ARTISAN> ListeArtisans = MyDB.ARTISAN.ToList();
                    return View(ListeArtisans);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        public ActionResult BannirArtisan(int id)
        {
            try
            {
                ARTISAN artisan = MyDB.ARTISAN.FirstOrDefault(a => a.ID_artisan == id);
                artisan.Artisan_Banni = true;
                MyDB.SaveChanges();
                return RedirectToAction("ListeArtisans");
            }
            catch
            {
                return RedirectToAction("ListeArtisans");
            }
        }

        public ActionResult AnnulerLeBanArtisan(int id)
        {
            try
            {
                ARTISAN artisan = MyDB.ARTISAN.FirstOrDefault(a => a.ID_artisan == id);
                artisan.Artisan_Banni = false;
                MyDB.SaveChanges();
                return RedirectToAction("ListeArtisans");
            }
            catch
            {
                return RedirectToAction("ListeArtisans");
            }
        }
    }
}
