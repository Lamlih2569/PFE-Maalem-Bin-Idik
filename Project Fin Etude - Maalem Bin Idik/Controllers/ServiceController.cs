using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_Fin_Etude___Maalem_Bin_Idik.Models;
using System.Net;
using System.Net.Mail;

namespace Project_Fin_Etude___Maalem_Bin_Idik.Controllers
{
    public class ServiceController : Controller
    {
        MAALEM_BIN_IDIK_DBEntities MyDB = new MAALEM_BIN_IDIK_DBEntities();

        public ActionResult ToutServices()
        {
            if (Session["ID"] == null)
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
                    List<SERVICE> services = MyDB.SERVICE.ToList();
                    return View(services);
                } 
            }
        }
        [HttpPost]
        public ActionResult ToutServices(ServiceFilter filter)
        {
            List<SERVICE> services;
            string metier = filter.metier;
            string ville = filter.ville;
            if (metier != "-- Choisissez un metier --" && ville == "-- Choisissez une ville --")
            {
                 services = MyDB.SERVICE.Where(m => m.METIER.Nom_metier == metier).ToList();
            }
            else if(metier == "-- Choisissez un metier --" && ville != "-- Choisissez une ville --")
            {
                services = MyDB.SERVICE.Where(m => m.Ville_service == ville).ToList();
            }
            else
            {
                services = MyDB.SERVICE.Where(m => m.METIER.Nom_metier == metier && m.Ville_service == ville).ToList();
            }
            return View(services);
        }

        public ActionResult ServicesParArtisan()
        {
            try
            {
                if (Session["ID"] == null)
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
                        if (Session["UserType"].ToString() == "utilisateur")
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            int id = int.Parse(Session["ID"].ToString());
                            Session["nbCommande"] = MyDB.COMMANDE_SERVICE.Where(c => c.Prix_service == 0 && c.SERVICE.ID_artisan == id).ToList().Count;
                            int ID_artisan = int.Parse(Session["ID"].ToString());
                            List<SERVICE> services = MyDB.SERVICE.Where(m => m.ARTISAN.ID_artisan == ID_artisan).ToList();
                            return View(services);
                        }
                    }  
                }
            }
            catch
            {
                return HttpNotFound();
            }
        }

        public ActionResult AjouterService()
        {
            if (Session["ID"] == null)
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
                    if (Session["UserType"].ToString() == "utilisateur")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        int id = int.Parse(Session["ID"].ToString());
                        Session["nbCommande"] = MyDB.COMMANDE_SERVICE.Where(c => c.Prix_service == 0 && c.SERVICE.ID_artisan == id).ToList().Count;
                        ViewBag.listMetier = MyDB.METIER.ToList();
                        return View();
                    }
                }         
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjouterService(SERVICE service)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    service.ID_artisan = int.Parse(Session["ID"].ToString());
                    MyDB.SERVICE.Add(service);
                    MyDB.SaveChanges();
                    Session["IsAdded"] = "OK";
                    Session["Error"] = null;
                }
                else
                {
                    Session["Error"] = "OK";
                }
            }
            catch
            {
                Session["Error"] = "OK";
            }
            return RedirectToAction("AjouterService");
        }

        public ActionResult ModifierService(int id)
        {
            if (Session["ID"] == null)
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
                    if (Session["UserType"].ToString() == "utilisateur")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        int idd = int.Parse(Session["ID"].ToString());
                        Session["nbCommande"] = MyDB.COMMANDE_SERVICE.Where(c => c.Prix_service == 0 && c.SERVICE.ID_artisan == idd).ToList().Count;
                        SERVICE service = MyDB.SERVICE.FirstOrDefault(s => s.ID_service == id);
                        if (service != null)
                        {
                            if (service.ID_artisan == int.Parse(Session["ID"].ToString()))
                            {
                                Session["ServiceAmodifier"] = id;
                                ViewBag.listMetier = MyDB.METIER.ToList();
                                return View(service);
                            }
                            else
                            {
                                return HttpNotFound();
                            }
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                }    
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifierService(SERVICE service)
        {
            try
            {
                if (Session["ServiceAmodifier"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        service.ID_artisan = int.Parse(Session["ID"].ToString());
                        int id = int.Parse(Session["ServiceAmodifier"].ToString());
                        SERVICE Service = MyDB.SERVICE.FirstOrDefault(s => s.ID_service == id);
                        Service.Titre_service = service.Titre_service;
                        Service.Description_service = service.Description_service;
                        Service.Ville_service = service.Ville_service;
                        Service.ID_artisan = service.ID_artisan;
                        Service.ID_metier = service.ID_metier;
                        MyDB.SaveChanges();
                        return RedirectToAction("ServicesParArtisan");
                    }
                    else
                    {
                        return RedirectToAction("ServicesParArtisan");
                    }
                }
                else
                {
                    return RedirectToAction("ServicesParArtisan");
                }
            }
            catch
            {
                return RedirectToAction("ServicesParArtisan");
            }
        }

        public ActionResult SupprimerService(int id)
        {
            if (Session["ID"] == null)
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
                    if (Session["UserType"].ToString() == "utilisateur")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        int idd = int.Parse(Session["ID"].ToString());
                        Session["nbCommande"] = MyDB.COMMANDE_SERVICE.Where(c => c.Prix_service == 0 && c.SERVICE.ID_artisan == idd).ToList().Count;
                        SERVICE service = MyDB.SERVICE.FirstOrDefault(s => s.ID_service == id);
                        if (service != null)
                        {
                            if (service.ID_artisan == int.Parse(Session["ID"].ToString()))
                            {
                                Session["ServiceAsupprimer"] = id;
                                return View(service);
                            }
                            else
                            {
                                return HttpNotFound();
                            }
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SupprimerService()
        {
            try
            {
                if (Session["ServiceAsupprimer"] != null)
                {
                    int id = int.Parse(Session["ServiceAsupprimer"].ToString());
                    SERVICE service = MyDB.SERVICE.FirstOrDefault(s => s.ID_service == id);
                    List<COMMANDE_SERVICE> commandes = MyDB.COMMANDE_SERVICE.Where(c => c.SERVICE.ID_service == id).ToList();
                    if (commandes.Count != 0)
                    {
                        foreach (COMMANDE_SERVICE commande in commandes)
                        {
                            MyDB.COMMANDE_SERVICE.Remove(commande);
                        }
                    }
                    MyDB.SERVICE.Remove(service);
                    MyDB.SaveChanges();
                    return RedirectToAction("ServicesParArtisan");
                }
                else
                {
                    return RedirectToAction("ServicesParArtisan");
                }
            }
            catch
            {
                return RedirectToAction("ServicesParArtisan");
            }
        }

        public ActionResult DetailService(int id)
        {
            try
            {
                if (Session["ID"] == null)
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
                        SERVICE service = MyDB.SERVICE.FirstOrDefault(s => s.ID_service == id);
                        if (service != null)
                        {
                            Session["IdService"] = service.ID_service;
                            Session["TitreService"] = service.Titre_service;
                            Session["DescriptionService"] = service.Description_service;
                            Session["VilleService"] = service.Ville_service;
                            Session["MetierService"] = service.METIER.Nom_metier;
                            Session["ArtisanService"] = service.ARTISAN.Nom_artisan + " " + service.ARTISAN.Prenom_artisan;
                            return View();
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                }
            }
            catch
            {
                return HttpNotFound();
            }
        }

        public ActionResult PasserCommande()
        {
            try
            {
                if (Session["ID"] == null)
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
                        if (Session["IdService"] != null)
                        {
                            COMMANDE_SERVICE commande = new COMMANDE_SERVICE();
                            commande.ID_service = int.Parse(Session["IdService"].ToString());
                            commande.ID_utilisateur = int.Parse(Session["ID"].ToString());
                            commande.DateDePrise = DateTime.Now;
                            commande.Prix_service = 0;
                            MyDB.COMMANDE_SERVICE.Add(commande);
                            MyDB.SaveChanges();

                            int Id_Last_Commande_Service = MyDB.COMMANDE_SERVICE.OrderByDescending(p => p.ID_commande).FirstOrDefault().ID_service;
                            int Id_artisan = MyDB.SERVICE.Where(s => s.ID_service == Id_Last_Commande_Service).FirstOrDefault().ID_artisan;
                            string Email_artisan = MyDB.ARTISAN.Where(a => a.ID_artisan == Id_artisan).FirstOrDefault().Email_artisan;

                            MailMessage mail = new MailMessage();
                            NetworkCredential credential = new NetworkCredential("contact.maalem.bin.idik@gmail.com", "maalembinidik2569");
                            mail.From = new MailAddress("contact.maalem.bin.idik@gmail.com");
                            mail.To.Add(new MailAddress(Email_artisan));
                            mail.Subject = "Nouvelle Commande | Maalem Bin Idik";
                            mail.IsBodyHtml = true;
                            string body = "<div style='margin:0;padding:0;background-color:#f6f9fc;font-family:sans-serif'><center style='width:100%;table-layout:fixed;background-color:#f6f9fc;padding-bottom:40px;padding-top:40px'><div style='max-width:600px;background-color:#ffffff'><table align='center' style='margin:0 auto;width:100%;max-width:600px;border-spacing:0'><tbody><tr><td style = 'padding:0'><table width='100%' style='border-spacing:0'><tbody><tr><td><img src='https://www.dropbox.com/s/e2h30d446irv3yx/Groupe%202.jpg?raw=1' alt = 'LOGO' style = 'border:0'></td></tr></tbody></table></td></tr><tr><td style = 'padding:0'><table width = '100%' style = 'border-spacing:0;padding:10px;padding-top:50px;padding-bottom:50px;color:#4a4a4a' ><tbody><tr><td style = 'padding:0;background-color:#ffffff;padding:10px'><h3 style = 'text-align:center'>Nouvelle Commande | Maalem Bin Idik</h3></td></tr><tr><td style = 'padding:0'><p> Bonjour,<br>vous avez reçu une nouvelle commande de l'un de vos services que vous avez publier dans notre plateforme.</p><p>Voici les informations du client :</p><ul><li><span style='font-weight: bold;'>Nom :</span>" + Session["Nom"].ToString() + "</li><li><span style='font-weight: bold;'>Prénom :</span>" + Session["Prenom"].ToString() + "</li><li><span style='font-weight: bold;'>Tele :</span> 0" + Session["Tele"].ToString() + "</li><li><span style='font-weight: bold;'>Email :</span>" + Session["Email"].ToString() + "</li></ul><p>Veuillez le contacter par son téléphone ou bien son email dans le plus bref délai pour finaliser la commande</p></td></tr><tr><td style = 'padding:0'><p style = 'text-align:center'><a href='http://localhost:62290/Login' style='padding:10px 20px;background-color:#FAD107;text-decoration:none;color:black;font-weight: bold;'>Accéder à mon compte</a></p></td></tr><tr><td style='padding:0'><p> Si vous rencontrez des problèmes, veuillez contacter l'assistance à <strong><a href = 'contact.maalem.bin.idik@gmail.com'> contact.maalem.bin.idik@gmail.<wbr>com </a></strong></p></td></tr></tbody></table></td></tr><tr><td style='padding:0;height:30px;background-color:#4a4a4a'></td></tr></tbody></table></div></center></div> ";
                            mail.Body = body;

                            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                            smtpClient.EnableSsl = true;
                            smtpClient.Credentials = credential;
                            smtpClient.Send(mail);
                            return View();
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            catch
            {
                return HttpNotFound();
            }
        }

        public ActionResult CommandeParArtisan()
        {
            try
            {
                if (Session["ID"] == null)
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
                        if (Session["UserType"].ToString() == "utilisateur")
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            int id = int.Parse(Session["ID"].ToString());
                            Session["nbCommande"] = MyDB.COMMANDE_SERVICE.Where(c => c.Prix_service == 0 && c.SERVICE.ID_artisan == id).ToList().Count;
                            int ID_artisan = int.Parse(Session["ID"].ToString());
                            List<COMMANDE_SERVICE> commands = MyDB.COMMANDE_SERVICE.Where(c => c.SERVICE.ID_artisan == ID_artisan && c.Prix_service == 0).ToList();
                            return View(commands);
                        }
                    }
                }
            }
            catch
            {
                return HttpNotFound();
            }
        }

        public ActionResult FinaliserCommande(int id)
        {
            if (Session["ID"] == null)
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
                    if (Session["UserType"].ToString() == "utilisateur")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        int idd = int.Parse(Session["ID"].ToString());
                        Session["nbCommande"] = MyDB.COMMANDE_SERVICE.Where(c => c.Prix_service == 0 && c.SERVICE.ID_artisan == idd).ToList().Count;
                        COMMANDE_SERVICE commande = MyDB.COMMANDE_SERVICE.FirstOrDefault(c => c.ID_commande == id);
                        if (commande != null)
                        {
                            if (commande.SERVICE.ARTISAN.ID_artisan == int.Parse(Session["ID"].ToString()))
                            {
                                Session["CommandeAfinaliser"] = id;
                                return View();
                            }
                            else
                            {
                                return HttpNotFound();
                            }
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinaliserCommande(COMMANDE_SERVICE commande1)
        {
            try
            {
                if (Session["CommandeAfinaliser"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        int id = int.Parse(Session["CommandeAfinaliser"].ToString());
                        COMMANDE_SERVICE commande = MyDB.COMMANDE_SERVICE.FirstOrDefault(c => c.ID_commande == id);
                        commande.Prix_service = commande1.Prix_service;
                        MyDB.SaveChanges();
                        return RedirectToAction("CommandeParArtisan");
                    }
                    else
                    {
                        return RedirectToAction("CommandeParArtisan");
                    }
                }
                else
                {
                    return RedirectToAction("CommandeParArtisan");
                }
            }
            catch
            {
                return RedirectToAction("CommandeParArtisan");
            }
        }

        public ActionResult AnnulerCommande(int id)
        {
            if (Session["ID"] == null)
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
                    if (Session["UserType"].ToString() == "utilisateur")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        COMMANDE_SERVICE commande = MyDB.COMMANDE_SERVICE.FirstOrDefault(c => c.ID_commande == id);
                        if (commande != null)
                        {
                            if (commande.SERVICE.ARTISAN.ID_artisan == int.Parse(Session["ID"].ToString()))
                            {
                                MyDB.COMMANDE_SERVICE.Remove(commande);
                                MyDB.SaveChanges();
                                return RedirectToAction("CommandeParArtisan");
                            }
                            else
                            {
                                return HttpNotFound();
                            }
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                }
            }
        }
    }
}
