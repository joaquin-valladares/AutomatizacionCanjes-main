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
    public class TBL_MaestrosCanjesController : Controller
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

        // GET: TBL_MaestrosCanjes
        //*********** Inicio de ActionResult de mantenimiento

        public ActionResult MaestrodeCanjes(string CodCasa) 
        {
            try 
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

                //string param = Session["MC_Create"] as string;
                //if (param != null) 
                //{
                //    CodCasa = param;
                //    Session["MC_Create"] = "";
                //}

                List<SelectListItem> filtro = (from t in db.VSP_CASA_SAP.OrderBy(x => x.CASA)
                                            select new SelectListItem { Value = t.COD_CASA, Text = t.CASA }).ToList();

                ViewBag.CodCasa = filtro;

                //if (CodCasa != null) 
                //{
                //     Session["MC_Create"] = CodCasa;
                //     return View(db.VSP_Casa_ClientePrv_Sociedad.OrderBy(y => y.Nombre_según_Proveedor).Where(x => x.CodCasa == CodCasa).ToList());
                //}
                return View(db.VSP_Casa_ClientePrv_Sociedad.OrderBy(y => y.Nombre_según_Proveedor).Where(x => x.CodCasa == CodCasa).ToList());
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            List<VSP_Casa_ClientePrv_Sociedad> lst = new List<VSP_Casa_ClientePrv_Sociedad>();
            lst.Add(new VSP_Casa_ClientePrv_Sociedad { CodCasa="", Casa = "", CodProvSAP = "", Nombre_según_Proveedor = "", CodClienteSAP = "", CodSoc = "", Sociedad = "" });
            return View(lst);
        }

        public ActionResult Create1_TBL_MaestrosCanjes(string Cod_Casa_SAP, string Cod_Cliente_SAP, string Sociedad) 
        {
            List<TBL_MaterialesSAP> lst = new List<TBL_MaterialesSAP>();
            try 
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

                string msj1 = Session["MC_Create2"] as string;
                string msj2 = Session["MC_Create3"] as string;

                if (msj1 == "Y")
                {
                    ViewBag.MC_Create1 = "Y";
                    Session["MC_Create2"] = "";
                }
                if (msj2 == "Y")
                {
                    ViewBag.MC_Create2 = "Y";
                    Session["MC_Create3"] = "";
                }

                if (Cod_Casa_SAP !=null && Cod_Casa_SAP != "") 
                {
                    int PSociedad = Convert.ToInt32(Sociedad);
                    var casa = (from t in db.VSP_CASA_SAP
                                where t.COD_CASA == Cod_Casa_SAP
                                select t).ToList();
                    var proveedor = (from t in db.VSP_PROVEEDORES_SAP
                                     where t.KUNNR == Cod_Cliente_SAP
                                     select t).ToList();
                    var sociedad = (from t in db.TBL_Sociedades
                                    where t.id == PSociedad
                                    select t).ToList();

                    ViewBag.Casa = casa[0].CASA;
                    ViewBag.Proveedor = proveedor[0].NAME1;
                    ViewBag.Sociedad = sociedad[0].nombre_sociedad;

                    ViewBag.Cod_Casa_SAP = Cod_Casa_SAP;
                    ViewBag.Cod_Cliente_SAP = Cod_Cliente_SAP;
                    ViewBag.Id = Sociedad;

                    lst = db.TBL_MaterialesSAP.SqlQuery("select * from TBL_MaterialesSAP where MATNR COLLATE SQL_Latin1_General_CP850_BIN2 not in(select Cod_Prod_SAP from [dbo].[TBL_MaestrosCanjes] " +
                        "where Cod_Casa_SAP='" + Cod_Casa_SAP + "' and Cod_Cliente_SAP='" + Cod_Cliente_SAP + "' and Sociedad='" + Sociedad + "') and MATKL='" + Cod_Casa_SAP + "' and MATLAB <> ''").ToList();
                    return View(lst);
                }
                
            }
            catch(Exception ex) 
            {
                Console.Write(ex.Message);
            }
            return RedirectToAction("MaestrodeCanjes");
        }
        public ActionResult Create2_TBL_MaestrosCanjes(string Cod_Casa_SAP, string Cod_Cliente_SAP, string Id, string MATNR, string MATLAB)
        {
            try
            {
                var prov = (from t in db.VSP_PROVEEDORES_SAP where t.KUNNR == Cod_Cliente_SAP
                            select t).ToList();

                db.sp_TBL_MaestrosCanjes_create(Cod_Casa_SAP, Cod_Cliente_SAP, Id, MATNR, MATLAB, prov[0].NAME1);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            Session["MC_Create2"] = "Y";
            return RedirectToAction("Create1_TBL_MaestrosCanjes", "TBL_MaestrosCanjes", new { Cod_Casa_SAP = Cod_Casa_SAP, Cod_Cliente_SAP = Cod_Cliente_SAP, Sociedad = Id });
        }
        public ActionResult Create3_TBL_MaestrosCanjes(string Cod_Casa_SAP, string Cod_Cliente_SAP, string Id) 
        {
            List<TBL_MaterialesSAP> lst = new List<TBL_MaterialesSAP>();
            try 
            {
                var prov = (from t in db.VSP_PROVEEDORES_SAP
                            where t.KUNNR == Cod_Cliente_SAP
                            select t).ToList();

                lst = db.TBL_MaterialesSAP.SqlQuery("select * from TBL_MaterialesSAP where MATNR COLLATE SQL_Latin1_General_CP850_BIN2 not in(select Cod_Prod_SAP from [dbo].[TBL_MaestrosCanjes] " +
                       "where Cod_Casa_SAP='" + Cod_Casa_SAP + "' and Cod_Cliente_SAP='" + Cod_Cliente_SAP + "' and Sociedad='" + Id + "') and MATKL='" + Cod_Casa_SAP + "' and MATLAB <> ''").ToList();
                
                if (lst.Count > 0) 
                {
                    foreach (var item in lst)
                    {
                        db.sp_TBL_MaestrosCanjes_create(Cod_Casa_SAP, Cod_Cliente_SAP, Id, item.MATNR, item.MATLAB, prov[0].NAME1);
                    }
                    Session["MC_Create3"] = "Y";
                }
            }
            catch(Exception ex) 
            {
                Console.Write(ex.Message);
            }
            return RedirectToAction("Create1_TBL_MaestrosCanjes", "TBL_MaestrosCanjes", new { Cod_Casa_SAP = Cod_Casa_SAP, Cod_Cliente_SAP = Cod_Cliente_SAP, Sociedad = Id });
        }
        
        //*********** Fin de ActionResult de mantenimiento

        public ActionResult Index(string Cod_Casa_SAP, string Cod_Cliente_SAP, string Sociedad)
        {
            try
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

                if (Cod_Casa_SAP !=null && Cod_Casa_SAP != "") 
                {
                    int PSociedad = Convert.ToInt32(Sociedad);
                    var casa = (from t in db.VSP_CASA_SAP
                                where t.COD_CASA == Cod_Casa_SAP
                                select t).ToList();
                    var proveedor = (from t in db.VSP_PROVEEDORES_SAP
                                     where t.KUNNR == Cod_Cliente_SAP
                                     select t).ToList();
                    var sociedad = (from t in db.TBL_Sociedades
                                    where t.id == PSociedad
                                    select t).ToList();

                        ViewBag.Casa = casa[0].CASA;
                        ViewBag.Proveedor = proveedor[0].NAME1;
                        ViewBag.Sociedad = sociedad[0].nombre_sociedad;

                        ViewBag.Cod_Casa_SAP = Cod_Casa_SAP;
                        ViewBag.Cod_Cliente_SAP = Cod_Cliente_SAP;
                        ViewBag.Id = Sociedad;

                    var lista = (from x in db.TBL_MaestrosCanjes
                                 where x.Cod_Casa_SAP == Cod_Casa_SAP && x.Cod_Cliente_SAP == Cod_Cliente_SAP && x.Sociedad == Sociedad
                                 select x).ToList();

                    IEnumerable<TBL_MaestrosCanjes> TBL_MaestrosCanjes = (from t in lista
                                                                          select new TBL_MaestrosCanjes
                                                                          {
                                                                              Cod_Casa_SAP = t.Cod_Casa_SAP,
                                                                              Cod_Prov_SAP = t.Cod_Prov_SAP,
                                                                              Cod_Cliente_SAP = t.Cod_Cliente_SAP,
                                                                              Sociedad = t.Sociedad,
                                                                              Cod_Prod_SAP = t.Cod_Prod_SAP,
                                                                              Nombre_SAP = t.Nombre_SAP,
                                                                              Nombre_Segun_Proveedor = t.Nombre_Segun_Proveedor
                                                                          });

                    return View(TBL_MaestrosCanjes);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return RedirectToAction("MaestrodeCanjes");
        }

        // GET: TBL_MaestrosCanjes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_MaestrosCanjes tBL_MaestrosCanjes = db.TBL_MaestrosCanjes.Find(id);
            if (tBL_MaestrosCanjes == null)
            {
                return HttpNotFound();
            }
            return View(tBL_MaestrosCanjes);
        }

        // GET: TBL_MaestrosCanjes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TBL_MaestrosCanjes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cod_Casa_SAP,Cod_Prod_SAP,Nombre_SAP,Nombre_Segun_Proveedor")] TBL_MaestrosCanjes tBL_MaestrosCanjes)
        {
            if (ModelState.IsValid)
            {
                db.TBL_MaestrosCanjes.Add(tBL_MaestrosCanjes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tBL_MaestrosCanjes);
        }

        // GET: TBL_MaestrosCanjes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_MaestrosCanjes tBL_MaestrosCanjes = db.TBL_MaestrosCanjes.Find(id);
            if (tBL_MaestrosCanjes == null)
            {
                return HttpNotFound();
            }
            return View(tBL_MaestrosCanjes);
        }

        // POST: TBL_MaestrosCanjes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cod_Casa_SAP,Cod_Prod_SAP,Nombre_SAP,Nombre_Segun_Proveedor")] TBL_MaestrosCanjes tBL_MaestrosCanjes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tBL_MaestrosCanjes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tBL_MaestrosCanjes);
        }

        // GET: TBL_MaestrosCanjes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_MaestrosCanjes tBL_MaestrosCanjes = db.TBL_MaestrosCanjes.Find(id);
            if (tBL_MaestrosCanjes == null)
            {
                return HttpNotFound();
            }
            return View(tBL_MaestrosCanjes);
        }
        public ActionResult DeleteConfirmed(string Cod_Casa_SAP, string Cod_Prov_SAP, string Cod_Cliente_SAP, string Sociedad, string MATNR)
        {
            TBL_MaestrosCanjes tBL_MaestrosCanjes = db.TBL_MaestrosCanjes.Find(Cod_Casa_SAP, Cod_Prov_SAP, Cod_Cliente_SAP, Sociedad, MATNR);
            db.TBL_MaestrosCanjes.Remove(tBL_MaestrosCanjes);
            db.SaveChanges();
            return RedirectToAction("Index","TBL_MaestrosCanjes", new { Cod_Casa_SAP = Cod_Casa_SAP, Cod_Cliente_SAP = Cod_Cliente_SAP, Sociedad = Sociedad });
        }

        public ActionResult EliminarMasivo(string Cod_Casa_SAP, string Cod_Cliente_SAP, string Sociedad)
        {
            db.sp_TBL_MaestrosCanjes_EliminarMasivo(Cod_Casa_SAP, Cod_Cliente_SAP, Sociedad);
            return RedirectToAction("Index", "TBL_MaestrosCanjes", new { Cod_Casa_SAP = Cod_Casa_SAP, Cod_Cliente_SAP = Cod_Cliente_SAP, Sociedad = Sociedad });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
       
        //********************** Funciones Extras **********************
        private class Cl_Interna
        {
            private string CodCasa { get; set; }
            private string Casa { get; set; }
            private string CodProvSAP { get; set; }
            private string Nombre_Segun_Proveedor {get;set;}
            private string CodSoc { get; set; }
            private string Sociedad { get; set; }
        }
        //********************** Fin Funciones Extras **********************
    }
}
