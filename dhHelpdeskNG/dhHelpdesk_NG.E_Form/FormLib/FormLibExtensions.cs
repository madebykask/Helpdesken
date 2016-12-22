using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using ECT.FormLib.Models;
using ECT.Model.Entities;

namespace ECT.FormLib
{
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

        public static string FindDashCharachter(this string s)
        {
            if(string.IsNullOrEmpty(s)) return s;
            int found = 0;            
            if(string.IsNullOrEmpty(s))
                throw new ArgumentNullException(s);
            
           
            string resultString;

            found = s.IndexOf("-");
            if(found > 0)
            {
               resultString = s.Substring(found + 1);
                
               string checkIsNumber = System.Text.RegularExpressions.Regex.Match(resultString, @"\d+").Value;

               do
               {
                   found += resultString.IndexOf("-");
                   resultString = s.Substring(found ++);
                   checkIsNumber = System.Text.RegularExpressions.Regex.Match(resultString, @"\d+").Value;
                   if (resultString.StartsWith(" "))
                       checkIsNumber = " " + checkIsNumber;                   

               } while (!resultString.StartsWith(checkIsNumber));

                   s = s.Remove(found - 2);
            }

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

        public static string CurrentRoot(this UrlHelper urlHelper)
        {
            var rd = urlHelper.RequestContext.RouteData;
            // in case using virtual dirctory 
            var rootUrl = urlHelper.Content("~/");

            if (rootUrl.StartsWith("/") || rootUrl.StartsWith("\\"))
                rootUrl = rootUrl.Remove(0);

            return rootUrl;
        }

        public static string CurrentArea(this UrlHelper urlHelper)
        {
            var rd = urlHelper.RequestContext.RouteData;
            // in case using virtual dirctory 
            var rootUrl = urlHelper.Content("~/");

            if (rd.DataTokens["area"] != null)
                return rd.DataTokens["area"].ToString();
            return ""; 
        }

        public static string GetUserFromAdPath(this string path)
        {
            string s = path;
            int stop = s.IndexOf("\\", StringComparison.Ordinal);
            if(stop <= -1)
            {
                return string.Empty;
            }

            s = s.Substring(stop + 1, s.Length - stop - 1);
            if(s.Length > 20)
            {
                s = s.Substring(0, 20);
            }

            return s;
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
            return (string)helper.ViewData[FormLibConstants.ViewData.Message];
        }

        public static string Version(this HtmlHelper helper)
        {
            var asm = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(asm.Location);
            return string.Format("{0}.{1}.{2}.{3}", fvi.ProductMajorPart, fvi.ProductMinorPart, fvi.ProductBuildPart, fvi.ProductPrivatePart);
        }
    }

    public static class ControllerExtensions
    {
        public static FormLibPdfContentResult Pdf(this Controller controller, byte[] fileContents)
        {
            return new FormLibPdfContentResult(fileContents);
        }

        public static FormLibPdfContentResult Pdf(this Controller controller, byte[] fileContents, string fileName)
        {
            return new FormLibPdfContentResult(fileContents, fileName);
        }

        public static string BaseUrl(this Controller controller) 
        {
            return BaseUrl(controller, null);
        }

        public static string BaseUrl(this Controller controller, string relativeUrl)
        {
            var request = controller.Request;

            return string.Format("{0}://{1}{2}",
                (request.IsSecureConnection) ? "https" : "http",
                request.Headers["Host"],
                (string.IsNullOrEmpty(relativeUrl)) ? "" : VirtualPathUtility.ToAbsolute(relativeUrl));
        }
    }

    public static class UrlExtensions
    {
        public static string Absolute(this UrlHelper url, string relativeUrl)
        {
            var request = url.RequestContext.HttpContext.Request;

            return string.Format("{0}://{1}{2}",
                (request.IsSecureConnection) ? "https" : "http",
                request.Headers["Host"],
                VirtualPathUtility.ToAbsolute(relativeUrl));
        }
    }

    public static class DateTimeExtensions
    {

    }
}