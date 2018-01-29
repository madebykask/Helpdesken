using DH.Helpdesk.Common.Configuration;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Web.Infrastructure.Configuration.Concrete
{
    public interface IAdfsClaimsSettings
    {
        string ClaimDomain { get; }
        string ClaimUserId { get; }
        string ClaimEmployeeNumber { get; }
        string ClaimFirstName { get; }
        string ClaimLastName { get; }
        string ClaimEmail { get; }
        string ClaimPhone { get; }
    }

    public interface IAdfsConfiguration : IAdfsClaimsSettings
    {
        string DefaultUserId { get; }
        string DefaultEmployeeNumber { get; }
        bool SsoLog { get; }
    }

    public class AdfsConfiguration : IAdfsConfiguration
    {
        #region IAdfsClaimsSettings

        public string ClaimDomain
        {
            get { return AppConfigHelper.GetAppSetting(FederationServiceKeys.ClaimDomain); }
        }

        public string ClaimUserId
        {
            get { return AppConfigHelper.GetAppSetting(FederationServiceKeys.ClaimUserId); }
        }

        public string ClaimEmployeeNumber
        {
            get { return AppConfigHelper.GetAppSetting(FederationServiceKeys.ClaimEmployeeNumber); }
        }

        public string ClaimFirstName
        {
            get { return AppConfigHelper.GetAppSetting(FederationServiceKeys.ClaimFirstName); }
        }

        public string ClaimLastName
        {
            get { return AppConfigHelper.GetAppSetting(FederationServiceKeys.ClaimLastName); }
        }

        public string ClaimEmail
        {
            get { return AppConfigHelper.GetAppSetting(FederationServiceKeys.ClaimEmail); }
        }

        public string ClaimPhone
        {
            get { return AppConfigHelper.GetAppSetting(FederationServiceKeys.ClaimPhone); }
        }

        #endregion

        public string DefaultUserId
        {
            get { return AppConfigHelper.GetAppSetting(AppSettingsKey.DefaultUserId); }
        }

        public string DefaultEmployeeNumber
        {
            get { return AppConfigHelper.GetAppSetting(AppSettingsKey.DefaultEmployeeNumber); }
        }

        public bool SsoLog
        {
            get { return AppConfigHelper.GetBoolean(AppSettingsKey.SSOLog) ?? false; }
        }
    }
}