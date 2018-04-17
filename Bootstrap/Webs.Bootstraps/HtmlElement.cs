using System.Collections.Generic;
using System.Web.Mvc;

namespace Webs.Bootstraps
{
    public abstract class HtmlElement
    {
        // Fields
        protected readonly IDictionary<string, object> htmlAttributes;

        protected string tag;

        // Methods
        internal HtmlElement(string tag, object htmlAttributes)
        {
            this.tag = tag;
            this.htmlAttributes = HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes);
        }

        // Properties
        internal string EndTag
        {
            get
            {
                return string.Format("</{0}>", tag);
            }
        }

        internal virtual string StartTag
        {
            get
            {
                TagBuilder builder = new TagBuilder(tag);
                builder.MergeAttributes<string, object>(htmlAttributes);
                return builder.ToString(TagRenderMode.StartTag);
            }
        }
        /// <summary>
        /// 确保拥有样式，无则添加
        /// </summary>
        /// <param name="className">样式名</param>
        protected void EnsureClass(string className)
        {
            EnsureHtmlAttribute("class", className, false);
        }
        /// <summary>
        /// 确保有Html属性，无则添加
        /// </summary>
        /// <param name="key">属性键</param>
        /// <param name="value">属性值</param>
        /// <param name="replaceExisting">true存在时替换，否则追加</param>
        protected void EnsureHtmlAttribute(string key, string value, bool replaceExisting = true)
        {
            if (htmlAttributes.ContainsKey(key))
            {
                if (replaceExisting)
                {
                    htmlAttributes[key] = value;
                }
                else
                {
                    htmlAttributes[key] += " " + value;
                }
            }
            else
            {
                htmlAttributes.Add(key, value);
            }
        }
    }
}