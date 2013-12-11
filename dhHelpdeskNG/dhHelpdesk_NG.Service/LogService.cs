using System;
using System.Collections.Generic;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Service
{
    public interface ILogService
    {
        IDictionary<string, string> Validate(Log logToValidate);
    }

    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(
            ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public IDictionary<string, string> Validate(Log logToValidate)
        {
            if (logToValidate == null)
                throw new ArgumentNullException("logtovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }
    }
}
