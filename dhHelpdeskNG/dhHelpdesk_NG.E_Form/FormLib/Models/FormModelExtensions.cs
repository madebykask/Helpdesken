using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using DH.Helpdesk.EForm.Model.Entities;


namespace DH.Helpdesk.EForm.FormLib.Models
{
    public static class FormModelExtensions
    {
        public static HtmlString Label(this FormModel model, string name)
        {
            return Label(model, name, model.Language);
        }

        public static HtmlString Label(this FormModel model, string name, string language)
        {
            var sb = new StringBuilder();
            var node = model.GetElement(name);

            if (node == null || !node.HasAttributes || node.Attribute("label") == null)
                return new HtmlString(sb.ToString());

            var asterisk = string.Empty;

            if (node.Attribute("mandatory") != null)
                asterisk = " <span class=\"text-error asterisk asterisk_" + name + "\">*</span>";

            if (model.UserGroups != null)
            {
                var state = model.GetNode("state");

                if (state != null || state.HasAttributes)
                {
                    var currentState = state.Attribute("status") != null ? state.Attribute("status").Value : "";
                    if (currentState != string.Empty)
                    {
                        var status = state.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
                        if (status != null)
                        {
                            var userGroups = status.Descendants("userGroup").Where(x => model.UserGroups.Contains(x.Attribute("name").Value));
                            if (userGroups.Any())
                            {
                                foreach (var userGroup in userGroups)
                                {
                                    var optionalFields = userGroup.Descendants("optionalFields").FirstOrDefault();

                                    if (optionalFields == null || optionalFields.Value == string.Empty)
                                        continue;

                                    if (optionalFields.Value.Split(',').Contains(name))
                                        asterisk = string.Empty;
                                }
                            }
                        }
                    }
                }
            }

            sb.Append(model.Translate(node.Attribute("label").Value, language) + asterisk);

            return new HtmlString(sb.ToString());
        }

        public static string GetDocumentAnswer(this FormModel model, string documentFieldName)
        {
            DocumentField documentField = new DocumentField();

            //find contract field based on the value from name
            try
            {
                documentField = model.DocumentData.DocumentFields.Find(x => x.DocumentFieldName == documentFieldName);
            }
            catch (Exception)
            {
            }

            //the answer don´t exist
            if (documentField == null)
            {
                return model.GetDocumentLabelEmpty(documentFieldName).ToUpper();
            }

            //if mandatory and value is null/empty
            if (documentField.IsMandatory == true && string.IsNullOrEmpty(documentField.DocumentFieldValue.Trim()))
            {
                return model.GetDocumentLabelEmpty(documentFieldName).ToUpper();
            }

            return documentField.DocumentFieldValue;
        }

        public static string GetDocumentAnswer(this FormModel model, string documentFieldName, bool isMandatory)
        {
            SetDocumentMandatoryField(model, documentFieldName, isMandatory);

            return GetDocumentAnswer(model, documentFieldName);

        }

        public static HtmlString GetStyleAnswer(this FormModel model, string documentFieldName)
        {
            SetDocumentMandatoryField(model, documentFieldName, false);

            DocumentField documentField = new DocumentField();

            //find contract field based on the value from name
            try
            {
                documentField = model.DocumentData.DocumentFields.Find(x => x.DocumentFieldName == documentFieldName);
            }
            catch (Exception)
            {
                return new HtmlString("");
            }

            //the answer don´t exist
            if (documentField == null)
            {
                return new HtmlString("");
            }


            return new HtmlString(documentField.DocumentFieldValue);



        }

        public static string GetDocumentTranslatedAnswer(this FormModel model, string documentFieldName, bool toLower)
        {
            DocumentField documentField = new DocumentField();

            //find contract field based on the value from name
            try
            {
                documentField = model.DocumentData.DocumentFields.Find(x => x.DocumentFieldName == documentFieldName);
            }
            catch (Exception)
            {
            }

            //the answer don´t exist
            if (documentField == null)
            {
                return model.GetDocumentLabelEmpty(documentFieldName).ToUpper();
            }

            //if mandatory and value is null/empty
            if (documentField.IsMandatory == true && string.IsNullOrEmpty(documentField.DocumentFieldValue))
            {
                return model.GetDocumentLabelEmpty(documentFieldName).ToUpper();
            }

            if (toLower == true)
            {
                return model.Translate(documentField.DocumentFieldValue, model.Language).ToLower();
            }
            else
            {
                return model.Translate(documentField.DocumentFieldValue, model.Language);
            }
        }

