

using DH.Helpdesk.BusinessData.Models.Shared.Input;
using System;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseSolutionConditionModel: INewBusinessModel
    {
        public CaseSolutionConditionModel()
        {

        }

        public int Id { get; set; }
        public int CaseSolution_Id { get; set; }
        public Guid CaseSolutionConditionGUID { get; set; }
        public string CaseField_Name { get; set; }
        public string Values { get; set; }
        public int Sequence { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ChangedDate { get; set; }
    }
}