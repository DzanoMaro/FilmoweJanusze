using System.Web;
using System.Web.Optimization;

namespace FilmoweJanusze
{
    public class BundleConfig
    {
        // Aby uzyskać więcej informacji o grupowaniu, odwiedź stronę https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-1.12.1.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/searchautocomplete.js"
                        ));

            // Użyj wersji deweloperskiej biblioteki Modernizr do nauki i opracowywania rozwiązań. Następnie, kiedy wszystko będzie
            // gotowe do produkcji, użyj narzędzia do kompilowania ze strony https://modernizr.com, aby wybrać wyłącznie potrzebne testy.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/MyFunctions").Include(
                    "~/Scripts/MyFunctions.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/Site.css"));
        }
    }
}
