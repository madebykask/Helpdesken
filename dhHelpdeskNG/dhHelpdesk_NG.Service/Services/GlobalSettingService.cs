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

		List<string> GetFileUploadWhiteList();

		void SetFileUploadWhiteList(List<string> fileExtensions);
		bool IsExtensionInWhitelist(string extension);
	}

    public class GlobalSettingService : IGlobalSettingService
    {
        private readonly IGlobalSettingRepository _globalSettingRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

        #region ctor()

#pragma warning disable 0618
        public GlobalSettingService(IGlobalSettingRepository globalSettingRepository, IUnitOfWork unitOfWork)
        {
            this._globalSettingRepository = globalSettingRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

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

		public List<string> GetFileUploadWhiteList()
		{
			var whiteList = (List<string>)null;
			var settings = _globalSettingRepository.Get();
			if (settings.FileUploadExtensionWhitelist != null)
			{
				whiteList = settings.FileUploadExtensionWhitelist.Split(';').Select(o => o.ToLower()).ToList();
			}
			return whiteList;
		}

		public void SetFileUploadWhiteList(List<string> fileExtensions)
		{
			var settings = _globalSettingRepository.Get();
			if (fileExtensions != null)
			{
				settings.FileUploadExtensionWhitelist = fileExtensions.Aggregate((o, p) => o + ";" + p);
			}
			else
			{
				// null is disabled
				settings.FileUploadExtensionWhitelist = null;
			}
			Commit();
		}

		public void Commit()
        {
            this._unitOfWork.Commit();
        }

		public bool IsExtensionInWhitelist(string extension)
		{
			var whiteList = this.GetFileUploadWhiteList();

			if (whiteList != null)
			{
				extension = extension.Replace(".", "").ToLower();
				if (!whiteList.Contains(extension))
				{
					return false;
				}
			}

			return true;
		}
	}
}
