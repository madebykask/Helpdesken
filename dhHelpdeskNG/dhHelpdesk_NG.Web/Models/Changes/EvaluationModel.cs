namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class EvaluationModel
    {
        [LocalizedDisplay("Change evaluation")]
        public string ChangeEvaluation { get; set; }

        [LocalizedDisplay("Attached files")]
        public List<string> AttachedFiles { get; set; }

        [LocalizedDisplay("Evaluation ready")]
        public bool EvaluationReady { get; set; }
    }
}