        /// <summary>
        /// Return translated answer in lower cases
        /// </summary>
        public static string GetDocumentTranslatedAnswer(this FormModel model, string documentFieldName)
        {
            return GetDocumentTranslatedAnswer(model, documentFieldName, true);
        }

        public static void SetDocumentMandatoryField(this FormModel model, string documentFieldName, bool isMandatory)
        {
            try
            {
                model.DocumentData.DocumentFields.Find(x => x.DocumentFieldName == documentFieldName).IsMandatory = isMandatory;
            }
            catch (Exception)
            {
                // throw;
            }
        }

        /// <summary>
        /// Set DocumentMandatory field to true
        /// </summary>
        public static void SetDocumentMandatoryField(this FormModel model, string documentFieldName)
        {
            SetDocumentMandatoryField(model, documentFieldName, true);
        }

        //public static string GetDocumentText(this FormModel model, string value1)
        //{
        //    return GetDocumentText(model, "", value1, "");
        //}

        /// <summary>
        /// Get Text from Database
        /// </summary>
        /// <param name="value1">key to find the correct text</param>
        public static string GetDocumentText(this FormModel model, string value1)
        {
            return GetDocumentText(model, "", value1, "", "", "", "", null, null);
        }

        /// <summary>
        /// Get Text from Database
        /// </summary>
        /// <param name="value1">key to find the correct text</param>
        /// <param name="replaceValue1">value to replace {0} with</param>
        public static string GetDocumentText(this FormModel model, string value1, string replaceValue1)
        {
            return GetDocumentText(model, "", value1, "", "", "", replaceValue1, null, null);
        }

        /// <summary>
        /// Get Text from Database, Country Specific
        /// </summary>
        /// <param name="textType">Type of text to find</param>
        /// <param name="value1">key to find the correct text</param>
        /// <param name="replaceValue1">value to replace {0} with</param>
        /// <param name="customerId">customerId</param>
        public static string GetDocumentText(this FormModel model, string textType, string value1, string replaceValue1, int customerId)
        {
            return GetDocumentText(model, textType, value1, "", "", "", replaceValue1, customerId, null);
        }

        /// <summary>
        /// Get Text from Database
        /// </summary>
        /// <param name="textType">Type of text to find</param>
        /// <param name="value1">key to find the correct text</param>
        /// <param name="replaceValue1">value to replace {0} with</param>
        public static string GetDocumentText(this FormModel model, string textType, string value1, string replaceValue1)
        {
            return GetDocumentText(model, textType, value1, "", "", "", replaceValue1, null, null);
        }

        /// <summary>
        /// Get Text from Database, with IF
        /// </summary>
        /// <param name="textType">Type of text to find</param>
        /// <param name="value1">key to find the correct text</param>
        /// <param name="operator1">= or !=</param>        
        /// <param name="value2">key nr 2 to find the correct text</param>    
        /// <param name="operator2">= or !=</param>     
        public static string GetDocumentText(this FormModel model, string textType, string value1, string operator1, string value2, string operator2)
        {
            return GetDocumentText(model, textType, value1, operator1, value2, operator2, "", null, null);
        }

        /// <summary>
        /// Get Text from Database, with REPLACE
        /// </summary>
        /// <param name="textType">Type of text to find</param>
        /// <param name="value1">key to find the correct text</param>
        /// <param name="operator1">= or !=</param>        
        /// <param name="value2">key nr 2 to find the correct text</param>    
        /// <param name="operator2">= or !=</param>     
        /// <param name="replaceValue1">value to replace {0} with</param>
        public static string GetDocumentText(this FormModel model, string textType, string value1, string operator1, string value2, string operator2, string replaceValue1)
        {
            return GetDocumentText(model, textType, value1, operator1, value2, operator2, replaceValue1, null, null);
        }

