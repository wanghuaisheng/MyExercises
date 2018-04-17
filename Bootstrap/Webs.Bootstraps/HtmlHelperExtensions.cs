using System.Web.Mvc;

namespace Webs.Bootstraps
{
    public static class HtmlHelperExtensions
    {
        public static Bootstrap<TModel> Bootstrap<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new Bootstrap<TModel>(htmlHelper);
        }
        public static MvcHtmlString DebButton<TModel>(this HtmlHelper<TModel> htmlHelper,string text,string onClick)
        {
            var builder = new TagBuilder("button");
            builder.MergeAttribute("type", "button");
            //builder.MergeAttribute("value", text);
            builder.SetInnerText(text);

            if (!string.IsNullOrEmpty(onClick))
            {
                builder.MergeAttribute("onclick", onClick);
            }

            //builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            //switch (color)
            //{
            //    case BootstrapNamedColor.Important: builder.AddCssClass("btn btn-danger"); break;
            //    case BootstrapNamedColor.Default: builder.AddCssClass("btn"); break;
            //    case BootstrapNamedColor.Info: builder.AddCssClass("btn btn-info"); break;
            //    case BootstrapNamedColor.Inverse: builder.AddCssClass("btn btn-inverse"); break;
            //    case BootstrapNamedColor.Primary: builder.AddCssClass("btn btn-primary"); break;
            //    case BootstrapNamedColor.Success: builder.AddCssClass("btn btn-success"); break;
            //    case BootstrapNamedColor.Warning: builder.AddCssClass("btn btn-warning"); break;
            //    default: builder.AddCssClass("btn"); break;
            //}

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}