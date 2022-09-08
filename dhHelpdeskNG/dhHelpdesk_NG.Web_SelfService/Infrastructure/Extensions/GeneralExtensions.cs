namespace DH.Helpdesk.SelfService.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Domain;
    using HtmlAgilityPack;

    public static class GeneralExtensions
    {
        public static bool UserHasRole(UserOverview user, string roleToHave)
        {
            var bolRet = false;

            if (user != null)
                if (user.UserGroupId.ToString() == roleToHave)
                {
                    bolRet = true;
                }

            return bolRet;
        }

        public static string ReturnSelectedValueCaseSolution(int id, IEnumerable<CaseSolutionSchedule> css)
        {
            string strRet = "";

            if (css != null)
            {
                foreach (CaseSolutionSchedule s in css)
                {
                    if (s.CaseSolution_Id == id)
                    {
                        strRet = "checked";
                        break;
                    }
                }
            }

            return strRet;
        }

        public static string CaseSolution_SetSelectedScheduleDay_Day(this HtmlHelper html, string compare, CaseSolutionSchedule css)
        {
            if (css != null)
            {
                foreach (var s in css.ScheduleDay.Split(','))
                {
                    if (s == compare)
                        return " checked=\"checked\"";
                }
            }

            return "";
        }

        public static string returnMaskedPwdForCustomerSettings(int id, string ldapPassword)
        {
            if (id == 0 && ldapPassword == null || ldapPassword == "")
                return string.Empty;
            else
                return "|||||||||||";
        }

        public static string HTMLToTableCell(this string input)
        {

            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(input);


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