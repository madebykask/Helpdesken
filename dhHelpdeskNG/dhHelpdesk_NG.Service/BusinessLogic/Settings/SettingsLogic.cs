using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.Dal.Repositories;

namespace DH.Helpdesk.Services.BusinessLogic.Settings
{
    public interface ISettingsLogic
    {
        string GetFilePath(int customerId);
        string GetVirtualDirectoryPath(int customerId);
        string GetFilePath(CustomerSettings customerSettings);
    }

    public class SettingsLogic: ISettingsLogic
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IGlobalSettingRepository _globalSettingRepository;

        public SettingsLogic(ISettingRepository settingRepository, IGlobalSettingRepository globalSettingRepository)
        {
            _settingRepository = settingRepository;
            _globalSettingRepository = globalSettingRepository;
        }

        public string GetFilePath(int customerId)
        {
            var customerFilePath = this._settingRepository.GetMany(s => s.Customer_Id == customerId)
                .AsQueryable()
                .Select(s => s.PhysicalFilePath)
                .FirstOrDefault();
            return FilePath(customerFilePath);
        }

        public string GetFilePath(CustomerSettings customerSettings)
        {
            var customerFilePath = customerSettings.PhysicalFilePath;
            return FilePath(customerFilePath);
        }

        public string GetVirtualDirectoryPath(int customerId)
        {
            var virtualDirectoryPath = 
                _settingRepository.Get(s => s.Customer_Id == customerId, s => s.VirtualFilePath);

            if (string.IsNullOrEmpty(virtualDirectoryPath))
            {
                virtualDirectoryPath = this._globalSettingRepository.Get(s => true, s => s.VirtualFileFolder);
            }

            return virtualDirectoryPath ?? string.Empty;
        }

        #region private

        private string FilePath(string customerFilePath)
        {
            if (string.IsNullOrEmpty(customerFilePath))
            {
                var globalSetting = this._globalSettingRepository.GetAll()
                    .AsQueryable()
                    .FirstOrDefault();
                if (globalSetting != null)
                    customerFilePath = globalSetting.AttachedFileFolder;
            }

            return (string.IsNullOrEmpty(customerFilePath) ? string.Empty : customerFilePath);
        }

        #endregion
    }
}
