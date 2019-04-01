namespace ExtendedCase.HelpdeskApiClient
{
    public static class HelpdeskApiResources
    {
        public static CaseApiUrlsBuilder CaseApi = new CaseApiUrlsBuilder();

        #region CaseApiUrlsBuilder

        public class CaseApiUrlsBuilder
        {
            public static string ApiBaseUri = "CaseApi";

            public string BuildGetCaseUri(int caseId)
            {
                return string.Format($"{ApiBaseUri}/GetCase?id={caseId}");
            }
        }

        #endregion
    }
}