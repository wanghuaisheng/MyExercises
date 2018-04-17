using System.Web.Mvc;

namespace Webs.Bootstraps.Accordion
{
    /// <summary>
    /// 手风琴控件
    /// </summary>
    public class Accordion : HtmlElement
    {
        public string Id { get; set; }

        public Accordion(string id): this(id, null){}

        public Accordion(string id, object htmlAttributes): base("div", htmlAttributes)
        {
            Id = HtmlHelper.GenerateIdFromName(id);
            EnsureClass("accordion");
            EnsureHtmlAttribute("id", Id);
        }
    }
}