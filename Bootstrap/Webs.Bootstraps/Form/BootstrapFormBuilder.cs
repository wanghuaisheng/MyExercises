using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Webs.Bootstraps.Form
{
    public class BootstrapFormBuilder<TModel> : BuilderBase<TModel, BootstrapForm>
    {
        internal BootstrapFormBuilder(HtmlHelper<TModel> htmlHelper, BootstrapForm form)
            : base(htmlHelper, form)
        {
        }

        public ControlGroup<TModel> BeginControlGroup()
        {
            return new ControlGroup<TModel>(base.TxtWriter, HtmHelper);
        }

        #region Horizontal (Normal) Form

        #region CheckBox

        public void CheckBoxControlGroup(string name)
        {
            WriteControlGroup(name, HtmHelper.CheckBox(name));
        }

        public void CheckBoxControlGroup(string name, bool isChecked)
        {
            WriteControlGroup(name, HtmHelper.CheckBox(name, isChecked));
        }

        public void CheckBoxControlGroup(string name, object htmlAttributes)
        {
            WriteControlGroup(name, HtmHelper.CheckBox(name, htmlAttributes));
        }

        public void CheckBoxControlGroup(string name, IDictionary<string, object> htmlAttributes)
        {
            WriteControlGroup(name, HtmHelper.CheckBox(name, htmlAttributes));
        }

        public void CheckBoxControlGroup(string name, bool isChecked, object htmlAttributes)
        {
            WriteControlGroup(name, HtmHelper.CheckBox(name, isChecked, htmlAttributes));
        }

        public void CheckBoxControlGroup(string name, bool isChecked, IDictionary<string, object> htmlAttributes)
        {
            WriteControlGroup(name, HtmHelper.CheckBox(name, isChecked, htmlAttributes));
        }

        public void CheckBoxControlGroupFor(Expression<Func<TModel, bool>> expression)
        {
            WriteControlGroup(expression, HtmHelper.CheckBoxFor(expression));
        }

        public void CheckBoxControlGroupFor(Expression<Func<TModel, bool>> expression, object htmlAttributes)
        {
            WriteControlGroup(expression, HtmHelper.CheckBoxFor(expression, htmlAttributes));
        }

        public void CheckBoxControlGroupFor(Expression<Func<TModel, bool>> expression, IDictionary<string, object> htmlAttributes)
        {
            WriteControlGroup(expression, HtmHelper.CheckBoxFor(expression, htmlAttributes));
        }

        #endregion CheckBox

        #region DropDownList

        public void DropDownListControlGroup(string name)
        {
            WriteControlGroup(name, HtmHelper.DropDownList(name));
        }

        public void DropDownListControlGroup(string name, IEnumerable<SelectListItem> selectList)
        {
            WriteControlGroup(name, HtmHelper.DropDownList(name, selectList));
        }

        public void DropDownListControlGroup(string name, string optionLabel)
        {
            WriteControlGroup(name, HtmHelper.DropDownList(name, optionLabel));
        }

        public void DropDownListControlGroup(string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            WriteControlGroup(name, HtmHelper.DropDownList(name, selectList, htmlAttributes));
        }

        public void DropDownListControlGroup(string name, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            WriteControlGroup(name, HtmHelper.DropDownList(name, selectList, htmlAttributes));
        }

        public void DropDownListControlGroup(string name, IEnumerable<SelectListItem> selectList, string optionLabel)
        {
            WriteControlGroup(name, HtmHelper.DropDownList(name, selectList, optionLabel));
        }

        public void DropDownListControlGroup(string name, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            WriteControlGroup(name, HtmHelper.DropDownList(name, selectList, optionLabel, htmlAttributes));
        }

        public void DropDownListControlGroup(string name, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            WriteControlGroup(name, HtmHelper.DropDownList(name, selectList, optionLabel, htmlAttributes));
        }

        public void DropDownListControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            WriteControlGroup(expression, HtmHelper.DropDownListFor(expression, selectList));
        }

        public void DropDownListControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            WriteControlGroup(expression, HtmHelper.DropDownListFor(expression, selectList, htmlAttributes));
        }

        public void DropDownListControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            WriteControlGroup(expression, HtmHelper.DropDownListFor(expression, selectList, htmlAttributes));
        }

        public void DropDownListControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel)
        {
            WriteControlGroup(expression, HtmHelper.DropDownListFor(expression, selectList, optionLabel));
        }

        public void DropDownListControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            WriteControlGroup(expression, HtmHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes));
        }

        public void DropDownListControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            WriteControlGroup(expression, HtmHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes));
        }

        #endregion DropDownList

        #region Password

        public void PasswordControlGroup(string name)
        {
            WriteControlGroup(name, HtmHelper.Password(name));
        }

        public void PasswordControlGroup(string name, object value)
        {
            WriteControlGroup(name, HtmHelper.Password(name, value));
        }

        public void PasswordControlGroup(string name, object value, object htmlAttributes)
        {
            WriteControlGroup(name, HtmHelper.Password(name, value, htmlAttributes));
        }

        public void PasswordControlGroup(string name, object value, IDictionary<string, object> htmlAttributes)
        {
            WriteControlGroup(name, HtmHelper.Password(name, value, htmlAttributes));
        }

        public void PasswordControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            WriteControlGroup(expression, HtmHelper.PasswordFor(expression));
        }

        public void PasswordControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            WriteControlGroup(expression, HtmHelper.PasswordFor(expression, htmlAttributes));
        }

        public void PasswordControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            WriteControlGroup(expression, HtmHelper.PasswordFor(expression, htmlAttributes));
        }

        #endregion Password

        #region TextBox

        public void TextBoxControlGroup(string name)
        {
            WriteControlGroup(name, HtmHelper.TextBox(name));
        }

        public void TextBoxControlGroup(string name, object value)
        {
            WriteControlGroup(name, HtmHelper.TextBox(name, value));
        }

        public void TextBoxControlGroup(string name, object value, object htmlAttributes)
        {
            WriteControlGroup(name, HtmHelper.TextBox(name, value, htmlAttributes));
        }

        public void TextBoxControlGroup(string name, object value, IDictionary<string, object> htmlAttributes)
        {
            WriteControlGroup(name, HtmHelper.TextBox(name, value, htmlAttributes));
        }

        public void TextBoxControlGroup(string name, object value, string format)
        {
            WriteControlGroup(name, HtmHelper.TextBox(name, value, format));
        }

        public void TextBoxControlGroup(string name, object value, string format, object htmlAttributes)
        {
            WriteControlGroup(name, HtmHelper.TextBox(name, value, format, htmlAttributes));
        }

        public void TextBoxControlGroup(string name, object value, string format, IDictionary<string, object> htmlAttributes)
        {
            WriteControlGroup(name, HtmHelper.TextBox(name, value, format, htmlAttributes));
        }

        public void TextBoxControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            WriteControlGroup(expression, HtmHelper.TextBoxFor(expression));
        }

        public void TextBoxControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            WriteControlGroup(expression, HtmHelper.TextBoxFor(expression, htmlAttributes));
        }

        public void TextBoxControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            WriteControlGroup(expression, HtmHelper.TextBoxFor(expression, htmlAttributes));
        }

        public void TextBoxControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression, string format)
        {
            WriteControlGroup(expression, HtmHelper.TextBoxFor(expression, format));
        }

        public void TextBoxControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes)
        {
            WriteControlGroup(expression, HtmHelper.TextBoxFor(expression, format, htmlAttributes));
        }

        public void TextBoxControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object> htmlAttributes)
        {
            WriteControlGroup(expression, HtmHelper.TextBoxFor(expression, format, htmlAttributes));
        }

        #endregion TextBox

        #endregion Horizontal (Normal) Form

        #region Inline Form

        public void LabelledCheckBoxFor(Expression<Func<TModel, bool>> expression)
        {
            var builder = new TagBuilder("label");
            builder.AddCssClass("checkbox");
            builder.InnerHtml = string.Concat(
                HtmHelper.CheckBoxFor(expression).ToString(),
                ExpressionHelper.GetExpressionText(expression));
            TxtWriter.Write(builder.ToString());
        }

        public void LabelledCheckBoxFor(Expression<Func<TModel, bool>> expression, object htmlAttributes)
        {
            var builder = new TagBuilder("label");
            builder.AddCssClass("checkbox");
            builder.InnerHtml = string.Concat(
                HtmHelper.CheckBoxFor(expression, htmlAttributes).ToString(),
                ExpressionHelper.GetExpressionText(expression));
            TxtWriter.Write(builder.ToString());
        }

        public void LabelledCheckBoxFor(Expression<Func<TModel, bool>> expression, IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("label");
            builder.AddCssClass("checkbox");
            builder.InnerHtml = string.Concat(
                HtmHelper.CheckBoxFor(expression, htmlAttributes).ToString(),
                ExpressionHelper.GetExpressionText(expression));
            TxtWriter.Write(builder.ToString());
        }

        #endregion Inline Form

        private void WriteControlGroup(string name, MvcHtmlString controlHtml)
        {
            using (var group = BeginControlGroup())
            {
                group.ControlLabel(name);
                using (var controls = group.BeginControlsSection())
                {
                    base.TxtWriter.Write(controlHtml);
                }
            }
        }

        private void WriteControlGroup<TProperty>(Expression<Func<TModel, TProperty>> expression, MvcHtmlString controlHtml)
        {
            using (var group = BeginControlGroup())
            {
                group.ControlLabelFor(expression);
                using (var controls = group.BeginControlsSection())
                {
                    base.TxtWriter.Write(controlHtml);
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}