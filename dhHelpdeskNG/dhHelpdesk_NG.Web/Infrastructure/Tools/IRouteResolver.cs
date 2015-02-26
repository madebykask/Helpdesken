namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System.Web.Mvc;

    public interface IRouteResolver
    {
        string ResolveStartPage(UrlHelper urlHelper, int startPage);

        string AbsolutePathToRelative(string path);
    }
}