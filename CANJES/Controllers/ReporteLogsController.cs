using CANJES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CANJES.Controllers
{
    public class ReporteLogsController : Controller
    {
        private AUTCANJESEntities db = new AUTCANJESEntities();

        // GET: ReporteLogs
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult ReporteLogs()
        {
            return View();
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
            var Lst3 = db.VSP_MATERIALES_CASAS.SqlQuery("select* from VSP_MATERIALES_CASAS").ToList();

            var Lista3 = Lst3.Select(x => new SelectListItem
            {
                Value = x.MATNR.ToString(),
                Text = x.MAKTX
            }).ToList();
            return Json(Lista3, JsonRequestBehavior.AllowGet);
        }
    }
}