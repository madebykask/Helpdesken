﻿using System;
using System.Collections;
using System.Collections.Generic;
using DH.Helpdesk.Common.Enums.Cases;

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

                if (type == typeof(DateTime))
                    return "date";

                if (type == typeof(int) || type == typeof(decimal))
                    return "number";

                if (type == typeof(string))
                    return "string";

                if (type.GetInterface(nameof(IEnumerable)) != null)
                    return "array";
                
                //TODO: add other types

                return jsonType;
            }
        }

        public T Value { get; set; }
        public CaseSectionType Section { get; set; }
        public List<KeyValuePair<string, string>> Options {get; set;} //TODO: use dictionary
    }
}
