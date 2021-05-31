using System.Web;
using System.Web.Optimization;

namespace CANJES
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/Scripts/sweetalert").Include(
                      "~/Scripts/sweetalert.js",
                      "~/Scripts/sweetalert.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/DataTables/jquery").Include(
                     "~/Scripts/DataTables/jquery.dataTables.js",
                     "~/Scripts/DataTables/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/DataTables/dataTables").Include(
                      "~/Scripts/DataTables/dataTables.buttons.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/DataTables/buttons").Include(
                      "~/Scripts/DataTables/buttons.flash.min.js",
                      "~/Scripts/DataTables/buttons.html5.min.js",
                      "~/Scripts/DataTables/buttons.print.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/DataTables/css/jquery.dataTables.min.css",
                      "~/Content/sweetalert/sweet-alert.css"));
        }
    }
}
