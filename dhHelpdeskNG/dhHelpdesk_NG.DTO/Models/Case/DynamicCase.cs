using System;
using System.IO;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class DynamicCase 
    {   
        public int CaseId { get; set; }
        public string FormPath { get; set; }
        public string FormName { get; set; }
        public int ViewMode { get; set; }
        public bool ExternalPage { get; set; }
        public bool ExternalSite { get; set; }
        public string Scheme { get; set; }
        public string Host { get; set; }

        #region BuildUrl

        public string BuildUrl()
        {
            if (string.IsNullOrEmpty(FormPath))
                return string.Empty;
            
            var urlPath = FormPath;
            var queryString = string.Empty;

            var qsIndex = urlPath.IndexOf("?", StringComparison.Ordinal);
            if (qsIndex != -1)
            {
                queryString = urlPath.Substring(qsIndex + 1).ToLower().Trim();
                urlPath = urlPath.Substring(0, qsIndex).ToLower().Trim();
            }
            
            var uriBuilder = new UriBuilder
            {
                Host = string.IsNullOrEmpty(Host) ? "localhost" : Host,
                Scheme = string.IsNullOrEmpty(Scheme) ? "http" : Scheme, 
                Path = urlPath,
                Query = queryString
            };

            var url = ExternalSite ?  uriBuilder.Uri.ToString() : uriBuilder.Uri.PathAndQuery;
            return url;
        }

        #endregion
    }
}
