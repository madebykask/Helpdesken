using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedCase.Dal.Repositories;

namespace ExtendedCase.Logic.Services
{
    public interface IGlobalSettingsService
    {
        IList<string> GetFileUploadWhiteList();
        bool IsExtensionInWhitelist(string extension);
    }

    public class GlobalSettingsService: IGlobalSettingsService
    {
        private readonly IGlobalSettingRepository _globalSettingRepository;

        public GlobalSettingsService(IGlobalSettingRepository globalSettingRepository)
        {
            _globalSettingRepository = globalSettingRepository;
        }

        public IList<string> GetFileUploadWhiteList()
        {
            var whiteList = (List<string>)null;
            var fileUploadExtensionWhitelist = _globalSettingRepository.GetFileUploadExtensionWhitelist();
            if (fileUploadExtensionWhitelist != null)
            {
                whiteList = fileUploadExtensionWhitelist.Split(';').Select(o => o.ToLower()).ToList();
            }
            return whiteList;
        }

        public bool IsExtensionInWhitelist(string extension)
        {
            var whiteList = this.GetFileUploadWhiteList();

            if (whiteList == null) return true;

            extension = extension.Replace(".", "").ToLower();
            return whiteList.Contains(extension);
        }
    }
}
