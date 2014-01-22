using System;
using System.Linq;
using System.Collections.Generic;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs.Case;

namespace dhHelpdesk_NG.Service
{
    public interface ILogFileService
    {
        byte[] GetFileContentByIdAndFileName(int logId, string fileName);
    }

    public class LogFileService : ILogFileService
    {
        private readonly ILogRepository _logRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LogFileService(
            ILogRepository logRepository
            , IUnitOfWork unitOfWork)
        {
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
        }

        public byte[] GetFileContentByIdAndFileName(int logId, string fileName)
        {
            return null;
        }

    }
}
