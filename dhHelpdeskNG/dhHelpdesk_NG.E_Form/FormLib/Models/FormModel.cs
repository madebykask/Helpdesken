using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Linq;
using DH.Helpdesk.EForm.Model.Entities;
using Newtonsoft.Json;
using System.ComponentModel;


namespace DH.Helpdesk.EForm.FormLib.Models
{
    public class FormModelFactory
    {
        public static FormModel InitNew(string xmlPath)
        {
            var model = new FormModel(xmlPath);
            model.Contract = new Contract { Initiator = FormLibSessions.User.FullName };
            model.Status = FormLibUtils.GetCaseInitState();
            model.UserGroups = FormLibSessions.User.WorkingGroups.Select(x => x.Name).Distinct().ToList();

            return model;
        }

        public static FormModel InitEdit(string xmlPath, Contract contract, bool looked)
        {
            var model = new FormModel(xmlPath);
            model.IsLooked = looked;
            model.Contract = contract;
            model.Status = contract.StateSecondaryId;
            model.UserGroups = FormLibSessions.User.WorkingGroups.Select(x => x.Name).Distinct().ToList();

            return model;
        }
    }

    public class FormModel
    {

        private int _status;
        private string _path;

        public string ActiveTab { get; set; }
        public string ActiveStatus { get; set; }
        public int Status { get { return _status; } set { _status = value; SetAttributeValue("state", "status", value.ToString(CultureInfo.InvariantCulture)); } }
        public Contract Contract { get; set; }
        public Company Company { get; set; }
        public Department Department { get; set; }
        public string MultiNr { get; set; }
        public DocumentData DocumentData { get; set; }
        public Form Form { get; set; }

        public string DefaultErrorMessage = "Error";
        public string DefaultMandatoryMessage = "This field is required.";
        public string DefaultRequiredMessage = "This field is left empty. Are you sure that you want to proceed?";
        public string DefaultMandatoryIfMessage = "This field is required if {0} has value {1}.";
        public string DefaultLaterOrEqualMessage = "This field must be later than or equal to {0}.";
        public string DefaultLessThanOrEqualMessage = "This field must be less than or equal to {0}.";
        public string DefaultDateRangeIfMessage = "{0} can't be longer than {1} months.";
        public string DefaultFutureDateMessage = "Please choose a date in the future";
        public string DefaultPastDateMessage = "Please choose a date in the past";

        public string DefaultValue = string.Empty;

        public XDocument FormXml { get; set; }
        public string FormXmlPath { get; private set; }
        public Guid FormGuid { get; private set; }
        public int FormId { get; set; }
        public int CustomerId { get; set; }
        public string Language { get; set; }
        public bool IsLooked { get; set; }
        public List<string> UserGroups { get; set; }

        public IList<StaticFile> StaticFiles { get; set; }

        [Description("This value is placed in e-Form XML root <form>)")]
        public Model.Enums.EForm.DataSource DataSource { get; set; }


        public FormModel(string xmlPath)
        {
            var di = new DirectoryInfo(HostingEnvironment.MapPath(FormLibConstants.Paths.XmlDirectory));
            _path = di.FullName;

            var realPath = Path.Combine(di.FullName, xmlPath);

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(realPath);

            FormXml = xmlDocument.ToXDocument();

            if (FormXml.Root.HasAttributes && FormXml.Root.Attribute("id") != null)
                FormId = int.Parse(FormXml.Root.Attribute("id").Value);

            if (FormXml.Root.HasAttributes && FormXml.Root.Attribute("guid") != null)
                FormGuid = Guid.Parse(FormXml.Root.Attribute("guid").Value);

            //Check DataSource for Form, Set XML as default.
            if (FormXml.Root.HasAttributes && FormXml.Root.Attribute("dataSource") != null)
            {
                DataSource = (Model.Enums.EForm.DataSource)int.Parse(FormXml.Root.Attribute("dataSource").Value);
            }
            else
            {
                DataSource = Model.Enums.EForm.DataSource.XML;
            }


            if (FormXml.Root.HasAttributes && FormXml.Root.Attribute("customerId") != null)
            {
                int number;
                bool customerIdBool = int.TryParse(FormXml.Root.Attribute("customerId").Value, out number);
                if (customerIdBool)
                {
                    CustomerId = number;
                    //CustomerId = int.Parse(FormXml.Root.Attribute("customerId").Value);
                }
            }

            FormXmlPath = xmlPath;

            Language = GetDefaultLanguage();

            // TODO: Change this
            // Not the best solution but it will have to work for now. DL 20140507
            if (FormLibSessions.User != null
                && !string.IsNullOrEmpty(FormLibSessions.User.Language)
                && IsValidLanguage(FormLibSessions.User.Language))
            {
                this.Language = FormLibSessions.User.Language;
            }
        }

        public void ReplaceNode(string node, string loadFromPath)
        {
            var state = GetNode(node);
            if (state == null)
                return;
            var elem = XElement.Load(Path.Combine(_path, loadFromPath));
            state.ReplaceNodes(elem);
        }

        public string Translate(string name)
        {
            return Translate(name, Language);
        }

        public string Translate(string name, string language)
        {
            ////Therese Will check this problem Out but till that time I return this back to Take the GLobal view to work.
            //return FormLibI18N.Translate(name, language);

            if (Form != null)
            {
                return FormLibI18N.Translate(name, language, Form.FormSettings.TextTypeId, Form.CustomerId, Form.FormGuid, "", Form.FormSettings.LogTranslations);
            }
            else
            {
                return FormLibI18N.Translate(name, language);
            }
        }

        //För att vi ska veta vad det är vi vill översätta så skickar vi in (knapp, select, search-select) osv...
        public string Translate(string name, string language, string source)
        {
            return FormLibI18N.Translate(name, language, Form.FormSettings.TextTypeId, Form.CustomerId, Form.FormGuid, source, Form.FormSettings.LogTranslations);
        }

        public string TranslateRevert(string text)
        {
            return TranslateRevert(text, Language);
        }

        public string TranslateRevert(string text, string language)
        {
            return FormLibI18N.TranslateRevert(text, language, Form.FormSettings.TextTypeId);
        }