        /// <summary>
        /// Get Text from Database, with REPLACE, Country Specific
        /// </summary>
        /// <param name="textType">Type of text to find</param>
        /// <param name="value1">key to find the correct text</param>
        /// <param name="operator1">= or !=</param>        
        /// <param name="value2">key nr 2 to find the correct text</param>    
        /// <param name="operator2">= or !=</param>     
        /// <param name="replaceValue1">value to replace {0} with</param>
        /// <param name="customerId">customerId</param> 
        public static string GetDocumentText(this FormModel model, string textType, string value1, string operator1, string value2, string operator2, string replaceValue1, int? customerId)
        {
            return GetDocumentText(model, textType, value1, operator1, value2, operator2, replaceValue1, customerId, null);
        }

        public static string GetDocumentText(this FormModel model, string textType, string value1, string operator1, string value2, string operator2, string replaceValue1, int? customerId, Guid? formGuid)
        {
            //Quick fix! /TAN
            DH.Helpdesk.EForm.Model.Contrete.DocumentTextRepository documentTextRepository = new DH.Helpdesk.EForm.Model.Contrete.DocumentTextRepository(System.Configuration.ConfigurationManager.ConnectionStrings["DSN"].ConnectionString);

            return documentTextRepository.GetText(textType, value1, operator1, value2, operator2, replaceValue1, customerId, formGuid);
        }

        public static HtmlString Element(this FormModel model, string name)
        {
            var readOnly = model.ReadOnly(name);
            return Element(model, name, model.Language, readOnly, null, true);
        }

        public static HtmlString Element(this FormModel model, string name, bool autoComplete = true)
        {
            var readOnly = model.ReadOnly(name);
            return Element(model, name, model.Language, readOnly, null, autoComplete);
        }

        public static HtmlString Element(this FormModel model, string name, string css)
        {
            var readOnly = model.ReadOnly(name);
            return Element(model, name, model.Language, readOnly, css, false);
        }

        public static HtmlString Element(this FormModel model, string name, string[] workingGroups)
        {
            var readOnly = model.ReadOnly(name, workingGroups);
            return Element(model, name, model.Language, readOnly, null, true);
        }

        public static HtmlString Element(this FormModel model, string name, string[] workingGroups, bool locked)
        {
            var readOnly = model.EditOnly(name, workingGroups);
            return Element(model, name, model.Language, readOnly, null, true);
        }

        public static HtmlString Element(this FormModel model, string name, string language, bool readOnly, string css)
        {
            return Element(model, name, model.Language, readOnly, css, true);
        }

