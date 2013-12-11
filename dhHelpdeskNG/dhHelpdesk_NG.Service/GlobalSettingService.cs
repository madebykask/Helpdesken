using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IGlobalSettingService
    {
        IList<GlobalSetting> GetGlobalSettings();

        void SaveGlobalSetting(GlobalSetting globalSetting, out IDictionary<string, string> errors);

        void Commit();
    }

    public class GlobalSettingService : IGlobalSettingService
    {
        private readonly IGlobalSettingRepository _globalSettingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GlobalSettingService(
            IGlobalSettingRepository globalSettingRepository,
            IUnitOfWork unitOfWork)
        {
            _globalSettingRepository = globalSettingRepository;
            _unitOfWork = unitOfWork;
        }
        
        public IList<GlobalSetting> GetGlobalSettings()
        {
            return _globalSettingRepository.GetAll().ToList();
        }

        public void SaveGlobalSetting(GlobalSetting globalSetting, out IDictionary<string, string> errors)
        {
            if (globalSetting == null)
                throw new ArgumentNullException("globalsetting");

            errors = new Dictionary<string, string>();

            if (globalSetting.Id == 0)
                _globalSettingRepository.Add(globalSetting);
            else
                _globalSettingRepository.Update(globalSetting);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
