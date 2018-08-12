namespace DH.Helpdesk.Domain.Cases
{
    public class CaseSectionField : Entity
    {
        public int CaseSection_Id { get; set; }

        public int CaseFieldSetting_Id { get; set; }

        public virtual CaseSection CaseSection { get; set; }

        public virtual CaseFieldSetting CaseFieldSetting { get; set; }
    }
}
