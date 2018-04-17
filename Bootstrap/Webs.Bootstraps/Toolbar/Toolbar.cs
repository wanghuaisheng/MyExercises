namespace Webs.Bootstraps.Toolbar
{
    /// <summary>
    /// 工具条
    /// </summary>
    public class Toolbar : HtmlElement
    {
        public Toolbar() : this(null) { }

        public Toolbar(object htmlAttributes) : base("div", htmlAttributes)
        {
            EnsureClass("btn-toolbar");
        }
    }
}