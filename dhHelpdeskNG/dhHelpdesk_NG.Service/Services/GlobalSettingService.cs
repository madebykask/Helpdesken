namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

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

        #region ctor()

        public GlobalSettingService(IGlobalSettingRepository globalSettingRepository, IUnitOfWork unitOfWork)
        {
            this._globalSettingRepository = globalSettingRepository;
            this._unitOfWork = unitOfWork;
        }

        #endregion
        
        public IList<GlobalSetting> GetGlobalSettings()
        {
            return this._globalSettingRepository.GetAll().ToList();
        }

        public void SaveGlobalSetting(GlobalSetting globalSetting, out IDictionary<string, string> errors)
        {
            if (globalSetting == null)
                throw new ArgumentNullException("globalsetting");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(globalSetting.ApplicationName))
                errors.Add("GlobalSetting.ApplicationName", "Du måste ange ett namn");

            if (globalSetting.Id == 0)
                this._globalSettingRepository.Add(globalSetting);
            else
                this._globalSettingRepository.Update(globalSetting);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
