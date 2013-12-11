using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface ICaseFileService
    {
        //int GetNoCaseFiles(int caseid);
        IEnumerable<CaseFile> GetCaseFiles(int caseid);        
    }

    public class CaseFileService : ICaseFileService
    {
        private readonly ICaseFileRepository _caseFileRepository;
        
        public CaseFileService(
            ICaseFileRepository caseFileRepository)
        {
            _caseFileRepository = caseFileRepository;            
        }

        //public int GetNoCaseFiles(int caseId)
        //{
        //    return _caseFileRepository.GetNoCaseFiles(caseId);
        //}

        public IEnumerable<CaseFile> GetCaseFiles(int caseId)
        {
            return _caseFileRepository.GetCaseFiles(caseId).ToList();
        }
    }
}
