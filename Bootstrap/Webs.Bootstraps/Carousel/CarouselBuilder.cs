using System.Web.Mvc;

namespace Webs.Bootstraps.Carousel
{
    public class CarouselBuilder<TModel> : BuilderBase<TModel, Carousel>
    {
        private readonly UrlHelper urlHelper;
        private bool isFirstItem = true;

        internal CarouselBuilder(HtmlHelper<TModel> htmlHelper, Carousel carousel)
            : base(htmlHelper, carousel)
        {
            urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            base.TxtWriter.Write(@"<div class=""carousel-inner"">");
        }

        public void Item(string url, string altText)
        {
            if (isFirstItem)
            {
                base.TxtWriter.Write(@"<div class=""item active"">");
                isFirstItem = false;
            }
            else
            {
                base.TxtWriter.Write(@"<div class=""item"">");
            }

            base.TxtWriter.Write($@"<img src=""{urlHelper.Content(url)}"" alt=""{altText}"" />");
            base.TxtWriter.Write("</div>");
        }

        public CarouselCaptionPanel ItemWithCaption(string url, string altText, object htmlAttributes = null)
        {
            if (isFirstItem)
            {
                base.TxtWriter.Write(@"<div class=""item active"">");
                isFirstItem = false;
            }
            else
            {
                base.TxtWriter.Write(@"<div class=""item"">");
            }

            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttributes(HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes));
            imgBuilder.MergeAttribute("src", urlHelper.Content(url));
            imgBuilder.MergeAttribute("alt", altText);
            base.TxtWriter.Write(imgBuilder.ToString(TagRenderMode.SelfClosing));
            return new CarouselCaptionPanel(base.TxtWriter);
        }

        public override void Dispose()
        {
            base.TxtWriter.Write("</div>");

            base.TxtWriter.Write(string.Format(@"<a class=""left carousel-control"" data-slide=""prev"" href=""#{0}"">‹</a>", base.HtmElement.Id));
            base.TxtWriter.Write(string.Format(@"<a class=""right carousel-control"" data-slide=""next"" href=""#{0}"">›</a>", base.HtmElement.Id));

            base.TxtWriter.Write(base.HtmElement.EndTag);

            base.TxtWriter.Write(string.Format(
@"<script type=""text/javascript"">
    $(document).ready(function(){{
        $('#{0}').carousel({{
            interval: {1}
        }})
    }});
</script>", base.HtmElement.Id, base.HtmElement.Interval));
        }
    }
}