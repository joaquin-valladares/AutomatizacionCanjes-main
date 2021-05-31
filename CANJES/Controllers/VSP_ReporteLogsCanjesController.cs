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
    public class VSP_ReporteLogsCanjesController : Controller
    {
        private AUTCANJESEntities db = new AUTCANJESEntities();

        // GET: VSP_ReporteLogsCanjes
        //public ActionResult Index()
        //{
        //    return View(db.VSP_ReporteLogsCanjes.ToList());
        //}

        public ActionResult Index(string id_sociedad2/*, string id_centro*/, DateTime ? Fecha_inicial, DateTime ? Fecha_final)
        {
            List<ClsReporteLogs> lst = (from f in db.VSP_ReporteLogsCanjes
                                               where f.Sociedad == id_sociedad2
                                               //&& f.CENTRO == id_centro
                                               && f.Fecha >= Fecha_inicial && f.Fecha <= Fecha_final
                                               select new ClsReporteLogs
                                               {
                                                   COD_ProdLAB = f.Cod_Prod_SAP.ToString(),
                                                   Unidad = f.UNIDAD,
                                                   observacion = f.Observaciones,
                                                   PRODUCTO = f.Nombre_Segun_Proveedor,
                                                   Cant_sol = (int)f.Cantidad_Solicitada,
                                                   Estado = f.Estado,
                                                   nombreCliente = f.Nombre_Cliente,
                                                   ID_Sociedad= f.Sociedad,
                                                   //ID_Centro= f.CENTRO
                                               }).ToList();
            return View(lst);
        }

        [HttpPost]
        public ActionResult filtrarLista(string id_sociedad2/*, string id_centro*/, DateTime ? Fecha_inicial, DateTime ? Fecha_final)
        {
            if (!string.IsNullOrEmpty(id_sociedad2))
            {
                List<ClsReporteLogs> lst = (from f in db.VSP_ReporteLogsCanjes
                                            where f.Sociedad == id_sociedad2
                                            //&& f.CENTRO == id_centro
                                            && f.Fecha >= Fecha_inicial && f.Fecha <= Fecha_final
                                            select new ClsReporteLogs
                                            {
                                                COD_ProdLAB = f.Cod_Prod_SAP.ToString(),
                                                Unidad = f.UNIDAD,
                                                observacion = f.Observaciones,
                                                PRODUCTO = f.Nombre_Segun_Proveedor,
                                                NombreSAP = f.Nombre_SAP,
                                                Cant_sol = (int)f.Cantidad_Solicitada,
                                                Estado = f.Estado,
                                                nombreCliente = f.Nombre_Cliente,
                                                ID_Sociedad = f.Sociedad,
                                                //ID_Centro = f.CENTRO
                                            }).ToList();
                return View(lst);
            }
            else
            {
                List<ClsReporteLogs> lst = (from f in db.VSP_ReporteLogsCanjes
                                            where f.Sociedad == id_sociedad2
                                            //&& f.CENTRO == id_centro
                                            && f.Fecha >= Fecha_inicial && f.Fecha <= Fecha_final
                                            select new ClsReporteLogs
                                            {
                                                COD_ProdLAB = f.Cod_Prod_SAP.ToString(),
                                                Unidad = f.UNIDAD,
                                                observacion = f.Observaciones,
                                                PRODUCTO = f.Nombre_Segun_Proveedor,
                                                NombreSAP = f.Nombre_SAP,
                                                Cant_sol = (int)f.Cantidad_Solicitada,
                                                Estado = f.Estado,
                                                nombreCliente = f.Nombre_Cliente,
                                                ID_Sociedad = f.Sociedad,
                                                //ID_Centro= f.CENTRO
                                            }).ToList();
                return View(lst);
            }
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

        public JsonResult GetGRUPOPROD()
        {
            //string user = Session["Usuario"] as string;
            var Lst3 = db.VSP_MATERIALES_CASAS.SqlQuery("select * from VSP_MATERIALES_CASAS").ToList();

            var Lista3 = Lst3.Select(x => new SelectListItem
            {
                //Value = x.MATNR.ToString(),
                Value = x.MATKL,
                Text = x.MAKTX
            }).ToList();
            return Json(Lista3, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPaises()
        {
            List<SelectListItem> Lst4 = (from p in db.VSP_MATERIALES_CASAS
                                         select new SelectListItem
                                         { Value = p.MATNR.ToString(), Text = p.MAKTX }).ToList();
            return Json(Lst4, JsonRequestBehavior.AllowGet);
        }
    }
}
