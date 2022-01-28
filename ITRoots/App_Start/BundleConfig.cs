using System.Web;
using System.Web.Optimization;

namespace ITRoots
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                      "~/Scripts/datatables.js",
                      "~/Scripts/jquery.dataTables.min.js",
                      "~/Scripts/jszip.min.js",
                      "~/Scripts/dataTables.buttons.min.js",
                      "~/Scripts/pdfmake.min.js",
                      "~/Scripts/vfs_fonts.js",
                      "~/Scripts/buttons.html5.min.js",
                      "~/Scripts/buttons.print.min.js"
                      ));

            bundles.Add(new Bundle("~/Content/css").Include(
                      "~/Content/datatables.css",
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
