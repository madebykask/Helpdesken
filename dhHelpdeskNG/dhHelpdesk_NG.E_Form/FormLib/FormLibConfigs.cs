using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ECT.FormLib.Configurable
{
    public class AccessManagment : ConfigurationSection
    {
        public static AccessManagment GetConfiguration()
        {
            var configuration = ConfigurationManager.GetSection("formLibConfigurable/accessManagment") as AccessManagment;

            if(configuration != null)
                return configuration;

            return new AccessManagment();
        }

        [ConfigurationProperty("api")]
        public ApiElement Api
        {
            get
            {
                return (ApiElement)this["api"]; 
            }
            set
            { this["api"] = value; }
        }

        [ConfigurationProperty("countries", IsDefaultCollection = true)]
        public CountryCollection Countries
        {
            get { return (CountryCollection)base["countries"]; }
        }
    }

    public sealed class ApiElement : ConfigurationElement
    {
        [ConfigurationProperty("uriPath", IsRequired = true)]
        public string UriPath
        {
            get { return (string)this["uriPath"]; }
            set { this["uriPath"] = value; }
        }

        [ConfigurationProperty("username", IsRequired = true)]
        public string Username
        {
            get { return (string)this["username"]; }
            set { this["username"] = value; }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }
    }

    public sealed class CountryElement : ConfigurationElement
    {
        [ConfigurationProperty("helpdeskCustomerId", IsKey = true, IsRequired = true)]
        public string HelpdeskCustomerId
        {
            get { return (string)this["helpdeskCustomerId"]; }
            set { this["helpdeskCustomerId"] = value; }
        }

        [ConfigurationProperty("employeePrefix", IsRequired = true)]
        public string EmployeePrefix
        {
            get { return (string)this["employeePrefix"]; }
            set { this["employeePrefix"] = value; }
        }

        [ConfigurationProperty("code", IsRequired = true)]
        public string Code
        {
            get { return (string)this["code"]; }
            set { this["code"] = value; }
        }
    }

    public sealed class CountryCollection : ConfigurationElementCollection, IEnumerable<CountryElement>
    {   
        
        protected override ConfigurationElement CreateNewElement()
        {
            return new CountryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CountryElement)element).HelpdeskCustomerId;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "country"; }
        }

        public CountryElement this[int index]
        {
            get { return (CountryElement)BaseGet(index); }
            set
            {
                if(BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        new public CountryElement this[string helpdeskCustomerId]
        {
            get { return (CountryElement)BaseGet(helpdeskCustomerId); }
        }

        public bool ContainsKey(string key)
        {
            bool result = false;
            object[] keys = BaseGetAllKeys();

            foreach(object obj in keys)
            {
                if((string)obj == key)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        #region IEnumerable<CountryElement> Members

        public new IEnumerator<CountryElement> GetEnumerator()
        {
            int count = base.Count;
            for(int i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as CountryElement;
            }
        }

        #endregion
    }
}