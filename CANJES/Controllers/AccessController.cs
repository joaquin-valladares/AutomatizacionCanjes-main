using CANJES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
//using SweetAlert;


namespace CANJES.Controllers
{
    public class AccessController : Controller
    {
        //cambiar la direccion cuando este montado en el servidor
        //string urlDomain = "http://172.16.2.127:8777/";
        string urlDomain = "http://172.16.2.127:8777/";
        // GET: Access
        public ActionResult Index()
        {
            return View();
        }


        //cuando le da a olvidó su contraseña, entra aqui
        [HttpGet]
        public ActionResult StartRecovery()
        {
            Models.ViewModels.ClsRecoveryViewModel model = new Models.ViewModels.ClsRecoveryViewModel();
            return View(model);
        }


        [HttpPost]
        public ActionResult StartRecovery(Models.ViewModels.ClsRecoveryViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                string token = GetSha256(Guid.NewGuid().ToString());


                //para buscar exactamente en la bd don de esté el correo creado:
                AUTCANJESEntities db = new AUTCANJESEntities();
                var ouser = db.TBL_UsuariosCanjes.Where(d => d.email == model.email).FirstOrDefault();
                if (ouser != null)
                {
                    ouser.token_recovery = token;
                    db.Entry(ouser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    //envio del correo
                    SendEmail(ouser.email, token);
                    ViewBag.Success = "Porfavor revisa tu correo donde encontraras el enlace que te permitirá cambiar tu contraseña de manera segura";
                }

                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //cuando le envian el correo con el token entra aqui
        [HttpGet]
        public ActionResult Recovery(string token)
        {
            Models.ViewModels.ClsRecoveryPassViewModel model = new Models.ViewModels.ClsRecoveryPassViewModel();
            model.token = token;
            AUTCANJESEntities db = new AUTCANJESEntities();
            if (model.token == null || model.token.Trim().Equals(""))
            {
                return View("Index", "Home");
            }
            var ouser = db.TBL_UsuariosCanjes.Where(d => d.token_recovery == model.token).FirstOrDefault();
            if (ouser == null)
            {
                ViewBag.Error = "El token ha expirado";
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult Recovery(Models.ViewModels.ClsRecoveryPassViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                AUTCANJESEntities db = new AUTCANJESEntities();
                var ouser = db.TBL_UsuariosCanjes.Where(d => d.token_recovery == model.token).FirstOrDefault();
                if (ouser != null)
                {
                    ouser.pass = model.password;
                    ouser.token_recovery = null;
                    db.Entry(ouser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            ViewBag.Success2 = "Contraseña modificada exitosamente";
            return RedirectToAction("Index", "Home");
        }



        #region HELPERS
        private string GetSha256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++)
            {
                sb.AppendFormat("{0:x2}", stream[i]);
            }
            return sb.ToString();
        }

        //para la funcion que envia el correo, se necesita un correo de origen para enviar
        private void SendEmail(string emailDestino, string token)
        {
            string emailOrigen = "procesosPapeleria2@gmail.com";
            string Contrasenia = "GrupoFarinter2021";
            string url = urlDomain + "/Access/Recovery/?token=" + token;
            MailMessage oMailMessage = new MailMessage(emailOrigen, emailDestino, "Recuperación de contraseña", "<p>Has solicitado un cambio de contraseña en la plataforma de canjes de Grupo Farinter<p><br>" +
               "<a href='" + url + "'>Click aquí para recuperar</a>");
            oMailMessage.IsBodyHtml = true;
            SmtpClient oSmtpClient = new SmtpClient("smtp.gmail.com");
            oSmtpClient.EnableSsl = true;
            oSmtpClient.UseDefaultCredentials = false;
            oSmtpClient.Port = 587;
            oSmtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, Contrasenia);
            oSmtpClient.Send(oMailMessage);

        }


        #endregion
    }
}