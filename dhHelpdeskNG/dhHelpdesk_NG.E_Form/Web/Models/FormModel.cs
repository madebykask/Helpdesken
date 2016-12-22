using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using ECT.Model.Entities;

namespace ECT.Web.Models
{
    public class FormModel : BaseModel
    {
        public FormModel(string xmlPath)
            : base(xmlPath)
        {
            if(SessionFacade.User != null
                && !string.IsNullOrEmpty(SessionFacade.User.Language)
                && IsValidLanguage(SessionFacade.User.Language))
            {
                this.Language = SessionFacade.User.Language;
            }
        }

        private int _status;

        public string ActiveTab { get; set; }
        public string ActiveStatus { get; set; }
        public int Status { get { return _status; } set { _status = value; SetAttributeValue("state", "status", value.ToString(CultureInfo.InvariantCulture)); } }
        public Contract Contract { get; set; }
        public Company Company { get; set; }
        public IDictionary<string, string> FormDictionary { get; set; }
        public IDictionary<string, string> GvValues { get; set; }

        public string ShowGVData(string formFieldName)
        {
            string ret = string.Empty;
            ret = GvValues != null ? GvValues.FirstOrDefault(x => x.Key == formFieldName).Value : "";
            return ret;
        }

        public IList<string> TypeAhead(string node, string dependentValue)
        {
            var elem = GetElement(node);
            if(elem == null) return null;

            var l = new List<string>();
            var dependent = elem.Element("dependent");
            if(dependent != null && dependentValue != "")
            {
                var sel = dependent.Descendants("sel").Where(x => x.Attribute("value").Value == dependentValue).FirstOrDefault();
                if(sel != null)
                {
                    var dep = sel.Value.Split(',');
                    for(int i = 0; i < dep.Length; i++)
                    {
                        if(dep[i].Trim() == "") continue;
                        l.Add(dep[i].Trim());
                    }

                    return l;
                }
            }

            var options = elem.Descendants("options").FirstOrDefault();
            if(options != null)
            {
                var dec = options.Descendants().ToList();
                for(int i = 0; i < dec.Count; i++)
                {
                    if(dec[i].Value == "") continue;
                    l.Add(dec[i].Value);
                }
            }

            return l;
        }
    }
} 