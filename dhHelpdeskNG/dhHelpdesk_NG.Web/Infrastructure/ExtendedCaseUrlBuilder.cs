namespace DH.Helpdesk.Web.Infrastructure
{
    #region ExtededCaseUrlParams

    public class ExtededCaseUrlParams
    {
        public int formId { get; set; }
        public int caseStatus { get; set; }
        public int userRole { get; set; }
        public string userGuid { get; set; }
        public int customerId { get; set; }
        public bool autoLoad { get; set; }
    }

    #endregion

    public static class ExtendedCaseUrlBuilder
    {
        public static string BuildInitiatorExtendedCaseFormUrl(string urlMask, int formId)
        {
            var urlBld = 
                UrlBuilder.Create(urlMask)
                    .ClearParams()
                    .SetParam(Constants.ExtendedCaseUrlKeys.FormId, formId.ToString())
                    .SetParam(Constants.ExtendedCaseUrlKeys.AutoLoad, "true");
            
            return urlBld.BuildUrl();
        }

        public static string BuildExtendedCaseUrl(string urlMask, ExtededCaseUrlParams @params)
        {
            var urlBld =
                UrlBuilder.Create(urlMask)
                    .RemoveParam(Constants.ExtendedCaseUrlKeys.LanguageId)       //majid: sent in by js function
                    .RemoveParam(Constants.ExtendedCaseUrlKeys.ExtendedCaseGuid) //majid: sent in by js function
                    .SetParam(Constants.ExtendedCaseUrlKeys.CaseStatus, @params.caseStatus.ToString())
                    .SetParam(Constants.ExtendedCaseUrlKeys.UserRole, @params.userRole.ToString()) //majid: NOTE, this is from now on userWorkingGroupId. 
                    .SetParam(Constants.ExtendedCaseUrlKeys.UserGuid, @params.userGuid)
                    .SetParam(Constants.ExtendedCaseUrlKeys.CustomerId, @params.customerId.ToString());

            if (@params.formId > 0)
            {
                urlBld.SetParam(Constants.ExtendedCaseUrlKeys.FormId, @params.formId.ToString());
            }

            if (@params.autoLoad)
            {
                urlBld.SetParam(Constants.ExtendedCaseUrlKeys.AutoLoad, "true");
            }

            var extendedCasePath = urlBld.BuildUrl();
            return extendedCasePath;
        }
    }
}