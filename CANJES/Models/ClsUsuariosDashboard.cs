using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CANJES.Models
{
    public class ClsUsuariosDashboard
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string pass { get; set; }
        public string email { get; set; }
        public string estado { get; set; }
        public int id_estado { get; set; }
        public string rol { get; set; }
        public int id_rol { get; set; }
        public string sociedades { get; set; }
        public int id_sociedad { get; set; }
        public string centros { get; set; }
        public int id_centro { get; set; }

    }

}