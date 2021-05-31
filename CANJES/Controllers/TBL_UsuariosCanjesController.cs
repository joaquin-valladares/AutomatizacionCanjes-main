using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CANJES.Models;

namespace CANJES.Controllers
{
    public class TBL_UsuariosCanjesController : Controller
    {
        private AUTCANJESEntities db = new AUTCANJESEntities();
        public bool Permisos()
        {
            string user = Session["Usuario"] as string;

            var Lst = db.TBL_UsuariosCanjes.SqlQuery("select distinct * from TBL_UsuariosCanjes where nombreUsuario = {0}", user).ToList();

            int? rol_id = Lst[0].id_rol;

            var Lst2 = db.TBL_UsuariosCanjes_Roles.SqlQuery("select * from TBL_UsuariosCanjes_Roles where id_rol = {0}", rol_id).ToList();

            if (Lst2[0].rol == "Administrador")
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        // GET: TBL_UsuariosSolicitantes
        [Authorize]
        public ActionResult Index()
        {
            if (Permisos() == true)
            {

                List<ClsUsuariosDashboard> lst = (from t in db.VSP_DatosUsuarios
                                                  orderby t.id
                                                  select new ClsUsuariosDashboard
                                                  {
                                                      id = t.id,
                                                      nombre = t.nombreUsuario,
                                                      email = t.email,
                                                      pass = t.pass,
                                                      sociedades = t.nombre_sociedad,
                                                      id_sociedad = (int)t.id_sociedad,
                                                      centros = t.nombre_centro,
                                                      id_centro = (int)t.id_centro,
                                                      estado = t.estado,
                                                      id_estado = (int)t.id_estado,
                                                      rol = t.rol,
                                                      id_rol = (int)t.id_rol,
                                                  }).ToList();
                return View(lst);
            }
            else
            {
                return RedirectToAction("AccesoDenegado", "Home");
            }
        }


        // GET: TBL_UsuariosCanjes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_UsuariosCanjes tBL_UsuariosCanjes = db.TBL_UsuariosCanjes.Find(id);
            if (tBL_UsuariosCanjes == null)
            {
                return HttpNotFound();
            }
            return View(tBL_UsuariosCanjes);
        }

        // GET: TBL_UsuariosCanjes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TBL_UsuariosCanjes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombreUsuario,pass,email,id_sociedad,id_centro,id_estado,id_rol")] TBL_UsuariosCanjes tBL_UsuariosCanjes)
        {
            if (ModelState.IsValid)
            {
                db.TBL_UsuariosCanjes.Add(tBL_UsuariosCanjes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tBL_UsuariosCanjes);
        }

        // GET: TBL_UsuariosCanjes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_UsuariosCanjes tBL_UsuariosCanjes = db.TBL_UsuariosCanjes.Find(id);
            if (tBL_UsuariosCanjes == null)
            {
                return HttpNotFound();
            }
            return View(tBL_UsuariosCanjes);
        }

        // POST: TBL_UsuariosCanjes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombreUsuario,pass,email,id_sociedad,id_centro,id_estado,id_rol")] TBL_UsuariosCanjes tBL_UsuariosCanjes, string id_sociedad2, string id_estado2, string id_rol2, string id_centro2)
        {
            if (ModelState.IsValid)
            {
                short estado = Convert.ToInt16(id_estado2);
                short rol = Convert.ToInt16(id_rol2);
                short sociedad = Convert.ToInt16(id_sociedad2);
                tBL_UsuariosCanjes.id_sociedad = sociedad;
                tBL_UsuariosCanjes.id_estado = estado;
                tBL_UsuariosCanjes.id_rol = rol;
                db.Entry(tBL_UsuariosCanjes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tBL_UsuariosCanjes);
        }

        // GET: TBL_UsuariosCanjes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_UsuariosCanjes tBL_UsuariosCanjes = db.TBL_UsuariosCanjes.Find(id);
            if (tBL_UsuariosCanjes == null)
            {
                return HttpNotFound();
            }
            return View(tBL_UsuariosCanjes);
        }

        // POST: TBL_UsuariosCanjes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TBL_UsuariosCanjes tBL_UsuariosCanjes = db.TBL_UsuariosCanjes.Find(id);
            db.TBL_UsuariosCanjes.Remove(tBL_UsuariosCanjes);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        // ******************************************** Funciones ********************************************
        ////traer info de sociedades
        public JsonResult GetSociedades()
        {
            string user = Session["Usuario"] as string;
            var Lst1 = db.TBL_Sociedades.SqlQuery("select distinct * from TBL_Sociedades").ToList();

            var Lista1 = Lst1.Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.nombre_sociedad
            }).ToList();
            return Json(Lista1, JsonRequestBehavior.AllowGet);
        }

        ////traer info de centros
        public JsonResult GetCentros(int sociedad_id)
        {
            var Lst2 = db.TBL_Centros.SqlQuery("select distinct cn.* from TBL_Centros CN, TBL_Sociedades S where cn.id_sociedad = {0}", sociedad_id).ToList();
            var Lista2 = Lst2.Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.nombre_centro
            }).ToList();
            return Json(Lista2, JsonRequestBehavior.AllowGet);
        }

        ////traer info de estado
        public JsonResult GetEstado()
        {
            var Lst3 = db.TBL_UsuariosCanjes_Estado.SqlQuery("select distinct * from TBL_UsuariosCanjes_Estado").ToList();
            var Lista3 = Lst3.Select(x => new SelectListItem
            {
                Value = x.id_estado.ToString(),
                Text = x.estado
            }).ToList();
            return Json(Lista3, JsonRequestBehavior.AllowGet);
        }

        ////traer info de roles
        public JsonResult GetRoles()
        {
            var Lst4 = db.TBL_UsuariosCanjes_Roles.SqlQuery("select distinct * from TBL_UsuariosCanjes_Roles").ToList();
            var Lista4 = Lst4.Select(x => new SelectListItem
            {
                Value = x.id_rol.ToString(),
                Text = x.rol
            }).ToList();
            return Json(Lista4, JsonRequestBehavior.AllowGet);
        }
    }
}
