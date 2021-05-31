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
    public class TBL_Casa_Proveedor_SAPController : Controller
    {
        private AUTCANJESEntities db = new AUTCANJESEntities();

        public bool Permisos()
        {
            string user = Session["Usuario"] as string;

            var Lst = db.TBL_UsuariosCanjes.SqlQuery("select distinct * from TBL_UsuariosCanjes where nombreUsuario = {0}", user).ToList();
            if (Lst.Count == 0)
            {
                return false;
            }
            int? rol_id = Lst[0].id_rol;

            var Lst2 = db.TBL_UsuariosCanjes_Roles.SqlQuery("select * from TBL_UsuariosCanjes_Roles where id_rol = {0}", rol_id).ToList();
            if (Lst2.Count == 0)
            {
                return false;
            }
            if (Lst2[0].rol == "Administrador")
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // GET: TBL_Casa_Proveedor_SAP
        public ActionResult Index()
        {
            if (Permisos() == true)
            {
                goto y;
            }
            else
            {
                return RedirectToAction("AccesoDenegado", "Home");
            }

            y:
            return View(db.VSP_Casa_ClientePrv_Sociedad.ToList());
        }

        // GET: TBL_Casa_Proveedor_SAP/Details/5
        public ActionResult Details(string codcasa, string codprov, string codcliente, string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Casa_Proveedor_SAP tBL_Casa_Proveedor_SAP = db.TBL_Casa_Proveedor_SAP.Find(codcasa, codprov, codcliente, id);
            if (tBL_Casa_Proveedor_SAP == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Casa_Proveedor_SAP);
        }

        // GET: TBL_Casa_Proveedor_SAP/Create
        public ActionResult Create()
        {
            if (Permisos() == true)
            {
                goto y;
            }
            else
            {
                return RedirectToAction("AccesoDenegado", "Home");
            }

            y:
            List<SelectListItem> lst1 = (from t in db.VSP_CASA_SAP.OrderBy(x => x.CASA)
                                         select new SelectListItem { Value = t.COD_CASA, Text = t.CASA }).ToList();
            List<SelectListItem> lst2 = (from t in db.VSP_PROVEEDORES_SAP.OrderBy(x => x.NAME1)
                                         where t.MANDT == "300"
                                         select new SelectListItem { Value = t.KUNNR, Text = t.KUNNR + "-" + t.NAME1 }).ToList();
            List<SelectListItem> lst3 = (from t in db.TBL_Sociedades.OrderBy(x => x.nombre_sociedad)
                                         select new SelectListItem { Value = t.id.ToString(), Text = t.nombre_sociedad }).ToList();

            ViewBag.Cod_Casa_SAP = lst1;
            ViewBag.Cod_Cliente_SAP = lst2;
            ViewBag.Sociedad = lst3;
            return View();
        }

        // POST: TBL_Casa_Proveedor_SAP/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cod_Casa_SAP,Cod_Prov_SAP,Cod_Cliente_SAP,Sociedad")] TBL_Casa_Proveedor_SAP tBL_Casa_Proveedor_SAP)
        {
            if (ModelState.IsValid)
            {
                var lst = (from t in db.TBL_Casa_Proveedor_SAP
                           where t.Cod_Casa_SAP == tBL_Casa_Proveedor_SAP.Cod_Casa_SAP &&
                           t.Cod_Cliente_SAP == tBL_Casa_Proveedor_SAP.Cod_Cliente_SAP && t.Sociedad == tBL_Casa_Proveedor_SAP.Sociedad
                           select t).ToList();

                if (lst.Count == 1)
                {
                    ViewBag.MsjCreate = "Ya Existe";
                    List<SelectListItem> lst1 = (from t in db.VSP_CASA_SAP.OrderBy(x => x.CASA)
                                                 select new SelectListItem { Value = t.COD_CASA, Text = t.CASA }).ToList();
                    List<SelectListItem> lst2 = (from t in db.VSP_PROVEEDORES_SAP.OrderBy(x => x.NAME1)
                                                 where t.MANDT == "300"
                                                 select new SelectListItem { Value = t.KUNNR, Text = t.KUNNR + "-" + t.NAME1 }).ToList();
                    List<SelectListItem> lst3 = (from t in db.TBL_Sociedades.OrderBy(x => x.nombre_sociedad)
                                                 select new SelectListItem { Value = t.id.ToString(), Text = t.nombre_sociedad }).ToList();

                    ViewBag.Cod_Casa_SAP = lst1;
                    ViewBag.Cod_Cliente_SAP = lst2;
                    ViewBag.Sociedad = lst3;
                }
                else
                {
                    var MyQuery = db.VSP_Casa_Proveedor_SAP.SqlQuery("select * from VSP_Casa_Proveedor_SAP where KUNNR={0} and BUKRS={1}", tBL_Casa_Proveedor_SAP.Cod_Cliente_SAP, tBL_Casa_Proveedor_SAP.Sociedad).ToList();

                    if (MyQuery.Count > 0)
                    {
                        tBL_Casa_Proveedor_SAP.Cod_Prov_SAP = MyQuery[0].LIFNR;
                        db.TBL_Casa_Proveedor_SAP.Add(tBL_Casa_Proveedor_SAP);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.MsjError = "Error";
                        List<SelectListItem> lst1 = (from t in db.VSP_CASA_SAP.OrderBy(x => x.CASA)
                                                     select new SelectListItem { Value = t.COD_CASA, Text = t.CASA }).ToList();
                        List<SelectListItem> lst2 = (from t in db.VSP_PROVEEDORES_SAP.OrderBy(x => x.NAME1)
                                                     where t.MANDT == "300"
                                                     select new SelectListItem { Value = t.KUNNR, Text = t.KUNNR + "-" + t.NAME1 }).ToList();
                        List<SelectListItem> lst3 = (from t in db.TBL_Sociedades.OrderBy(x => x.nombre_sociedad)
                                                     select new SelectListItem { Value = t.id.ToString(), Text = t.nombre_sociedad }).ToList();

                        ViewBag.Cod_Casa_SAP = lst1;
                        ViewBag.Cod_Cliente_SAP = lst2;
                        ViewBag.Sociedad = lst3;
                        return View(tBL_Casa_Proveedor_SAP);
                    }
                }
            }

            return View(tBL_Casa_Proveedor_SAP);
        }

        // GET: TBL_Casa_Proveedor_SAP/Edit/5
        public ActionResult Edit(string codcasa, string codprov, string id)
        {
            if (Permisos() == true)
            {
                goto y;
            }
            else
            {
                return RedirectToAction("AccesoDenegado", "Home");
            }

            y:
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Casa_Proveedor_SAP tBL_Casa_Proveedor_SAP = db.TBL_Casa_Proveedor_SAP.Find(codcasa, codprov, id);

            List<SelectListItem> lst1 = (from t in db.TBL_Sociedades.OrderBy(x => x.nombre_sociedad)
                                         select new SelectListItem { Value = t.id.ToString(), Text = t.nombre_sociedad }).ToList();

            ViewBag.Sociedades = lst1;

            if (tBL_Casa_Proveedor_SAP == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Casa_Proveedor_SAP);
        }

        // POST: TBL_Casa_Proveedor_SAP/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cod_Casa_SAP,Cod_Prov_SAP,Cod_Cliente_SAP,Sociedad")] TBL_Casa_Proveedor_SAP tBL_Casa_Proveedor_SAP)
        {
            if (ModelState.IsValid)
            {
                var lst = (from t in db.TBL_Casa_Proveedor_SAP
                           where t.Cod_Casa_SAP == tBL_Casa_Proveedor_SAP.Cod_Casa_SAP &&
                           t.Cod_Cliente_SAP == tBL_Casa_Proveedor_SAP.Cod_Cliente_SAP && t.Sociedad == tBL_Casa_Proveedor_SAP.Sociedad
                           select t).ToList();

                if (lst.Count == 1)
                {
                    ViewBag.MsjUpdate = "Ya Existe";
                    List<SelectListItem> lst1 = (from t in db.TBL_Sociedades.OrderBy(x => x.nombre_sociedad)
                                                 select new SelectListItem { Value = t.id.ToString(), Text = t.nombre_sociedad }).ToList();
                    ViewBag.Sociedades = lst1;
                }
                else
                {
                    db.Entry(tBL_Casa_Proveedor_SAP).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(tBL_Casa_Proveedor_SAP);
        }

        // GET: TBL_Casa_Proveedor_SAP/Delete/5
        public ActionResult Delete(string codcasa, string codprov, string codcliente, string id)
        {
            if (Permisos() == true)
            {
                goto y;
            }
            else
            {
                return RedirectToAction("AccesoDenegado", "Home");
            }

            y:
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_Casa_Proveedor_SAP tBL_Casa_Proveedor_SAP = db.TBL_Casa_Proveedor_SAP.Find(codcasa, codprov, codcliente, id);
            if (tBL_Casa_Proveedor_SAP == null)
            {
                return HttpNotFound();
            }
            return View(tBL_Casa_Proveedor_SAP);
        }

        // POST: TBL_Casa_Proveedor_SAP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string Cod_Casa_SAP, string Cod_Prov_SAP, string Cod_Cliente_SAP, string Sociedad)
        {
            TBL_Casa_Proveedor_SAP tBL_Casa_Proveedor_SAP = db.TBL_Casa_Proveedor_SAP.Find(Cod_Casa_SAP, Cod_Prov_SAP, Cod_Cliente_SAP, Sociedad);
            db.TBL_Casa_Proveedor_SAP.Remove(tBL_Casa_Proveedor_SAP);
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

        //Funciones Extras

        //FIN Funciones Extras
    }
}
