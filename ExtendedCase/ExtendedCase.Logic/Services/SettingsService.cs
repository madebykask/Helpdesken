using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedCase.Dal.Repositories;

namespace ExtendedCase.Logic.Services
{
    public interface ISettingsService
    {
        string GetFilePath(int customerId);
    }

    public class SettingsService: ISettingsService
    {

        private readonly ISettingsRepository _settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }


        public string GetFilePath(int customerId)
        {
            return _settingsRepository.GetFilePath(customerId);
        }
    }
}
