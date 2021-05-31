using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CANJES.Models;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;

namespace CANJES.Controllers
{
    public class TBL_EncabezadoSAPController : Controller
    {
        
        private AUTCANJESEntities db = new AUTCANJESEntities();

        public bool Permisos()
        {
            string user = Session["Usuario"] as string;

            var Lst = db.TBL_UsuariosCanjes.SqlQuery("select distinct * from TBL_UsuariosCanjes where nombreUsuario = {0}", user).ToList();

            int? rol_id = Lst[0].id_rol;

            var Lst2 = db.TBL_UsuariosCanjes_Roles.SqlQuery("select * from TBL_UsuariosCanjes_Roles where id_rol = {0}", rol_id).ToList();

            if (Lst2[0].rol == "Cliente")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET: TBL_EncabezadoSAP
        public ActionResult Index()
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
            }
            catch(Exception ex) 
            {
                return HttpNotFound(ex.Message);
            }
            y:
            string id = Session["Id_Usuario"] as string;
            int cid = Convert.ToInt32(id);
            return View(db.VSP_Autorizaciones_Encabezado.Where(x => x.Estado == "GU" && x.Id_Usuario == cid).ToList());
        }
        public ActionResult AutorizacionDetalle(int? Documento, string Cod_Prov_SAP, string Cod_Cliente_SAP, string Cod_Casa_SAP, string Sociedad, string Cliente)
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

                var cli = db.VSP_CLIENTE_X_SOCIEDAD_SAP.Where(i => i.CODIGO_CLIENTE.Contains(Cliente) && i.SOCIEDAD_CLIENTE == Sociedad);

                string client =  Cliente.PadLeft(10, '0');

                var matches = db.VSP_CLIENTE_X_SOCIEDAD_SAP.FirstOrDefault(i => i.CODIGO_CLIENTE== client && i.SOCIEDAD_CLIENTE == Sociedad);

                
                



                ViewBag.Documento = Documento;
                ViewBag.Cod_Prov_SAP = Cod_Prov_SAP;
                ViewBag.Cod_Cliente_SAP = Cod_Cliente_SAP;
                ViewBag.Cod_Casa_SAP = Cod_Casa_SAP;
                ViewBag.Sociedad = Sociedad;
                ViewBag.Cliente = Cliente;
                if (matches != null)
                {
                    ViewBag.nombre_cliente = matches.NOMBRE_CLIENTE;
                }
                else {
                    ViewBag.nombre_cliente = "";
                }
               
                //var cliente_nombre = db.VSP_CLIENTE_X_SOCIEDAD_SAP.First(x => x.CODIGO_CLIENTE == Cliente);




                var lst = (from t in db.VSP_Autorizaciones_Encabezado
                           where t.Documento == Documento && t.Cod_Prov_SAP == Cod_Prov_SAP && t.Cod_Cliente_SAP == Cod_Cliente_SAP
                           && t.Cod_Casa_SAP == Cod_Casa_SAP && t.Sociedad == Sociedad
                           select t).ToList();
                ViewBag.Proveedor = lst[0].Proveedor;
                ViewBag.Casa = lst[0].CASA;
                ViewBag.NSociedad = lst[0].Nombre_de_Sociedad;
                ViewBag.Observaciones = lst[0].Observaciones;
                ViewBag.Cliente = lst[0].Cliente;
                ViewBag.Centro = lst[0].Centro;

                //var lst1 = (from t in db.TBL_Centros select t).ToList();

                //var Centro = lst1.Select(a => new SelectListItem
                //{
                //    Value = a.id.ToString(),
                //    Text = a.nombre_centro
                //});
                //ViewBag.Centro = Centro;
                ViewBag.Meses = 3;
                string Invalidantes = Session["Invalidantes"] as string;
                string sin_detalle = Session["SIN_DETALLE"] as string;
                string sin_canje = Session["SIN_CANJE"] as string;
                string requisitos = Session["Requisitos"] as string;
                string procesado = Session["Procesado"] as string;
                string Evaluado = Session["Evaluado"] as string;

