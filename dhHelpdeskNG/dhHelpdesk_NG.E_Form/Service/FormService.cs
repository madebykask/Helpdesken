using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using DH.Helpdesk.EForm.Model.Abstract;

namespace DH.Helpdesk.EForm.Service
{
    // Lägga FormModel här istället, yes indeed?

    public interface IFormService
    {
        Form InitForm(Guid guid);
        List<Form> Forms(Guid guid);
    }

    public class FormService : IFormService
    {
        public Form InitForm(Guid guid)
        {
            var formSeed = new FormSeed();
            return formSeed.Init(guid);
        }

        public List<Form> Forms(Guid guid)
        {
            var formSeed = new FormSeed();
            return formSeed.Forms(guid);
        }
    }

    /*
     * Form logic
     * */

    /*
     * Css:
     *      Elements:
     *          .element, .element-description, .element-paragraph, .element-text, .element-select, .element-date
     *      Columns (used by "Choice elements": 
     *          .columns-one, .columns-two, .columns-three, .side-by-side
     *      Editor:
     *          .handle
     * */

    public enum Status
    {
        Working = 1,
        Published = 2,
        Archived = 3
    }

    public enum ChangeLogAction
    {
        Create = 1,
        Update = 2,
        Publish = 3
    }

    public enum Columns
    {
        One = 1,
        Two = 2,
        Three = 3,
        SideBySide = 4
    }

    public class ChangeLog
    {
        public int Id { get; set; }
        public int ChangeByUserId { get; set; }
        public DateTime ChangeDate { get; set; }
        public Guid FormVersion { get; set; }
        public ChangeLogAction Action { get; set; }
    }

    public class Attachment
    {

    }

    public class Form
    {
        public int Id { get; set; }
        public Guid FormGuid { get; set; }
        public Guid FormVersion { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public List<Tab> Tabs { get; set; }
        public List<ChangeLog> ChangeLogs { get; set; }
        public List<Attachment> Attachments { get; set; }

        public string Html()
        {
            var sb = new StringBuilder();
            return sb.ToString();
        }

        public bool Valid()
        {
            return true;
            //if(Elements == null) return true;
            //return Elements.Any(elem => !elem.Validation.Valid());
        }

        public string GetStatusText()
        {
            if(Status.Archived == Status)
                return "Archived";

            if(Status.Working == Status)
                return "Working";

            if(Status.Published == Status)
                return "Published";

            return "";
        }
    }

    public class Tab
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public string Name { get; set; }
        public List<Element> Elements { get; set; }

        public string Html()
        {
            var sb = new StringBuilder();

            if(Elements != null)
            {
                for(int i = 0; i < Elements.Count; i++)
                {
                    sb.AppendLine(Elements[i].Html());
                }
            }

            return sb.ToString();
        }
    }

    /*
     * Form logic, element classes
     * */

    public class Validation
    {
        public int Id { get; set; }
        public bool Mandatory { get; set; }
        public string RegularExpression { get; set; }

        public bool Valid()
        {
            return true;
        }

        private string _defaultMandatoryMessage = "This field is required";
    }

    public abstract class Element
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public Validation Validation { get; set; }
        public string Answer { get; set; }
        public string DefaultValue { get; set; }
        public bool Disabled { get; set; }