        public IDictionary<string, string> GetStatusActions()
        {
            var dictionary = new Dictionary<string, string>();

            var node = GetNode("state");
            if (node == null || !node.HasAttributes) return dictionary;

            var currentState = node.Attribute("status") != null ? node.Attribute("status").Value : "";
            if (currentState == string.Empty) return dictionary;

            var status = node.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
            if (status == null) return dictionary;

            XElement actions = null;

            if (UserGroups != null)
            {
                var userGroups = status.Descendants("userGroup");

                if (userGroups.Any())
                {
                    foreach (var group in UserGroups)
                    {
                        actions = userGroups.Where(x => x.Attribute("name").Value == group).Descendants("actions").FirstOrDefault();
                    }
                }
            }

            if (actions == null)
                actions = status.Element("actions");

            if (actions == null) return dictionary;

            var actionOptions = actions.Descendants("action");

            foreach (var action in actionOptions)
                dictionary.Add(action.Attribute("status").Value, action.Attribute("text").Value);

            return dictionary;
        }

        public string GetStatusActionInternalValue(string actionStatus)
        {
            var node = GetNode("state");
            if (node == null || !node.HasAttributes)
                return actionStatus;

            var currentState = node.Attribute("status") != null ? node.Attribute("status").Value : "";
            if (currentState == string.Empty)
                return actionStatus;

            var status = node.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
            if (status == null)
                return actionStatus;

            var action = status.Descendants("action").FirstOrDefault(x => x.HasAttributes && x.Attribute("status").Value == actionStatus);
            if (action == null)
                return actionStatus;

            return action.Attribute("internal-val") != null ? action.Attribute("internal-val").Value : action.Attribute("status").Value;
        }

        public void ChangeStatusActionInternalValue(string text, string value)
        {
            var node = GetNode("state");
            if (node == null || !node.HasAttributes)
                return;

            var actions = node.Descendants("action")
                .Where(x => x.HasAttributes && x.Attribute("internal-val") != null && x.Attribute("text") != null && x.Attribute("text").Value == text);

            foreach (var item in actions)
                item.Attribute("internal-val").Value = value;
        }

        public Guid GetInitFormGuid(string actionStatus)
        {
            var node = GetNode("state");
            if (node == null || !node.HasAttributes)
                return Guid.Empty;

            var currentState = node.Attribute("status") != null ? node.Attribute("status").Value : "";
            if (currentState == string.Empty)
                return Guid.Empty;

            var status = node.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
            if (status == null)
                return Guid.Empty;

            var action = status.Descendants("action").FirstOrDefault(x => x.HasAttributes && x.Attribute("status").Value == actionStatus);
            if (action == null)
                return Guid.Empty;

            return action.Attribute("init-form-guid") != null ? new Guid(action.Attribute("init-form-guid").Value) : Guid.Empty;
        }

        public string GetDefaultLanguage()
        {
            var node =
                (from p in FormXml.Descendants("languages").Descendants().Where(x => x.HasAttributes)
                 select p).FirstOrDefault();

            if (node == null)
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

            if (node == null || !node.HasAttributes) return DefaultValue;

            var answer = node.Descendants("answer").FirstOrDefault();

            return answer != null ? answer.Value : DefaultValue;
        }

        public string GetDocumentLabelEmpty(string name)
        {
            var node = GetElement(name);

            if (node == null || !node.HasAttributes || node.Attribute("label") == null)
            {
                return "[" + name.ToUpper() + "]";
            }

            return "[" + Translate(node.Attribute("label").Value.ToString(), Language).ToUpper() + "]";
        }


        public string GetLabel(string name)
        {
            var node = GetElement(name);
            //no match, return name
            if (node == null || !node.HasAttributes || node.Attribute("label") == null)
            {
                return name;
            }

            return node.Attribute("label").Value.ToString();
        }

        public string GetElementType(string name)
        {
            var node = GetElement(name);
            //no match, return name
            if (node == null || !node.HasAttributes || node.Attribute("type") == null)
            {
                return "";
            }

            return node.Attribute("type").Value.ToString();
        }

        public string GetAttributeValue(string name, string attributeName)
        {
            var node = GetElement(name);
            //no match, return name
            if (node == null || !node.HasAttributes || node.Attribute(attributeName) == null)
            {
                return "";
            }

            return node.Attribute(attributeName).Value.ToString();

        }


        public string GetTranslatedAnswer(string nodeName)
        {
            return GetTranslatedAnswer(nodeName, Language);
        }

        public string GetTranslatedAnswer(string name, string language)
        {
            var node = GetElement(name);
            if (node == null || !node.HasAttributes) return DefaultValue;

            var answer = node.Descendants("answer").FirstOrDefault();

            return answer != null ? Translate(answer.Value, language) : DefaultValue;
        }

        public void SetAnswer(string name, string value)
        {
            if (value == null || name == null) return;
            var node = GetElement(name);
            if (node == null) return;

            var answer = node.Descendants("answer").FirstOrDefault();

            if (answer == null)
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

            if (node != null && node.HasAttributes && node.Attribute(attribute) != null)
                node.Attribute(attribute).Value = value;
        }

        public bool ReadOnly(string name)
        {
            if (IsLooked)
                return true;

            var node = GetNode("state");
            if (node == null || !node.HasAttributes) return false;

            var currentState = node.Attribute("status") != null ? node.Attribute("status").Value : "";
            if (currentState == string.Empty) return false;

            var status = node.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
            if (status == null) return false;

            XElement readOnlyTabs = null;

            if (UserGroups != null)
            {
                var userGroups = status.Descendants("userGroup");

                if (userGroups.Any())
                {
                    foreach (var group in UserGroups)
                    {
                        readOnlyTabs = userGroups.Where(x => x.Attribute("name").Value == group).Descendants("readOnlyTabs").FirstOrDefault();
                    }
                }
            }

            if (readOnlyTabs == null)
                readOnlyTabs = status.Element("readOnlyTabs");

            if (readOnlyTabs != null && readOnlyTabs.Value != string.Empty)
            {
                if (readOnlyTabs.Value.Split(',')
                    .Select(readOnlyTab => FormXml.Descendants("tab")
                        .FirstOrDefault(x => x.HasAttributes && x.Attribute("name").Value == readOnlyTab))
                        .Where(element => element != null)
                        .Any(element => GetChildElement(element, name) != null))
                {
                    return true;
                }
            }

            var readOnlyFields = status.Descendants("readOnlyFields").FirstOrDefault();

            //Om man inte tillhör d

            if (readOnlyFields == null || readOnlyFields.Value == string.Empty) return false;

            return (from readOnlyField in readOnlyFields.Value.Split(',') where readOnlyField == name select GetElement(readOnlyField) != null).FirstOrDefault();
        }

