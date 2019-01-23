using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FilmoweJanusze.Helpers
{
    public static class ImageHelper
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string src)
        {
            return Image(helper, src, null, null);
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string src, object htmlAttributes)
        {
            return Image(helper, src, null, htmlAttributes);
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string src, string actionurl)
        {
            return Image(helper, src, actionurl, null);
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string src, string actionurl, object htmlAttributes)
        {
            if (!String.IsNullOrEmpty(src))
            {
                // Create tag builder
                var figure = new TagBuilder("figure");
                var img = new TagBuilder("img");

                // Add attributes
                img.MergeAttribute("src", src);
                img.MergeAttributes(new RouteValueDictionary(htmlAttributes));

                if (!String.IsNullOrEmpty(actionurl))
                {
                    var anchor = new TagBuilder("a");
                    anchor.Attributes["href"] = actionurl;
                    return MvcHtmlString.Create(figure.ToString(TagRenderMode.StartTag) + anchor.ToString(TagRenderMode.StartTag) + img.ToString(TagRenderMode.SelfClosing) + anchor.ToString(TagRenderMode.EndTag) + figure.ToString(TagRenderMode.EndTag));
                }
                else
                {
                    return MvcHtmlString.Create(figure.ToString(TagRenderMode.StartTag) + img.ToString(TagRenderMode.SelfClosing) + figure.ToString(TagRenderMode.EndTag));
                }
            }
            else return MvcHtmlString.Empty;

        }


    }
}