using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FilmoweJanusze.Helpers
{
    public static class BeginCarouselHelper
    {
        public static IDisposable BeginCarousel(this HtmlHelper htmlHelper, string id)
        {

            return null;
        }

        class BeginCarouselClass : IDisposable
        {
            private HtmlHelper htmlHelper;

            public BeginCarouselClass(HtmlHelper htmlHelper)
            {
                this.htmlHelper = htmlHelper;
            }

            public void Dispose()
            {

            }
        }

    }
}