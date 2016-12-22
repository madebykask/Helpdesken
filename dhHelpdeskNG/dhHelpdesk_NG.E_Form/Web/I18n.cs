using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using ECT.Core;
using ECT.Core.Cache;

namespace ECT.Web
{
    public class I18N
    {
        public static string Translate(string name, string language) 
        {
            var cache = new CacheProvider();

            var xi18N = cache.Get(Constants.Cache.XI18N) as XDocument;

            if(xi18N == null)
            {
                var path = System.Web.HttpContext.Current.Server.MapPath("~/Xml_Data");
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(Path.Combine(path, "I18n.xml"));
                xi18N = xmlDocument.ToXDocument();

                if(xi18N != null)
                    cache.Set(Constants.Cache.XI18N, xi18N, Constants.Cache.CacheTime);
            }

            if (xi18N == null || string.IsNullOrWhiteSpace(name)) return name;

            var node =
                (from p in xi18N.Descendants("lang").Where(x => x.Attribute("name").Value == language)
                    .Descendants("data").Where(x => String.Equals(x.Attribute("name").Value, name, StringComparison.CurrentCultureIgnoreCase))
                 select p).FirstOrDefault();

            return node != null ? node.Value : name;
        }
    }
}