        public static HtmlString Element(this FormModel model, string formFieldName, string language, bool readOnly, string css, bool autoComplete)
        {
            var sb = new StringBuilder();
            var node = model.GetElement(formFieldName);

            if (node == null || !node.HasAttributes || node.Attribute("type") == null)
                return new HtmlString(sb.ToString());

            var elementName = (node.Attribute("name") != null && !string.IsNullOrEmpty(node.Attribute("name").Value) ? node.Attribute("name").Value : "");

            var defaultValue = (node.Attribute("default") != null && !string.IsNullOrEmpty(node.Attribute("default").Value)) ? node.Attribute("default").Value : "";
            var answer = node.Descendants("answer").FirstOrDefault();
            var error = node.Descendants("error").FirstOrDefault();
            var disabled = readOnly ? " disabled" : "";
            var cssclass = ((node.Attribute("cssclass") != null) ? " " + node.Attribute("cssclass").Value.ToString() : "");
            var type = (node.Attribute("type") != null ? node.Attribute("type").Value : "");

            //For data attributes
            var dataValue = "";

            //Current Record
            var formfieldidentifier = (node.Attribute("formfieldidentifier") != null && !string.IsNullOrEmpty(node.Attribute("formfieldidentifier").Value)) ? node.Attribute("formfieldidentifier").Value : "";

            //Current Record - set default value to element
            //This works for all except for (New)Company, (New)BusinessUnit, (New)Function and (New)Department. Since they are triggered by EmployeeSearch. /Tan
            if (!string.IsNullOrEmpty(formfieldidentifier))
            {
                bool defaultbyformfieldidentifier = (node.Attribute("defaultbyformfieldidentifier") == null || node.Attribute("defaultbyformfieldidentifier").Value == "1" ? true : false);
                dataValue += " data-defaultbyformfieldidentifier=\"" + (defaultbyformfieldidentifier ? "1" : "0") + "\"";
            }

            ////ReadOnlyIf - For future do not remove this code /TAN
            ////type = element
            ////<readOnlyIf type="element" name="IsManager">Yes</readOnlyIf>
            //var readOnlyIfs = node.Parent.Descendants("readOnlyIf").Where(x => x.Attribute("type") != null && x.Attribute("type").Value == "element" && x.Attribute("name").Value == name);

            //foreach (var readonlyIf in readOnlyIfs.Where(readonlyIf => readonlyIf.HasAttributes && readonlyIf.Attribute("name") != null))
            //{
            //    //element that should be affected
            //    var readonlyIfTarget = readonlyIf.Parent.Attribute("name").Value;
            //    var readonlyIfValue = readonlyIf.Value;

            //    dataValue += "data-target=\"" + readonlyIfTarget + "\"";
            //    dataValue += "data-target-value=\"" + readonlyIfValue + "\"";

            //    css += " readonlyif";
            //}

            if (!(node == null || !node.HasAttributes || node.Attribute("disabled") == null))
                if (node.Attribute("disabled").Value == "1")
                    disabled = "disabled";

            var autoCompleteStr = autoComplete ? "" : " autocomplete=\"off\"";
            if (css == null)
                css = "";

            if (type == "hidden")
            {
                sb.Append("<input name=\"" + elementName
                          + "\" id=\"" + elementName + "\" type=\"hidden\" value=\""
                          + (answer != null ? answer.Value : defaultValue) + "\" " + disabled + dataValue + " />");
            }

            if (type == "text")
            {
                sb.Append("<input class=\"" + css + cssclass + "\" " + autoCompleteStr + " name=\"" + elementName
                          + "\" id=\"" + elementName + "\" type=\"text\" value=\""
                          + (answer != null ? answer.Value : defaultValue) + "\" " + disabled + dataValue + " />");
            }

            if (type == "label")
            {
                //Label
                sb.Append("<span " + (!string.IsNullOrEmpty(cssclass) ? "class=\"" + cssclass + "\"" : "") + " id=\"" + elementName + "\">" + model.Translate((!string.IsNullOrEmpty(defaultValue) ? defaultValue : "")) + "</span>");

                //Hidden field for saving value in tblFormFieldValues
                sb.Append("<input " + (!string.IsNullOrEmpty(css) ? "class=\"" + css + "\"" : "") + " " + autoCompleteStr + " name=\"" + elementName + "\" id=\"hidden_" + elementName + "\" type=\"hidden\" value=\"" + (!string.IsNullOrEmpty(defaultValue) ? defaultValue : "") + dataValue + "\" />");
            }

            if (type == "typeahead")
            {
                sb.Append("<input class=\"typeahead " + css + "\" autocomplete=\"off\" name=\"" + elementName
                          + "\" id=\"" + elementName + "\" type=\"text\" data-provide=\"typeahead\" value=\""
                          + (answer != null ? answer.Value : "") + "\" " + disabled + dataValue + " />");
            }

            if (type == "textarea")
            {
                sb.Append("<textarea rows=\"7\" class=\"" + css + "\" name=\"" + elementName
                          + "\"" + disabled + dataValue + ">" + (answer != null ? answer.Value : "") + "</textarea>");
            }

            if (type == "file")
            {
                sb.Append("<input class=\"" + css + "\" name=\"" + elementName
                          + "\" id=\"" + elementName + "\" type=\"file\" multiple=\"multiple\" " + disabled + dataValue + " />");
            }

            if (type == "date")
            {
                var format = string.Empty;

                if (node.Attribute("format") != null)
                    format = node.Attribute("format").Value;

                if (string.IsNullOrEmpty(css))
                    css = "span12";

                if (defaultValue == "now")
                    defaultValue = DateTime.Now.ToString(format);

                sb.Append("<div id=\"date_" + elementName + "\" class=\"" + " input-append date dateie " + disabled + "\" data-date=\"" + (answer != null ? answer.Value : "") + "\" data-date-format=\"" + format.ToLower() + "\" >");
                sb.Append("<input name=\"" + elementName
                          + "\" id=\"" + elementName + "\" class=\""
                          + css + cssclass + " append-ie8\" type=\"text\" value=\"" + (answer != null ? answer.Value : defaultValue) + "\" " + disabled + dataValue + " />");

                sb.Append("<span class=\"add-on" + (readOnly ? " disabled" : "") + "\"><i class=\"icon-calendar" + (readOnly ? " disabled" : "") + "\"></i></span>");
                sb.Append("</div>");
            }

            if (type == "checkbox")
            {
                var on = node.Element("on");
                var off = node.Element("off");

                if (on != null & off != null)
                {
                    var onValue = on.Attribute("value") != null ? on.Attribute("value").Value : model.DefaultValue;
                    var offValue = off.Attribute("value") != null ? off.Attribute("value").Value : model.DefaultValue;
                    var selected = (answer != null && answer.Value.Split(',')[0] == onValue) ? " checked=\"checked\"" : "";

                    sb.Append("<label class=\"inline checkbox\">");
                    sb.Append("<input class=\"" + css + cssclass + "\" id=\"" + elementName + "\" name=\"" + elementName
                              + "\" type=\"checkbox\" value=\"" + onValue + "\" " + selected + "  " + disabled + dataValue + " />");
                    sb.Append(model.Translate(on.Value, model.Language, "checkbox"));

                    sb.Append("</label>");
                    sb.Append("<input name=\"" + elementName + "\" type=\"hidden\" value=\"" + offValue + "\" " + disabled + " />");
                }
            }

            if (type == "radio")
            {
                var options = node.Descendants("options").FirstOrDefault();

                if (options != null)
                {
                    foreach (var option in options.Descendants())
                    {
                        var value = option.Attribute("value") != null ? option.Attribute("value").Value : model.DefaultValue;
                        var selected = (answer != null && answer.Value == value) ? " checked=\"checked\"" : "";

                        sb.Append("<label class=\"inline radio\">");
                        sb.Append("<input class=\"" + css + "\" name=\"" + elementName
                                  + "\" type=\"radio\" value=\"" + value + "\" " + selected + " " + disabled + dataValue + " />");
                        sb.Append(model.Translate(option.Value, model.Language, "radio"));
                        sb.Append("</label>");
                    }
                }
            }

            if (type == "multi")
            {
                var options = node.Descendants("options").FirstOrDefault();
                var optGroups = node.Descendants("optgroup");

                var elements = optGroups as XElement[] ?? optGroups.ToArray();
                if (elements.Any())
                {
                    sb.Append(MultiList(model, node, options, answer, css, disabled));
                }
                else
                {
                    if (options != null)
                    {
                        foreach (var option in options.Descendants())
                        {
                            var value = option.Attribute("value") != null ? option.Attribute("value").Value : model.DefaultValue;
                            var selected = (answer != null && !string.IsNullOrEmpty(answer.Value) && answer.Value.Split(',').Contains(value)) ? " checked=\"checked\"" : "";

                            sb.Append("<label class=\"checkbox\">");
                            sb.Append("<input class=\"" + css + "\" name=\"" + elementName
                                      + "\" type=\"checkbox\" value=\"" + value + "\" " + selected + " " + disabled + dataValue + " />");
                            sb.Append(model.Translate(option.Value, model.Language, "multi"));
                            sb.Append("</label>");
                        }
                    }
                }
                sb.Append("<input name=\"" + elementName + "\" type=\"hidden\" value=\"\" " + disabled + " />");
            }

            //TAN 2016-04-16
            //merge "select"/"search-select". only difference is that 
            //'search select' needs to have class: search-select /TAN 2016-04-16
            //'select' - If Loaded through db, populate with default or answer. Only if select! /TAN 2016-04-16
            if (node.Attribute("type").Value == "select" || node.Attribute("type").Value == "search-select")
            
            {
                var options = node.Descendants("options").FirstOrDefault();
                var predefined = node.Descendants("predefined");
                var xElements = predefined as XElement[] ?? predefined.ToArray();
                css = (xElements.Any() ? "predefined " + css : css);

                var searchSelectClass = "";

                if (node.Attribute("type").Value == "search-select")
                {
                    searchSelectClass = " search-select";
                }

                bool translate = (node.Attribute("translate") == null || node.Attribute("translate").Value.ToLower() == "true" ? true : false);

                sb.Append("<select class=\"" + css + cssclass + searchSelectClass +  "\" name=\"" + elementName + "\" id=\"" + elementName + "\"" + disabled + dataValue + ">");
                if (options != null)
                {
                    var optDisabled = string.Empty;

                    foreach (var option in options.Descendants())
                    {
                        optDisabled = string.Empty;

                        if (option.Attribute("disabled") != null)
                        {
                            if (option.Attribute("disabled").Value == "1")
                            {
                                optDisabled = "disabled";
                            }
                        }

                        if (FormLibUtils.IsSelfService() && option.Attribute("disableForSelfService") != null)
                        {
                            optDisabled = " disabled";
                        }

                        var value = option.Attribute("value") != null ? option.Attribute("value").Value : model.DefaultValue;
                        var optionValue = (value != model.DefaultValue) ? " value=\"" + value + "\"" : model.DefaultValue;

                        if (answer == null)
                        {
                            sb.Append("<option" + optionValue + ((defaultValue == value) ? " selected=\"selected\"" : "") + " " + optDisabled + " >");
                        }
                        else
                        {
                            sb.Append("<option" + optionValue + ((answer != null && answer.Value == value) ? " selected=\"selected\"" : "") + " " +  optDisabled + " >");
                        }

                        if (translate)
                        {
                            sb.Append(model.Translate(option.Value, language, type));
                        }
                        else
                        {
                            sb.Append(option.Value);
                        }

                        sb.Append("</option>");
                    }
                    /////////////////////////////
                    // By: AC 2014-10-08  
                    // Checks for source attribute, if that exists and if database-stored dropdown value does not exist in xml, append-, disable-, and select the database-item
                    // Exception for Poland.
                    if (!(model.FormXmlPath.Contains("poland")))
                    {
                        if (node.Attribute("source") == null || node.Attribute("source").Value != "database")
                        {
                            if (answer != null && answer.Value != "")
                            {
                                var o = options.Descendants().Where(x => x.Attributes("value").FirstOrDefault() != null);

                                bool ValueExistsInDropDown = o.Any(x => x.Attributes("value").FirstOrDefault().Value == answer.Value);
                                if (!ValueExistsInDropDown)
                                {
                                    sb.Append("<option value=\"" + answer.Value + "\" selected=\"selected\"" + " disabled" + ">" + answer.Value);
                                    sb.Append("</option>");
                                };
                            }
                        }
                    }
                }
                // By: DL 2015-01-28
                // If Loaded through db, populate with default or answer.
                else if ((node.Attribute("source") == null || node.Attribute("source").Value == "database"))
                {
                    //not for search-select
                    if (type == "select")
                    {

                        if (answer == null)
                        {
                            sb.Append("<option value=\"" + defaultValue + "\" >");

                            if (translate)
                            {
                                sb.Append(model.Translate(defaultValue, language, "select"));
                            }
                            else
                            {
                                sb.Append(defaultValue);
                            }
                        }
                        else
                        {
                            sb.Append("<option value=\"" + answer.Value + "\">");
                            if (translate)
                            {
                                sb.Append(model.Translate(answer.Value, language, "select"));
                            }
                            else
                            {
                                sb.Append(answer.Value);
                            }
                        }

                        sb.Append("</option>");
                    }
                }

                sb.Append("</select>");

                if (xElements.Any())
                {
                    foreach (var pre in xElements)
                        sb.Append("<div style=\"display:none;\" class=\"predefined_" + elementName + "\">" + model.ToJson(pre) + "</div>");
                }

                if (node.Attribute("source") != null && node.Attribute("source").Value == "database")
                {
                    sb.Append("<input type=\"hidden\" id=\"val_" + elementName + "\" value=\"" + ((answer != null) ? answer.Value : defaultValue) + "\" />");
                }
                else
                {
                    //write out a hidden with saved value //TAN 2015-12-11
                    sb.Append("<input type=\"hidden\" id=\"hidden_" + elementName + "\" value=\"" + ((answer != null) ? answer.Value : defaultValue) + "\" />");
                }
            }

            if (type == "typeahead-simple")
            {
                var ci = "";
                if (node.Attribute("customid") != null)
                    ci = node.Attribute("customid").Value;

                var source = "";
                if (node.Attribute("source") != null)
                {
                    var comma = false;
                    source = "[";

                    var options = node.Descendants("option").ToArray();

                    for (int i = 0; i < options.Length; i++)
                    {
                        if (options[i].Value == "") continue;
                        if (comma)
                            source += "\",\"" + options[i].Value + "";
                        else
                        {
                            comma = true;
                            source += "\"" + options[i].Value + "";
                        }
                    }

                    source += "\"]";
                }

                // TODO: What's the difference? DL 20141712
                if (node.Attribute("default") == null)
                {
                    var dependent = node.Element("dependent") != null ? node.Element("dependent").Attribute("name").Value : "";

                    sb.Append("<input data-dependent=\"" + dependent + "\" data-node=" + elementName
                              + " class=\"typeahead-simple " + css + "\" autocomplete=\"off\" name=\"" + elementName
                              + "\" id=\"" + elementName + "\" type=\"text\" data-provide=\"typeahead\" value=\""
                              + (answer != null ? answer.Value : "") + "\" " + disabled + dataValue + " data-source='" + source.Replace("'", "&#39;") + "'" + (ci != "" ? " customid ='" + ci + "'" : "") + " />");
                }
                else
                {
                    var dependent = node.Element("dependent") != null ? node.Element("dependent").Attribute("name").Value : "";

                    sb.Append("<input data-dependent=\"" + dependent + "\" data-node=" + elementName
                              + " class=\"typeahead-simple " + css + "\" autocomplete=\"off\" name=\"" + elementName
                              + "\" id=\"" + elementName + "\" type=\"text\" data-provide=\"typeahead\" value=\""
                              + (answer != null ? answer.Value : defaultValue) + "\" " + disabled + dataValue + " data-source='" + source.Replace("'", "&#39;") + "'" + (ci != "" ? " customid ='" + ci + "'" : "") + " />");
                }
            }

            //Write Helptext if there is no error
            if (error == null)
            {
                sb.Append(HelpText(model, node));
            }

            if (readOnly)
                sb.Append("<input type=\"hidden\" id=\"hidden_" + elementName + "\" name=\"" + elementName + "\" value=\"" + ((answer != null) ? answer.Value : defaultValue) + "\" />");

            if (error == null) return new HtmlString(sb.ToString());

            sb.Append("<label for=\"" + elementName + "\" class=\"error\" >");
            sb.Append(error.Value);
            sb.Append("</label>");

            //Keep here, write out Helptext after error message //TAN
            sb.Append(HelpText(model, node));

            return new HtmlString(sb.ToString());
        }

