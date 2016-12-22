using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Xml;
using System.Xml.Linq;
using ECT.Core;
using ECT.Model.Entities;
using ECT.Web.Helpers;
using ECT.Web.Models;

namespace ECT.Web
{
    public static class StringExtensions
    {
        public static string AddSlash(this string s)
        {
            if(string.IsNullOrEmpty(s))
                throw new ArgumentNullException(s);

            if(!s.EndsWith("\\"))
                s = s + "\\";

            return s;
        }

        public static string FindMinusCharachter(this string s)
        {
            if(string.IsNullOrEmpty(s)) return s;
            int found = 0;
            if(string.IsNullOrEmpty(s))
                throw new ArgumentNullException(s);
            found = s.IndexOf("-");
            if(found > 0)
                s = s.Substring(found + 1);
            return s;
        }

        public static string CurrentController(this UrlHelper urlHelper)
        {
            var rd = urlHelper.RequestContext.RouteData;
            // in case using virtual dirctory 
            var rootUrl = urlHelper.Content("~/");

            if(rd.DataTokens["area"] != null)
                return string.Format("{0}{1}/{2}/", rootUrl, rd.DataTokens["area"], rd.GetRequiredString("controller"));
            return string.Format("{0}{1}/", rootUrl, rd.GetRequiredString("controller"));
        }
    }

    public static class DocumentExtensions
    {
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using(var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        public static XmlNode ToXmlNode(this XElement element)
        {
            using(var xmlReader = element.CreateReader())
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }

        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using(var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        public static XElement ToXElement(this XmlNode node)
        {
            var xDoc = new XDocument();
            using(var xmlWriter = xDoc.CreateWriter())
                node.WriteTo(xmlWriter);
            return xDoc.Root;
        }
    }

    public static class ListExtensions
    {
        public static IDictionary<string, string> ToDictionary(this NameValueCollection coll)
        {
            var dict = new Dictionary<string, string>();

            if(coll == null) return dict;

            foreach(var key in coll.Keys)
                dict.Add(key.ToString(), coll[key.ToString()]);

            return dict;
        }

        public static IDictionary<string, string> ToDictionary(this IList<Option> options)
        {
            var dict = new Dictionary<string, string>();

            if(options == null) return dict;

            foreach(var option in options)
                dict.Add(option.Name, option.Key);

            return dict;
        }
    }

    public static class HtmlHelperExtensions
    {
        public static string Message(this HtmlHelper helper)
        {
            return (string)helper.ViewData[Constants.ViewData.Message];
        }

        public static string Version(this HtmlHelper helper)
        {
            var asm = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(asm.Location);
            return string.Format("{0}.{1}.{2}.{3}", fvi.ProductMajorPart, fvi.ProductMinorPart, fvi.ProductBuildPart, fvi.ProductPrivatePart);
        }
    }

    public static class RazorExtensions
    {
        // http://haacked.com/archive/2011/02/27/templated-razor-delegates.aspx
        public static HelperResult List<T>(this IEnumerable<T> items, Func<T, HelperResult> template)
        {
            return new HelperResult(writer =>
            {
                foreach(var item in items)
                {
                    template(item).WriteTo(writer);
                }
            });
        }
    }

    public static class ControllerExtensions
    {
        public static PdfContentResult Pdf(this Controller controller, byte[] fileContents)
        {
            return new PdfContentResult(fileContents);
        }

        public static PdfContentResult Pdf(this Controller controller, byte[] fileContents, string fileName)
        {
            return new PdfContentResult(fileContents, fileName);
        }
    }
}