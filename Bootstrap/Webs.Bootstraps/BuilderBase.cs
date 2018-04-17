using System;
using System.IO;
using System.Web.Mvc;

namespace Webs.Bootstraps
{
    /// <summary>
    /// 构建器基类
    /// </summary>
    /// <typeparam name="TModel">视图模型</typeparam>
    /// <typeparam name="T">Html元素</typeparam>
    public abstract class BuilderBase<TModel, T> : IDisposable where T : HtmlElement
    {
        // 字段
        protected readonly T HtmElement;
        protected readonly TextWriter TxtWriter;
        protected readonly HtmlHelper<TModel> HtmHelper;
        // 方法
        internal BuilderBase(HtmlHelper<TModel> htmlHelper, T element)
        {
            HtmElement = element ?? throw new ArgumentNullException(nameof(element));
            TxtWriter = htmlHelper.ViewContext.Writer;
            TxtWriter.Write(HtmElement.StartTag);
            HtmHelper = htmlHelper;
        }
        /// <summary>
        /// 务必调用，写入闭合标签
        /// </summary>
        public virtual void Dispose()
        {
            TxtWriter.Write(HtmElement.EndTag);
        }
    }
}