                if (requisitos == "N")
                {
                    ViewBag.Requisitos = requisitos;
                    Session["Requisitos"] = "";
                }

                if (Evaluado == "Y")
                {
                    ViewBag.Evaluado = Evaluado;
                    Session["Evaluado"] = "";
                }

                if (procesado == "Y") 
                {
                    ViewBag.Procesado = procesado;
                    Session["Procesado"] = "";
                }

                if (Invalidantes == "Y")
                {
                    ViewBag.Errores = Invalidantes;
                    Session["Invalidantes"] = "";
                }

                if(sin_detalle == "Y") 
                {
                    ViewBag.Sin_Detalle = sin_detalle;
                    Session["SIN_DETALLE"] = "";
                }

                if (sin_canje == "Y")
                {
                    ViewBag.Sin_Canje = sin_canje;
                    Session["SIN_CANJE"] = "";
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return View(db.VSP_Autorizaciones_Detalle.Where(x => x.Documento == Documento).ToList());
        }
        public ActionResult Autorizar(int? Documento, string Cod_Prov_SAP, string Cod_Cliente_SAP, string Cod_Casa_SAP, string Sociedad, string Centro, int Meses, string Cliente)
        {
            int doc = Convert.ToInt32(Documento);
            try
            {
                if (Documento != null)
                {
                    var head = db.TBL_EncabezadoSAP.First(x => x.Documento == Documento);
                    head.Cliente = Cliente;
                    db.SaveChanges();

                    



                    var Evaluado = db.TBL_DetalleSAP.SqlQuery("select * from TBL_DetalleSAP where Documento={0} and Mensaje1='SI_EXISTE' and Mensaje2='SI' and Mensaje3='CANJE'", Documento).ToList();

                    if (Evaluado.Count > 0) //Verifica si ya fue evaluado, es algo temporal!
                    {
                        Session["Evaluado"] = "Y";
                        return RedirectToAction("AutorizacionDetalle", "TBL_EncabezadoSAP", new { Documento = Documento, Cod_Prov_SAP = Cod_Prov_SAP, Cod_Cliente_SAP = Cod_Cliente_SAP, Cod_Casa_SAP = Cod_Casa_SAP, Sociedad = Sociedad, Cliente= Cliente });
                    }

                    //Valida si existe el material en SAP
                    var objdetalle = ZRFC_VALIDATE_MATERIAL(doc); // Paso1
                    string mensaje = "";
                    if (objdetalle.Count == 0)
                    {
                        return RedirectToAction("AutorizacionDetalle", "TBL_EncabezadoSAP", new { Documento = Documento, Cod_Prov_SAP = Cod_Prov_SAP, Cod_Cliente_SAP = Cod_Cliente_SAP, Cod_Casa_SAP = Cod_Casa_SAP, Sociedad = Sociedad, Cliente= Cliente });
                    }

                    foreach (var item in objdetalle)
                    {
                        mensaje = item.mensaje;
                        mensaje = mensaje.Substring(1, 9);
                        db.sp_TBL_DetalleSAP_update1(item.Documento, item.Material, mensaje);
                    }
                    //Valida si existe canje para el material y cliente
                    var objdetalle1 = ZRFC_VALIDATE_MATERIAL_INVOICE(doc, Sociedad, Cliente, Meses); // Paso2
                    if (objdetalle1.Count == 0)
                    {
                        Session["SIN_CANJE"] = "Y";
                        return RedirectToAction("AutorizacionDetalle", "TBL_EncabezadoSAP", new { Documento = Documento, Cod_Prov_SAP = Cod_Prov_SAP, Cod_Cliente_SAP = Cod_Cliente_SAP, Cod_Casa_SAP = Cod_Casa_SAP, Sociedad = Sociedad, Cliente = Cliente });
                    }
                    foreach (var item in objdetalle1)
                    {
                        mensaje = item.mensaje;
                        mensaje = mensaje.Substring(1, 2);
                        db.sp_TBL_DetalleSAP_update2(item.Documento, item.Material, mensaje); // Paso3
                    }
                    //Valida si existe detalle para el material en SAP
                    Centro = Centro.Replace("{", "");
                    Centro = Centro.Replace("}", "");
                    Centro = Centro.Replace("values", "");
                    Centro = Centro.Replace("=", "");
                    Centro = Centro.Trim();
                    var objdetalle2 = ZRFC_GET_STOCK_FOR_CANJES(doc, Centro, "UNI");
                    string fecha_cad = "";
                    DateTime fecha_actual = new DateTime();

                    if (objdetalle2.Count > 0)
                    {
                        foreach (var item in objdetalle2)
                        {
                            List<Cls_Detalle2> lista = new List<Cls_Detalle2>();
                            if (item.MESSAGE != "")
                            {
                                mensaje = item.MESSAGE;
                                //mensaje = mensaje.Substring(1, 100);
                                db.sp_TBL_DetalleSAP_update3(item.DOCUMENTO, item.MATERIAL, mensaje);
                            }
                            else
                            {
                                // *** Recupera información de las tablas para comparar fechas y lotes
                                var Detalle1 = db.TBL_DetalleSAP.SqlQuery("select * from TBL_DetalleSAP where Documento={0} and Cod_Prod_SAP={1} and Mensaje3 <> ''", Documento, item.MATERIAL).ToList();
                                string mensaje3 = "";
                                if (Detalle1.Count > 0)
                                {
                                    mensaje3 = "X";
                                }

                                var encabezado = (from t in db.TBL_EncabezadoSAP
                                                  where t.Documento == Documento
                                                  select t).ToList();
                                fecha_actual = encabezado[0].Fecha;
                                //***

                                if (mensaje3 == "")
                                {
                                    var mlote = (from t in objdetalle2
                                                 where t.DOCUMENTO == Documento && t.MATERIAL == item.MATERIAL
                                                 select t).ToList();

                                    //decimal vcantidad = 0; string nlote = "";

                                    if (mlote.Count > 1) //Cuando el material tiene más de un lote
                                    {
                                        foreach (var x in mlote)
                                        {
                                            var fecha_comparacion = db.sp_convertir_fecha(x.FECHA_CAD).ToList();
                                            if (Convert.ToDateTime(fecha_comparacion[0].Value) >= fecha_actual)
                                            {
                                                lista.Add(new Cls_Detalle2
                                                {
                                                    DOCUMENTO = x.DOCUMENTO,
                                                    MATERIAL = x.MATERIAL,
                                                    ALMACEN = x.ALMACEN,
                                                    CENTRO = x.CENTRO,
                                                    LOTE = x.LOTE,
                                                    FECHA_CAD = x.FECHA_CAD,
                                                    CANTIDAD = x.CANTIDAD,
                                                    UNIDAD = x.UNIDAD
                                                });
                                                //vcantidad = vcantidad + x.CANTIDAD;
                                            }
                                        }

                                        if (lista.Count > 0)//Materiales con lotes vigentes o próximos a vencer
                                        {
                                            //var menor = (from t in lista select t).Min(x => x.LOTE).ToString();
                                            //var ULista = (from t in lista where t.LOTE == menor select t).ToList();
                                            //nlote = ULista[0].LOTE;
                                            //fecha_cad = ULista[0].FECHA_CAD;
                                            //db.sp_TBL_DetalleSAP_update2_1(ULista[0].DOCUMENTO, ULista[0].MATERIAL, vcantidad, ULista[0].CENTRO, ULista[0].ALMACEN, nlote, fecha_cad, ULista[0].UNIDAD, "CANJE");
                                            var LstLotes = (from t in lista select t).ToList();
                                            foreach (var Ilotes in LstLotes)
                                            {
                                                db.sp_TBL_DetalleSAP2(Ilotes.DOCUMENTO, Ilotes.MATERIAL, Ilotes.CANTIDAD, Ilotes.ALMACEN, Ilotes.CENTRO, Ilotes.LOTE, Ilotes.FECHA_CAD, Ilotes.UNIDAD, "CANJE");
                                            }
                                        }
                                        else //Materiales con lotes vencidos
                                        {
                                            db.sp_TBL_DetalleSAP_update3(item.DOCUMENTO, item.MATERIAL, "LOTES_VENCIDOS");
                                        }
                                    }
                                    else //Cuando el material sólo tiene un lote
                                    {
                                        fecha_cad = mlote[0].FECHA_CAD;
                                        var fecha_comparacion = db.sp_convertir_fecha(fecha_cad).ToList();
                                        if (Convert.ToDateTime(fecha_comparacion[0].Value) >= fecha_actual)//Materiales con lotes vigentes o próximos a vencer
                                        {
                                            db.sp_TBL_DetalleSAP_update2_1(mlote[0].DOCUMENTO, mlote[0].MATERIAL, mlote[0].CANTIDAD, mlote[0].CENTRO, mlote[0].ALMACEN, mlote[0].LOTE, fecha_cad, mlote[0].UNIDAD, "CANJE");
                                        }
                                        else //Materiales con lotes vencidos
                                        {
                                            db.sp_TBL_DetalleSAP_update3(item.DOCUMENTO, item.MATERIAL, "LOTES_VENCIDOS");
                                        }
                                    }
                                }
                            }
                        }

                        //List<TBL_DetalleSAP> Detalle_Autorizar = new List<TBL_DetalleSAP>();
                        var Detalle_Autorizar = db.TBL_DetalleSAP.SqlQuery("select * from TBL_DetalleSAP where Documento={0} and Mensaje1='SI_EXISTE' and Mensaje2='SI' and Mensaje3='CANJE'", Documento).ToList();
                        if (Detalle_Autorizar.Count > 0) 
                        {
                            Session["Procesado"] = "Y";
                        }
                        else
                        {
                            Session["Invalidantes"] = "Y";
                        }
                        return RedirectToAction("AutorizacionDetalle", "TBL_EncabezadoSAP", new { Documento = Documento, Cod_Prov_SAP = Cod_Prov_SAP, Cod_Cliente_SAP = Cod_Cliente_SAP, Cod_Casa_SAP = Cod_Casa_SAP, Sociedad = Sociedad,Cliente=Cliente });
                    }
                    else
                    {
                        var Detalle = (from t in db.TBL_DetalleSAP
                                       where t.Documento == Documento && t.Mensaje1 == "SI_EXISTE"
                                       select t).ToList();
                        foreach (var item in Detalle)
                        {
                            db.sp_TBL_DetalleSAP_update3(item.Documento, item.Cod_Prod_SAP, "SIN_DETALLE");
                        }
                        Session["SIN_DETALLE"] = "Y";
                        return RedirectToAction("AutorizacionDetalle", "TBL_EncabezadoSAP", new { Documento = Documento, Cod_Prov_SAP = Cod_Prov_SAP, Cod_Cliente_SAP = Cod_Cliente_SAP, Cod_Casa_SAP = Cod_Casa_SAP, Sociedad = Sociedad,Cliente=Cliente });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return RedirectToAction("Index", "TBL_EncabezadoSAP");
        }

        public ActionResult Confirmar(int? Documento)
        {
            List<TBL_EncabezadoSAP> enc = new List<TBL_EncabezadoSAP>();
            try
            {
                var lista = db.TBL_DetalleSAP.SqlQuery("select * from TBL_DetalleSAP where Documento={0} and Mensaje1='SI_EXISTE' and Mensaje2='SI' and Mensaje3='CANJE'", Documento).ToList();

                if (lista.Count > 0)
                {
                    db.sp_TBL_EncabezadoSAP_cambio_status2(Documento, "PR");
                    return RedirectToAction("Index", "TBL_EncabezadoSAP");
                }
                else 
                {
                    Session["Requisitos"] = "N";
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            enc = db.TBL_EncabezadoSAP.SqlQuery("select * from TBL_EncabezadoSAP where Documento={0}", Documento).ToList();
            return RedirectToAction("AutorizacionDetalle", "TBL_EncabezadoSAP", new { Documento = Documento, Cod_Prov_SAP = enc[0].Cod_Prov_SAP, Cod_Cliente_SAP = enc[0].Cod_Cliente_SAP, Cod_Casa_SAP = enc[0].Cod_Casa_SAP, Sociedad = enc[0].Sociedad, Cliente = enc[0].Cliente });
        }

        #region "Consumo de WS para RFC"
        public List<Cls_Detalle> ZRFC_VALIDATE_MATERIAL(int Documento)
        {
            string PosDev = "";
            List<Cls_Detalle> ObjDetalle = new List<Cls_Detalle>();
            try
            {

                string Baseurl = HttpContext.Application["sitio"] as string;
                string apiUrl = Baseurl + "api/CanjesAPI/ZRFC_VALIDATE_MATERIAL";

                var detalle = (from t in db.TBL_DetalleSAP
                               where t.Documento == Documento
                               select t).ToList();
                foreach (var item in detalle)
                {
                    parametrosRFC = new
                    {
                        material = item.Cod_Prod_SAP
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
                        PosDev = m;// Devuelve SI_EXISTE ó NO_EXISTE

                        ObjDetalle.Add(new Cls_Detalle
                        {
                            Documento = Documento,
                            Material = item.Cod_Prod_SAP,
                            mensaje = m
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ObjDetalle;
        }
        public object parametrosRFC
        {
            get; set;
        }
        public class Cls_Detalle
        {
            public int Documento { get; set; }
            public string Material { get; set; }
            public string mensaje { get; set; }
        }
        public class Cls_Detalle2
        {
            public int DOCUMENTO { get; set; }
            public string MATERIAL { get; set; }
            public string CENTRO { get; set; }
            public string ALMACEN { get; set; }
            public string LOTE { get; set; }
            public string FECHA_CAD { get; set; }
            public decimal CANTIDAD { get; set; }
            public string UNIDAD { get; set; }
            public string TYPE { get; set; }
            public string MESSAGE { get; set; }
        }
        public List<Cls_Detalle> ZRFC_VALIDATE_MATERIAL_INVOICE(int Documento, string sociedad, string cliente, int meses)
        {
            string PosDev = "";
            List<Cls_Detalle> ObjDetalle = new List<Cls_Detalle>();
            try
            {
                string Baseurl = HttpContext.Application["sitio"] as string;
                string apiUrl = Baseurl + "api/CanjesAPI/ZRFC_VALIDATE_MATERIAL_INVOICE";

                var Detalle1 = (from t in db.TBL_DetalleSAP
                                where t.Documento == Documento && t.Mensaje1 == "SI_EXISTE"
                                select t).ToList();

                if (Detalle1.Count == 0)
                {
                    goto y;
                }

                sociedad = sociedad.Replace("{","");
                sociedad = sociedad.Replace("}", "");
                sociedad = sociedad.Replace("values", "");
                sociedad = sociedad.Replace("=", "");
                sociedad = sociedad.Trim();

                cliente = cliente.Replace("{", "");
                cliente = cliente.Replace("}", "");
                cliente = cliente.Replace("values", "");
                cliente = cliente.Replace("=", "");
                cliente = cliente.Trim();

                foreach (var item in Detalle1)
                {
                    parametrosRFC = new
                    {
                        sociedad = sociedad,
                        cliente = cliente,
                        material = item.Cod_Prod_SAP,
                        meses = meses
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
                        PosDev = m;// Devuelve SI_EXISTE ó NO_EXISTE

                        ObjDetalle.Add(new Cls_Detalle
                        {
                            Documento = Documento,
                            Material = item.Cod_Prod_SAP,
                            mensaje = m
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            y:
            return ObjDetalle;
        }
        public List<Cls_Detalle2> ZRFC_GET_STOCK_FOR_CANJES(int Documento, string centro, string unidad)
        {
            string PosDev = "";
            List<Cls_Detalle2> ObjDetalle2 = new List<Cls_Detalle2>();
            try
            {
                string Baseurl = HttpContext.Application["sitio"] as string;
                string apiUrl = Baseurl + "api/CanjesAPI/ZRFC_GET_STOCK_FOR_CANJES";

                var Detalle2 = (from t in db.TBL_DetalleSAP
                                where t.Documento == Documento && t.Mensaje1 == "SI_EXISTE" && t.Mensaje2 == "SI"
                                select t).ToList();

                if (Detalle2.Count == 0)
                {
                    goto y;
                }

                foreach (var item in Detalle2)
                {
                    parametrosRFC = new
                    {
                        centro = centro,
                        material = item.Cod_Prod_SAP,
                        cantidad = item.Cantidad_Solicitada,
                        unidad = unidad
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
                        PosDev = m;// Devuelve el detalle del material en SAP
                        PosDev = PosDev.Substring(1, 2);
                        if (PosDev != "[]")
                        {
                            string json = JsonConvert.DeserializeObject<string>(m);
                            dynamic ddd = JsonConvert.DeserializeObject<dynamic>(json);
                            //var listProductos = JsonConvert.DeserializeObject<List<Cls_Detalle2>>(m);
                            foreach (dynamic prod in ddd)
                            {
                                if (prod.MATERIAL == null)
                                {
                                    prod.MATERIAL = item.Cod_Prod_SAP;
                                }

                                ObjDetalle2.Add(new Cls_Detalle2
                                {
                                    DOCUMENTO = Documento,
                                    MATERIAL = prod.MATERIAL,
                                    CENTRO = prod.CENTRO,
                                    ALMACEN = prod.ALMACEN,
                                    LOTE = prod.LOTE,
                                    FECHA_CAD = prod.FECHA_CAD,
                                    CANTIDAD = prod.CANTIDAD,
                                    UNIDAD = prod.UNIDAD,
                                    TYPE = prod.TYPE,
                                    MESSAGE = prod.MESSAGE
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            y:
            return ObjDetalle2;
        }
        #endregion
        public ActionResult Edit_Detalle(int Documento, string Material, decimal Cantidad)
        {
            int doc = 0; string codcasa = ""; string codprov = ""; string soc = "";
            try
            {
                db.sp_TBL_DetalleSAP_update(Documento, Material, Cantidad);
                var lst = (from t in db.TBL_EncabezadoSAP
                           where t.Documento == Documento
                           select t).ToList();
                doc = lst[0].Documento;
                codcasa = lst[0].Cod_Casa_SAP;
                codprov = lst[0].Cod_Prov_SAP;
                soc = lst[0].Sociedad;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return RedirectToAction("AutorizacionDetalle", "TBL_EncabezadoSAP", new { Documento = doc, Cod_Prov_SAP = codprov, Cod_Casa_SAP = codcasa, Sociedad = soc });
        }

        public ActionResult Delete_Detalle(int Documento, string Material)
        {
            int doc = 0; string codcasa = ""; string codprov = ""; string soc = "";
            try
            {
                db.sp_TBL_DetalleSAP_delete2(Documento, Material);
                var lst = (from t in db.TBL_EncabezadoSAP
                           where t.Documento == Documento
                           select t).ToList();
                doc = lst[0].Documento;
                codcasa = lst[0].Cod_Casa_SAP;
                codprov = lst[0].Cod_Prov_SAP;
                soc = lst[0].Sociedad;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return RedirectToAction("AutorizacionDetalle", "TBL_EncabezadoSAP", new { Documento = doc, Cod_Prov_SAP = codprov, Cod_Casa_SAP = codcasa, Sociedad = soc });
        }
        // GET: TBL_EncabezadoSAP/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_EncabezadoSAP tBL_EncabezadoSAP = db.TBL_EncabezadoSAP.Find(id);
            if (tBL_EncabezadoSAP == null)
            {
                return HttpNotFound();
            }
            return View(tBL_EncabezadoSAP);
        }

        // GET: TBL_EncabezadoSAP/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TBL_EncabezadoSAP/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Documento,Cod_Casa_SAP,Cod_Prov_SAP,Cod_Cliente_SAP,Nombre_Cliente,Observaciones,Estado,Fecha")] TBL_EncabezadoSAP tBL_EncabezadoSAP)
        {
            if (ModelState.IsValid)
            {
                db.TBL_EncabezadoSAP.Add(tBL_EncabezadoSAP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tBL_EncabezadoSAP);
        }

        // GET: TBL_EncabezadoSAP/Edit/5
        public ActionResult Edit(int? Documento, string Cod_Prov_SAP, string Cod_Cliente_SAP, string Cod_Casa_SAP, string Sociedad)
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
            if (Documento == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var lst = (from t in db.TBL_EncabezadoSAP
                       where t.Documento == Documento && t.Cod_Prov_SAP == Cod_Prov_SAP && t.Cod_Cliente_SAP == Cod_Cliente_SAP
                       && t.Cod_Casa_SAP == Cod_Casa_SAP && t.Sociedad == Sociedad
                       select t).ToList();

            TBL_EncabezadoSAP tBL_EncabezadoSAP = new TBL_EncabezadoSAP();
            foreach (var item in lst)
            {
                tBL_EncabezadoSAP.Documento = item.Documento;
                tBL_EncabezadoSAP.Cod_Casa_SAP = item.Cod_Casa_SAP;
                tBL_EncabezadoSAP.Cod_Prov_SAP = item.Cod_Prov_SAP;
                tBL_EncabezadoSAP.Sociedad = item.Sociedad;
                tBL_EncabezadoSAP.Cliente = item.Cliente;
                tBL_EncabezadoSAP.Observaciones = item.Observaciones;
                tBL_EncabezadoSAP.Estado = item.Estado;
                tBL_EncabezadoSAP.Fecha = item.Fecha;
            }

            if (tBL_EncabezadoSAP == null)
            {
                return HttpNotFound();
            }
            return View(tBL_EncabezadoSAP);
        }

        // POST: TBL_EncabezadoSAP/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Documento,Cod_Casa_SAP,Cod_Prov_SAP,Cod_Cliente_SAP,Sociedad,Cliente,Observaciones,Estado,Documento_SAP,Fecha_Autorizacion,Fecha_DocumentoSAP,Fecha_Migo,MESSAGE,TYPE,Fecha")] TBL_EncabezadoSAP tBL_EncabezadoSAP)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    //db.Entry(tBL_EncabezadoSAP).State = EntityState.Modified;
                    //db.SaveChanges();
                    db.sp_TBL_EncabezadoSAP_update(tBL_EncabezadoSAP.Documento, tBL_EncabezadoSAP.Cliente, tBL_EncabezadoSAP.Observaciones);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return View(tBL_EncabezadoSAP);
        }

        // GET: TBL_EncabezadoSAP/Delete/5
        public ActionResult Delete(int? Documento, string Cod_Prov_SAP, string Cod_Cliente_SAP, string Cod_Casa_SAP, string Sociedad)
        {
            TBL_EncabezadoSAP tBL_EncabezadoSAP = new TBL_EncabezadoSAP();
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
                if (Documento == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var lst = (from t in db.TBL_EncabezadoSAP
                           where t.Documento == Documento && t.Cod_Prov_SAP == Cod_Prov_SAP && t.Cod_Cliente_SAP == Cod_Cliente_SAP
                           && t.Cod_Casa_SAP == Cod_Casa_SAP && t.Sociedad == Sociedad
                           select t).ToList();

                foreach (var item in lst)
                {
                    tBL_EncabezadoSAP.Documento = item.Documento;
                    tBL_EncabezadoSAP.Cod_Casa_SAP = item.Cod_Casa_SAP;
                    tBL_EncabezadoSAP.Cod_Prov_SAP = item.Cod_Prov_SAP;
                    tBL_EncabezadoSAP.Sociedad = item.Sociedad;
                    tBL_EncabezadoSAP.Cliente = item.Cliente;
                    tBL_EncabezadoSAP.Observaciones = item.Observaciones;
                    tBL_EncabezadoSAP.Estado = item.Estado;
                    tBL_EncabezadoSAP.Fecha = item.Fecha;
                }
                if (tBL_EncabezadoSAP == null)
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return View(tBL_EncabezadoSAP);
        }

        // POST: TBL_EncabezadoSAP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? Documento, string Cod_Prov_SAP, string Cod_Cliente_SAP, string Cod_Casa_SAP, string Sociedad)
        {
            try
            {
                db.sp_TBL_EncabezadoSAP_Delete(Documento);
                db.sp_TBL_DetalleSAP_Delete(Documento);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
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
