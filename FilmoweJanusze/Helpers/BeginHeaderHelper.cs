using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FilmoweJanusze.Helpers
{
    public static class BeginHeaderHelper
    {
        
        public static IDisposable BeginHeader(this HtmlHelper htmlHelper, string MainTitle)
        {
            return BeginHeader(htmlHelper, MainTitle, String.Empty);
        }

        public static IDisposable BeginHeader(this HtmlHelper htmlHelper, string MainTitle, string SubTitle)
        {
            htmlHelper.ViewContext.Writer.Write(
                "<div class=\"page-header well-jm realative\"> " +
                "<div class=\"col-xs-10 col-sm-11\">" +
                "<h3>" + MainTitle + "</h3>");

            if(!String.IsNullOrEmpty(SubTitle))
            {
                htmlHelper.ViewContext.Writer.Write("<h4>" + SubTitle + "</h4>");
            }

            htmlHelper.ViewContext.Writer.Write(
                "</div>"
                );
            return new BeginHeaderClass(htmlHelper);
        }

        class BeginHeaderClass : IDisposable
        {
            private HtmlHelper htmlHelper;

            public BeginHeaderClass(HtmlHelper htmlHelper)
            {
                this.htmlHelper = htmlHelper;
            }

            public void Dispose()
            {
                this.htmlHelper.ViewContext.Writer.Write(
                    "<div class=\"clearfix\"></div></div>"
                    );
            }
        }

    }


}