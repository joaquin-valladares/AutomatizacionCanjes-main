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
    public class VSP_Casa_ClientePrv_SociedadController : Controller
    {
        public bool Permisos()
        {
            string user = Session["Usuario"] as string;

            var Lst = db.TBL_UsuariosCanjes.SqlQuery("select distinct * from TBL_UsuariosCanjes where nombreUsuario = {0}", user).ToList();

            if(Lst.Count == 0) 
            {
                return false;
            }

            int? rol_id = Lst[0].id_rol;

            var Lst2 = db.TBL_UsuariosCanjes_Roles.SqlQuery("select * from TBL_UsuariosCanjes_Roles where id_rol = {0}", rol_id).ToList();

            if (Lst2.Count == 0)
            {
                return false;
            }

            if (Lst2[0].rol == "Cliente")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Permisos2()
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

            if (Lst2[0].rol == "Autorizador")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Permisos3()
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

            if (Lst2[0].rol == "Procesador")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region "Código Utilizable"
        private AUTCANJESEntities db = new AUTCANJESEntities();

        // GET: VSP_Casa_ClientePrv_Sociedad
        public ActionResult Index(string CodCasa)
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
                // ******* Sección de variables globales para mensajes *******
                string VSPCCS_NoExcel = Session["VSPCCS_NoExcel"] as string;
                string Excel_SinDatos = Session["Excel_SinDatos"] as string;
                string Excel_Columnas = Session["Excel_Columnas"] as string;
                string Excel_Material = Session["Excel_Material"] as string;
                string Excel_MatCarEsp = Session["Excel_MatCarEsp"] as string;
                string Excel_MatRepetidos = Session["Excel_MatRepetidos"] as string;
                string Excel_Cantidad = Session["Excel_Cantidad"] as string;
                string Excel_CantNumero = Session["Excel_CantNumero"] as string;
                string Excel_Cargado = Session["Excel_Cargado"] as string;
                string Excel_NombreColumna = Session["Excel_NombreColumna"] as string;

                if (VSPCCS_NoExcel == "Y")
                {
                    ViewBag.VSPCCS_NoExcel = VSPCCS_NoExcel;
                    Session["VSPCCS_NoExcel"] = "";
                }

                if (Excel_NombreColumna == "Y")
                {
                    ViewBag.Excel_NombreColumna = Excel_NombreColumna;
                    Session["Excel_NombreColumna"] = "";
                }

                if (Excel_SinDatos == "Y")
                {
                    ViewBag.Excel_SinDatos = Excel_SinDatos;
                    Session["Excel_SinDatos"] = "";
                }

                if (Excel_Columnas == "Y")
                {
                    ViewBag.Excel_Columnas = Excel_Columnas;
                    Session["Excel_Columnas"] = "";
                }

                if (Excel_Material == "Y")
                {
                    ViewBag.Excel_Material = Excel_Material;
                    Session["Excel_Material"] = "";
                }

                if (Excel_MatCarEsp == "Y")
                {
                    ViewBag.Excel_MatCarEsp = Excel_MatCarEsp;
                    Session["Excel_MatCarEsp"] = "";
                }

                if (Excel_MatRepetidos == "Y")
                {
                    ViewBag.Excel_MatRepetidos = Excel_MatRepetidos;
                    Session["Excel_MatRepetidos"] = "";
                }

                if (Excel_Cantidad == "Y")
                {
                    ViewBag.Excel_Cantidad = Excel_Cantidad;
                    Session["Excel_Cantidad"] = "";
                }

                if (Excel_CantNumero == "Y")
                {
                    ViewBag.Excel_CantNumero = Excel_CantNumero;
                    Session["Excel_CantNumero"] = "";
                }

                if (Excel_Cargado == "Y")
                {
                    ViewBag.Excel_Cargado = Excel_Cargado;
                    Session["Excel_Cargado"] = "";
                }
                // Fin Seccion de VG

                List<SelectListItem> filtro = (from t in db.VSP_CASA_SAP.OrderBy(x => x.CASA)
                                               select new SelectListItem { Value = t.COD_CASA, Text = t.CASA }).ToList();

                ViewBag.CodCasa = filtro;

                List<SelectListItem> filtro2 = (from t in db.VSP_PROVEEDORES_SAP.OrderBy(x=>x.NAME1)
                                                select new SelectListItem { Value=t.KUNNR, Text= t.KUNNR + "-" + t.NAME1}).ToList();
                ViewBag.Cliente = filtro2;

                if (CodCasa != null)
                {
                    Session["VSPMC_Create"] = CodCasa;
                    return View(db.VSP_Casa_ClientePrv_Sociedad.OrderBy(y => y.Nombre_según_Proveedor).Where(x => x.CodCasa == CodCasa).ToList());
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            List<VSP_Casa_ClientePrv_Sociedad> lst = new List<VSP_Casa_ClientePrv_Sociedad>();
            lst.Add(new VSP_Casa_ClientePrv_Sociedad { CodCasa = "", Casa = "", CodProvSAP = "", Nombre_según_Proveedor = "", CodSoc = "", Sociedad = "" });
            return View(lst);
        }
        [HttpPost, ActionName("CargarArchivo")]
        public ActionResult CargarArchivo(HttpPostedFileBase file, string Cod_Casa_SAP, string Cod_Prov_SAP, string Sociedad, string Cliente, string Observacion, string Nombre_Segun_Proveedor, string Cod_Cliente_SAP)
        {
            string codcasa = Session["VSPMC_Create"] as string;
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
                        Session["VSPCCS_NoExcel"] = "Y";
                        return RedirectToAction("Index", "VSP_Casa_ClientePrv_Sociedad", new { CodCasa = codcasa });
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
                        Session["Excel_SinDatos"] = "Y"; //Variable global sin datos
                        return RedirectToAction("Index", "VSP_Casa_ClientePrv_Sociedad", new { CodCasa = codcasa });
                    }

                    //Validación si el archivo de excel no contiene el número de columnas requerido
                    if ((tabla.Columns.Count < 2) || (tabla.Columns.Count > 2))
                    {
                        Session["Excel_Columnas"] = "Y"; //Variable global columnas requeridas "Y" si no cumple
                        return RedirectToAction("Index", "VSP_Casa_ClientePrv_Sociedad", new { CodCasa = codcasa });
                    }

                    //Validación si el archivo de excel no contiene el nombre de columna requerido
                    if ((tabla.Columns[0].ColumnName != "Material") || (tabla.Columns[1].ColumnName != "Cantidad"))
                    {
                        Session["Excel_NombreColumna"] = "Y"; //Variable global nombre columna requeridas "Y" si no cumple
                        return RedirectToAction("Index", "VSP_Casa_ClientePrv_Sociedad", new { CodCasa = codcasa });
                    }

                    //Validación si el archivo de excel contiene datos repetidos, columna material
                    var ColumnaMaterial = new List<string>();
                    var ColumnaCantidad = new List<string>();
                    foreach (DataRow k in tabla.Rows)
                    {
                        ColumnaMaterial.Add(k[0].ToString());
                        ColumnaCantidad.Add(k[1].ToString());
                    }

                    // Columna Material
                    var x = from ob in ColumnaMaterial group ob by ob into g select new { Name = g.Key, Duplicatecount = g.Count() };
                    foreach (var m in x)
                    {
                        Console.WriteLine(m.Name + "--" + m.Duplicatecount);
                        if ((m.Name == null) || (m.Name == ""))
                        {
                            Session["Excel_Material"] = "Y"; //Variable global material "Y" si no contiene valores
                            return RedirectToAction("Index", "VSP_Casa_ClientePrv_Sociedad", new { CodCasa = codcasa });
                        }

                        // Verifica si el material no contiene caracteres especiales
                        var withoutSpecial = new string(m.Name.Where(c => Char.IsLetterOrDigit(c) || Char.IsWhiteSpace(c) || Char.IsPunctuation(c)).ToArray());
                        //string vname = m.Name.Replace(" ","");
                        string vname = m.Name;
                        // Si tiene caracteres especiales
                        //if (vname != withoutSpecial)
                        //{
                        //    Session["Excel_MatCarEsp"] = "Y"; //Variable global caracteres especiales "Y" si contiene 
                        //    return RedirectToAction("Index", "VSP_Casa_ClientePrv_Sociedad", new { CodCasa = codcasa });
                        //}

                        // Nombres repetidos
                        if (m.Duplicatecount > 1)
                        {
                            Session["Excel_MatRepetidos"] = "Y"; //Variable global repetidos "Y" si contiene 
                            return RedirectToAction("Index", "VSP_Casa_ClientePrv_Sociedad", new { CodCasa = codcasa });
                        }
                    }

                    // Columna Cantidad
                    var z = from ob in ColumnaCantidad group ob by ob into g select new { Cantidad = g.Key, Duplicatecount = g.Count() };
                    foreach (var m in z)
                    {
                        Console.WriteLine(m.Cantidad + "--" + m.Duplicatecount);
                        if ((m.Cantidad == null) || (m.Cantidad == ""))
                        {
                            Session["Excel_Cantidad"] = "Y"; //Variable global material "Y" sino contiene valores
                            return RedirectToAction("Index", "VSP_Casa_ClientePrv_Sociedad", new { CodCasa = codcasa });
                        }

                        // Verifica si el material no contiene caracteres especiales
                        var withoutSpecial = new string(m.Cantidad.Where(c => Char.IsNumber(c)).ToArray());
                        //string vname = m.Name.Replace(" ","");
                        string vname = m.Cantidad;
                        // Valida si es número
                        if (vname != withoutSpecial)
                        {
                            Session["Excel_CantNumero"] = "Y"; //Variable global Cantidad Npumero "Y" si no contiene  
                            return RedirectToAction("Index", "VSP_Casa_ClientePrv_Sociedad", new { CodCasa = codcasa });
                        }
                    }
                    // *********** Inicio Proceso Final de validación e inserción de datos ***********

                    List<Cls_DatosExcel_Detalle> DatosExcel = new List<Cls_DatosExcel_Detalle>();
                    List<Cls_DatosNoCargados_Detalle> SinCargaExcel = new List<Cls_DatosNoCargados_Detalle>();
                    string material = ""; decimal cantidad = 0;
                    foreach (DataRow dr in dataSet.Tables[0].Rows) // Validación si esta registrado el material
                    {
                        //Muestras los valores obteniendolos con el Índice o el Nombre de la columna, 
                        //   de la siguiente manera:

                        material = dr["Material"].ToString();
                        cantidad = Convert.ToDecimal(dr["Cantidad"].ToString());

                        var MQuery = db.TBL_MaestrosCanjes.SqlQuery("select * from TBL_MaestrosCanjes where Cod_Casa_SAP={0} and Cod_Cliente_SAP={1} and Sociedad={2} and Nombre_SAP={3}", Cod_Casa_SAP, Cod_Cliente_SAP, Sociedad, material).ToList();

                        if (MQuery.Count > 0)
                        {
                            DatosExcel.Add(new Cls_DatosExcel_Detalle
                            {
                                Material = MQuery[0].Cod_Prod_SAP,
                                Nombre_SAP = material,
                                Cantidad_Solicitada = cantidad
                            });
                        }
                        else
                        {
                            SinCargaExcel.Add(new Cls_DatosNoCargados_Detalle
                            {
                                Nombre_SAP2 = material,
                                Cantidad_Solicitada2 = cantidad,
                                Status = "No existe en tabla maestra"
                            });
                        }
                    }

                    if (DatosExcel.Count > 0)
                    {
                        string centro = Session["Centro"] as string;
                        int ccentro = Convert.ToInt32(centro);
                        string idusuario = Session["Id_Usuario"] as string;
                        int ciusuario = Convert.ToInt32(idusuario);
                        //Guardar / Procedimiento almacenado Encabezado
                        db.sp_TBL_EncabezadoSAP(Cod_Casa_SAP, Cod_Prov_SAP, Cod_Cliente_SAP, Sociedad, ccentro, Cliente, Observacion, ciusuario).ToString();
                        //var MQuery1 = db.TBL_EncabezadoSAP.SqlQuery("select max(Documento) AS Documento  from TBL_EncabezadoSAP where Cod_Casa_SAP={0} and Cod_Prov_SAP={1} and Sociedad={2} and Estado='GU'", Cod_Casa_SAP,Cod_Prov_SAP,Sociedad).ToList();
                        var MQuery1 = db.TBL_EncabezadoSAP.Where(c => c.Cod_Casa_SAP == Cod_Casa_SAP && c.Cod_Prov_SAP == Cod_Prov_SAP && c.Sociedad == Sociedad && c.Estado == "GU").Max(y => y.Documento);

                        foreach (var item in DatosExcel)
                        {
                            //Guardar / Procedimiento almacenado Detalle
                            db.sp_TBL_DetalleSAP(MQuery1, item.Material, Nombre_Segun_Proveedor, item.Cantidad_Solicitada).ToString();
                        }
                        Session["Excel_Cargado"] = "Y"; //Variable global Excel cargado "Y" se cargo 
                    }

                    if (SinCargaExcel.Count > 0)
                    {
                        var myExport = new CsvExport();
                        foreach (var item in SinCargaExcel)
                        {
                            myExport.AddRow();
                            myExport["Material"] = item.Nombre_SAP2;
                            myExport["Cantidad"] = item.Cantidad_Solicitada2;
                            myExport["Status"] = item.Status;
                        }

                        //Agregado nuevo: incluye lo cargado correctamente y el usuario sepa.
                        if (DatosExcel.Count > 0)
                        {
                            foreach (var items in DatosExcel)
                            {
                                myExport.AddRow();
                                myExport["Material"] = items.Nombre_SAP;
                                myExport["Cantidad"] = items.Cantidad_Solicitada;
                                myExport["Status"] = "Se cargo exitosamente";
                            }
                        }
                        // Fin Agregado

                        byte[] myCsvData = myExport.ExportToBytes();

                        //// Obtiene la respuesta actual
                        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                        // Tipo de contenido para forzar la descarga
                        response.ContentType = "application/octet-stream";
                        response.AddHeader("Content-Disposition", "attachment; filename=" + "Materiales No cargados " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm tt", CultureInfo.InvariantCulture) + ".csv");
                        response.StatusCode = (int)HttpStatusCode.OK;
                        // Envia los bytes
                        response.BinaryWrite(myCsvData);
                        HttpContext.Response.Flush(); // '// Sends all currently buffered output To the client.
                        HttpContext.Response.SuppressContent = true; // '// Gets Or sets a value indicating whether To send HTTP content To the client.
                        HttpContext.ApplicationInstance.CompleteRequest();

                        // Borra la respuesta
                        response.Clear();
                        response.ClearContent();
                    }
                    // *********** FIN Proceso Final de validación e inserción de datos ***********
                }
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }

            return RedirectToAction("Index", "VSP_Casa_ClientePrv_Sociedad", new { CodCasa = codcasa });
        }

        public ActionResult Documentos_Generados()
        {
            try 
            {
                if (Permisos2() == true)
                {
                    goto y;
                }
                else
                {
                    return RedirectToAction("AccesoDenegado", "Home");
                }
            }
            catch(Exception ex) 
            {
                return HttpNotFound(ex.Message);
            }
            y:
            return View(db.VSP_Autorizaciones_Encabezado.Where(x => x.Estado == "PR").ToList());
        }

        public ActionResult DocumentoDetalle(int? Documento, string Cod_Prov_SAP, string Cod_Cliente_SAP, string Cod_Casa_SAP, string Sociedad, string Cliente) 
        {
            try
            {
                if (Permisos2() == true)
                {
                    goto y;
                }
                else
                {
                    return RedirectToAction("AccesoDenegado", "Home");
                }

                y:
                string client = Cliente.PadLeft(10, '0');

                var matches = db.VSP_CLIENTE_X_SOCIEDAD_SAP.FirstOrDefault(i => i.CODIGO_CLIENTE == client && i.SOCIEDAD_CLIENTE == Sociedad);
                if (matches != null)
                {
                    ViewBag.nombre_cliente = matches.NOMBRE_CLIENTE;
                }
                else
                {
                    ViewBag.nombre_cliente = "";
                }
                ViewBag.Documento = Documento;
                ViewBag.Cod_Prov_SAP = Cod_Prov_SAP;
                ViewBag.Cod_Cliente_SAP = Cod_Cliente_SAP;
                ViewBag.Cod_Casa_SAP = Cod_Casa_SAP;
                ViewBag.Sociedad = Sociedad;
                ViewBag.Cliente = Cliente;

                var lst = (from t in db.VSP_Autorizaciones_Encabezado
                           where t.Documento == Documento && t.Cod_Cliente_SAP == Cod_Cliente_SAP
                           && t.Cod_Casa_SAP == Cod_Casa_SAP && t.Sociedad == Sociedad
                           select t).ToList();
                ViewBag.Proveedor = lst[0].Proveedor;
                ViewBag.Casa = lst[0].CASA;
                ViewBag.NSociedad = lst[0].Nombre_de_Sociedad;
                ViewBag.Observaciones = lst[0].Observaciones;
                ViewBag.Cliente = lst[0].Cliente;
                ViewBag.Centro = lst[0].Centro;
                string Invalidante = Session["Invalidante"] as string;
                if(Invalidante == "Y") 
                {
                    ViewBag.Invalidante = Invalidante;
                    Session["Invalidante"] = "";
                }
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
            return View(db.VSP_Autorizaciones_Detalle.Where(x => x.Documento == Documento && x.Mensaje1 == "SI_EXISTE" && x.Mensaje2 == "SI" && x.Mensaje3 == "CANJE").ToList());
        }

        public ActionResult Confirmar(int? Documento)
        {
            List<TBL_EncabezadoSAP> enc = new List<TBL_EncabezadoSAP>();
            try
            {
                var lista = db.TBL_DetalleSAP.SqlQuery("select * from TBL_DetalleSAP where Documento={0} and Mensaje1='SI_EXISTE' and Mensaje2='SI' and Mensaje3='CANJE'", Documento).ToList();

                if (lista.Count > 0)
                {
                    db.sp_TBL_EncabezadoSAP_cambio_status1(Documento, "AU");
                }
                else 
                {
                    Session["Invalidante"] = "Y";
                    enc = db.TBL_EncabezadoSAP.SqlQuery("select * from TBL_EncabezadoSAP where Documento={0}", Documento).ToList();
                    return RedirectToAction("DocumentoDetalle", "VSP_Casa_ClientePrv_Sociedad", new { Documento = Documento, Cod_Prov_SAP = enc[0].Cod_Prov_SAP, Cod_Cliente_SAP = enc[0].Cod_Cliente_SAP, Cod_Casa_SAP = enc[0].Cod_Casa_SAP, Sociedad = enc[0].Sociedad, Cliente = enc[0].Cliente });
                }
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
            return RedirectToAction("Documentos_Generados", "VSP_Casa_ClientePrv_Sociedad");
        }

        public ActionResult Documentos_Autorizados()
        {
            try
            {
                if (Permisos2() == true)
                {
                    goto y;
                }
                else
                {
                    return RedirectToAction("AccesoDenegado", "Home");
                }
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
            y:
            return View(db.VSP_Autorizaciones_Encabezado.Where(x => x.Estado == "AU").ToList());
        }

        public ActionResult Autorizados_Detalle(int? Documento, string Cod_Prov_SAP, string Cod_Cliente_SAP, string Cod_Casa_SAP, string Sociedad, string Cliente)
        {
            try
            {
                if (Permisos2() == true)
                {
                    goto y;
                }
                else
                {
                    return RedirectToAction("AccesoDenegado", "Home");
                }

                y:
                string client = Cliente.PadLeft(10, '0');

                var matches = db.VSP_CLIENTE_X_SOCIEDAD_SAP.FirstOrDefault(i => i.CODIGO_CLIENTE == client && i.SOCIEDAD_CLIENTE == Sociedad);
                if (matches != null)
                {
                    ViewBag.nombre_cliente = matches.NOMBRE_CLIENTE;
                }
                else
                {
                    ViewBag.nombre_cliente = "";
                }
                ViewBag.Documento = Documento;
                ViewBag.Cod_Prov_SAP = Cod_Prov_SAP;
                ViewBag.Cod_Cliente_SAP = Cod_Cliente_SAP;
                ViewBag.Cod_Casa_SAP = Cod_Casa_SAP;
                ViewBag.Sociedad = Sociedad;
                ViewBag.Cliente = Cliente;

                var lst = (from t in db.VSP_Autorizaciones_Encabezado
                           where t.Documento == Documento && t.Cod_Cliente_SAP == Cod_Cliente_SAP
                           && t.Cod_Casa_SAP == Cod_Casa_SAP && t.Sociedad == Sociedad
                           select t).ToList();
                ViewBag.Proveedor = lst[0].Proveedor;
                ViewBag.Casa = lst[0].CASA;
                ViewBag.NSociedad = lst[0].Nombre_de_Sociedad;
                ViewBag.Observaciones = lst[0].Observaciones;
                ViewBag.Cliente = lst[0].Cliente;
                ViewBag.Centro = lst[0].Centro;
                string Invalidante = Session["Invalidante"] as string;
                string rechazado = Session["Rechazado"] as string;
                if (Invalidante == "Y")
                {
                    ViewBag.Invalidante = Invalidante;
                    Session["Invalidante"] = "";
                }
                if (rechazado == "Y") 
                {
                    ViewBag.Rechazado = rechazado;
                    Session["Rechazado"] = "";
                    //Tabla de log de errores 23/04/2021
                    var myExport = new CsvExport();
                    var logerror = (from t in db.TBL_LOGERROR
                                    where t.Documento == Documento
                                    select t).ToList();

                    foreach (var item in logerror)
                    {
                        myExport.AddRow();
                        myExport["Documento"] = item.Documento;
                        myExport["Error"] = item.MESSAGE_ERROR;
                        myExport["Tipo"] = item.TYPE;
                        myExport["Mensaje"] = "No se pudo aplicar el documento";
                    }

                    byte[] myCsvData = myExport.ExportToBytes();

                    //// Obtiene la respuesta actual
                    System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                    // Tipo de contenido para forzar la descarga
                    response.ContentType = "application/octet-stream";
                    response.AddHeader("Content-Disposition", "attachment; filename=" + "Migo LOG Errores Documento " + Documento + " " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm tt", CultureInfo.InvariantCulture) + ".csv");
                    response.StatusCode = (int)HttpStatusCode.OK;
                    // Envia los bytes
                    response.BinaryWrite(myCsvData);
                    HttpContext.Response.Flush(); // '// Sends all currently buffered output To the client.
                    HttpContext.Response.SuppressContent = true; // '// Gets Or sets a value indicating whether To send HTTP content To the client.
                    HttpContext.ApplicationInstance.CompleteRequest();

                    // Borra la respuesta
                    response.Clear();
                    response.ClearContent();
                }
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
            return View(db.VSP_Autorizaciones_Detalle.Where(x => x.Documento == Documento && x.Mensaje1 == "SI_EXISTE" && x.Mensaje2 == "SI" && x.Mensaje3 == "CANJE").ToList());
        }

        public ActionResult Confirmar_Autorizados(int? Documento,string Receptor)
        {
            List<TBL_EncabezadoSAP> enc = new List<TBL_EncabezadoSAP>();
            try
            {
                var lista = db.TBL_DetalleSAP.SqlQuery("select * from TBL_DetalleSAP where Documento={0} and Mensaje1='SI_EXISTE' and Mensaje2='SI' and Mensaje3='CANJE'", Documento).ToList();

                if (lista.Count > 0)
                {
                    int doc = Convert.ToInt32(Documento);
                    var objdetalle = ZRFC_GOODS_MOVEMENT_CANJES(doc,Receptor);
                    if(objdetalle.Count > 0) 
                    {
                        foreach (var item in objdetalle) 
                        {
                            if(item.MESSAGE == "") 
                            {
                                item.TYPE = "S";
                                item.MESSAGE = "MIGO";
                                db.sp_TBL_DetalleSAP_update2_2(doc, item.EP_MATERIAL_DOCUMENT, item.EP_DOCUMENT_YEAR, item.MESSAGE, item.TYPE);
                            }
                            else 
                            {
                                db.sp_LOGERROR(doc, item.MESSAGE, item.TYPE);
                                var logerror = (from t in db.TBL_LOGERROR
                                                where t.Documento == doc
                                                select t).ToList();
                                if (logerror.Count == 1) 
                                {
                                    db.sp_TBL_DetalleSAP_update2_2(doc, item.EP_MATERIAL_DOCUMENT, item.EP_DOCUMENT_YEAR, "Se produjo un error", "E");
                                }
                            }
                        }
                    }
                    var listaOK = db.TBL_DetalleSAP.SqlQuery("select * from TBL_DetalleSAP where Documento={0} and Mensaje1='SI_EXISTE' and Mensaje2='SI' and Mensaje3='CANJE' and Mensaje4='MIGO'", Documento).ToList();
                    if(listaOK.Count > 0) 
                    {
                        db.sp_TBL_EncabezadoSAP_cambio_status3(doc, "AP");
                    }
                    else 
                    {
                        Session["Rechazado"] = "Y";
                        db.sp_TBL_EncabezadoSAP_cambio_status2(doc, "RE");
                        enc = db.TBL_EncabezadoSAP.SqlQuery("select * from TBL_EncabezadoSAP where Documento={0}", Documento).ToList();
                        return RedirectToAction("Autorizados_Detalle", "VSP_Casa_ClientePrv_Sociedad", new { Documento = Documento, Cod_Prov_SAP = enc[0].Cod_Prov_SAP, Cod_Cliente_SAP = enc[0].Cod_Cliente_SAP, Cod_Casa_SAP = enc[0].Cod_Casa_SAP, Sociedad = enc[0].Sociedad, Cliente = enc[0].Cliente });
                    }
                }
                else
                {
                    Session["Invalidante"] = "Y";
                    enc = db.TBL_EncabezadoSAP.SqlQuery("select * from TBL_EncabezadoSAP where Documento={0}", Documento).ToList();
                    return RedirectToAction("Autorizados_Detalle", "VSP_Casa_ClientePrv_Sociedad", new { Documento = Documento, Cod_Prov_SAP = enc[0].Cod_Prov_SAP, Cod_Cliente_SAP = enc[0].Cod_Cliente_SAP, Cod_Casa_SAP = enc[0].Cod_Casa_SAP, Sociedad = enc[0].Sociedad, Cliente = enc[0].Cliente });
                }
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
            return RedirectToAction("Documentos_Autorizados", "VSP_Casa_ClientePrv_Sociedad");
        }
        public object parametrosRFC
        {
            get; set;
        }

        public class Cls_DetalleMigo 
        { 
            public string EP_MATERIAL_DOCUMENT { get; set; }
            public string EP_DOCUMENT_YEAR { get; set; }
            public string TYPE { get; set; }
            public string MESSAGE { get; set; }
        }
        public List<Cls_DetalleMigo> ZRFC_GOODS_MOVEMENT_CANJES(int Documento,string Receptor) 
        {
            string PosDev = "";
            List<Cls_DetalleMigo> ObjDetalleMigo = new List<Cls_DetalleMigo>();
            try 
            {
                string Baseurl = HttpContext.Application["sitio"] as string;
                string apiUrl = Baseurl + "api/CanjesAPI/ZRFC_GOODS_MOVEMENT_CANJES";

                var MyQueryEnc = db.TBL_EncabezadoSAP.SqlQuery("select * from TBL_EncabezadoSAP where Documento={0}",Documento).ToList();
                //var MyQueryDet = db.TBL_DetalleSAP.SqlQuery("select * from TBL_DetalleSAP where Documento={0}", Documento).ToList();

                string cprov = MyQueryEnc[0].Cod_Cliente_SAP; 

                parametrosRFC = new
                {
                    documento = Documento,
                    proveedor = cprov, //KUNNR (Antes LIFNR)
                    receptor=Receptor
                    //receptor = MyQueryEnc[0].Cod_Cliente_SAP, //KUNNR
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(parametrosRFC);

                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(600);
                HttpResponseMessage response = client.PostAsync(
                    apiUrl,
                    new StringContent(inputJson, Encoding.UTF8, "application/json")
                    ).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // La respuesta es correcta
                    var data = response.Content.ReadAsStringAsync();
                    var m = data.Result;
                    PosDev = m;// Devuelve el detalle de MIGO
                    PosDev = PosDev.Substring(1, 2);
                    if (PosDev != "[]")
                    {
                        string json = JsonConvert.DeserializeObject<string>(m);
                        dynamic ddd = JsonConvert.DeserializeObject<dynamic>(json);

                        foreach (dynamic prod in ddd)
                        {
                            ObjDetalleMigo.Add(new Cls_DetalleMigo
                            {
                                EP_MATERIAL_DOCUMENT = prod.ep_material_document,
                                EP_DOCUMENT_YEAR = prod.ep_document_year,
                                TYPE = prod.type,
                                MESSAGE = prod.message
                            });
                        }
                    }
                }

            }
            catch(Exception ex) 
            {
               Console.Write(ex.Message);
            }
            return ObjDetalleMigo;
        }
        //Clase paramétros a cargar
        private class Cls_DatosExcel_Detalle
        {
            public string Material { get; set; }
            public string Nombre_SAP { get; set; }
            public decimal Cantidad_Solicitada { get; set; }
        }
        private class Cls_DatosNoCargados_Detalle
        {
            public string Nombre_SAP2 { get; set; }
            public decimal Cantidad_Solicitada2 { get; set; }
            public string Status { get; set; }
        }
        //Fin paramétros a cargar
        #endregion

        #region "ActionResult sin utilizar"
        // GET: VSP_Casa_ClientePrv_Sociedad/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VSP_Casa_ClientePrv_Sociedad vSP_Casa_ClientePrv_Sociedad = db.VSP_Casa_ClientePrv_Sociedad.Find(id);
            if (vSP_Casa_ClientePrv_Sociedad == null)
            {
                return HttpNotFound();
            }
            return View(vSP_Casa_ClientePrv_Sociedad);
        }

        // GET: VSP_Casa_ClientePrv_Sociedad/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VSP_Casa_ClientePrv_Sociedad/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodCasa,Casa,CodProvSAP,Nombre_según_Proveedor,CodSoc,Sociedad")] VSP_Casa_ClientePrv_Sociedad vSP_Casa_ClientePrv_Sociedad)
        {
            if (ModelState.IsValid)
            {
                db.VSP_Casa_ClientePrv_Sociedad.Add(vSP_Casa_ClientePrv_Sociedad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vSP_Casa_ClientePrv_Sociedad);
        }

        // GET: VSP_Casa_ClientePrv_Sociedad/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VSP_Casa_ClientePrv_Sociedad vSP_Casa_ClientePrv_Sociedad = db.VSP_Casa_ClientePrv_Sociedad.Find(id);
            if (vSP_Casa_ClientePrv_Sociedad == null)
            {
                return HttpNotFound();
            }
            return View(vSP_Casa_ClientePrv_Sociedad);
        }

        // POST: VSP_Casa_ClientePrv_Sociedad/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodCasa,Casa,CodProvSAP,Nombre_según_Proveedor,CodSoc,Sociedad")] VSP_Casa_ClientePrv_Sociedad vSP_Casa_ClientePrv_Sociedad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vSP_Casa_ClientePrv_Sociedad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vSP_Casa_ClientePrv_Sociedad);
        }

        // GET: VSP_Casa_ClientePrv_Sociedad/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VSP_Casa_ClientePrv_Sociedad vSP_Casa_ClientePrv_Sociedad = db.VSP_Casa_ClientePrv_Sociedad.Find(id);
            if (vSP_Casa_ClientePrv_Sociedad == null)
            {
                return HttpNotFound();
            }
            return View(vSP_Casa_ClientePrv_Sociedad);
        }

        // POST: VSP_Casa_ClientePrv_Sociedad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            VSP_Casa_ClientePrv_Sociedad vSP_Casa_ClientePrv_Sociedad = db.VSP_Casa_ClientePrv_Sociedad.Find(id);
            db.VSP_Casa_ClientePrv_Sociedad.Remove(vSP_Casa_ClientePrv_Sociedad);
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
        #endregion



    }
}
