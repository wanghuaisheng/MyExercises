using System.Web.Mvc;

namespace Webs.Bootstraps.Accordion
{
    /// <summary>
    /// 手风琴构造器
    /// </summary>
    /// <typeparam name="TModel">视图模型类型</typeparam>
    public class AccordionBuilder<TModel> : BuilderBase<TModel, Accordion>
    {

        /// <summary>
        /// 手风琴构造器
        /// </summary>
        internal AccordionBuilder(HtmlHelper<TModel> htmlHelper, Accordion accordion) : base(htmlHelper, accordion) { }

        public AccordionPanel BeginPanel(string title, string id)
        {
            return new AccordionPanel(TxtWriter, title, id, HtmElement.Id);
        }

    }
}