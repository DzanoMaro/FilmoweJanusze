using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace FilmoweJanusze.Helpers
{
    public static class BeginPanelHelper
    {
        public static IDisposable BeginPanel(this HtmlHelper htmlHelper, string Title, string id, string classname, HelperResult helperResult = null)
        {
            string temp = String.Empty;

            htmlHelper.ViewContext.Writer.Write(
                "<div class=\"panel-group nopadding\">" +
                "<div class=\"panel panel-default\">" +
                "<div class=\"panel-heading container-fluid\">");

            if (helperResult != null)
            {
                htmlHelper.ViewContext.Writer.Write(
                    "<div class=\"col-xs-10 col-sm-11 nopadding\">"
                    );
            }

            htmlHelper.ViewContext.Writer.Write(
                "<h4 class=\"panel-title\">" +
                "<a data-toggle=\"collapse\" href=\"#" + id + "\" class=\"panel-link\">" +
                Title +
                "</a></h4></div>");

            if (helperResult != null)
            {
                htmlHelper.ViewContext.Writer.Write(
                    
                    helperResult.ToString() +
                    "</div>"
                    );
            }

            htmlHelper.ViewContext.Writer.Write(
                "<div id=\"" + id + "\" class=\"panel-collapse collapse " + classname + "\">" +
                "<div class=\"panel-body nopadding\">"
                );

            return new BeginPanelClass(htmlHelper);
        }

        class BeginPanelClass : IDisposable
        {
            private HtmlHelper htmlHelper;

            public BeginPanelClass(HtmlHelper htmlHelper)
            {
                this.htmlHelper = htmlHelper;
            }

            public void Dispose()
            {
                this.htmlHelper.ViewContext.Writer.Write(
                    "</div></div></div><div style=\"clear: both\"></div></div>"
                    );
            }
        }
    }
}