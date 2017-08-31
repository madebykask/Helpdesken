namespace DH.Helpdesk.Domain.ExtendedCaseEntity
{
    public class Case_ExtendedCaseEntity
    {       
        public int Case_Id { get; set; }

        public int ExtendedCaseData_Id { get; set; }

        public virtual Case CaseEntity { get; set; }

        public virtual ExtendedCaseDataEntity ExtendedCaseData { get; set; }
    }
}