        public static StringBuilder HelpText(this FormModel model, XElement node)
        {
            StringBuilder sb = new StringBuilder();

            //Keep here, place after error message
            if (node.Attribute("helptext") != null)
            {
                sb.Append("<div id=\"" + "helptext_" + node.Attribute("name").Value + "\" class=\"helptext\">");
                sb.Append(model.Translate((!string.IsNullOrEmpty(node.Attribute("helptext").Value) ? node.Attribute("helptext").Value : "")));
                sb.Append("</div>");
            }

            return sb;
        }

        public static string[] GetDataChangeFields(this FormModel model, string value)
        {
            var node = model.GetNode("dependencies");

            if (node == null || !node.HasElements) return new string[] { };

            var firstOrDefault = node.Descendants().FirstOrDefault(x => x.Attribute("value").Value == value);
            if (firstOrDefault == null) return new string[] { };

            var lines = firstOrDefault.Value.Replace("\r\n", "").Split(',');
            var elems = new string[lines.Length];
            for (var i = 0; i < lines.Length; i++)
                elems[i] = lines[i].Trim();

            return elems;
        }

        public static string GetDataChangeSectionName(this FormModel model, string value)
        {
            var node = model.GetElement("DataChangeSelection");

            if (node == null || !node.HasAttributes || !node.HasElements) return string.Empty;

            var option = node.Descendants("option").FirstOrDefault(x => x.Attribute("value").Value == value);

            return option != null ? option.Value : string.Empty;
        }

        public static string GetOptionText(this FormModel model, string name, string value)
        {
            var node = model.GetElement(name);

            if (node == null || !node.HasAttributes || !node.HasElements) return string.Empty;

            var option = node.Descendants("option").FirstOrDefault(x => x.Attribute("value").Value == value);

            return option != null ? option.Value : string.Empty;
        }

        private static string MultiList(FormModel model, XElement parent, XElement options, XElement answer, string css, string disabled)
        {
            var sb = new StringBuilder(string.Empty);

            if (options != null)
            {
                var nodes = options.Elements();
                sb.Append("<ul class=\"clearfix\">");
                foreach (var node in nodes)
                {
                    sb.Append("<li>");
                    if (node.Name == "optgroup")
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
                        if (notice != null)
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
    }
}