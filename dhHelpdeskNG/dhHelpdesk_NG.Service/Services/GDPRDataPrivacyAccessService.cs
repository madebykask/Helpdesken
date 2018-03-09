using DH.Helpdesk.Dal.Repositories.GDPR;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Services.Services
{
    public interface IGDPRDataPrivacyAccessService
    {
        GDPRDataPrivacyAccess GetByUserId(int userId);
    }

    public class GDPRDataPrivacyAccessService : IGDPRDataPrivacyAccessService
    {
        private readonly IGDPRDataPrivacyAccessRepository _privacyAccessRepository;

        #region ctor()

        public GDPRDataPrivacyAccessService(IGDPRDataPrivacyAccessRepository privacyAccessRepository)
        {
            _privacyAccessRepository = privacyAccessRepository;
        }

        #endregion

        public GDPRDataPrivacyAccess GetByUserId(int userId)
        {
            return _privacyAccessRepository.GetByUserId(userId);
        }
    }
}