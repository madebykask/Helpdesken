namespace DH.Helpdesk.Web.Models
{
    using System;

    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Domain;

    public class SurveyViewModel
    {
        public string SurveyTextResult { get; set; }

        public static SurveyViewModel CreateFromSurvey(Survey survey)
        {
            var res = new SurveyViewModel();
            switch (survey.VoteResult)
            {
                case SurveyVoteResult.BAD:
                    res.SurveyTextResult = "Inte nöjd";
                    break;
                case SurveyVoteResult.NORMAL:
                    res.SurveyTextResult = "Nöjd";
                    break;
                case SurveyVoteResult.GOOD:
                    res.SurveyTextResult = "Mycket nöjd";
                    break;
                default:
                    throw new Exception("Survey result is not defined");
            }

            return res;
        }
    }
}
