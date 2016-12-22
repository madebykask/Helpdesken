using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace ECT.Web.Models
{
    public class BaseModel
    {
        public string DefaultErrorMessage = "Error";
        public string DefaultMandatoryMessage = "This field is required.";
        public string DefaultRequiredMessage = "This field is left empty. Are you sure that you want to proceed?";
        public string DefaultMandatoryIfMessage = "This field is required if {0} has value {1}.";
        public string DefaultLaterOrEqualMessage = "This field must be later than or equal to {0}.";
        public string DefaultLessThanOrEqualMessage = "This field must be less than or equal to {0}.";
        public string DefaultDateRangeIfMessage = "{0} can't be longer than {1} months.";
        public string DefaultValue = string.Empty;

        public XDocument FormXml { get; set; } 
        public string FormXmlPath { get; private set; }
        public int FormId { get; set; }
        public int CustomerId { get; set; }
        public string Language { get; set; }
        public bool IsLooked { get; set; }

        public BaseModel(string xmlPath, bool invalidate = false)
        {
            var path = System.Web.HttpContext.Current.Server.MapPath("~/Xml_Data");
            var realPath = Path.Combine(path, xmlPath);

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(realPath);

            FormXml = xmlDocument.ToXDocument();

            if(FormXml.Root.HasAttributes && FormXml.Root.Attribute("id") != null)
                FormId = int.Parse(FormXml.Root.Attribute("id").Value);

            if(FormXml.Root.HasAttributes && FormXml.Root.Attribute("customerId") != null)
                CustomerId = int.Parse(FormXml.Root.Attribute("customerId").Value);

            FormXmlPath = xmlPath;

            Language = GetDefaultLanguage();
        }

        public string Translate(string name)
        {
            return Translate(name, Language);
        }

        public string Translate(string name, string language)
        {
            return I18N.Translate(name, language);
        }

        public IDictionary<string, string> GetStatusActions()
        {
            var dictionary = new Dictionary<string, string>();

            var node = GetNode("state");
            if(node == null || !node.HasAttributes) return dictionary;

            var currentState = node.Attribute("status") != null ? node.Attribute("status").Value : "";
            if(currentState == string.Empty) return dictionary;

            var status = node.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
            if(status == null) return dictionary;

            var actions = status.Descendants("action");

            foreach(var action in actions)
                dictionary.Add(action.Attribute("status").Value, action.Attribute("text").Value);

            return dictionary;
        }

        public string GetDefaultLanguage()
        {
            var node =
                (from p in FormXml.Descendants("languages").Descendants().Where(x => x.HasAttributes)
                 select p).FirstOrDefault();

            if(node == null)
                throw new Exception("Default attribute is missing.");

            return node.Value;
        }

        public bool IsValidLanguage(string language)
        {
            var node =
                (from p in FormXml.Descendants("languages").Descendants().Where(x => x.Value == language)
                 select p).FirstOrDefault();

            return node != null;
        }

        public string GetAnswer(string name)
        {
            var node = GetElement(name);

            if(node == null || !node.HasAttributes) return DefaultValue;

            var answer = node.Descendants("answer").FirstOrDefault();

            return answer != null ? answer.Value : DefaultValue;
        }

        public string GetTranslatedAnswer(string nodeName)
        {
            return GetTranslatedAnswer(nodeName, Language);
        }

        public string GetTranslatedAnswer(string name, string language)
        {
            var node = GetElement(name);
            if(node == null || !node.HasAttributes) return DefaultValue;

            var answer = node.Descendants("answer").FirstOrDefault();

            return answer != null ? Translate(answer.Value, language) : DefaultValue;
        }

        public void SetAnswer(string name, string value)
        {
            if(value == null || name == null) return;
            var node = GetElement(name);
            if(node == null) return;

            var answer = node.Descendants("answer").FirstOrDefault();

            if(answer == null)
            {
                answer = new XElement("answer") { Value = value };
                node.Add(answer);
            }
            else
                answer.Value = value;
        }

        public void SetAttributeValue(string nodeName, string attribute, string value)
        {
            var node =
                (from p in FormXml.Descendants(nodeName)
                 select p).FirstOrDefault();

            if(node != null && node.HasAttributes && node.Attribute(attribute) != null)
                node.Attribute(attribute).Value = value;
        }

        public bool ReadOnly(string name)
        {
            if(IsLooked)
                return true;

            var node = GetNode("state");
            if(node == null || !node.HasAttributes) return false;

            var currentState = node.Attribute("status") != null ? node.Attribute("status").Value : "";
            if(currentState == string.Empty) return false;

            var status = node.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
            if(status == null) return false;

            var readOnlyTabs = status.Descendants("readOnlyTabs").FirstOrDefault();

            if(readOnlyTabs != null && readOnlyTabs.Value != string.Empty)
            {
                if(readOnlyTabs.Value.Split(',')
                    .Select(readOnlyTab => FormXml.Descendants("tab")
                        .FirstOrDefault(x => x.HasAttributes && x.Attribute("name").Value == readOnlyTab))
                        .Where(element => element != null)
                        .Any(element => GetChildElement(element, name) != null))
                {
                    return true;
                }
            }

            var readOnlyFields = status.Descendants("readOnlyFields").FirstOrDefault();
            if(readOnlyFields == null || readOnlyFields.Value == string.Empty) return false;

            return (from readOnlyField in readOnlyFields.Value.Split(',') where readOnlyField == name select GetElement(readOnlyField) != null).FirstOrDefault();
        }

        public bool ReadOnly(string name, string[] workingGroups)
        {
            if(IsLooked)
                return true;

            var node = GetNode("state");
            if(node == null || !node.HasAttributes) return false;

            var currentState = node.Attribute("status") != null ? node.Attribute("status").Value : "";
            if(currentState == string.Empty) return false;

            var status = node.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
            if(status == null) return false;

            var readOnly = ReadOnly(name);

            if(workingGroups == null)
                return readOnly;

            var workingGroupRules = status.Descendants("workingGroupRules");

            if(workingGroupRules.Any())
            {
                foreach(var workingGroup in workingGroups)
                {
                    foreach(var workingGroupRule in workingGroupRules.Where(x => x.Attribute("workingGroup").Value == workingGroup).Descendants("visibleFields"))
                    {
                        if((from field in workingGroupRule.Value.Split(',') where field == name select field).FirstOrDefault() != null)
                            readOnly = false;
                    }
                }
            }

            return readOnly;
        }

        public bool VisibleTab(string tab)
        {
            var node = GetNode("state");

            if(node == null || !node.HasAttributes) return false;

            var currentState = node.Attribute("status") != null ? node.Attribute("status").Value : "";
            if(currentState == string.Empty) return false;

            var status = node.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
            if(status == null) return false;

            var visibleTabs = status.Descendants("visibleTabs").FirstOrDefault();
            if(visibleTabs == null || visibleTabs.Value == string.Empty) return false;

            return visibleTabs.Value.Split(',').Any(visibleTab => visibleTab == tab);
        }

        public string GetErrorMessage(string name)
        {
            return GetErrorMessage(name, Language);
        }

        public string GetErrorMessage(string name, string language)
        {
            var node = GetElement(name);

            if(node == null) return string.Empty;

            var error = GetChildElement(node, "error");
            return error != null ? Translate(error.Value, language) : string.Empty;
        }

        public string[] GetErrorMessages()
        {
            return GetErrorMessages(Language);
        }

        public string[] GetErrorMessages(string language)
        {
            var nodes =
                (from p in FormXml.Descendants("error")
                 select p);

            var xElements = nodes as XElement[] ?? nodes.ToArray();
            return xElements.Any() ? (from x in xElements select Translate(x.Value, language)).ToArray() : new string[0];
        }

        public bool TabHasError(string tab)
        {
            var nodes = (from p in FormXml.Descendants("tab").Where(x => x.Attribute("name").Value == tab).Descendants("error")
                         select p);

            return (nodes.Any());
        }

        public bool IsValid(ref IDictionary<string, string> dictionary)
        {
            return IsValid(ref dictionary, null);
        }

        public bool IsValid(ref IDictionary<string, string> dictionary, int? actionState)
        {
            return IsValid(ref dictionary, actionState, null);
        }

        public bool IsValid(ref IDictionary<string, string> dictionary, int? actionState, string[] workingGroups)
        {
            var nodes = new List<XElement>();

            // Validate depending on action
            if(actionState.HasValue)
            {
                var state = FormXml.Descendants("state").FirstOrDefault();
                if(state != null && state.HasAttributes)
                {
                    var currentState = state.Attribute("status") != null ? state.Attribute("status").Value : "";
                    if(currentState != string.Empty)
                    {
                        var status = state.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
                        if(status != null)
                        {
                            var action = status.Descendants("action").FirstOrDefault(x => x.HasAttributes && x.Attribute("status").Value == actionState.ToString());
                            if(action != null)
                            {
                                var validateTabs = action.Element("validateTabs");
                                if(validateTabs != null && validateTabs.Value != string.Empty)
                                {
                                    foreach(var validateTab in validateTabs.Value.Split(','))
                                    {
                                        var tab = validateTab;
                                        nodes.AddRange((from n in FormXml.Descendants("tab").Where(x => x.Attribute("name").Value == tab).Elements()
                                                        select n));
                                    }
                                }

                                var validateFields = action.Element("validateFields");
                                if(validateFields != null && validateFields.Value != string.Empty)
                                {
                                    nodes.AddRange(validateFields.Value.Split(',').Select(GetElement).Where(node => node != null));
                                }

                                if(action.Element("validateWhatYouSee") != null)
                                    nodes.AddRange(dictionary.Select(pair => GetElement(pair.Key)).Where(node => node != null));



                                // special working group condition overwrites if visible 
                                var workingGroupRules = action.Descendants("workingGroupRules");
                                if(workingGroups != null && workingGroupRules.Any())
                                {
                                    foreach(var workingGroup in workingGroups)
                                    {
                                        // mandatory rules
                                        foreach(var mandatoryField in workingGroupRules.Where(x => x.Attribute("workingGroup").Value == workingGroup).Descendants("mandatoryFields"))
                                        {
                                            if(mandatoryField.Value != string.Empty)
                                            {
                                                var elems = mandatoryField.Value.Split(',').Select(GetElement).Where(node => node != null);

                                                foreach(var elem in elems)
                                                {
                                                    var name = elem.Attribute("name").Value;

                                                    if(!dictionary.ContainsKey(name)) continue;
                                                    
                                                    var value = dictionary[name];

                                                    if(string.IsNullOrEmpty(value))
                                                    {
                                                        var errorElement = new XElement("error") { Value = Translate(DefaultMandatoryMessage) };
                                                        elem.Add(errorElement);
                                                    }

                                                    nodes.Add(elem);
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            // Validate what we see
            else
            {
                nodes.AddRange(dictionary.Select(pair => GetElement(pair.Key)).Where(node => node != null));
            }

            foreach(var node in nodes.Where(node => node.HasAttributes))
            {
                XElement errorElement;
                var validate = false;

                var name = node.Attribute("name").Value;
                var value = dictionary.ContainsKey(name) ? dictionary[name] : null;

                // validation group
                if(node.Attribute("validationgroup") != null)
                {
                    var validateValidationGroup = false;
                    var validationGroups = GetElments("validationgroup", node.Attribute("validationgroup").Value);

                    foreach(var validationGroup in validationGroups)
                    {
                        var answer = GetAnswer(validationGroup.Attribute("name").Value);
                        if(answer != DefaultValue)
                        {
                            validateValidationGroup = true;
                            break;
                        }
                    }

                    if(!validateValidationGroup)
                        continue;
                }

                // Validate If
                var validateIfs = node.Descendants("validateIf");
                if(!validateIfs.Any())
                    validate = true;

                foreach(var validateIf in validateIfs)
                {
                    var validateIfElement = GetElement(validateIf.Attribute("name").Value);

                    if(validateIfElement == null) continue;

                    var validateIfValue = GetAnswer(validateIf.Attribute("name").Value);

                    foreach(var s in validateIfValue.Split(','))
                    {
                        if(validateIf.Value.Split(',').Contains(s))
                            validate = true;
                    }
                }

                if(validate)
                {
                    // Mandatory if
                    var mandatoryIfs = node.Descendants("mandatoryIf");
                    var checkMandatoryIf = false;

                    foreach(var mandatoryif in mandatoryIfs.Where(mandatoryif => mandatoryif.HasAttributes && mandatoryif.Attribute("name") != null))
                    {
                        if(checkMandatoryIf) continue;

                        var mandatoryIfElement = GetElement(mandatoryif.Attribute("name").Value);

                        if(mandatoryIfElement == null) continue;

                        var mandatoryIfValue = GetAnswer(mandatoryif.Attribute("name").Value);

                        foreach(var s in mandatoryIfValue.Split(','))
                        {
                            if(mandatoryif.Value.Split(',').Contains(s))
                                checkMandatoryIf = true;
                        }

                        if(checkMandatoryIf && string.IsNullOrEmpty(value))
                        {
                            errorElement = new XElement("error")
                            {
                                Value = Translate(DefaultMandatoryMessage)
                            };
                            node.Add(errorElement);
                        }
                    }

                    // Mandatory if not empty

                    var mandatoryIfNotEmptys = node.Descendants("mandatoryIfNotEmpty");
                    var checkMandatoryIfNotEmpty = false;

                    foreach(var mandatoryIfNotEmpty in mandatoryIfNotEmptys.Where(x => x.HasAttributes && x.Attribute("name") != null))
                    {
                        if(checkMandatoryIfNotEmpty) continue;

                        var mandatoryIfNotEmptyElement = GetElement(mandatoryIfNotEmpty.Attribute("name").Value);

                        if(mandatoryIfNotEmptyElement == null) continue;

                        if(GetAnswer(mandatoryIfNotEmpty.Attribute("name").Value) != DefaultValue)
                            checkMandatoryIfNotEmpty = true;

                        if(checkMandatoryIfNotEmpty && string.IsNullOrEmpty(value))
                        {
                            errorElement = new XElement("error")
                            {
                                Value = Translate(DefaultMandatoryMessage)
                            };
                            node.Add(errorElement);
                        }
                    }

                    // Mandatory
                    if(node.Attribute("mandatory") != null && string.IsNullOrEmpty(value))
                    {
                        errorElement = new XElement("error") { Value = Translate(DefaultMandatoryMessage) };
                        node.Add(errorElement);
                    }

                    // Required
                    if (node.Attribute("required") != null && string.IsNullOrEmpty(value))
                    {
                        errorElement = new XElement("error") { Value = Translate(DefaultRequiredMessage) };
                        node.Add(errorElement);
                    }

                    // groupSum

                    var groupSums = node.Descendants("groupSum");

                    foreach(var groupSum in groupSums.Where(x => x.HasAttributes && x.Attribute("name") != null))
                    {
                        var message = groupSum.Attribute("message").Value;
                        var sum = 0;
                        var groupSumValue = int.Parse(groupSum.Value);

                        foreach(var s in groupSum.Attribute("name").Value.Split(','))
                        {
                            var add = GetAnswer(s);
                            int o;
                            if(add != "" && int.TryParse(add, out o))
                                sum = sum + o;
                        }

                        if(sum != groupSumValue && !string.IsNullOrEmpty(value))
                        {
                            errorElement = new XElement("error")
                            {
                                Value = Translate(message)
                            };
                            node.Add(errorElement);
                        }
                    }

                    // Regex If
                    var checkRegexIf = false;
                    var regexIfs = node.Elements("regexIf");
                    var regexIfElements = regexIfs as XElement[] ?? regexIfs.ToArray();
                    if(regexIfElements.Any() && !string.IsNullOrEmpty(value))
                    {
                        foreach(var regex in regexIfElements)
                        {
                            if(checkRegexIf) continue;

                            if(!regex.HasAttributes) continue;

                            var regexIfElement = GetElement(regex.Attribute("name").Value);

                            if(regexIfElement == null) continue;

                            var regexIfValue = GetAnswer(regexIfElement.Attribute("name").Value);

                            foreach(var s in regexIfValue.Split(','))
                            {
                                if(regex.Value.Split(',').Contains(s))
                                    checkRegexIf = true;
                            }

                            if(checkRegexIf)
                            {
                                var attrInherit = regex.Attribute("inherit");
                                var attrPattern = regex.Attribute("pattern");
                                var attrMessage = regex.Attribute("message");

                                var message = string.Empty;
                                var pattern = string.Empty;

                                if(!string.IsNullOrEmpty(attrInherit.Value))
                                {
                                    var inheritRegexs = GetNode("regexs");

                                    if(inheritRegexs != null && inheritRegexs.HasElements)
                                    {
                                        var inheritRegex = inheritRegexs.Descendants().FirstOrDefault(x => x.Name == attrInherit.Value);

                                        if(inheritRegex != null)
                                        {
                                            if(!string.IsNullOrEmpty(inheritRegex.Value))
                                                pattern = inheritRegex.Value;
                                            if(inheritRegex.HasAttributes && !string.IsNullOrEmpty(inheritRegex.Attribute("message").Value))
                                                message = inheritRegex.Attribute("message").Value;
                                        }
                                    }
                                }

                                if(!string.IsNullOrEmpty(attrMessage.Value))
                                    message = attrMessage.Value;

                                if(!string.IsNullOrEmpty(attrPattern.Value))
                                    pattern = attrPattern.Value;

                                if(Regex.IsMatch(value, pattern)) continue;

                                errorElement = new XElement("error")
                                {
                                    Value = message != string.Empty ? Translate(message) : Translate(DefaultErrorMessage)
                                };
                                node.Add(errorElement);
                                break;
                            }
                        }
                    }

                    // Regex
                    var regexs = node.Elements("regex");
                    var xElements = regexs as XElement[] ?? regexs.ToArray();
                    if(xElements.Any() && !string.IsNullOrEmpty(value))
                    {
                        foreach(var regex in xElements)
                        {
                            if(!regex.HasAttributes) continue;

                            var attrInherit = regex.Attribute("inherit");
                            var attrPattern = regex.Attribute("pattern");
                            var attrMessage = regex.Attribute("message");

                            var message = string.Empty;
                            var pattern = string.Empty;

                            if(!string.IsNullOrEmpty(attrInherit.Value))
                            {
                                var inheritRegexs = GetNode("regexs");

                                if(inheritRegexs != null && inheritRegexs.HasElements)
                                {
                                    var inheritRegex = inheritRegexs.Descendants().FirstOrDefault(x => x.Name == attrInherit.Value);

                                    if(inheritRegex != null)
                                    {
                                        if(!string.IsNullOrEmpty(inheritRegex.Value))
                                            pattern = inheritRegex.Value;
                                        if(inheritRegex.HasAttributes && !string.IsNullOrEmpty(inheritRegex.Attribute("message").Value))
                                            message = inheritRegex.Attribute("message").Value;
                                    }
                                }
                            }

                            if(!string.IsNullOrEmpty(attrMessage.Value))
                                message = attrMessage.Value;

                            if(!string.IsNullOrEmpty(attrPattern.Value))
                                pattern = attrPattern.Value;

                            if(Regex.IsMatch(value, pattern)) continue;

                            errorElement = new XElement("error")
                            {
                                Value = message != string.Empty ? Translate(message) : Translate(DefaultErrorMessage)
                            };
                            node.Add(errorElement);
                            break;
                        }
                    }


                    // Later than or equal    
                    if(!string.IsNullOrEmpty(value))
                    {
                        var laterThanOrEquals = node.Descendants("laterThanOrEqual");
                        foreach(
                            var laterThanOrEqual in
                                laterThanOrEquals.Where(laterThanOrEqual => laterThanOrEqual.HasAttributes && laterThanOrEqual.Attribute("name") != null)
                            )
                        {
                            var compareWith = GetElement(laterThanOrEqual.Attribute("name").Value);

                            if((node.Attribute("type") == null || node.Attribute("type").Value != "date")
                                && (compareWith.Attribute("type") == null || compareWith.Attribute("type").Value != "date")) continue;

                            var lbl = compareWith.Attribute("label") != null ? compareWith.Attribute("label").Value : laterThanOrEqual.Attribute("name").Value;
                            var compareWithAnswer = GetAnswer(laterThanOrEqual.Attribute("name").Value);

                            var date1 = ConvertToDateTime(value, node.Attribute("format").Value);
                            var date2 = ConvertToDateTime(compareWithAnswer, compareWith.Attribute("format").Value);

                            var result = DateTime.Compare(date1.GetValueOrDefault(), date2.GetValueOrDefault());

                            if(result >= 0) continue;

                            errorElement = new XElement("error")
                            {
                                Value = string.Format(Translate(DefaultLaterOrEqualMessage), Translate(lbl))
                            };

                            node.Add(errorElement);
                        }

                        // less than or equal
                        var lessThanOrEquals = node.Descendants("lessThanOrEqual");
                        foreach(
                            var lessThanOrEqual in
                                lessThanOrEquals.Where(lessThanOrEqual => lessThanOrEqual.HasAttributes && lessThanOrEqual.Attribute("name") != null)
                            )
                        {
                            var compareWith = GetElement(lessThanOrEqual.Attribute("name").Value);

                            if((node.Attribute("type") == null || node.Attribute("type").Value != "date")
                                && (compareWith.Attribute("type") == null || compareWith.Attribute("type").Value != "date")) continue;

                            var lbl = compareWith.Attribute("label") != null ? compareWith.Attribute("label").Value : lessThanOrEqual.Attribute("name").Value;
                            var compareWithAnswer = GetAnswer(lessThanOrEqual.Attribute("name").Value);

                            var date1 = ConvertToDateTime(value, node.Attribute("format").Value);
                            var date2 = ConvertToDateTime(compareWithAnswer, compareWith.Attribute("format").Value);

                            var result = DateTime.Compare(date1.GetValueOrDefault(), date2.GetValueOrDefault());

                            if(result <= 0) continue;

                            errorElement = new XElement("error")
                            {
                                Value = string.Format(Translate(DefaultLessThanOrEqualMessage), Translate(lbl))
                            };

                            node.Add(errorElement);
                        }

                        // range if
                        var dateRangeIfs = node.Descendants("dateRangeIf");
                        foreach(
                            var dateRangeIf in dateRangeIfs)
                        {
                            var dateRangeIfAnswer = GetAnswer(dateRangeIf.Attribute("name").Value);
                            if(dateRangeIf.Value != dateRangeIfAnswer) continue;

                            var compareWith = GetElement(dateRangeIf.Attribute("range-start-element").Value);

                            if((node.Attribute("type") == null || node.Attribute("type").Value != "date")
                                && (compareWith.Attribute("type") == null || compareWith.Attribute("type").Value != "date")) continue;

                            var compareWithAnswer = GetAnswer(dateRangeIf.Attribute("range-start-element").Value);

                            var range = dateRangeIf.Attribute("range").Value;
                            var lbl = compareWith.Attribute("label").Value;

                            var date1 = ConvertToDateTime(value, node.Attribute("format").Value);
                            var date2 = ConvertToDateTime(compareWithAnswer, compareWith.Attribute("format").Value);

                            date2 = date2.GetValueOrDefault().AddMonths(int.Parse(range));

                            var result = DateTime.Compare(date1.GetValueOrDefault(), date2.GetValueOrDefault());

                            if(result <= 0) continue;

                            errorElement = new XElement("error")
                            {
                                Value = string.Format(Translate(DefaultDateRangeIfMessage), Translate(dateRangeIfAnswer), range)
                            };

                            node.Add(errorElement);
                        }
                    }
                }
            }

            
            return
                !(from p in FormXml.Descendants("error")
                  select p).Any();
        }

        public void TempSave(ref IDictionary<string, string> dictionary)
        {
            FormXml.Descendants("error").Remove();

            var nodes =
                (from p in FormXml.Descendants("tab").Descendants().Where(x => x.HasAttributes && x.Attribute("name") != null && x.Name == "element")
                 select p);

            foreach(var node in nodes)
            {
                if(!node.HasAttributes) continue;

                var name = node.Attribute("name").Value;
                var value = dictionary.ContainsKey(name) ? dictionary[name] : null;
                var answer = node.Descendants("answer").FirstOrDefault();

                if(answer == null)
                {
                    answer = new XElement("answer") { Value = value ?? DefaultValue };
                    node.Add(answer);
                }
                else
                    answer.Value = value ?? DefaultValue;
            }
        }

        public void PopulateForm(IDictionary<string, string> dictionary)
        {
            var nodes =
                (from p in FormXml.Descendants("tab").Descendants().Where(x => x.HasAttributes && x.Attribute("name") != null && x.Name == "element")
                 select p);

            foreach(var node in nodes)
            {
                var errorElement = node.Descendants("error").FirstOrDefault();

                if(errorElement != null)
                    errorElement.Remove();

                var name = node.Attribute("name").Value;
                var value = dictionary.ContainsKey(name) ? dictionary[name] : null;

                var answer = node.Descendants("answer").FirstOrDefault();

                if(answer == null)
                {
                    if(value != null)
                    {
                        answer = new XElement("answer") { Value = value ?? DefaultValue };
                        node.Add(answer);
                    }
                }
                else
                    answer.Value = value ?? DefaultValue;
            }
        }

        public void PopulateFormElementWithOptions(string name, IDictionary<string, string> dictionary)
        {
            var node = GetElement(name);

            if(node == null || dictionary == null) return;

            var options = node.Descendants("options").FirstOrDefault();
            if(options != null)
                options.Remove();

            options = new XElement("options");

            var option = new XElement("option") { Value = "" };
            var attribute = new XAttribute("value", "");
            option.Add(attribute);
            options.Add(option);

            foreach(var pair in dictionary)
            {
                option = new XElement("option") { Value = pair.Key };
                attribute = new XAttribute("value", pair.Value);
                option.Add(attribute);
                options.Add(option);
            }

            node.Add(options);
        }

        public XElement GetElement(string name)
        {
            return (from n in FormXml.Descendants("element").Where(x => x.HasAttributes && x.Attribute("name") != null && x.Attribute("name").Value == name)
                    select n).FirstOrDefault();
        }

        public XElement GetChildElement(XElement parent, string name)
        {
            return parent.Descendants().FirstOrDefault(x => x.HasAttributes && x.Attribute("name") != null && x.Attribute("name").Value == name);
        }



        public void SetNodeValue(string nodeName, string value)
        {
            var node = GetNode(nodeName);

            if(node == null)
            {
                node = new XElement(nodeName) { Value = value };
                if(FormXml != null && FormXml.Root != null)
                    FormXml.Root.Add(node);
            }
            else
                node.Value = value;
        }

        public XElement GetNode(string nodeName)
        {
            return (from n in FormXml.Descendants().Where(x => x.Name == nodeName)
                    select n).FirstOrDefault();
        }

        public XElement GetNode(string nodeName, string attributeName)
        {
            return (from n in FormXml.Descendants().Where(x => x.Name == nodeName)
                    select n).FirstOrDefault(x => x.HasAttributes && x.Attribute("name") != null && x.Attribute("name").Value == attributeName);
        }

        public IEnumerable<XElement> GetElments(string attributeName)
        {
            return (from n in FormXml.Descendants().Where(x => x.Name == "element"
                && x.HasAttributes
                && x.Attribute(attributeName) != null)
                    select n);
        }

        public IEnumerable<XElement> GetElments(string attributeName, string attributeValue)
        {
            return (from n in FormXml.Descendants().Where(x => x.Name == "element"
                && x.HasAttributes
                && x.Attribute(attributeName) != null
                && x.Attribute(attributeName).Value == attributeValue)
                    select n);
        }

        public string ToJson(string nodeName)
        {
            var xNode = GetElement(nodeName);
            var xmlNode = xNode.ToXmlNode();
            return Regex.Replace(JsonConvert.SerializeXmlNode(xmlNode, Newtonsoft.Json.Formatting.Indented), "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);
            //return JsonConvert.SerializeXmlNode(xmlNode);
        }

        public string ToJson(XElement node)
        {
            var xmlNode = node.ToXmlNode();
            xmlNode = TrimXmlNode(xmlNode);

            return Regex.Replace(JsonConvert.SerializeXmlNode(xmlNode, Newtonsoft.Json.Formatting.Indented), "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);
            //return = JsonConvert.SerializeXmlNode(xmlNode);
        }

        public void AddError(string nodeName, string message)
        {
            var node = GetElement(nodeName);
            if(node == null) return;

            var errorElement = new XElement("error") { Value = message };
            node.Add(errorElement);
        }

        public void ClearErrors()
        {
            FormXml.Descendants("error").Remove();
        }

        public string ClientValidate(int actionState = 0)
        {
            var nodes = new List<XElement>();

            // Validate depending on action
            var state = FormXml.Descendants("state").FirstOrDefault();

            if(state != null && state.HasAttributes && state.Attribute("status") != null)
            {
                var currentState = state.Attribute("status").Value;
                if(currentState != string.Empty)
                {
                    var status = state.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
                    if(status != null)
                    {
                        var action = status.Descendants("action")
                            .FirstOrDefault(x => x.HasAttributes && x.Attribute("status").Value == actionState.ToString(CultureInfo.InvariantCulture));

                        if(action != null)
                        {
                            var validateTabs = action.Element("validateTabs");
                            if(validateTabs != null && validateTabs.Value != string.Empty)
                            {
                                foreach(var validateTab in validateTabs.Value.Split(','))
                                {
                                    var tab = validateTab;
                                    nodes.AddRange((from n in FormXml.Descendants("tab").Where(x => x.Attribute("name").Value == tab).Elements()
                                                    select n));
                                }
                            }
                            var validateFields = action.Element("validateFields");
                            if(validateFields != null && validateFields.Value != string.Empty)
                            {
                                nodes.AddRange(validateFields.Value.Split(',').Select(GetElement).Where(node => node != null));
                            }

                            if(action.Element("validateWhatYouSee") != null)
                                nodes.AddRange((from n in FormXml.Descendants("tab").Descendants("element")
                                                select n));
                        }
                    }
                }
            }

            var clientValidates = new ArrayList();

            foreach(var node in nodes.Where(x => x.Attribute("validationgroup") == null))
            {
                var obj = new ClientValidate { Name = node.Attribute("name").Value };

                if(node.Attribute("mandatory") != null)
                {
                    obj.Rules.Add("required", true.ToString());
                    //obj.Messages.Add("required", DefaultMandatoryMessage);
                    clientValidates.Add(obj);
                }
            }

            var json = JsonConvert.SerializeObject(clientValidates);
            return json;
        }

        public DateTime? ConvertToDateTime(string name)
        {
            var element = GetElement(name);
            var answer = GetAnswer(name);

            if(element == null || answer == null) return null;
            if(element.Attribute("type").Value != "date") return null;

            var format = element.Attribute("format").Value;
            return ConvertToDateTime(answer, format);
        }

        public DateTime? ConvertToDateTime(string value, string format)
        {
            DateTime dt;
            if(DateTime.TryParseExact(value, format, null, DateTimeStyles.None, out dt))
                return dt;
            return null;
        }

        private XmlNode TrimXmlNode(XmlNode node)
        {
            if(node.InnerXml != "")
                node.InnerXml = node.InnerXml.Trim().Replace("\r\n", "");

            if(node.HasChildNodes)
            {
                foreach(XmlNode child in node.ChildNodes)
                    TrimXmlNode(child);
            }

            return node;
        }
    }
}