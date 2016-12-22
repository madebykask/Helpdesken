using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Linq;
using DH.Helpdesk.EForm.Core.Cache;
using DH.Helpdesk.EForm.Model.Abstract;
using DH.Helpdesk.EForm.Model.Contrete;
using DH.Helpdesk.EForm.Model.Entities;

namespace DH.Helpdesk.EForm.FormLib
{
    public interface IFormLibI18N
    {
        string Translate(string name, string language);
    }

    // TODO: This is ugly fix with DI pattern in future...
    public class FormLibI18N
    {
        //This is used by contract in Poland and SouthKorea
        //TODO: Do we need this?
        public static string Translate(string name, string language)
        {
            if(string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(language)) return name;

            var cache = new CacheProvider();
            var xi18N = cache.Get(FormLibConstants.Cache.XI18N) as List<TextTranslation>;

            if(xi18N == null)
            {
                var textRepository = new TextRepository(System.Configuration.ConfigurationManager.ConnectionStrings["DSN"].ConnectionString);
                xi18N = textRepository.GetTextTranslations(null).ToList();

                if(xi18N != null)
                    cache.Set(FormLibConstants.Cache.XI18N, xi18N, FormLibConstants.Cache.CacheTimeInMinutes);
            }

            var translation = xi18N.Where(x => x.LanguageId.ToLower() == language.ToLower() && x.Text.Trim().ToLower() == name.Trim().ToLower()).FirstOrDefault();

            return translation == null ? name : (string.IsNullOrEmpty(translation.Translation)) ? name : translation.Translation;
        }

        public static string Translate(string name, string language, int? textType, int customerId, Guid formGuid, string source, bool logTranslations)
        {

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(language)) return name;
            
            var textRepository = new TextRepository(System.Configuration.ConfigurationManager.ConnectionStrings["DSN"].ConnectionString);

            //If set to true, it logs all request in tblLogTextTranslations. This is set for each customer in tblFormSettings.
            if (logTranslations)
            {
                textRepository.LogTextTranslation(name, customerId, language, formGuid, source);
            } 

            var cache = new CacheProvider();
            var xi18N = cache.Get(FormLibConstants.Cache.XI18N + textType.ToString()) as List<TextTranslation>;

            if (xi18N == null)
            {
                xi18N = textRepository.GetTextTranslations(textType).ToList();
                if (xi18N != null)
                    cache.Set(FormLibConstants.Cache.XI18N + textType.ToString(), xi18N, FormLibConstants.Cache.CacheTimeInMinutes);
            }

            var translation = xi18N.Where(x => x.LanguageId.ToLower() == language.ToLower() && x.Text.Trim().ToLower() == name.Trim().ToLower()).FirstOrDefault();

            return translation == null ? name : (string.IsNullOrEmpty(translation.Translation)) ? name : translation.Translation;
        }


        public static string TranslateRevert(string text, string language, int? textType)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(language)) return text;


            var cache = new CacheProvider();
            var xi18N = cache.Get(FormLibConstants.Cache.XI18N + textType.ToString()) as List<TextTranslation>;

            if (xi18N == null)
            {
                var textRepository = new TextRepository(System.Configuration.ConfigurationManager.ConnectionStrings["DSN"].ConnectionString);
                xi18N = textRepository.GetTextTranslations(textType).ToList();

                if (xi18N != null)
                    cache.Set(FormLibConstants.Cache.XI18N + textType.ToString(), xi18N, FormLibConstants.Cache.CacheTimeInMinutes);
            }

            //Get Text
            var translation = xi18N.Where(x => x.Translation.Trim().ToLower() == text.Trim().ToLower()).FirstOrDefault();

            return translation == null ? text : (string.IsNullOrEmpty(translation.Text)) ? text : translation.Text;
        }
    }

    //public class FormLibI18N
    //{
    //    public static string Translate(string name, string language)
    //    {
    //        var cache = new CacheProvider();

    //        var xi18N = cache.Get(FormLibConstants.Cache.XI18N) as XDocument;

    //        if(xi18N == null)
    //        {
    //            var path = new DirectoryInfo(HostingEnvironment.MapPath(FormLibConstants.Paths.XmlDirectory)).FullName;
    //            var xmlDocument = new XmlDocument();
    //            xmlDocument.Load(Path.Combine(path, "I18n.xml"));
    //            xi18N = xmlDocument.ToXDocument();

    //            if(xi18N != null)
    //                cache.Set(FormLibConstants.Cache.XI18N, xi18N, FormLibConstants.Cache.CacheTimeInMinutes);
    //        }

    //        if(xi18N == null || string.IsNullOrWhiteSpace(name)) return name;

    //        var node =
    //            (from p in xi18N.Descendants("lang").Where(x => x.Attribute("name").Value == language)
    //                .Descendants("data").Where(x => String.Equals(x.Attribute("name").Value, name, StringComparison.CurrentCultureIgnoreCase))
    //             select p).FirstOrDefault();

    //        return node != null ? node.Value : name;
    //    }
    //}
}