using System.Configuration;
using System.IO;
using ECT.Core;
using ECT.Core.FileStore;
using ECT.Core.Service;
using ECT.Model.Abstract;
using ECT.Model.Entities;

namespace ECT.Service
{
    public class FileService : IFileService
    {
        private readonly IContractRepository _contractRepository;

        public FileService(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public void SaveUploads(string[] uploads, int caseId, string uploadPath, string uniqueId)
        {
            if(uploads != null && string.IsNullOrEmpty(uploadPath) && string.IsNullOrEmpty(uniqueId))
                return;

            if(uploads != null && !string.IsNullOrEmpty(uploadPath))
            {
                var contract = _contractRepository.Get(caseId);
                var folder = GetFolderPath(contract.CaseNumber);
                var path = Path.Combine(folder, contract.CaseNumber);

                foreach(var file in uploads)
                {
                    var sourcePath = Path.Combine(uploadPath, uniqueId, file);
                    var destinationPath = Path.Combine(path, file);

                    if(FileStore.Move(sourcePath, destinationPath))
                    {
                        _contractRepository.SaveCaseFile(new CaseFile
                        {
                            CaseId = caseId,
                            FileName = file
                        });
                    }
                }
            }
        }

        public string GetFolderPath(string caseNumber)
        {
            // First we look in tblSettings - PhysicalFilePath
            // if file doesn't exists whe look in tblGlobalSettings - AttachedFileFolder
            var folder = _contractRepository.GetFolderPath(caseNumber);
            return folder;
        }
    }
}
