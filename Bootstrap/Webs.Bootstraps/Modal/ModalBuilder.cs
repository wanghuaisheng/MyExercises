using System.Web.Mvc;

namespace Webs.Bootstraps.Modal
{
    public class ModalBuilder<TModel> : BuilderBase<TModel, Modal>
    {
        internal ModalBuilder(HtmlHelper<TModel> htmlHelper, Modal modal)
            : base(htmlHelper, modal)
        {
        }

        public ModalSectionPanel BeginHeader()
        {
            return new ModalSectionPanel(ModalSection.Header, base.TxtWriter);
        }

        public ModalSectionPanel BeginBody()
        {
            return new ModalSectionPanel(ModalSection.Body, base.TxtWriter);
        }

        public ModalSectionPanel BeginFooter()
        {
            return new ModalSectionPanel(ModalSection.Footer, base.TxtWriter);
        }
    }
}