using System.Web.Mvc;

namespace Webs.Bootstraps.Toolbar
{

    public class ToolbarBuilder<TModel> : BuilderBase<TModel, Toolbar>
    {

        internal ToolbarBuilder(HtmlHelper<TModel> htmlHelper, Toolbar toolbar) : base(htmlHelper, toolbar) { }

        public ButtonGroup BeginButtonGroup()
        {
            return new ButtonGroup(TxtWriter);
        }

    }
}