using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface ICaseFileService
    {
        IList<CaseFile> GetCaseFiles(int caseid);
        byte[] GetFileContentByIdAndFileName(int id, string fileName);
        List<string> FindFileNamesByCaseId(int id);
    }

    public class CaseFileService : ICaseFileService
    {
        private readonly ICaseFileRepository _caseFileRepository;
        
        public CaseFileService(
            ICaseFileRepository caseFileRepository)
        {
            _caseFileRepository = caseFileRepository;            
        }

        public IList<CaseFile> GetCaseFiles(int caseId)
        {
            return _caseFileRepository.GetCaseFiles(caseId).ToList();
        }

        public byte[] GetFileContentByIdAndFileName(int id, string fileName)
        {
            //Todo anrop till repository
            return null;
        }

        public List<string> FindFileNamesByCaseId(int id)
        {
            //Todo anrop till repository
            return null;
        }

    }
}