        [Obsolete("Only used for poland")]
        public bool ReadOnly(string name, string[] workingGroups)
        {
            if (IsLooked)
                return true;

            var node = GetNode("state");
            if (node == null || !node.HasAttributes) return false;

            var currentState = node.Attribute("status") != null ? node.Attribute("status").Value : "";
            if (currentState == string.Empty) return false;

            var status = node.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
            if (status == null) return false;

            var readOnly = ReadOnly(name);

            if (workingGroups == null)
                return readOnly;

            var workingGroupRules = status.Descendants("workingGroupRules");

            if (workingGroupRules.Any())
            {
                foreach (var workingGroup in workingGroups)
                {
                    foreach (var workingGroupRule in workingGroupRules.Where(x => x.Attribute("workingGroup").Value == workingGroup).Descendants("visibleFields"))
                    {
                        if ((from field in workingGroupRule.Value.Split(',') where field == name select field).FirstOrDefault() != null)
                            readOnly = false;
                    }
                }
            }

            return readOnly;
        }

        public bool EditOnly(string name, string[] workingGroups)
        {
            if (IsLooked)
                return true;

            var node = GetNode("state");
            if (node == null || !node.HasAttributes) return false;

            var currentState = node.Attribute("status") != null ? node.Attribute("status").Value : "";
            if (currentState == string.Empty) return false;

            var status = node.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
            if (status == null) return false;

            // var readOnly = ReadOnly(name);
            //set field to readonly as default
            var readOnly = true;

            if (workingGroups == null)
                return readOnly;

            var workingGroupRules = status.Descendants("workingGroupRules");

            if (workingGroupRules.Any())
            {
                foreach (var workingGroup in workingGroups)
                {
                    foreach (var workingGroupRule in workingGroupRules.Where(x => x.Attribute("workingGroup").Value == workingGroup).Descendants("editOnlyFields"))
                    {
                        if ((from field in workingGroupRule.Value.Split(',') where field == name select field).FirstOrDefault() != null)
                            readOnly = false;
                    }
                }
            }

            return readOnly;
        }


        public bool VisibleTab(string tab)
        {
            var node = GetNode("state");

            if (node == null || !node.HasAttributes) return false;

            var currentState = node.Attribute("status") != null ? node.Attribute("status").Value : "";
            if (currentState == string.Empty) return false;

            var status = node.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
            if (status == null) return false;

            XElement visibleTabs = null;

            if (UserGroups != null)
            {
                var userGroups = status.Descendants("userGroup");

                if (userGroups.Any())
                {
                    foreach (var group in UserGroups)
                    {
                        visibleTabs = userGroups.Where(x => x.Attribute("name").Value == group).Descendants("visibleTabs").FirstOrDefault();
                    }
                }
            }

            if (visibleTabs == null)
                visibleTabs = status.Element("visibleTabs");

            if (visibleTabs == null || visibleTabs.Value == string.Empty) return false;

            return visibleTabs.Value.Split(',').Any(visibleTab => visibleTab == tab);
        }

        public bool ReadOnlyTab(string tab)
        {
            var node = GetNode("state");

            if (node == null || !node.HasAttributes) return false;

            var currentState = node.Attribute("status") != null ? node.Attribute("status").Value : "";
            if (currentState == string.Empty) return false;

            var status = node.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
            if (status == null) return false;

            XElement readOnlyTabs = null;

            if (UserGroups != null)
            {
                var userGroups = status.Descendants("userGroup");

                if (userGroups.Any())
                {
                    foreach (var group in UserGroups)
                    {
                        readOnlyTabs = userGroups.Where(x => x.Attribute("name").Value == group).Descendants("readOnlyTabs").FirstOrDefault();
                    }
                }
            }

            if (readOnlyTabs == null)
                readOnlyTabs = status.Element("readOnlyTabs");

            if (readOnlyTabs == null || readOnlyTabs.Value == string.Empty) return false;

            return readOnlyTabs.Value.Split(',').Any(visibleTab => visibleTab == tab);
        }

        public string GetErrorMessage(string name)
        {
            return GetErrorMessage(name, Language);
        }

