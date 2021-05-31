using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CANJES.Models
{
    public class ClsRepLogs
    {
        public string CodLabProd { get; set; }
        public string Cod_Prod_SAP { get; set; }
        public string Nombre_Segun_Proveedor { get; set; }
        public decimal Cantidad_Solicitada { get; set; }
        public decimal Cantidad_Enviar { get; set; }
        public decimal Saldo_SAP { get; set; }
        public string UNIDAD { get; set; }
        public string Mensaje1 { get; set; }
        public string Mensaje2 { get; set; }
        public string Mensaje3 { get; set; }
        public string Mensaje4 { get; set; }
        public string Sociedad { get; set; }
        public string Nombre_Sociedad { get; set; }
        public string Nombre_Cliente { get; set; }
        public string Estado { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
        public int Documento { get; set; }
        public string Nombre_SAP { get; set; }
        public string CENTRO { get; set; }
        public string Nombre_Centro { get; set; }
        public string NumMigo { get; set; }
        public string anioMigo { get; set; }
        public DateTime Fecha_MIGO { get; set; }
    }
}