using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DH.Helpdesk.Services.BusinessLogic.Cases
{
    public class ExtendedCaseTemplateParser
    {
        #region BracketType

        public enum BracketType
        {
            None = 0,
            Curly = 1,
            Square = 2
        }

        #endregion

        #region jsObjects

        private readonly string[] _jsObjects = {
            "dataSource",
            "multiSectionAction",
            "populateAction",
            "enableAction",
            "disabledStateBehavior",
            "dataSources",
            "validators"
        };

        #endregion

        #region Public Methods

        public Dictionary<string, string> ExtractCaseBindingFields(string template)
        {
            Dictionary<string, string> errors;
            var caseBindingFieldsMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var templateModel = Parse(template, out errors);

            if (errors != null && errors.Any())
                throw new Exception("Template parsing failed.");
            
            if (templateModel?.tabs != null)
            {
                //field path examples:
                //tabs.ServiceRequestDetails.sections.Address.instances[0].controls.PostalCode
                //tabs.ServiceRequestDetails.sections.STD_S_CoWorkerSearch.instances[0].controls.Company

                foreach (var tabModel in templateModel.tabs)
                {
                    var tabId = tabModel.id;
                    foreach (var sectionModel in tabModel.sections)
                    {
                        var sectionId = sectionModel.id;
                        foreach (var controlModel in sectionModel.controls)
                        {
                            if (!string.IsNullOrEmpty(controlModel.caseBinding))
                            {
                                var fieldPath =
                                    $"tabs.{tabId}.sections.{sectionId}.instances[0].controls.{controlModel.id}";

                                caseBindingFieldsMap.Add(controlModel.caseBinding, fieldPath);
                            }
                        }
                    }
                }
            }

            return caseBindingFieldsMap;
        }

        public TemplateModel Parse(string template, out Dictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(template))
                return null;
            
            var contentStartIndex = template.IndexOf("tabs:", StringComparison.OrdinalIgnoreCase);
            if (contentStartIndex == -1)
                return null;

            //todo: remove comments from templaet

            //extract tabs content 
            var content = "{" + template.Substring(contentStartIndex);

            //remove special characters
            content = Regex.Replace(content, @"\/\*[\s\S]*?\*\/|([^:]|^)\/\/.*$", "", RegexOptions.Multiline); // remove comments
            content = content.Replace(Environment.NewLine, string.Empty).Replace("\t", " "); //remove new lines 
            content = Regex.Replace(content, @"(""(?:[^""\\]|\\.)*"")|\s+", "$1", RegexOptions.Multiline | RegexOptions.IgnoreCase); // remove spaces

            //remove functions
            var jsonContent = TrimFunctions(content);

            //remove objects
            foreach (var jsObject in _jsObjects)
            {
                jsonContent = TrimObjects(jsonContent, jsObject);
            }

            Dictionary<string, string> dictionary = errors;
            var model = JsonConvert.DeserializeObject<TemplateModel>(jsonContent, new JsonSerializerSettings()
            {
                Formatting = Formatting.None,
                Error = (object sender, ErrorEventArgs e) =>
                {
                    e.ErrorContext.Handled = true;
                    dictionary.Add(e.ErrorContext.Path, e.ErrorContext.Error.Message);
                }
            });

            //todo: log errors

            return model;
        }

        #endregion

        #region Private Methods

        private static string TrimFunctions(string input)
        {
            var strBld = new StringBuilder();
            var curPos = 0;

            var startPos = input.IndexOf("function", 0, StringComparison.OrdinalIgnoreCase);

            if (startPos != -1)
            {
                while (startPos != -1)
                {
                    var content = input.Substring(curPos, startPos - curPos);
                    strBld.Append(content);

                    var endPos = FindMatchingBracketIndex(input, startPos, BracketType.Curly);
                    if (endPos == -1)
                        throw new Exception("Failed to find matching bracket");

                    //add "" instead of function body
                    strBld.Append("\"\"");

                    curPos = endPos + 1;
                    startPos = input.IndexOf("function", curPos, StringComparison.OrdinalIgnoreCase);
                }

                if (curPos < input.Length - 1)
                    strBld.Append(input.Substring(curPos));
            }
            else
            {
                strBld.Append(input);
            }

            return strBld.ToString();
        }

        private static string TrimObjects(string input, string objectName)
        {
            var strBld = new StringBuilder();
            var curPos = 0;

            var startPos = input.IndexOf($"{objectName}:", 0, StringComparison.OrdinalIgnoreCase);

            if (startPos != -1)
            {
                while (startPos != -1)
                {
                    var content = input.Substring(curPos, startPos - curPos);
                    strBld.Append(content);

                    var character = input[startPos + objectName.Length + 1];
                    var bracketType = ResolveBracketType(character);

                    // check if its object or array. If not - just skip it. 
                    if (bracketType == BracketType.None)
                    {
                        curPos = startPos;

                        //find next object
                        startPos = input.IndexOf($"{objectName}:", curPos + objectName.Length + 1, StringComparison.OrdinalIgnoreCase);
                    }
                    else
                    {
                        var endPos = FindMatchingBracketIndex(input, startPos, bracketType);
                        if (endPos != -1)
                        {
                            curPos = endPos + 1;
                            startPos = input.IndexOf($"{objectName}:", curPos, StringComparison.OrdinalIgnoreCase);

                            //replace object with empty string
                            strBld.Append($"{objectName}: \"\"");
                        }
                    }
                }

                //read rest of the content
                if (curPos < input.Length - 1)
                    strBld.Append(input.Substring(curPos));
            }
            else
            {
                strBld.Append(input);
            }

            return strBld.ToString();
        }

        private static int FindMatchingBracketIndex(string input, int startPos, BracketType bracketType)
        {
            var openBracketChar = ResolveBracketCharacter(bracketType);
            var closingBracketChar = ResolveBracketCharacter(bracketType, true);
            var str = input.Substring(startPos);

            var stack = new Stack<char>();
            for (int i = 0; i < str.Length; i++)
            {
                var current = str[i];

                if (current == openBracketChar)
                {
                    stack.Push(current);
                }
                else if (current == closingBracketChar)
                {
                    if (stack.Count == 0)
                        return -1;

                    var last = stack.Peek();

                    if (last == openBracketChar)
                    {
                        stack.Pop();

                        if (stack.Count == 0)
                            return startPos + i;

                    }
                    else
                        return -1;
                }
            }

            return -1;
        }

        private static BracketType ResolveBracketType(char character)
        {
            var bracketType = BracketType.None;
            if (character == '[')
                bracketType = BracketType.Square;
            else if (character == '{')
                bracketType = BracketType.Curly;

            return bracketType;
        }

        private static char ResolveBracketCharacter(BracketType bracketType, bool isClosing = false)
        {
            switch (bracketType)
            {
                case BracketType.Curly:
                    return isClosing ? '}' : '{';
                case BracketType.Square:
                    return isClosing ? ']' : '[';
                default:
                    throw new NotSupportedException();
            }
        }

        #endregion
    }

    #region Models

    public class TemplateModel
    {
        public List<TabModel> tabs { get; set; }
    }

    public class TabModel
    {
        public string id { get; set; }
        public List<SectionModel> sections { get; set; }
    }

    public class SectionModel
    {
        public string id { get; set; }
        public List<ControlModel> controls { get; set; }
    }
    
    public class ControlModel
    {
        public string id { get; set; }
        public string caseBinding { get; set; }
    }

    #endregion
}