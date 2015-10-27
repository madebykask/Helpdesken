namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Common.Enums;

    public class Survey : Entity
    {
        public int CaseId { get; set; }

        public SurveyVoteResult VoteResult { get; set; }

        public static Survey CreateFromString(int caseId, string voteId = "good")
        {
            if (string.IsNullOrEmpty(voteId))
            {
                voteId = "good";
            }

            var voteStr = voteId.ToLower();
            switch (voteStr)
            {
                case "good":
                    return new Survey() { CaseId = caseId, VoteResult = SurveyVoteResult.GOOD };
                case "bad":
                    return new Survey() { CaseId = caseId, VoteResult = SurveyVoteResult.BAD };
            }

            return new Survey() { CaseId = caseId, VoteResult = SurveyVoteResult.NORMAL };
        }
    }
}
