namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data
{
    public sealed class EvaluationFieldGroupDto
    {
        public EvaluationFieldGroupDto(string evaluation, bool evaluationReady)
        {
            Evaluation = evaluation;
            EvaluationReady = evaluationReady;
        }

        public string Evaluation { get; private set; }

        public bool EvaluationReady { get; private set; }
    }
}
