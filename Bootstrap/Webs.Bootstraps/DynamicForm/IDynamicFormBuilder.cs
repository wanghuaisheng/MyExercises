using System.Web.Mvc;

namespace Webs.Bootstraps.DynamicForm
{
    public interface IDynamicFormBuilder<TModel>
    {
        MvcHtmlString Build(TModel model);
    }
}