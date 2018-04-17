using System;
using System.IO;

namespace Webs.Bootstraps.Thumbnails
{
    public class ThumbnailCaptionPanel : IDisposable
    {
        private readonly TextWriter textWriter;

        internal ThumbnailCaptionPanel(TextWriter writer)
        {
            this.textWriter = writer;
            this.textWriter.Write(@"<div class=""caption"">");
        }

        public void Dispose()
        {
            this.textWriter.Write("</div>");
        }
    }
}