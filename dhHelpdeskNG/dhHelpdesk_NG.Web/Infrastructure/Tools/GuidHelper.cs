namespace dhHelpdesk_NG.Web.Infrastructure.Tools
{
    using System;

    public static class GuidHelper
    {
        public static bool IsGuid(string guid)
        {
            Guid result;
            return Guid.TryParse(guid, out result);
        }
    }
}