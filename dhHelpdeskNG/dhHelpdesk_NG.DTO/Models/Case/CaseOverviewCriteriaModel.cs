namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseOverviewCriteriaModel
    {
        public CaseOverviewCriteriaModel()
        {
            
        }

        public bool MyCasesRegistrator { get; set; }
        public bool MyCasesInitiator { get; set; }
        public bool MyCasesFollower { get; set; }
        public bool MyCasesRegarding { get; set; }
        public bool MyCasesUserGroup { get; set; }

    }
}
