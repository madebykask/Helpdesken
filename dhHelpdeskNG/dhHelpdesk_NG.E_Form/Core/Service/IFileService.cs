using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.EForm.Core.Service
{
    public interface IFileService
    {
        void SaveUploads(string[] uploads, int caseId, string uploadPath, string uniqueId);
        string GetFolderPath(string caseNumber);
    }
}