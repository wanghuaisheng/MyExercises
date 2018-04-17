using System.Web.Mvc;

namespace Webs.Bootstraps.Thumbnails
{
    public class ThumbnailBuilder<TModel> : BuilderBase<TModel, Thumbnail>
    {
        internal ThumbnailBuilder(HtmlHelper<TModel> htmlHelper, Thumbnail thumbnail)
            : base(htmlHelper, thumbnail)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            base.TxtWriter.Write(base.HtmElement.InternalImageTemplate
                .Replace("#{src}", urlHelper.Content(thumbnail.ImageSource))
                .Replace("#{alt}", thumbnail.ImageAltText));
        }

        public ThumbnailCaptionPanel BeginCaptionPanel()
        {
            return new ThumbnailCaptionPanel(base.TxtWriter);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}