        public string GetErrorMessage(string name, string language)
        {
            var node = GetElement(name);

            if (node == null) return string.Empty;

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
            if (actionState.HasValue)
            {
                var state = FormXml.Descendants("state").FirstOrDefault();
                if (state != null && state.HasAttributes)
                {
                    var currentState = state.Attribute("status") != null ? state.Attribute("status").Value : "";
                    if (currentState != string.Empty)
                    {
                        var status = state.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
                        if (status != null)
                        {
                            var action = status.Descendants("action").FirstOrDefault(x => x.HasAttributes && x.Attribute("status").Value == actionState.ToString());
                            if (action != null)
                            {
                                var validateTabs = action.Element("validateTabs");
                                if (validateTabs != null && validateTabs.Value != string.Empty)
                                {
                                    foreach (var validateTab in validateTabs.Value.Split(','))
                                    {
                                        var tab = validateTab;
                                        nodes.AddRange((from n in FormXml.Descendants("tab").Where(x => x.Attribute("name").Value == tab).Elements()
                                                        select n));
                                    }
                                }

                                var validateFields = action.Element("validateFields");
                                if (validateFields != null && validateFields.Value != string.Empty)
                                {
                                    nodes.AddRange(validateFields.Value.Split(',').Select(GetElement).Where(node => node != null));
                                }

                                if (action.Element("validateWhatYouSee") != null)
                                    nodes.AddRange(dictionary.Select(pair => GetElement(pair.Key)).Where(node => node != null));

                                // special working group condition overwrites if visible 
                                var workingGroupRules = action.Descendants("workingGroupRules");
                                if (workingGroups != null && workingGroupRules.Any())
                                {
                                    foreach (var workingGroup in workingGroups)
                                    {
                                        // mandatory rules
                                        foreach (var mandatoryField in workingGroupRules.Where(x => x.Attribute("workingGroup").Value == workingGroup).Descendants("mandatoryFields"))
                                        {
                                            if (mandatoryField.Value != string.Empty)
                                            {
                                                var elems = mandatoryField.Value.Split(',').Select(GetElement).Where(node => node != null);

                                                foreach (var elem in elems)
                                                {
                                                    var name = elem.Attribute("name").Value;

                                                    if (!dictionary.ContainsKey(name)) continue;

                                                    var value = dictionary[name];

                                                    if (string.IsNullOrEmpty(value))
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

            foreach (var node in nodes.Where(node => node.HasAttributes))
            {
                XElement errorElement;
                var validate = false;

                var name = node.Attribute("name").Value;
                var value = dictionary.ContainsKey(name) ? dictionary[name] : null;

                // validation group
                if (node.Attribute("validationgroup") != null)
                {
                    var validateValidationGroup = false;
                    var validationGroups = GetElments("validationgroup", node.Attribute("validationgroup").Value);

                    foreach (var validationGroup in validationGroups)
                    {
                        var answer = GetAnswer(validationGroup.Attribute("name").Value);
                        if (answer != DefaultValue)
                        {
                            validateValidationGroup = true;
                            break;
                        }
                    }

                    if (!validateValidationGroup)
                        continue;
                }

                // Validate If
                var validateIfs = node.Descendants("validateIf");
                if (!validateIfs.Any())
                    validate = true;

                foreach (var validateIf in validateIfs)
                {
                    var validateIfElement = GetElement(validateIf.Attribute("name").Value);

                    if (validateIfElement == null) continue;

                    var validateIfValue = GetAnswer(validateIf.Attribute("name").Value);

                    foreach (var s in validateIfValue.Split(','))
                    {
                        if (validateIf.Value.Split(',').Contains(s))
                            validate = true;
                    }
                }

                if (validate)
                {
                    // disabling Mandatory rules
                    var eventFieldName = dictionary.ContainsKey("FirstEventName") ? dictionary["FirstEventName"] : null;
                    var isApplyedRule = false;

                    if (!string.IsNullOrEmpty(eventFieldName))
                    {
                        var allFieldArray = eventFieldName.Split(';').ToList();
                        foreach (var field in allFieldArray)
                        {
                            if (!string.IsNullOrEmpty(field))
                            {
                                var fieldArray = field.Split(':').ToList();
                                if (fieldArray.Count >= 2)
                                {
                                    var curfieldName = fieldArray[0];
                                    var curfieldValue = fieldArray[1];
                                    var nodeName = node.Attribute("name").Value;
                                    if (curfieldName == nodeName)
                                    {
                                        if (curfieldValue == "0" && string.IsNullOrEmpty(value))
                                        {
                                            isApplyedRule = true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var checkMandatoryValidation = true;
                    if (UserGroups != null)
                    {
                        var state = GetNode("state");

                        if (state != null || state.HasAttributes)
                        {
                            var currentState = state.Attribute("status") != null ? state.Attribute("status").Value : "";
                            if (currentState != string.Empty)
                            {
                                var status = state.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
                                if (status != null)
                                {
                                    var userGroups = status.Descendants("userGroup").Where(x => UserGroups.Contains(x.Attribute("name").Value));
                                    if (userGroups.Any())
                                    {
                                        foreach (var userGroup in userGroups)
                                        {
                                            var optionalFields = userGroup.Descendants("optionalFields").FirstOrDefault();

                                            if (optionalFields == null || optionalFields.Value == string.Empty)
                                                continue;

                                            if (optionalFields.Value.Split(',').Contains(name))
                                                checkMandatoryValidation = false;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (checkMandatoryValidation)
                    {
                        // Mandatory if
                        var mandatoryIfs = node.Descendants("mandatoryIf");
                        var checkMandatoryIf = false;

                        //mandatory if - on checkbox_group
                        if (node.Attribute("type").Value!= null & node.Attribute("type").Value == "checkbox_group")
                        {

                            foreach (var mandatoryif in mandatoryIfs.Where(mandatoryif => mandatoryif.HasAttributes && mandatoryif.Attribute("name") != null))
                            {
                                if (checkMandatoryIf) continue;

                                var mandatoryIfElement = GetElement(mandatoryif.Attribute("name").Value);

                                if (mandatoryIfElement == null) continue;

                                var mandatoryIfValue = GetAnswer(mandatoryif.Attribute("name").Value);

                                foreach (var s in mandatoryIfValue.Split(','))
                                {
                                    if (mandatoryif.Value.Split(',').Contains(s))
                                        checkMandatoryIf = true;
                                }

                                bool hasValue = false;

                                if (checkMandatoryIf)
                                {

                                    //Loop through nodes that are descendant to "checkbox_group"
                                    foreach (var item in node.Descendants())
                                    {
                                        if (item.Attribute("type") != null)
                                        {
                                            if (item.Attribute("type").Value == "checkbox")
                                            {
                                                //if one checkbox in group is checked, it should be ok
                                                if (GetAnswer(item.Attribute("name").Value).Contains("1"))
                                                {
                                                    hasValue = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    if (!hasValue && checkMandatoryIf)
                                    {
                                        errorElement = new XElement("error")
                                        {
                                            Value = Translate(DefaultMandatoryMessage)
                                        };
                                        node.Add(errorElement);

                                    }

                                }
                            }
                        }
                        //regular mandatory if
                        else
                        {
                            foreach (var mandatoryif in mandatoryIfs.Where(mandatoryif => mandatoryif.HasAttributes && mandatoryif.Attribute("name") != null))
                            {
                                if (checkMandatoryIf) continue;

                                var mandatoryIfElement = GetElement(mandatoryif.Attribute("name").Value);

                                if (mandatoryIfElement == null) continue;

                                var mandatoryIfValue = GetAnswer(mandatoryif.Attribute("name").Value);

                                foreach (var s in mandatoryIfValue.Split(','))
                                {
                                    if (mandatoryif.Value.Split(',').Contains(s))
                                        checkMandatoryIf = true;
                                }

                                if (!isApplyedRule && checkMandatoryIf && string.IsNullOrEmpty(value))
                                {
                                    errorElement = new XElement("error")
                                    {
                                        Value = Translate(DefaultMandatoryMessage)
                                    };
                                    node.Add(errorElement);
                                }
                            }
                        }





                        // Mandatory if not empty
                        var mandatoryIfNotEmptys = node.Descendants("mandatoryIfNotEmpty");
                        var checkMandatoryIfNotEmpty = false;

                        foreach (var mandatoryIfNotEmpty in mandatoryIfNotEmptys.Where(x => x.HasAttributes && x.Attribute("name") != null))
                        {
                            if (checkMandatoryIfNotEmpty) continue;

                            var mandatoryIfNotEmptyElement = GetElement(mandatoryIfNotEmpty.Attribute("name").Value);

                            if (mandatoryIfNotEmptyElement == null) continue;

                            if (GetAnswer(mandatoryIfNotEmpty.Attribute("name").Value) != DefaultValue)
                                checkMandatoryIfNotEmpty = true;

                            if (!isApplyedRule && checkMandatoryIfNotEmpty && string.IsNullOrEmpty(value))
                            {
                                errorElement = new XElement("error")
                                {
                                    Value = Translate(DefaultMandatoryMessage)
                                };
                                node.Add(errorElement);
                            }
                        }

                        // Mandatory if not Equal
                        var mandatoryIfNotEquals = node.Descendants("mandatoryIfNotEqual");
                        var checkMandatoryIfNotEqual = false;

                        foreach (var mandatoryIfNotEqual in mandatoryIfNotEquals.Where(x => x.HasAttributes && x.Attribute("name") != null))
                        {
                            if (checkMandatoryIfNotEqual) continue;

                            var mandatoryIfNotEqualElement = GetElement(mandatoryIfNotEqual.Attribute("name").Value);

                            if (mandatoryIfNotEqualElement == null) continue;
                            var mandatoryIfEqualValue = GetAnswer(mandatoryIfNotEqual.Attribute("name").Value);

                            if (mandatoryIfEqualValue != "" && mandatoryIfNotEqual.Value != mandatoryIfEqualValue)
                                checkMandatoryIfNotEqual = true;


                            if (!isApplyedRule && checkMandatoryIfNotEqual && string.IsNullOrEmpty(value))
                            {
                                errorElement = new XElement("error")
                                {
                                    Value = Translate(DefaultMandatoryMessage)
                                };
                                node.Add(errorElement);
                            }
                        }

                        // Mandatory                                                            
                        if (!isApplyedRule && node.Attribute("mandatory") != null && string.IsNullOrWhiteSpace(value))
                        {
                            errorElement = new XElement("error") { Value = Translate(DefaultMandatoryMessage) };
                            node.Add(errorElement);
                        }
                    }


                    // Required
                    if (!isApplyedRule && node.Attribute("required") != null && string.IsNullOrEmpty(value))
                    {
                        errorElement = new XElement("error") { Value = Translate(DefaultRequiredMessage) };
                        node.Add(errorElement);
                    }



                    // groupSum

                    var groupSums = node.Descendants("groupSum");

                    foreach (var groupSum in groupSums.Where(x => x.HasAttributes && x.Attribute("name") != null))
                    {
                        var message = groupSum.Attribute("message").Value;
                        var sum = 0;
                        var groupSumValue = int.Parse(groupSum.Value);

                        foreach (var s in groupSum.Attribute("name").Value.Split(','))
                        {
                            var add = GetAnswer(s);
                            int o;
                            if (add != "" && int.TryParse(add, out o))
                                sum = sum + o;
                        }

                        if (sum != groupSumValue && !string.IsNullOrEmpty(value))
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
                    if (regexIfElements.Any() && !string.IsNullOrEmpty(value))
                    {
                        foreach (var regex in regexIfElements)
                        {
                            if (checkRegexIf) continue;

                            if (!regex.HasAttributes) continue;

                            var regexIfElement = GetElement(regex.Attribute("name").Value);

                            if (regexIfElement == null) continue;

                            var regexIfValue = GetAnswer(regexIfElement.Attribute("name").Value);

                            foreach (var s in regexIfValue.Split(','))
                            {
                                if (regex.Value.Split(',').Contains(s))
                                    checkRegexIf = true;
                            }

                            if (checkRegexIf)
                            {
                                var attrInherit = regex.Attribute("inherit");
                                var attrPattern = regex.Attribute("pattern");
                                var attrMessage = regex.Attribute("message");

                                var message = string.Empty;
                                var pattern = string.Empty;

                                if (!string.IsNullOrEmpty(attrInherit.Value))
                                {
                                    var inheritRegexs = GetNode("regexs");

                                    if (inheritRegexs != null && inheritRegexs.HasElements)
                                    {
                                        var inheritRegex = inheritRegexs.Descendants().FirstOrDefault(x => x.Name == attrInherit.Value);

                                        if (inheritRegex != null)
                                        {
                                            if (!string.IsNullOrEmpty(inheritRegex.Value))
                                                pattern = inheritRegex.Value;
                                            if (inheritRegex.HasAttributes && !string.IsNullOrEmpty(inheritRegex.Attribute("message").Value))
                                                message = inheritRegex.Attribute("message").Value;
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(attrMessage.Value))
                                    message = attrMessage.Value;

                                if (!string.IsNullOrEmpty(attrPattern.Value))
                                    pattern = attrPattern.Value;

                                if (Regex.IsMatch(value, pattern)) continue;

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
                    if (xElements.Any() && !string.IsNullOrEmpty(value))
                    {
                        foreach (var regex in xElements)
                        {
                            if (!regex.HasAttributes) continue;

                            var attrInherit = regex.Attribute("inherit");
                            var attrPattern = regex.Attribute("pattern");
                            var attrMessage = regex.Attribute("message");

                            var validateAllExceptFor = regex.Attribute("validateAllExceptFor");
                            var fieldToValidate = regex.Attribute("dropDownFieldToValidate");
                            var validateFor = regex.Attribute("validateFor");

                            if (validateAllExceptFor != null && fieldToValidate != null)
                            {
                                //Check which fieldToValidate that should NOT be validated
                                var fieldToValidateValue = regex.Attribute("dropDownFieldToValidate").Value;
                                var validateAllExceptForValue = regex.Attribute("validateAllExceptFor").Value;

                                string answerFieldToValidate = FormModelExtensions.GetOptionText(this, fieldToValidateValue, GetAnswer(fieldToValidateValue));

                                //
                                if (answerFieldToValidate.Length > 0 && validateAllExceptForValue.Contains(answerFieldToValidate))
                                {
                                    break;
                                }
                            }

                            if (validateFor != null && fieldToValidate != null)
                            {
                                //Check which fieldToValidate that should be validated
                                var fieldToValidateValue = regex.Attribute("dropDownFieldToValidate").Value;
                                var validateForValue = regex.Attribute("validateFor").Value;

                                string answerFieldToValidate = FormModelExtensions.GetOptionText(this, fieldToValidateValue, GetAnswer(fieldToValidateValue));

                                if (!validateForValue.Contains(answerFieldToValidate))
                                {
                                    break;
                                }
                            }


                            var message = string.Empty;
                            var pattern = string.Empty;

                            if (!string.IsNullOrEmpty(attrInherit.Value))
                            {
                                var inheritRegexs = GetNode("regexs");

                                if (inheritRegexs != null && inheritRegexs.HasElements)
                                {
                                    var inheritRegex = inheritRegexs.Descendants().FirstOrDefault(x => x.Name == attrInherit.Value);

                                    if (inheritRegex != null)
                                    {
                                        if (!string.IsNullOrEmpty(inheritRegex.Value))
                                            pattern = inheritRegex.Value;
                                        if (inheritRegex.HasAttributes && !string.IsNullOrEmpty(inheritRegex.Attribute("message").Value))
                                            message = inheritRegex.Attribute("message").Value;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(attrMessage.Value))
                                message = attrMessage.Value;

                            if (!string.IsNullOrEmpty(attrPattern.Value))
                                pattern = attrPattern.Value;

                            if (Regex.IsMatch(value, pattern)) continue;

                            errorElement = new XElement("error")
                            {
                                Value = message != string.Empty ? Translate(message) : Translate(DefaultErrorMessage)
                            };
                            node.Add(errorElement);
                            break;
                        }
                    }

                    // Future
                    if (node.Attribute("future") != null && !string.IsNullOrEmpty(value))
                    {
                        var date = ConvertToDateTime(value, node.Attribute("format").Value);
                        var result = DateTime.Compare(date.GetValueOrDefault(), DateTime.Now);

                        if (result < 0)
                        {
                            errorElement = new XElement("error") { Value = Translate(DefaultFutureDateMessage) };
                            node.Add(errorElement);
                        }
                    }

                    // Past
                    if (node.Attribute("past") != null && !string.IsNullOrEmpty(value))
                    {
                        var date = ConvertToDateTime(value, node.Attribute("format").Value);
                        var result = DateTime.Compare(date.GetValueOrDefault(), DateTime.Now);

                        if (result >= 0)
                        {
                            errorElement = new XElement("error") { Value = Translate(DefaultPastDateMessage) };
                            node.Add(errorElement);
                        }
                    }

                    // Later than or equal    
                    if (!string.IsNullOrEmpty(value))
                    {
                        var laterThanOrEquals = node.Descendants("laterThanOrEqual");
                        foreach (
                            var laterThanOrEqual in
                                laterThanOrEquals.Where(laterThanOrEqual => laterThanOrEqual.HasAttributes && laterThanOrEqual.Attribute("name") != null)
                            )
                        {
                            var compareWith = GetElement(laterThanOrEqual.Attribute("name").Value);
                            var attrMessage = laterThanOrEqual.Attribute("message");

                            if ((node.Attribute("type") == null || node.Attribute("type").Value != "date")
                                && (compareWith.Attribute("type") == null || compareWith.Attribute("type").Value != "date")) continue;

                            var lbl = compareWith.Attribute("label") != null ? compareWith.Attribute("label").Value : laterThanOrEqual.Attribute("name").Value;
                            var compareWithAnswer = GetAnswer(laterThanOrEqual.Attribute("name").Value);

                            var message = string.Empty;

                            if (attrMessage != null)
                                message = attrMessage.Value;

                            if (value.Contains('/'))
                            {
                                value = value.Replace('/', '.');
                            }
                            if (compareWithAnswer.Contains('/'))
                            {
                                compareWithAnswer = compareWithAnswer.Replace('/', '.');
                            }

                            var date1 = ConvertToDateTime(value, node.Attribute("format").Value);
                            var date2 = ConvertToDateTime(compareWithAnswer, compareWith.Attribute("format").Value);

                            var result = DateTime.Compare(date1.GetValueOrDefault(), date2.GetValueOrDefault());

                            if (result >= 0) continue;

                            errorElement = new XElement("error")
                            {
                                //Value = string.Format(Translate(DefaultLaterOrEqualMessage), Translate(lbl))
                                Value = message != string.Empty ? Translate(message) : string.Format(Translate(DefaultLaterOrEqualMessage), Translate(lbl))
                            };

                            node.Add(errorElement);
                        }

                        // less than or equal
                        var lessThanOrEquals = node.Descendants("lessThanOrEqual");
                        foreach (
                            var lessThanOrEqual in
                                lessThanOrEquals.Where(lessThanOrEqual => lessThanOrEqual.HasAttributes && lessThanOrEqual.Attribute("name") != null)
                            )
                        {
                            var compareWith = GetElement(lessThanOrEqual.Attribute("name").Value);

                            if ((node.Attribute("type") == null || node.Attribute("type").Value != "date")
                                && (compareWith.Attribute("type") == null || compareWith.Attribute("type").Value != "date")) continue;

                            var lbl = compareWith.Attribute("label") != null ? compareWith.Attribute("label").Value : lessThanOrEqual.Attribute("name").Value;
                            var compareWithAnswer = GetAnswer(lessThanOrEqual.Attribute("name").Value);

                            if (value.Contains('/'))
                            {
                                value = value.Replace('/', '.');
                            }
                            if (compareWithAnswer.Contains('/'))
                            {
                                compareWithAnswer = compareWithAnswer.Replace('/', '.');
                            }


                            var date1 = ConvertToDateTime(value, node.Attribute("format").Value);
                            var date2 = ConvertToDateTime(compareWithAnswer, compareWith.Attribute("format").Value);

                            var result = DateTime.Compare(date1.GetValueOrDefault(), date2.GetValueOrDefault());

                            if (result <= 0) continue;

                            errorElement = new XElement("error")
                            {
                                Value = string.Format(Translate(DefaultLessThanOrEqualMessage), Translate(lbl))
                            };

                            node.Add(errorElement);
                        }

                        // Contract Start Date Check
                        var checkStartDates = node.Descendants("checkStartDate");
                        foreach (
                            var checkStartDate in
                                checkStartDates.Where(checkStartDate => checkStartDate.HasAttributes && checkStartDate.Attribute("name") != null)
                            )
                        {

                            var compairWith = GetAnswer(checkStartDate.Attribute("name").Value);
                            if (compairWith != "")
                            {
                                var message = checkStartDate.Attribute("message").Value;
                                var exceptions = checkStartDate.Attribute("exception").Value;
                                List<string> expList = exceptions.Split(',').ToList();

                                var isException = false;
                                var date = ConvertToDateTime(value, node.Attribute("format").Value);
                                if (date != null)
                                {
                                    for (int i = 0; i < expList.Count; i++)
                                    {
                                        var exception = expList[i];
                                        if (exception == compairWith)
                                            isException = true;
                                    }
                                    if (isException) continue;
                                    else
                                    {
                                        if (date.Value.Day == 1 || date.Value.Day == 15) continue;

                                        errorElement = new XElement("error")
                                        {
                                            Value = string.Format(Translate(message))
                                        };

                                        node.Add(errorElement);
                                    }
                                }
                            }

                        }

                        // age check
                        var ageChecks = node.Descendants("agecheck");
                        foreach (
                            var ageCheck in
                                ageChecks.Where(ageCheck => ageCheck.HasAttributes && ageCheck.Attribute("name") != null)
                            )
                        {

                            var validateForValue = "";
                            try
                            {
                                validateForValue = ageCheck.Attribute("validateFor").Value;
                            }
                            catch (Exception)
                            {
                            }

                            var fieldToValidateValue = "";
                            try
                            {
                                fieldToValidateValue = ageCheck.Attribute("dropDownFieldToValidate").Value;
                            }
                            catch (Exception)
                            {
                            }

                            string companyToValidate = FormModelExtensions.GetOptionText(this, fieldToValidateValue, GetAnswer(fieldToValidateValue));

                            if (validateForValue.Contains(companyToValidate) | validateForValue == "")
                            {
                                var compareWith = GetElement(ageCheck.Attribute("name").Value);
                                var message = ageCheck.Attribute("message").Value;
                                var yearShift = int.Parse(ageCheck.Attribute("age").Value);

                                if ((node.Attribute("type") == null || node.Attribute("type").Value != "date")
                                    && (compareWith.Attribute("type") == null || compareWith.Attribute("type").Value != "date")) continue;

                                var lbl = compareWith.Attribute("label") != null ? compareWith.Attribute("label").Value : ageCheck.Attribute("name").Value;
                                var compareWithAnswer = GetAnswer(ageCheck.Attribute("name").Value);

                                if (compareWithAnswer == "")
                                    continue;


                                if (value.Contains('/'))
                                {
                                    value = value.Replace('/', '.');
                                }

                                var date1 = ConvertToDateTime(value, node.Attribute("format").Value);

                                if (compareWithAnswer.Contains('/'))
                                {
                                    compareWithAnswer = compareWithAnswer.Replace('/', '.');
                                }
                                var date2 = ConvertToDateTime(compareWithAnswer, compareWith.Attribute("format").Value);

                                if (!(date1 == null || date2 == null))
                                {
                                    var result = DateTime.Compare(date2.GetValueOrDefault(), DateTime.Now);

                                    DateTime? currentDate = result > 0 ? date2 : DateTime.Now;

                                    var result2 = DateTime.Compare(date1.Value, currentDate.Value.AddYears(-yearShift));

                                    if (result2 <= 0)
                                    {
                                        continue;
                                    }

                                    errorElement = new XElement("error")
                                    {
                                        Value = string.Format(Translate(message))
                                    };

                                    node.Add(errorElement);
                                }
                                else
                                {
                                    errorElement = new XElement("error")
                                    {
                                        Value = string.Format(Translate(message))
                                    };

                                    node.Add(errorElement);
                                }

                            }
                        }


                        // range if
                        var dateRangeIfs = node.Descendants("dateRangeIf");
                        foreach (
                            var dateRangeIf in dateRangeIfs)
                        {
                            var dateRangeIfAnswer = GetAnswer(dateRangeIf.Attribute("name").Value);
                            if (dateRangeIf.Value != dateRangeIfAnswer) continue;

                            var compareWith = GetElement(dateRangeIf.Attribute("range-start-element").Value);

                            if ((node.Attribute("type") == null || node.Attribute("type").Value != "date")
                                && (compareWith.Attribute("type") == null || compareWith.Attribute("type").Value != "date")) continue;

                            var compareWithAnswer = GetAnswer(dateRangeIf.Attribute("range-start-element").Value);

                            var range = dateRangeIf.Attribute("range").Value;
                            var lbl = compareWith.Attribute("label").Value;
                            var message = "";

                            if (dateRangeIf.Attribute("message") != null)
                            {
                                message = dateRangeIf.Attribute("message").Value;
                            }

                            var date1 = ConvertToDateTime(value, node.Attribute("format").Value);
                            var date2 = ConvertToDateTime(compareWithAnswer, compareWith.Attribute("format").Value);

                            date2 = date2.GetValueOrDefault().AddMonths(int.Parse(range));

                            var result = DateTime.Compare(date1.GetValueOrDefault(), date2.GetValueOrDefault());

                            if (result <= 0) continue;

                            if (message != "")
                            {
                                errorElement = new XElement("error")
                                {
                                    Value = string.Format(Translate(message), Translate(dateRangeIfAnswer), range)
                                };
                            }
                            else
                            {
                                errorElement = new XElement("error")
                                {
                                    Value = string.Format(Translate(DefaultDateRangeIfMessage), Translate(dateRangeIfAnswer), range)
                                };
                            }
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

            foreach (var node in nodes)
            {
                if (!node.HasAttributes) continue;

                var name = node.Attribute("name").Value;

                if (!dictionary.ContainsKey(name))
                    continue;

                var value = dictionary.ContainsKey(name) ? dictionary[name] : null;
                var answer = node.Descendants("answer").FirstOrDefault();

                if (answer == null)
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

            foreach (var node in nodes)
            {
                var errorElement = node.Descendants("error").FirstOrDefault();

                if (errorElement != null)
                    errorElement.Remove();

                var name = node.Attribute("name").Value;
                var value = dictionary.ContainsKey(name) ? dictionary[name] : null;

                var answer = node.Descendants("answer").FirstOrDefault();

                if (answer == null)
                {
                    if (value != null)
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

            if (node == null || dictionary == null) return;

            var options = node.Descendants("options").FirstOrDefault();
            if (options != null)
                options.Remove();

            options = new XElement("options");

            var option = new XElement("option") { Value = "" };
            var attribute = new XAttribute("value", "");
            option.Add(attribute);
            options.Add(option);

            foreach (var pair in dictionary)
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
            return parent.Descendants("element").FirstOrDefault(x => x.HasAttributes && x.Attribute("name") != null && x.Attribute("name").Value == name);
        }

        public void SetNodeValue(string nodeName, string value)
        {
            var node = GetNode(nodeName);

            if (node == null)
            {
                node = new XElement(nodeName) { Value = value };
                if (FormXml != null && FormXml.Root != null)
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
        }

        public string ToJson(XElement node)
        {
            var xmlNode = node.ToXmlNode();
            xmlNode = TrimXmlNode(xmlNode);

            return Regex.Replace(JsonConvert.SerializeXmlNode(xmlNode, Newtonsoft.Json.Formatting.Indented), "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);
        }

        public void AddError(string nodeName, string message)
        {
            var node = GetElement(nodeName);
            if (node == null) return;

            var errorElement = new XElement("error") { Value = message };
            node.Add(errorElement);
        }

        public void ClearErrors()
        {
            FormXml.Descendants("error").Remove();
        }

        public string ClientValidate(int actionState = 0, string eventFieldName = "")
        {
            var nodes = new List<XElement>();

            // Validate depending on action
            var state = FormXml.Descendants("state").FirstOrDefault();

            if (state != null && state.HasAttributes && state.Attribute("status") != null)
            {
                var currentState = state.Attribute("status").Value;
                if (currentState != string.Empty)
                {
                    var status = state.Descendants("status").FirstOrDefault(x => x.HasAttributes && x.Attribute("value").Value == currentState);
                    if (status != null)
                    {
                        var action = status.Descendants("action")
                            .FirstOrDefault(x => x.HasAttributes && x.Attribute("status").Value == actionState.ToString(CultureInfo.InvariantCulture));

                        if (action != null)
                        {
                            var validateTabs = action.Element("validateTabs");
                            if (validateTabs != null && validateTabs.Value != string.Empty)
                            {
                                foreach (var validateTab in validateTabs.Value.Split(','))
                                {
                                    var tab = validateTab;
                                    nodes.AddRange((from n in FormXml.Descendants("tab").Where(x => x.Attribute("name").Value == tab).Elements()
                                                    select n));
                                }
                            }
                            var validateFields = action.Element("validateFields");
                            if (validateFields != null && validateFields.Value != string.Empty)
                            {
                                nodes.AddRange(validateFields.Value.Split(',').Select(GetElement).Where(node => node != null));
                            }

                            if (action.Element("validateWhatYouSee") != null)
                                nodes.AddRange((from n in FormXml.Descendants("tab").Descendants("element")
                                                select n));
                        }
                    }
                }
            }

            var clientValidates = new ArrayList();

            foreach (var node in nodes.Where(x => x.Attribute("validationgroup") == null))
            {
                var obj = new ClientValidate { Name = node.Attribute("name").Value };

                var isApplyedRule = false;
                if (node.Attribute("mandatory") != null)
                {
                    if (!string.IsNullOrEmpty(eventFieldName))
                    {
                        var allFieldArray = eventFieldName.Split(';').ToList();
                        foreach (var field in allFieldArray)
                        {
                            if (!string.IsNullOrEmpty(field))
                            {
                                var fieldArray = field.Split(':').ToList();
                                if (fieldArray.Count >= 2)
                                {
                                    var curfieldName = fieldArray[0];
                                    var curfieldValue = fieldArray[1];
                                    if (curfieldName == obj.Name)
                                    {
                                        obj.Rules.Add("required", curfieldValue);
                                        isApplyedRule = true;
                                    }
                                }
                            }
                        }
                    }

                    if (!isApplyedRule)
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

            if (element == null || answer == null) return null;
            if (element.Attribute("type").Value != "date") return null;

            var format = element.Attribute("format").Value;
            return ConvertToDateTime(answer, format);
        }

        public DateTime? ConvertToDateTime(string value, string format)
        {
            DateTime dt;
            if (DateTime.TryParseExact(value, format, null, DateTimeStyles.None, out dt))
                return dt;
            return null;
        }

        private XmlNode TrimXmlNode(XmlNode node)
        {
            if (node.InnerXml != "")
                node.InnerXml = node.InnerXml.Trim().Replace("\r\n", "");

            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                    TrimXmlNode(child);
            }

            return node;
        }
    }
}