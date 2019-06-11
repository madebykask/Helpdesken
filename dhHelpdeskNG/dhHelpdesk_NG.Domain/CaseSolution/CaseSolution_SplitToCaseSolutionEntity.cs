namespace DH.Helpdesk.Domain
{
    public class CaseSolution_SplitToCaseSolutionEntity
    {
        public int CaseSolution_Id { get; set; }
        public int SplitToCaseSolution_Id { get; set; }

        public virtual CaseSolution CaseSolution { get; set; }
        public virtual CaseSolution SplitToCaseSolutionDescendant { get; set; }
    }
}
