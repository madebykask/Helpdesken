using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace ECT.Web.Models
{
    public static class BaseModelExtensions
    {
        public static HtmlString Label(this BaseModel model, string name)
        {
            return Label(model, name, model.Language);
        }

        public static HtmlString Label(this BaseModel model, string name, string language)
        {
            var sb = new StringBuilder();
            var node = model.GetElement(name);

            if(node == null || !node.HasAttributes || node.Attribute("label") == null)
                return new HtmlString(sb.ToString());

            var asterisk = string.Empty;

            if(node.Attribute("mandatory") != null)
                asterisk = " <span class=\"text-error asterisk asterisk_" + name + "\">*</span>";

            sb.Append(model.Translate(node.Attribute("label").Value, language) + asterisk);

            return new HtmlString(sb.ToString());
        }

        public static HtmlString Element(this BaseModel model, string name)
        {
            var readOnly = model.ReadOnly(name);
            return Element(model, name, model.Language, readOnly, null);
        }

        public static HtmlString Element(this BaseModel model, string name, string css)
        {
            var readOnly = model.ReadOnly(name);
            return Element(model, name, model.Language, readOnly, css);
        }

        public static HtmlString Element(this BaseModel model, string name, string[] workingGroups)
        {
            var readOnly = model.ReadOnly(name, workingGroups);
            return Element(model, name, model.Language, readOnly, null);
        }

        public static HtmlString Element(this BaseModel model, string name, string language, bool readOnly, string css)
        {
            var sb = new StringBuilder();
            var node = model.GetElement(name);

            if(node == null || !node.HasAttributes || node.Attribute("type") == null)
                return new HtmlString(sb.ToString());

            var defaultValue = (node.Attribute("default") != null && !string.IsNullOrEmpty(node.Attribute("default").Value)) ? node.Attribute("default").Value : "";
            var answer = node.Descendants("answer").FirstOrDefault();
            var error = node.Descendants("error").FirstOrDefault();
            var disabled = readOnly ? " disabled" : "";

            if(css == null)
                css = "";

            if(node.Attribute("type").Value == "hidden")
            {
                sb.Append("<input name=\"" + node.Attribute("name").Value
                          + "\" id=\"" + node.Attribute("name").Value + "\" type=\"hidden\" value=\""
                          + (answer != null ? answer.Value : defaultValue) + "\" " + disabled + " />");
            }

            if(node.Attribute("type").Value == "text")
            {
                sb.Append("<input class=\"" + css + "\" name=\"" + node.Attribute("name").Value
                          + "\" id=\"" + node.Attribute("name").Value + "\" type=\"text\" value=\""
                          + (answer != null ? answer.Value : defaultValue) + "\" " + disabled + " />");
            }

            if(node.Attribute("type").Value == "typeahead")
            {
                sb.Append("<input class=\"typeahead " + css + "\" autocomplete=\"off\" name=\"" + node.Attribute("name").Value
                          + "\" id=\"" + node.Attribute("name").Value + "\" type=\"text\" data-provide=\"typeahead\" value=\""
                          + (answer != null ? answer.Value : "") + "\" " + disabled + " />");
            }

            if(node.Attribute("type").Value == "textarea")
            {
                sb.Append("<textarea rows=\"7\" class=\"" + css + "\" name=\"" + node.Attribute("name").Value
                          + "\"" + disabled + ">" + (answer != null ? answer.Value : "") + "</textarea>");
            }

            if(node.Attribute("type").Value == "file")
            {
                sb.Append("<input class=\"" + css + "\" name=\"" + node.Attribute("name").Value
                          + "\" id=\"" + node.Attribute("name").Value + "\" type=\"file\" multiple=\"multiple\" " + disabled + " />");
            }

            if(node.Attribute("type").Value == "date")
            {
                var format = string.Empty;

                if(node.Attribute("format") != null)
                    format = node.Attribute("format").Value;

                if(string.IsNullOrEmpty(css))
                    css = "span12";

                if(defaultValue == "now")
                    defaultValue = DateTime.Now.ToString(format);

                sb.Append("<div id=\"date_" + node.Attribute("name").Value + "\" class=\"input-append date dateie " + disabled + "\" data-date=\"" + (answer != null ? answer.Value : "") + "\" data-date-format=\"" + format.ToLower() + "\" >");
                sb.Append("<input name=\"" + node.Attribute("name").Value
                          + "\" id=\"" + node.Attribute("name").Value + "\" class=\""
                          + css + " append-ie8\" type=\"text\" value=\"" + (answer != null ? answer.Value : defaultValue) + "\" " + disabled + " />");
                sb.Append("<span class=\"add-on\"><i class=\"icon-calendar\"></i></span>");
                sb.Append("</div>");
            }

            if(node.Attribute("type").Value == "checkbox")
            {
                var on = node.Element("on");
                var off = node.Element("off");

                if(@on != null & off != null)
                {
                    var onValue = @on.Attribute("value") != null ? @on.Attribute("value").Value : model.DefaultValue;
                    var offValue = off.Attribute("value") != null ? off.Attribute("value").Value : model.DefaultValue;
                    var selected = (answer != null && answer.Value.Split(',')[0] == onValue) ? " checked=\"checked\"" : "";

                    sb.Append("<label class=\"inline checkbox\">");
                    sb.Append("<input class=\"" + css + "\" name=\"" + node.Attribute("name").Value
                              + "\" type=\"checkbox\" value=\"" + onValue + "\" " + selected + "  " + disabled + " />");
                    sb.Append(model.Translate(@on.Value, model.Language));

                    sb.Append("</label>");
                    sb.Append("<input name=\"" + node.Attribute("name").Value + "\" type=\"hidden\" value=\"" + offValue + "\" " + disabled + " />");
                }
            }

            if(node.Attribute("type").Value == "radio")
            {
                var options = node.Descendants("options").FirstOrDefault();

                if(options != null)
                {
                    foreach(var option in options.Descendants())
                    {
                        var value = option.Attribute("value") != null ? option.Attribute("value").Value : model.DefaultValue;
                        var selected = (answer != null && answer.Value == value) ? " checked=\"checked\"" : "";

                        sb.Append("<label class=\"inline radio\">");
                        sb.Append("<input class=\"" + css + "\" name=\"" + node.Attribute("name").Value
                                  + "\" type=\"radio\" value=\"" + value + "\" " + selected + " " + disabled + " />");
                        sb.Append(model.Translate(option.Value, model.Language));
                        sb.Append("</label>");
                    }
                }
            }

            if(node.Attribute("type").Value == "multi")
            {
                var options = node.Descendants("options").FirstOrDefault();
                var optGroups = node.Descendants("optgroup");

                var elements = optGroups as XElement[] ?? optGroups.ToArray();
                if(elements.Any())
                {
                    sb.Append(MultiList(model, node, options, answer, css, disabled));
                }
                else
                {
                    if(options != null)
                    {
                        foreach(var option in options.Descendants())
                        {
                            var value = option.Attribute("value") != null ? option.Attribute("value").Value : model.DefaultValue;
                            var selected = (answer != null && !string.IsNullOrEmpty(answer.Value) && answer.Value.Split(',').Contains(value)) ? " checked=\"checked\"" : "";

                            sb.Append("<label class=\"checkbox\">");
                            sb.Append("<input class=\"" + css + "\" name=\"" + node.Attribute("name").Value
                                      + "\" type=\"checkbox\" value=\"" + value + "\" " + selected + " " + disabled + " />");
                            sb.Append(model.Translate(option.Value, model.Language));
                            sb.Append("</label>");
                        }
                    }
                }
                sb.Append("<input name=\"" + node.Attribute("name").Value + "\" type=\"hidden\" value=\"\" " + disabled + " />");
            }

            

            if(node.Attribute("type").Value == "select")
            {
                var options = node.Descendants("options").FirstOrDefault();
                var predefined = node.Descendants("predefined");
                var xElements = predefined as XElement[] ?? predefined.ToArray();
                css = (xElements.Any() ? "predefined " + css : css);

                sb.Append("<select class=\"" + css + "\" name=\"" + node.Attribute("name").Value + "\" id=\"" + node.Attribute("name").Value + "\"" + disabled + ">");
                if(options != null)
                {
                    foreach(var option in options.Descendants())
                    {
                        var value = option.Attribute("value") != null ? option.Attribute("value").Value : model.DefaultValue;
                        var optionValue = (value != model.DefaultValue) ? " value=\"" + value + "\"" : model.DefaultValue;

                        sb.Append("<option" + optionValue + ((answer != null && answer.Value == value) ? " selected=\"selected\"" : "") + ">");
                        sb.Append(model.Translate(option.Value, language));
                        sb.Append("</option>");
                    }
                }

                sb.Append("</select>");

                if(xElements.Any())
                {
                    foreach(var pre in xElements)
                        sb.Append("<div style=\"display:none;\" class=\"predefined_" + node.Attribute("name").Value + "\">" + model.ToJson(pre) + "</div>");
                }
            }

            if(node.Attribute("type").Value == "typeahead-simple")
            {
                var source = "";
                if(node.Attribute("source") != null)
                {
                    var comma = false;
                    source = "[";

                    var options = node.Descendants("option").ToArray();

                    for(int i = 0; i < options.Length; i++)
                    {
                        if(options[i].Value == "") continue;
                        if(comma)
                            source += "\",\"" + options[i].Value + "";
                        else
                        {
                            comma = true;
                            source += "\"" + options[i].Value + "";
                        }
                    }

                    source += "\"]";
                }

                var dependent = node.Element("dependent") != null ? node.Element("dependent").Attribute("name").Value : "";

                sb.Append("<input data-dependent=\"" + dependent + "\" data-node=" + node.Attribute("name").Value
                          + " class=\"typeahead-simple " + css + "\" autocomplete=\"off\" name=\"" + node.Attribute("name").Value
                          + "\" id=\"" + node.Attribute("name").Value + "\" type=\"text\" data-provide=\"typeahead\" value=\""
                          + (answer != null ? answer.Value : "") + "\" " + disabled + " data-source='" + source + "' />");
            }

            if(readOnly)
                sb.Append("<input type=\"hidden\" name=\"" + node.Attribute("name").Value + "\" value=\"" + ((answer != null) ? answer.Value : defaultValue) + "\" />");

            if(error == null) return new HtmlString(sb.ToString());

            sb.Append("<label for=\"" + node.Attribute("name").Value + "\" class=\"error\" >");
            sb.Append(error.Value);
            sb.Append("</label>");

            return new HtmlString(sb.ToString());
        }

        public static string[] GetDataChangeFields(this BaseModel model, string value)
        {
            var node = model.GetNode("dependencies");

            if(node == null || !node.HasElements) return new string[] { };

            var firstOrDefault = node.Descendants().FirstOrDefault(x => x.Attribute("value").Value == value);
            if(firstOrDefault == null) return new string[] { };

            var lines = firstOrDefault.Value.Replace("\r\n", "").Split(',');
            var elems = new string[lines.Length];
            for(var i = 0; i < lines.Length; i++)
                elems[i] = lines[i].Trim();

            return elems;
        }

        public static string GetDataChangeSectionName(this BaseModel model, string value)
        {
            var node = model.GetElement("DataChangeSelection");

            if(node == null || !node.HasAttributes || !node.HasElements) return string.Empty;

            var option = node.Descendants("option").FirstOrDefault(x => x.Attribute("value").Value == value);

            return option != null ? option.Value : string.Empty;
        }

        public static string GetOptionText(this BaseModel model, string name, string value)
        {
            var node = model.GetElement(name);

            if(node == null || !node.HasAttributes || !node.HasElements) return string.Empty;

            var option = node.Descendants("option").FirstOrDefault(x => x.Attribute("value").Value == value);

            return option != null ? option.Value : string.Empty;
        }

        private static string MultiList(BaseModel model, XElement parent, XElement options, XElement answer, string css, string disabled)
        {
            var sb = new StringBuilder(string.Empty);

            if(options != null)
            {
                var nodes = options.Elements();
                sb.Append("<ul class=\"clearfix\">");
                foreach(var node in nodes)
                {
                    sb.Append("<li>");
                    if(node.Name == "optgroup")
                    {
                        sb.Append("<p><strong>" + model.Translate(node.Attribute("label").Value) + "</strong></p>");
                        sb.Append(MultiList(model, parent, node, answer, css, disabled));
                    }
                    else
                    {
                        var value = node.Attribute("value") != null ? node.Attribute("value").Value : model.DefaultValue;
                        var selected = (answer != null && !string.IsNullOrEmpty(answer.Value) && answer.Value.Split(',').Contains(value)) ? " checked=\"checked\"" : "";

                        sb.Append("<label class=\"checkbox\">");
                        sb.Append("<input class=\"multi " + css + "\" id=\"" + parent.Attribute("name").Value + "_" + value + "\" name=\"" + parent.Attribute("name").Value
                                  + "\" type=\"checkbox\" value=\"" + value + "\" " + selected + " " + disabled + " />");
                        sb.Append(model.Translate(node.Value));
                        sb.Append("</label>");

                        // notice
                        var notice = node.Attribute("notice");
                        if(notice != null)
                        {
                            sb.Append("<span id=\"notice_" + parent.Attribute("name").Value + "_" + value + "\" style=\"display:none;\" class=\"notice\">" + model.Translate(notice.Value) + "</span>");
                        }
                    }
                    sb.Append("</li>");

                }
                sb.Append("</ul>");
            }

            return sb.ToString();
        }

        //  Obsolete
        /* 
        [Obsolete("Call function 'Element' instead.", false)]
        public static HtmlString Input(this BaseModel model, string name)
        {
            var readOnly = model.ReadOnly(name);
            return Input(model, name, model.Language, readOnly, null);
        }

        [Obsolete("Call function 'Element' instead.", false)]
        public static HtmlString Input(this BaseModel model, string name, string css)
        {
            var readOnly = model.ReadOnly(name);
            return Input(model, name, model.Language, readOnly, css);
        }

        [Obsolete("Call function 'Element' instead.", false)]
        public static HtmlString Input(this BaseModel model, string name, string language, bool readOnly, string css)
        {
            var sb = new StringBuilder();
            var node = model.GetElement(name);

            if(node == null || !node.HasAttributes || node.Attribute("type") == null)
                return new HtmlString(sb.ToString());

            var answer = node.Descendants("answer").FirstOrDefault();
            var error = node.Descendants("error").FirstOrDefault();
            var disabled = readOnly ? " disabled" : "";
            if(css == null)
                css = "";

            if(node.Attribute("type").Value == "text")
                sb.Append("<input class=\"" + css + "\" name=\"" + node.Attribute("name").Value
                          + "\" type=\"text\" value=\"" + (answer != null ? answer.Value : "") + "\" " + disabled + " />");

            if(node.Attribute("type").Value == "textarea")
                sb.Append("<textarea class=\"" + css + "\" name=\"" + node.Attribute("name").Value
                          + "\"" + disabled + ">" + (answer != null ? answer.Value : "") + "</textarea>");

            if(node.Attribute("type").Value == "date")
            {
                var format = "dd-mm-yyyy";

                if(node.Attribute("format") != null)
                    format = node.Attribute("format").Value;

                if(string.IsNullOrEmpty(css))
                    css = "span12";

                sb.Append("<div class=\"input-append date dateie " + disabled + "\" data-date=\"\" data-date-format=\"" + format + "\" >");
                sb.Append("<input name=\"" + node.Attribute("name").Value
                          + "\" class=\"" + css + "\" type=\"text\" value=\"" + (answer != null ? answer.Value : "") + "\" " + disabled + " />");
                sb.Append("<span class=\"add-on\"><i class=\"icon-calendar\"></i></span>");
                sb.Append("</div>");
            }

            if(node.Attribute("type").Value == "checkbox")
            {
                var on = node.Element("on");
                var off = node.Element("off");

                if(on != null && off != null)
                {
                    var onValue = on.Attribute("value") != null ? on.Attribute("value").Value : model.DefaultValue;
                    var offValue = off.Attribute("value") != null ? off.Attribute("value").Value : model.DefaultValue;
                    var selected = (answer != null && answer.Value.Split(',')[0] == onValue) ? " checked=\"checked\"" : "";

                    sb.Append("<label class=\"inline checkbox\">");
                    sb.Append("<input class=\"" + css + "\" name=\"" + node.Attribute("name").Value
                              + "\" type=\"checkbox\" value=\"" + onValue + "\" " + selected + "  " + disabled + " />");
                    sb.Append(model.Translate(on.Value, model.Language));

                    sb.Append("</label>");
                    sb.Append("<input name=\"" + node.Attribute("name").Value
                              + "\" type=\"hidden\" value=\"" + offValue + "\" " + disabled + " />");
                }
            }

            if(node.Attribute("type").Value == "radio")
            {
                var options = node.Descendants("options").FirstOrDefault();

                if(options != null)
                {
                    foreach(var option in options.Descendants())
                    {
                        var value = option.Attribute("value") != null ? option.Attribute("value").Value : model.DefaultValue;
                        var selected = (answer != null && answer.Value == value) ? " checked=\"checked\"" : "";

                        sb.Append("<label class=\"inline radio\">");
                        sb.Append("<input class=\"" + css + "\" name=\"" + node.Attribute("name").Value
                                  + "\" type=\"radio\" value=\"" + value + "\" " + selected + " " + disabled + " />");
                        sb.Append(model.Translate(option.Value, model.Language));
                        sb.Append("</label>");
                    }
                }
            }

            if(node.Attribute("type").Value == "multi")
            {
                var options = node.Descendants("options").FirstOrDefault();
                var optGroups = node.Descendants("optgroup");

                var xElements = optGroups as XElement[] ?? optGroups.ToArray();
                if(xElements.Any())
                {
                    foreach(var optGroup in xElements)
                    {
                        if(optGroup.HasAttributes && optGroup.Attribute("label") != null)
                            sb.Append("<p><strong>" + optGroup.Attribute("label").Value + "</strong></p>");
                        foreach(var option in optGroup.Descendants())
                        {
                            var value = option.Attribute("value") != null ? option.Attribute("value").Value : model.DefaultValue;
                            var selected = (answer != null && answer.Value.Split(',').Contains(value)) ? " checked=\"checked\"" : "";

                            sb.Append("<label class=\"checkbox\">");
                            sb.Append("<input class=\"" + css + "\" name=\"" + node.Attribute("name").Value
                                      + "\" type=\"checkbox\" value=\"" + value + "\" " + selected + " " + disabled + " />");
                            sb.Append(model.Translate(option.Value, model.Language));
                            sb.Append("</label>");
                        }
                    }
                }
                else
                {
                    if(options != null)
                    {
                        foreach(var option in options.Descendants())
                        {
                            var value = option.Attribute("value") != null ? option.Attribute("value").Value : model.DefaultValue;
                            var selected = (answer != null && answer.Value.Split(',').Contains(value)) ? " checked=\"checked\"" : "";

                            sb.Append("<label class=\"checkbox\">");
                            sb.Append("<input class=\"" + css + "\" name=\"" + node.Attribute("name").Value
                                      + "\" type=\"checkbox\" value=\"" + value + "\" " + selected + " " + disabled + " />");
                            sb.Append(model.Translate(option.Value, model.Language));
                            sb.Append("</label>");
                        }
                    }
                }
            }

            if(readOnly)
            {
                sb.Append("<input type=\"hidden\" name=\""
                          + node.Attribute("name").Value + "\" value=\"" + ((answer != null) ? answer.Value : "") + "\" />");
            }

            if(error == null) return new HtmlString(sb.ToString());

            sb.Append("<p class=\"text-error\" >");
            sb.Append(model.Translate(error.Value, language));
            sb.Append("</p>");

            return new HtmlString(sb.ToString());
        }

        [Obsolete("Call function 'Element' instead.", false)]
        public static HtmlString Select(this BaseModel model, string name)
        {
            var readOnly = model.ReadOnly(name);
            return Select(model, name, model.Language, readOnly, null);
        }

        [Obsolete("Call function 'Element' instead.", false)]
        public static HtmlString Select(this BaseModel model, string name, string css)
        {
            var readOnly = model.ReadOnly(name);
            return Select(model, name, model.Language, readOnly, css);
        }

        [Obsolete("Call function 'Element' instead.", false)]
        public static HtmlString Select(this BaseModel model, string name, string language, bool readOnly, string css)
        {
            var sb = new StringBuilder();

            var node = model.GetElement(name);

            if(node == null || !node.HasAttributes || node.Attribute("type") == null)
                return new HtmlString(sb.ToString());

            var answer = node.Descendants("answer").FirstOrDefault();
            var error = node.Descendants("error").FirstOrDefault();

            if(node.Attribute("type").Value == "select")
            {
                if(css == null)
                    css = "";

                var predefined = node.Descendants("predefined");
                var options = node.Descendants("options").FirstOrDefault();
                var xElements = predefined as XElement[] ?? predefined.ToArray();
                var cssClass = (xElements.Any() ? "predefined " : "");
                var disabled = readOnly ? " disabled" : "";

                sb.Append("<select class=\"" + cssClass + css + "\" name=\"" + node.Attribute("name").Value + "\" id=\"" + node.Attribute("name").Value + "\"" + disabled + ">");
                if(options != null)
                {
                    foreach(var option in options.Descendants())
                    {
                        var value = option.Attribute("value") != null ? option.Attribute("value").Value : model.DefaultValue;
                        var optionValue = (value != model.DefaultValue) ? " value=\"" + value + "\"" : model.DefaultValue;

                        sb.Append("<option" + optionValue + ((answer != null
                                                              && answer.Value == value) ? " selected=\"selected\"" : "") + ">");
                        sb.Append(model.Translate(option.Value, language));
                        sb.Append("</option>");
                    }
                }

                sb.Append("</select>");

                if(readOnly)
                {
                    sb.Append("<input type=\"hidden\" name=\""
                              + node.Attribute("name").Value + "\" value=\"" + ((answer != null) ? answer.Value : "") + "\" />");
                }

                //if(xElements.Any())
                //{
                //    foreach(var pre in xElements)
                //        sb.Append("<input class=\"predefined_" + node.Attribute("name").Value + "\" type=\"hidden\" value='" + model.ToJson(pre) + "' />");
                //}
            }

            if(error == null) return new HtmlString(sb.ToString());

            sb.Append("<p class=\"text-error\" >");
            sb.Append(model.Translate(error.Value, language));
            sb.Append("</p>");

            return new HtmlString(sb.ToString());
        }
        */
    }
}