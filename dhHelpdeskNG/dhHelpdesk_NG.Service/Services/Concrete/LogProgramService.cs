using DH.Helpdesk.BusinessData.Models.LogProgram;
using DH.Helpdesk.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.Services.Services.Concrete
{
    public interface ILogProgramService
    {
        void UpdateUserLogin(LogProgram logProgram);
    }

    public class LogProgramService : ILogProgramService
    {
        private readonly ILogProgramRepository _logProgramRepository;
        
        public LogProgramService(ILogProgramRepository logProgramRepository)
        {
            _logProgramRepository = logProgramRepository;
        }

        public void UpdateUserLogin(LogProgram logProgram)
        {
            _logProgramRepository.UpdateUserLogin(logProgram);
        }

    }
}
