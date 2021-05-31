using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CANJES.Models;
using ExcelDataReader;
using LinqToExcel;
using CANJES.Clases;
using System.Globalization;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace CANJES.Controllers
{
    public class TBL_MaterialesSAPController : Controller
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
        // GET: TBL_MaterialesSAP
        public ActionResult Index()
        {

            return View(db.VSP_CASA_SAP.ToList());
        }

        public ActionResult Registrar(string MATKL) 
        {
            List<VSP_MATERIALES_CASAS> lst = new List<VSP_MATERIALES_CASAS>();
            try 
            {
                lst = db.VSP_MATERIALES_CASAS.SqlQuery("select * from VSP_MATERIALES_CASAS where MATNR COLLATE SQL_Latin1_General_CP850_BIN2 not in(select MATNR from [dbo].[TBL_MaterialesSAP] " +
                          "where MATKL='" + MATKL + "') and MATKL='" + MATKL + "'").ToList();
                return View(lst);
            }
            catch(Exception ex) 
            {
                Console.Write(ex.Message);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Materiales(string matkl) 
        {
            List<Cls_ModelMateriales> ViewMat = new List<Cls_ModelMateriales>();
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

                if (matkl != null) 
                {
                    ViewMat = (from t in db.TBL_MaterialesSAP
                               where t.MATKL == matkl
                               select new Cls_ModelMateriales
                               {
                                   MATKL=t.MATKL,
                                   MATNR=t.MATNR,
                                   MAKTX=t.MAKTX,
                                   MATLAB=t.MATLAB
                               }).ToList();
                }

                var lst1 = (from t in db.VSP_CASA_SAP select t).ToList();

                var Casa = lst1.Select(a => new SelectListItem
                {
                    Value = a.COD_CASA,
                    Text = a.CASA
                });

                ViewBag.Casa = Casa;

                string error_rm = Session["ErrorRM"] as string;
                string registro_masivo = Session["RegistroMasivo1"] as string;

                if (registro_masivo == "Y")
                {
                    ViewBag.Registro_Masivo = registro_masivo;
                    Session["RegistroMasivo1"] = "";
                }

                if(error_rm == "Y") 
                {
                    ViewBag.ErrorRM = error_rm;
                    Session["ErrorRM"]="";
                }
            }
            catch(Exception ex) 
            {
                Console.Write(ex.Message);
            }
            return View(ViewMat);
        }
        public ActionResult Registro_Masivo() 
        {
            try 
            {
                db.sp_TBL_MaterialesSAP_Insert1();
                Session["RegistroMasivo1"] = "Y";
            }
            catch(Exception ex) 
            {
                Session["ErrorRM"] = "Y";
                Session["RegistroMasivo1"] = "";
                Console.Write(ex.Message);
            }
            return RedirectToAction("Materiales");
        }

        //[HttpPost, ActionName("CargaMasiva")]
        public ActionResult CargaMasiva(HttpPostedFileBase file) 
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
                // Validar Archivo
                if (file != null && file.ContentLength > 0) 
                {
                    // Valida si es un archivo de excel
                    if (!file.FileName.EndsWith(".xls") && !file.FileName.EndsWith(".xlsx"))
                    {
                        ViewBag.NoExcel = "Y";
                        goto x;
                    }
                    // .xlsx
                    IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(file.InputStream);
                    //// reader.IsFirstRowAsColumnNames
                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    };
                    DataSet dataSet = reader.AsDataSet(conf);
                    DataTable tabla = new DataTable();
                    tabla = dataSet.Tables[0];

                    //Validación si el archivo de excel contiene datos
                    if (tabla.Rows.Count == 0)
                    {
                        ViewBag.Excel_SinDatos = "Y"; //Variable global sin datos
                        goto x;
                    }
                    //Validación si el archivo de excel no contiene el número de columnas requerido
                    if ((tabla.Columns.Count < 3) || (tabla.Columns.Count > 3))
                    {
                        ViewBag.Excel_Columnas = "Y"; //Variable global columnas requeridas "Y" si no cumple
                        goto x;
                    }
                    //Validación si el archivo de excel no contiene el nombre de columna requerido
                    if ((tabla.Columns[0].ColumnName != "MATKL") || (tabla.Columns[1].ColumnName != "MATNR") || (tabla.Columns[2].ColumnName != "MATLAB"))
                    {
                        ViewBag.Excel_NombreColumna = "Y"; //Variable global nombre columna requeridas "Y" si no cumple
                        goto x;
                    }
                    //Validación si el archivo de excel contiene datos repetidos, columna material
                    var ColumnaMATNR = new List<string>();
                    var ColumnaMATKL = new List<string>();
                    var ColumnaMATLAB = new List<string>();
                    foreach (DataRow k in tabla.Rows)
                    {
                        ColumnaMATKL.Add(k[0].ToString());
                        ColumnaMATNR.Add(k[1].ToString());
                        ColumnaMATLAB.Add(k[2].ToString());
                    }

                    // Columna MATNR
                    var x = from ob in ColumnaMATNR group ob by ob into g select new { Name = g.Key, Duplicatecount = g.Count() };
                    foreach (var m in x)
                    {
                        Console.WriteLine(m.Name + "--" + m.Duplicatecount);
                        if ((m.Name == null) || (m.Name == ""))
                        {
                            ViewBag.Excel_MATNR = "Y"; //Variable global MATNR "Y" si no contiene valores
                            goto x;
                        }

                        // Nombres repetidos
                        if (m.Duplicatecount > 1)
                        {
                            ViewBag.Excel_MATNRRepetidos = "Y"; //Variable global repetidos "Y" si contiene 
                            goto x;
                        }
                    }
                    // Columna MATKL
                    var y = from ob in ColumnaMATKL group ob by ob into g select new { Name = g.Key, Duplicatecount = g.Count() };
                    foreach (var m in y)
                    {
                        Console.WriteLine(m.Name + "--" + m.Duplicatecount);
                        if ((m.Name == null) || (m.Name == ""))
                        {
                            ViewBag.Excel_MATKL = "Y"; //Variable global MATKL  "Y" si no contiene valores
                            goto x;
                        }
                    }
                    // Columna MATLAB
                    var z = from ob in ColumnaMATLAB group ob by ob into g select new { Name = g.Key, Duplicatecount = g.Count() };
                    foreach (var m in z)
                    {
                        Console.WriteLine(m.Name + "--" + m.Duplicatecount);
                        if ((m.Name == null) || (m.Name == ""))
                        {
                            ViewBag.Excel_MATLAB = "Y"; //Variable global MATLAB "Y" si no contiene valores
                            goto x;
                        }

                        // Nombres repetidos
                        if (m.Duplicatecount > 1)
                        {
                            ViewBag.Excel_MATLABRepetidos = "Y"; //Variable global repetidos "Y" si contiene 
                            goto x;
                        }
                    }

                    List<LogErroresMat> ListaErrores = new List<LogErroresMat>();
                    string MATKL, MATNR, MATLAB = "";
                    foreach (DataRow dr in dataSet.Tables[0].Rows) 
                    { 
                        MATKL= dr["MATKL"].ToString();
                        MATNR = dr["MATNR"].ToString();
                        MATLAB = dr["MATLAB"].ToString();

                        while (MATNR.Length < 18)
                        {
                            MATNR = "0" + MATNR;
                        }

                        var ListaMat = (from t in db.TBL_MaterialesSAP where t.MATKL == MATKL && t.MATNR == MATNR
                                        select t).ToList();

                        if (ListaMat.Count > 0) 
                        {
                            db.sp_TBL_MaterialesSAP_Update1(MATKL, MATNR, MATLAB);
                            ViewBag.Carga = "Y";
                        }
                        else 
                        {
                            ListaErrores.Add(new LogErroresMat { 
                                             MATKL=MATKL,
                                             MATNR=MATNR,
                                             MATLAB=MATLAB
                            });
                        }
                    }

                    if (ListaErrores.Count > 0) 
                    {
                        //Tabla de log de errores 24/04/2021
                        var myExport = new CsvExport();
                        foreach (var item in ListaErrores)
                        {
                            myExport.AddRow();
                            myExport["MATKL"] = item.MATKL;
                            myExport["MATNR"] = item.MATNR;
                            myExport["MATLAB"] = item.MATLAB;
                            myExport["Mensaje"] = "No existe el registro en la tabla de materiales";
                        }

                        byte[] myCsvData = myExport.ExportToBytes();

                        //// Obtiene la respuesta actual
                        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                        // Tipo de contenido para forzar la descarga
                        response.ContentType = "application/octet-stream";
                        response.AddHeader("Content-Disposition", "attachment; filename=" + "LOG Errores Producto Laboratorio " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm tt", CultureInfo.InvariantCulture) + ".csv");
                        response.StatusCode = (int)HttpStatusCode.OK;
                        // Envia los bytes
                        response.BinaryWrite(myCsvData);
                        HttpContext.Response.Flush(); // '// Sends all currently buffered output To the client.
                        HttpContext.Response.SuppressContent = true; // '// Gets Or sets a value indicating whether To send HTTP content To the client.
                        HttpContext.ApplicationInstance.CompleteRequest();

                        // Borra la respuesta
                        response.Clear();
                        response.ClearContent();
                        ViewBag.Carga = "";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Y";
                return HttpNotFound(ex.Message);
            }
            x:
            return View();
        }

        public class LogErroresMat 
        { 
            public string MATKL { get; set; }
            public string MATNR { get; set; }
            public string MATLAB { get; set; }
        }
        public ActionResult ModificarMaterial(string MATKL, string MATNR, string MATLAB) 
        {
            try 
            {
                db.sp_TBL_MaterialesSAP_Update1(MATKL, MATNR, MATLAB);
            }
            catch(Exception ex) 
            {
                Console.Write(ex.Message);
            }
            return RedirectToAction("Materiales", "TBL_MaterialesSAP",new { matkl = MATKL });
        }
        // GET: TBL_MaterialesSAP/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_MaterialesSAP tBL_MaterialesSAP = db.TBL_MaterialesSAP.Find(id);
            if (tBL_MaterialesSAP == null)
            {
                return HttpNotFound();
            }
            return View(tBL_MaterialesSAP);
        }

        // GET: TBL_MaterialesSAP/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TBL_MaterialesSAP/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MATKL,MATNR,MAKTX,MATLAB")] TBL_MaterialesSAP tBL_MaterialesSAP)
        {
            if (ModelState.IsValid)
            {
                db.TBL_MaterialesSAP.Add(tBL_MaterialesSAP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tBL_MaterialesSAP);
        }

        // GET: TBL_MaterialesSAP/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_MaterialesSAP tBL_MaterialesSAP = db.TBL_MaterialesSAP.Find(id);
            if (tBL_MaterialesSAP == null)
            {
                return HttpNotFound();
            }
            return View(tBL_MaterialesSAP);
        }

        // POST: TBL_MaterialesSAP/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MATKL,MATNR,MAKTX,MATLAB")] TBL_MaterialesSAP tBL_MaterialesSAP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tBL_MaterialesSAP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tBL_MaterialesSAP);
        }

        // GET: TBL_MaterialesSAP/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_MaterialesSAP tBL_MaterialesSAP = db.TBL_MaterialesSAP.Find(id);
            if (tBL_MaterialesSAP == null)
            {
                return HttpNotFound();
            }
            return View(tBL_MaterialesSAP);
        }

        // POST: TBL_MaterialesSAP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TBL_MaterialesSAP tBL_MaterialesSAP = db.TBL_MaterialesSAP.Find(id);
            db.TBL_MaterialesSAP.Remove(tBL_MaterialesSAP);
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
    }
}
