using HtmlAgilityPack;
using System;
using System.Web;

namespace DH.Helpdesk.SelfService.Infrastructure.Helpers
{
    public static class WebMethodsExtensions
    {
        private const string CustomerIdQueryStringParam = "customerId";
        private const string CustomerIdCookieName = "_customerId";

        public static void SetCustomerIdCookie(this HttpContextBase ctx, int customerId)
        {
            SetSessionCookie(ctx, CustomerIdCookieName, customerId.ToString());
        }

        public static int GetCustomerIdFromQueryString(this HttpContextBase ctx)
        {
            var val = ctx.Request.QueryString[CustomerIdQueryStringParam];
            var customerId = -1;
            int.TryParse(val, out customerId);
            return customerId;
        }

        public static int GetCustomerIdFromCookie(this HttpContextBase ctx)
        {
            var cookie = ctx.Request.Cookies[CustomerIdCookieName];
            var val = cookie != null ? cookie.Value : "";
            var customerId = -1;
            int.TryParse(val, out customerId);
            return customerId;
        }

        public static void SetSessionCookie(this HttpContextBase ctx, string name, string value)
        {
            var cookie = ctx.Request.Cookies[name] ?? new HttpCookie(name, value);
            cookie.Expires = DateTime.MinValue;
            cookie.HttpOnly = true;

            // Set the Secure flag if the request is made over HTTPS
            if (ctx.Request.IsSecureConnection)
            {
                cookie.Secure = true;
            }

            cookie.Value = value;
            ctx.Response.Cookies.Set(cookie);
        }

        public static void SetCookie(this HttpContextBase ctx, string name, string value, TimeSpan? expireTime = null)
        {
            var cookie = ctx.Request.Cookies[name] ?? new HttpCookie(name, value);
            cookie.Expires = expireTime != null ? DateTime.Now.Add(expireTime.Value) : DateTime.Now.AddYears(1);
            cookie.Value = value;
            // Set the Secure flag if the request is made over HTTPS
            if (ctx.Request.IsSecureConnection)
            {
                cookie.Secure = true;
            }
            ctx.Response.Cookies.Set(cookie);
        }
        public static string HTMLToTableCell(this string input)
        {

            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(input);

            HtmlNodeCollection tables = doc.DocumentNode.SelectNodes("//table[@style]");
            if (tables != null)
            {
                foreach (HtmlNode table in tables)
                {
                    table.Attributes["style"].Remove();

                }
            }

            HtmlNodeCollection tableWidths = doc.DocumentNode.SelectNodes("//table[@width]");
            if (tables != null)
            {
                foreach (HtmlNode table in tableWidths)
                {
                    table.Attributes["width"].Value = "300px";
                }
            }

            HtmlNodeCollection trs = doc.DocumentNode.SelectNodes("//tr[@style]");
            if (trs != null)
            {
                foreach (HtmlNode tr in trs)
                {
                    tr.Attributes["style"].Remove();
                }
            }

            HtmlNodeCollection divs = doc.DocumentNode.SelectNodes("//div[@style]");
            if (divs != null)
            {
                foreach (HtmlNode div in divs)
                {
                    string style = div.Attributes["style"].Value;
                    string newStyle = CleanWidth(style);
                    div.Attributes["style"].Value = newStyle;

                }
            }

            HtmlNodeCollection a = doc.DocumentNode.SelectNodes("//a[@style]");
            if (a != null)
            {
                foreach (HtmlNode singlea in a)
                {
                    string style = singlea.Attributes["style"].Value;
                    string newStyle = CleanWidth(style);
                    singlea.Attributes["style"].Value = newStyle;

                }
            }

            HtmlNodeCollection h = doc.DocumentNode.SelectNodes("//h[@style]");
            if (h != null)
            {
                foreach (HtmlNode singleh in h)
                {
                    singleh.Attributes["style"].Remove();

                }
            }
            HtmlNodeCollection a2 = doc.DocumentNode.SelectNodes("//a");
            if (a2 != null)
            {
                foreach (HtmlNode singlea in a2)
                {
                    if (singlea.Attributes["href"] != null)
                    {
                        singlea.Attributes.Add("class", "textblue");
                    }
                }
            }

            HtmlNodeCollection imgs = doc.DocumentNode.SelectNodes("//img[@style]");
            if (imgs != null)
            {
                foreach (HtmlNode img in imgs)
                {
                    string style = img.Attributes["style"].Value;
                    string newStyle = CleanWidth(style);
                    img.Attributes["style"].Remove();

                }
            }

            HtmlNode allNodes = doc.DocumentNode;
            var ret = allNodes.InnerHtml;

            return ret;
        }

        private static string CleanWidth(string oldStyles)
        {
            string newStyles = "";
            foreach (var entries in oldStyles.Split(';'))
            {
                var values = entries.Split(':');
                switch (values[0].ToLower().Trim())
                {

                    case "width":
                        break;

                    default:
                        if (values.Length == 2)
                        {
                            newStyles += values[0] + ":" + values[1] + ";";
                        }

                        break;

                }
            }
            return newStyles;
        }
    }
}