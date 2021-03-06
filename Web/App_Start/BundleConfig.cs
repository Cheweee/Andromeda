﻿using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/Jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/Jquery/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // готово к выпуску, используйте средство сборки по адресу https://modernizr.com, чтобы выбрать только необходимые тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/angular")
                .Include("~/Scripts/Angular/angular.*",
                "~/Scripts/Angular/angular-*"));

            bundles.Add(new ScriptBundle("~/bundles/material-data-table")
                .Include("~/Scripts/Angular/md-data-table.*"));

            bundles.Add(new ScriptBundle("~/bundles/andromeda")
                .Include("~/Scripts/Andromeda/andromeda.*"));

            bundles.Add(new StyleBundle("~/Content/angular")
                .Include("~/Content/Angular/angular-material.*"));

            bundles.Add(new StyleBundle("~/Content/material-data-table")
                .Include("~/Content/Angular/md-data-table.*"));

            bundles.Add(new StyleBundle("~/Content/andromeda")
                .Include("~/Content/Andromeda/andromeda.*"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/site.css"));
        }
    }
}
