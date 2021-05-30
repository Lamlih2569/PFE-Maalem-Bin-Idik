using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_Fin_Etude___Maalem_Bin_Idik.Models;
using Project_Fin_Etude___Maalem_Bin_Idik.Common_Methodes;
using System.Net;
using System.Net.Mail;

namespace Project_Fin_Etude___Maalem_Bin_Idik.Controllers
{
    public class LoginController : Controller
    {
        MAALEM_BIN_IDIK_DBEntities MyDB = new MAALEM_BIN_IDIK_DBEntities();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Authentification Model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string ModelPassword = PasswordEncryption.GetMD5Hash(Model.Password);
                    var userU = MyDB.UTILISATEUR.FirstOrDefault(m => m.Email_utilisateur == Model.Email && m.Mdp_utilisateur == ModelPassword);
                    var userA = MyDB.ARTISAN.FirstOrDefault(m => m.Email_artisan == Model.Email && m.Mdp_artisan == ModelPassword);
                    // Utilisateur
                    if (userU != null)
                    {
                        Session["ID"] = userU.ID_utilisateur;
                        Session["Nom"] = userU.Nom_utilisateur;
                        Session["Prenom"] = userU.Prenom_utilisateur;
                        Session["DateN"] = userU.DateN_utilisateur;
                        Session["Genre"] = userU.Genre_utilisateur;
                        Session["Tele"] = userU.Tele_utilisateur;
                        Session["Email"] = userU.Email_utilisateur;
                        Session["Erreur"] = null;
                        Session["UserType"] = "utilisateur";
                        if (userU.Utilisateur_Banni == true)
                        {
                            Session["Banni"] = "oui";
                            return RedirectToAction("PageDeBannissement", "Home");
                        }
                        else
                        {
                            if (userU.Email_verifier == true)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                return RedirectToAction("VerifyAccount", "Login");
                            }
                        }
                    }
                    else
                    {
                        // Artisan
                        if (userA != null)
                        {
                            Session["ID"] = userA.ID_artisan;
                            Session["Nom"] = userA.Nom_artisan;
                            Session["Prenom"] = userA.Prenom_artisan;
                            Session["DateN"] = userA.DateN_artisan;
                            Session["Genre"] = userA.Genre_artisan;
                            Session["Tele"] = userA.Tele_artisan;
                            Session["Email"] = userA.Email_artisan;
                            Session["Erreur"] = null;
                            Session["UserType"] = "artisan";
                            if (userA.Artisan_Banni == true)
                            {
                                Session["Banni"] = "oui";
                                return RedirectToAction("PageDeBannissement", "Home");
                            }
                            else
                            {
                                if (userA.Email_verifier == true)
                                {
                                    return RedirectToAction("Index", "Home");
                                }
                                else
                                {
                                    return RedirectToAction("VerifyAccount", "Login");
                                }
                            }                   
                        }
                        else
                        {
                            if(Model.Email == "admin" && Model.Password == "admin")
                            {
                                Session["UserType"] = "admin";
                                return RedirectToAction("Index", "Admin");
                            }
                            else
                            {
                                Session["Erreur"] = "Erreur";
                                return View();
                            }
                        }
                    }
                }
                else
                {
                    Session["Erreur"] = "Erreur";
                    return View();
                }
            }
            catch
            {
                return HttpNotFound();
            }
        }

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Registration Model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var email_verification = MyDB.UTILISATEUR.FirstOrDefault(m => m.Email_utilisateur == Model.Email_utilisateur);
                    var email_verification2 = MyDB.ARTISAN.FirstOrDefault(m => m.Email_artisan == Model.Email_utilisateur);
                    if (email_verification == null && email_verification2 == null)
                    {
                        if (Model.type_utilisateur == "utilisateur")
                        {
                            var User = new UTILISATEUR();
                            User.Nom_utilisateur = Model.Nom_utilisateur;
                            User.Prenom_utilisateur = Model.Prenom_utilisateur;
                            User.DateN_utilisateur = Model.DateN_utilisateur;
                            User.Genre_utilisateur = Model.Genre_utilisateur;
                            User.Tele_utilisateur = Model.Tele_utilisateur;
                            User.Email_utilisateur = Model.Email_utilisateur;
                            User.Mdp_utilisateur = PasswordEncryption.GetMD5Hash(Model.Mdp_utilisateur);
                            User.Email_verifier = false;
                            User.Utilisateur_Banni = false;
                            MyDB.UTILISATEUR.Add(User);
                            MyDB.SaveChanges();
                        }
                        else
                        {
                            var User = new ARTISAN();
                            User.Nom_artisan = Model.Nom_utilisateur;
                            User.Prenom_artisan = Model.Prenom_utilisateur;
                            User.DateN_artisan = Model.DateN_utilisateur;
                            User.Genre_artisan = Model.Genre_utilisateur;
                            User.Tele_artisan = Model.Tele_utilisateur;
                            User.Email_artisan = Model.Email_utilisateur;
                            User.Mdp_artisan = PasswordEncryption.GetMD5Hash(Model.Mdp_utilisateur);
                            User.Email_verifier = false;
                            User.Artisan_Banni = false;
                            MyDB.ARTISAN.Add(User);
                            MyDB.SaveChanges();
                        }
                        Session["Email"] = Model.Email_utilisateur;
                        return RedirectToAction("VerificationCodeSender", "Login");
                    }
                    else
                    {
                        Session["Statu"] = "Exist";
                        return View();
                    }
                }
                else
                {
                    Session["Statu"] = "Erreur";
                    return View();
                }
            }
            catch
            {
                Session["Statu"] = "Erreur";
                return View();
            }
        }

        public ActionResult VerifyAccount()
        {
            if(Session["Email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult VerifyAccount(Verification verification)
        {
            if (verification.Verification_code == int.Parse(Session["VerificationCode"].ToString()))
            {
                string email = Session["Email"].ToString();
                var user = MyDB.UTILISATEUR.FirstOrDefault(m => m.Email_utilisateur == email);
                var user2 = MyDB.ARTISAN.FirstOrDefault(m => m.Email_artisan == email);

                if(user == null)
                {
                    user2.Email_verifier = true;
                    MyDB.SaveChanges();
                }
                if(user2 == null)
                {
                    user.Email_verifier = true;
                    MyDB.SaveChanges();
                }

                MailMessage mail = new MailMessage();
                NetworkCredential credential = new NetworkCredential("contact.maalem.bin.idik@gmail.com", "maalembinidik2569");
                mail.From = new MailAddress("contact.maalem.bin.idik@gmail.com");
                mail.To.Add(new MailAddress(Session["Email"].ToString()));
                mail.Subject = "Verification Effectuée | Bienvenue A Maalem Bin Idik";
                mail.IsBodyHtml = true;
                string body = "<div style='margin:0;padding:0;background-color:#f6f9fc;font-family:sans-serif'><center style='width:100%;table-layout:fixed;background-color:#f6f9fc;padding-bottom:40px;padding-top:40px'><div style='max-width:600px;background-color:#ffffff'><table align='center' style='margin:0 auto;width:100%;max-width:600px;border-spacing:0'><tbody><tr><td style = 'padding:0'><table width='100%' style='border-spacing:0'><tbody><tr><td><img src='https://www.dropbox.com/s/e2h30d446irv3yx/Groupe%202.jpg?raw=1' alt = 'LOGO' style = 'border:0'></td></tr></tbody></table></td></tr><tr><td style = 'padding:0'><table width = '100%' style = 'border-spacing:0;padding:10px;padding-top:50px;padding-bottom:50px;color:#4a4a4a' ><tbody><tr><td style = 'padding:0;background-color:#ffffff;padding:10px'><h3 style = 'text-align:center'>Verification Effectuée | Bienvenue A Maalem Bin Idik</h3></td></tr><tr><td style = 'padding:0'><p> Bonjour,<br>Nous vous remercions de votre inscription. Votre compte est prêt et vous pouvez commencer à l'utilisée.</p></td></tr><tr><td style = 'padding:0'><p style = 'text-align:center'><a href='http://localhost:62290/Login' style='padding:10px 20px;background-color:#FAD107;text-decoration:none;color:black;font-weight: bold;'>Accéder à mon compte</a></p></td></tr><tr><td style='padding:0'><p> Si vous rencontrez des problèmes, veuillez contacter l'assistance à <strong><a href = 'contact.maalem.bin.idik@gmail.com'> contact.maalem.bin.idik@gmail.<wbr>com </a></strong></p></td></tr></tbody></table></td></tr><tr><td style='padding:0;height:30px;background-color:#4a4a4a'></td></tr></tbody></table></div></center></div> ";
                mail.Body = body;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = credential;
                smtpClient.Send(mail);

                return RedirectToAction("Index", "Login");
            }
            else
            {
                Session["CodeStatu"] = "code invalide";
                return View();
            }
        }

        public ActionResult VerificationCodeSender()
        {
            try
            {
                Random random = new Random();
                int verification_code = random.Next(1000000, 9999999);
                Session["VerificationCode"] = verification_code;

                MailMessage mail = new MailMessage();
                NetworkCredential credential = new NetworkCredential("contact.maalem.bin.idik@gmail.com", "maalembinidik2569");
                mail.From = new MailAddress("contact.maalem.bin.idik@gmail.com");
                mail.To.Add(new MailAddress(Session["Email"].ToString()));
                mail.Subject = "Vérification de l'address electronique";
                mail.IsBodyHtml = true;
                string body = "<div style='margin:0;padding:0;background-color:#f6f9fc;font-family:sans-serif'><center style='width:100%;table-layout:fixed;background-color:#f6f9fc;padding-bottom:40px;padding-top:40px'><div style='max-width:600px;background-color:#ffffff'><table align='center' style='margin:0 auto;width:100%;max-width:600px;border-spacing:0'><tbody><tr><td style = 'padding:0'><table width='100%' style='border-spacing:0'><tbody><tr><td><img src='https://www.dropbox.com/s/e2h30d446irv3yx/Groupe%202.jpg?raw=1' alt = 'LOGO' style = 'border:0'></td></tr></tbody></table></td></tr><tr><td style = 'padding:0'><table width = '100%' style = 'border-spacing:0;padding:10px;padding-top:50px;padding-bottom:50px;color:#4a4a4a' ><tbody><tr><td style = 'padding:0;background-color:#ffffff;padding:10px'><h3 style = 'text-align:center'> Vérification de l'adresse electronique</h3></td></tr><tr><td style = 'padding:0'><p> Bonjour,<br>Veuillez vérifier votre adresse electronique à l'aide du code ci-dessous pour terminer la configuration du compte. </p></td></tr><tr><td style = 'padding:0'><p style = 'text-align:center'><span style = 'padding:10px 20px;background-color:#dddddd'> " + verification_code + " </span></p></td></tr><tr><td style='padding:0'><p> Si vous rencontrez des problèmes avec la vérification de l'adresse electronique ou la création d'un compte, veuillez contacter l'assistance à <strong><a href = 'contact.maalem.bin.idik@gmail.com'> contact.maalem.bin.idik@gmail.<wbr>com </a></strong></p></td></tr></tbody></table></td></tr><tr><td style='padding:0;height:30px;background-color:#4a4a4a'></td></tr></tbody></table></div></center></div> ";
                mail.Body = body;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = credential;
                smtpClient.Send(mail);

                return RedirectToAction("VerifyAccount", "Login");
            }
            catch
            {
                return HttpNotFound();
            }
        }

        public ActionResult LogOut()
        {
            try
            {
                Session.Abandon();
                return RedirectToAction("Index", "Login");
            }
            catch
            {
                return HttpNotFound();
            }
        }
    }

}
