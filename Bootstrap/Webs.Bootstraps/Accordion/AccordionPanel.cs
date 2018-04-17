using System;
using System.IO;
using System.Web.Mvc;

namespace Webs.Bootstraps.Accordion
{
    public class AccordionPanel : IDisposable
    {
        private readonly TextWriter _textWriter;

        internal AccordionPanel(TextWriter writer, string title, string panelId, string parentAccordionId)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            _textWriter = writer;

            _textWriter.Write(@"<div class=""accordion-group"">");

            var builder = new TagBuilder("div");
            builder.AddCssClass("accordion-heading");

            var builder2 = new TagBuilder("a");
            builder2.Attributes.Add("href", "#" + panelId);
            builder2.AddCssClass("accordion-toggle");
            builder2.InnerHtml = title;
            builder2.MergeAttribute("data-toggle", "collapse");
            builder2.MergeAttribute("data-parent", "#" + parentAccordionId);

            builder.InnerHtml = builder2.ToString();

            _textWriter.Write(builder.ToString());

            builder = new TagBuilder("div");
            builder.AddCssClass("accordion-body collapse");
            builder.MergeAttribute("id", panelId);

            _textWriter.Write(builder.ToString(TagRenderMode.StartTag));
            _textWriter.Write(@"<div class=""accordion-inner"">");
        }

        public void Dispose()
        {
            _textWriter.Write("</div></div></div>");
        }
    }
}