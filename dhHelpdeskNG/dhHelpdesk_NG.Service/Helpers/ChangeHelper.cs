namespace DH.Helpdesk.Services.Helpers
{
    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Services.Localization;

    public static class ChangeHelper
    {
        public static string StatusToString(this StepStatus status)
        {            
            switch (status)
            {
                case StepStatus.Approved:
                    return Translator.Translate("Approve");
                case StepStatus.Rejected:
                    return Translator.Translate("Reject");
            }

            return string.Empty;
        }
    }
}