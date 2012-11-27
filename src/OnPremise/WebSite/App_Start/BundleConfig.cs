using System.Web.Optimization;

namespace Thinktecture.IdentityServer.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/js/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/js/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/js/modernizr").Include(
                        "~/Scripts/modernizr-{version}.js"));

            bundles.Add(new StyleBundle("~/bundles/css/site").Include(
                        "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/bundles/css/themes/base").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/bundles/css/mobile").Include(
                        "~/Content/jquery.mobile-{version}.css",
                        "~/Content/mobile.css"));
            bundles.Add(new ScriptBundle("~/bundles/js/mobile").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.mobile-{version}.js"));
        }
    }
}