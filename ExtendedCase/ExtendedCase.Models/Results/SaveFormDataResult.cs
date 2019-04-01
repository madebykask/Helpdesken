using System;

namespace ExtendedCase.Models.Results
{
    public class SaveFormDataResult 
    {
        public SaveFormDataResult()
        {
        }

        public SaveFormDataResult(int id, Guid extendedCaseGuid, int extendedCaseFormId)
        {
            Id = id;
            ExtendedCaseGuid = extendedCaseGuid;
            ExtendedCaseFormId = extendedCaseFormId;
        }

        public int Id { get; set; }
        public Guid ExtendedCaseGuid { get; set; }
        public int ExtendedCaseFormId { get; set; }
    }
}