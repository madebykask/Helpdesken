﻿namespace DH.Helpdesk.Web.Infrastructure.Tools.Concrete
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;

    using DH.Helpdesk.BusinessData.Enums.Users;

    public sealed class RouteResolver : IRouteResolver
    {
        public string ResolveStartPage(UrlHelper urlHelper, int startPage)
        {
            switch ((StartPage)startPage)
            {
                case StartPage.CaseSummary:
                    return urlHelper.Action("Index", "Cases");
                default:
                    return FormsAuthentication.DefaultUrl;
            }
        }

        public string AbsolutePathToRelative(string path)
        {
            try
            {
                var uri = new UriBuilder(path);
                return uri.Path;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}