        public abstract string Html();
        public abstract string EditHtml();
    }

    public class TextBox : Element
    {
        public bool Multiline { get; set; }

        public override string Html()
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("<div id=\"elem_{0}\" class=\"element element-text\">", Id));
            sb.AppendLine("<div class=\"handle\"></div>");
            sb.AppendLine(string.Format("<label for=\"{0}\">{1}</label>", Name, Label));

            if(Multiline)
            {
                sb.AppendLine(string.Format("<textarea name=\"{0}\" id=\"{0}\"{2}>{1}</textarea>", Name, DefaultValue, (Disabled ? " disabled" : "")));
            }
            else
            {
                sb.AppendLine(string.Format("<input type=\"text\" name=\"{0}\" id=\"{0}\" value=\"{1}\"{2} />", Name, DefaultValue, (Disabled ? " disabled" : "")));
            }

            if(Description != "")
                sb.AppendLine(string.Format("<div class=\"element-description\">{0}</div>", Description));

            if(Disabled)
                sb.AppendLine(string.Format("<hidden name=\"{0}\" value=\"{1}\"/>", Name, Answer));

            sb.AppendLine(string.Format("</div>"));

            return sb.ToString();
        }

        public override string EditHtml()
        {
            throw new NotImplementedException();
        }
    }

    public class Dropdown : Element
    {
        public bool MultiSelect { get; set; }
        public List<Option> Options { get; set; }

        public override string Html()
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("<div id=\"elem_{0}\" class=\"element element-select\">", Id));
            sb.AppendLine("<div class=\"handle\"></div>");
            sb.AppendLine(string.Format("<label for=\"{0}\">{1}</label>", Name, Label));

            sb.AppendLine(string.Format("<select name=\"{0}\" id=\"{0}\"{1}{2}>", Name, (MultiSelect ? " multiple" : ""), (Disabled ? " disabled" : "")));

            if(Options != null)
            {
                for(int i = 0; i < Options.Count; i++)
                {
                    sb.AppendLine(string.Format("<option value=\"{0}\"{2}>{1}</option>", Options[i].Value, Options[i].Text, (Options[i].Disabled ? " disabled" : "")));
                }
            }
            sb.AppendLine(string.Format("</select>"));

            if(Description != "")
                sb.AppendLine(string.Format("<div class=\"element-description\">{0}</div>", Description));

            if(Disabled)
                sb.AppendLine(string.Format("<hidden name=\"{0}\" value=\"{1}\"/>", Name, Answer));

            sb.AppendLine(string.Format("</div>"));

            return sb.ToString();
        }

        public override string EditHtml()
        {
            throw new NotImplementedException();
        }
    }

    public class Date : Element
    {
        public string Format { get; set; }

        public override string Html()
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("<div id=\"elem_{0}\" class=\"element element-date\">", Id));
            sb.AppendLine("<div class=\"handle\"></div>");
            sb.AppendLine(string.Format("<label for=\"{0}\">{1}</label>", Name, Label));

            sb.AppendLine(string.Format("<input type=\"text\" name=\"{0}\" id=\"{0}\" value=\"{1}\"{2} />", Name, DefaultValue, (Disabled ? " disabled" : "")));

            if(Description != "")
                sb.AppendLine(string.Format("<div class=\"element-description\">{0}</div>", Description));

            if(Disabled)
                sb.AppendLine(string.Format("<hidden name=\"{0}\" value=\"{1}\"/>", Name, Answer));

            sb.AppendLine(string.Format("</div>"));

            return sb.ToString();
        }

        private DateTime? ConvertToDateTime(string value)
        {
            if(string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            if(string.IsNullOrEmpty(Format))
                return null;

            DateTime dateTime;
            if(DateTime.TryParseExact(value, Format, null, DateTimeStyles.None, out dateTime))
                return dateTime;

            return null;
        }

        public override string EditHtml()
        {
            throw new NotImplementedException();
        }
    }

    public class Paragraph : Element
    {
        public override string Html()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("<div id=\"elem_{0}\" class=\"element element-paragraph\"><div class=\"handle\"></div>{1}</div>", Id, Label));
            return sb.ToString();
        }

        public override string EditHtml()
        {
            throw new NotImplementedException();
        }
    }

    public class Choice : Element
    {
        public bool Checkbox { get; set; }
        public IList<Option> Options { get; set; }
        public Columns Columns { get; set; }

        public override string Html()
        {
            throw new NotImplementedException();
        }

        private string GetColumnCss()
        {
            if(Columns.One == Columns)
                return "columns-one";

            if(Columns.Two == Columns)
                return "columns-two";

            if(Columns.Three == Columns)
                return "columns-three";

            if(Columns.SideBySide == Columns)
                return "side-by-side";

            return "";
        }

        public override string EditHtml()
        {
            throw new NotImplementedException();
        }
    }

    public class Option
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Disabled { get; set; }
    }
}
