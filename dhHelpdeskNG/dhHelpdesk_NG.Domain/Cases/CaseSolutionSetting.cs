namespace DH.Helpdesk.Domain.Cases
{
    using global::System;

    using DH.Helpdesk.Domain.Cases.Settings;

    public class CaseSolutionSetting : Entity
    {
        public int CaseSolution_Id { get; set; }

        public CaseSolutionFields CaseSolutionField { get; set; }

        public int Readonly { get; set; }

        public int Show { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

        public virtual CaseSolution CaseSolution { get; set; }
    }
}
