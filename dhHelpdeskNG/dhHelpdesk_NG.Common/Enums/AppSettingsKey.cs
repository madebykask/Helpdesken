﻿namespace DH.Helpdesk.Common.Enums
{
    public static class AppSettingsKey
    {
        public static readonly string HelpdeskPath = "dh_helpdeskaddress";
        public static readonly string FilesDirectory = "dh_filesDirectory";
        public static readonly string SmtpServer = "SmtpServer";
        public static readonly string SmtpPort = "SmtpPort";
        public static readonly string LoginMode = "LoginMode";
        public static readonly string CurrentApplicationType = "CurrentApplication";
        public static readonly string CaseList = "CaseList";
        public static readonly string DefaultUserId = "DefaultUserId";
        public static readonly string DefaultEmployeeNumber = "DefaultEmployeeNumber";
        public static readonly string ConfirmMsgAfterCaseRegistration = "ConfirmMsgAfterCaseRegistration";
        public static readonly string ShowCommunicationForSelfService = "ShowCommunicationForSelfService";

        public static readonly string ReCaptchaSiteKey = "reCaptchaSiteKey";
        public static readonly string ReCaptchaSecretKey = "reCaptchaSecretKey";


        public static readonly string SSOLog = "SSOLog";
        public static readonly string ApplicationId = "ApplicationId";
        public static readonly string SelfServiceAddress = "dh_selfserviceaddress";
        public static readonly string EncryptionKey = "encryptionkey";
        public static readonly string AmApiUri = "AM_Api_UriPath";
        public static readonly string AmApiUserName = "AM_Api_UserName";
        public static readonly string AmApiPassword = "AM_Api_Password";
        public static readonly string DefaultCulture = "DefaultCulture";

        public const string TokenLifeTime = "SSO.TokenLifeTime";
        public const string TokenMaxLifeTime = "SSO.TokenMaxLifeTime";
        public const string EnableSlidingExpiration = "SSO.EnableSlidingExpiration";
        public const string HandleSecurityTokenExceptions = "SSO.HandleSecurityTokenExceptions";
        public const string LogoutCustomerOnSessionExpire = "SSO.LogoutCustomerOnSessionExpire";
    }
}