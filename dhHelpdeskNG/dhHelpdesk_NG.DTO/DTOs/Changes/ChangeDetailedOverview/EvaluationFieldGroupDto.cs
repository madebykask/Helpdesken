namespace dhHelpdesk_NG.DTO.DTOs.Changes.ChangeDetailedOverview
{
    public sealed class EvaluationFieldGroupDto
    {
        public EvaluationFieldGroupDto(string evaluation, bool evaluationReady)
        {
            this.Evaluation = evaluation;
            this.EvaluationReady = evaluationReady;
        }

        public string Evaluation { get; private set; }

        public bool EvaluationReady { get; private set; }
    }
}
