using System.Web.Mvc;

namespace Webs.Bootstraps.HeroUnit
{
    public class HeroUnitBuilder<TModel> : BuilderBase<TModel, HeroUnit>
    {
        internal HeroUnitBuilder(HtmlHelper<TModel> htmlHelper, HeroUnit heroUnit)
            : base(htmlHelper, heroUnit)
        {
            base.TxtWriter.Write("<h1>");
            base.TxtWriter.Write(heroUnit.Heading);
            base.TxtWriter.Write("</h1>");

            base.TxtWriter.Write("<p>");
            base.TxtWriter.Write(heroUnit.Tagline);
            base.TxtWriter.Write("</p>");

            base.TxtWriter.Write("<p>");
        }

        public override void Dispose()
        {
            base.TxtWriter.Write("</p>");
            base.Dispose();
        }
    }
}