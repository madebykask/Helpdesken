using System;
using System.Collections;
using System.Collections.Generic;

namespace DH.Helpdesk.Models.Case.Field
{
    public interface IBaseCaseField
    {
        string Name { get; set; }
        string Label { get; set; }
        List<KeyValuePair<string, string>> Options {get; set;}
    };

    public class BaseCaseField<T> : IBaseCaseField
    {
        public BaseCaseField()
        {
            Options = new List<KeyValuePair<string, string>>();
        }

        public string Name { get; set; }
        public string Label { get; set; }

        public string JsonType
        {
            get
            {
                var jsonType = "";
                var type = typeof(T);

                if (type == typeof(DateTime)) jsonType = "date";
                if (type == typeof(int) || type == typeof(decimal)) jsonType = "number";
                if (type == typeof(string)) jsonType = "string";
                if (type.GetInterface(nameof(IEnumerable)) != null) jsonType = "array";
                //TODO: add other types

                return jsonType;
            }
        }

        public T Value { get; set; }
        public string Section { get; set; }
        public List<KeyValuePair<string, string>> Options {get; set;} //TODO: use dictionary
    }
}
