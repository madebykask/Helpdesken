using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DH.Helpdesk.SelfService.Infrastructure.Configuration
{
    #region SelfServiceUrlSettings

    public interface ISelfServiceUrlSettings
    {
        IList<string> AllowedUrls { get; }
        IList<string> DeniedUrls { get; }
    }

    public class SelfServiceUrlSettings : ISelfServiceUrlSettings
    {
        public IList<string> AllowedUrls { get; set; }
        public IList<string> DeniedUrls { get; set; }
    }

    #endregion

    public class SelfServiceUrlSetting : ConfigurationSection
    {
        public const string SectionPath = "selfServiceConfigurable/selfServiceUrlSetting";

        [ConfigurationProperty("allowedUrls", IsDefaultCollection = true)]
        public UrlCollection AllowedUrls
        {
            get { return (UrlCollection)base["allowedUrls"]; }
        }

        [ConfigurationProperty("deniedUrls", IsDefaultCollection = true)]
        public UrlCollection DeniedUrls
        {
            get { return (UrlCollection)base["deniedUrls"]; }
        }

        #region Factory Method

        public static ISelfServiceUrlSettings GetSelfServiceUrlSettings()
        {
            var setting = (SelfServiceUrlSetting)ConfigurationManager.GetSection(SectionPath);
            if (setting == null)
                return new SelfServiceUrlSettings();
            
            return new SelfServiceUrlSettings
            {
                AllowedUrls = setting.AllowedUrls?.Select(x => x.Path).ToList(),
                DeniedUrls = setting.DeniedUrls?.Select(x => x.Path).ToList()
            };
        }

        #endregion
    }

    #region UrlCollection 

    public sealed class UrlCollection : ConfigurationElementCollection, IEnumerable<UrlElement>
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new UrlElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UrlElement)element).Key;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "url"; }
        }

        public UrlElement this[int index]
        {
            get { return (UrlElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }       

        public bool ContainsKey(string key)
        {
            bool result = false;
            object[] keys = BaseGetAllKeys();

            foreach (object obj in keys)
            {
                if ((string)obj == key)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
       
        public new IEnumerator<UrlElement> GetEnumerator()
        {
            int count = base.Count;
            for (int i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as UrlElement;
            }
        }        
    }

    #endregion

    #region UrlElement

    public sealed class UrlElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }
    }

    #endregion
}