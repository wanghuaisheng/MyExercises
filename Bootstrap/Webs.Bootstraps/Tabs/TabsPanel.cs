using System;
using System.IO;
using System.Web.Mvc;

namespace Webs.Bootstraps.Tabs
{
    public class TabsPanel : IDisposable
    {
        private readonly string _tag;
        private readonly TextWriter _textWriter;

        internal TabsPanel(TextWriter writer, string tag, string id, bool isActive = false)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                throw new ArgumentNullException(nameof(tag));
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            _textWriter = writer;
            _tag = tag;
            var builder = new TagBuilder(_tag);
            builder.Attributes.Add("id", id);
            builder.AddCssClass("tab-pane");

            if (isActive)
            {
                builder.AddCssClass("active");
            }

            _textWriter.Write(builder.ToString(TagRenderMode.StartTag));
        }

        public void Dispose()
        {
            _textWriter.Write("</{0}>", _tag);
        }
    }
}