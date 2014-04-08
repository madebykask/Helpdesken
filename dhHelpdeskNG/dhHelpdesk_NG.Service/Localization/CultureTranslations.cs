namespace DH.Helpdesk.Services.Localization
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CultureTranslations
    {
        #region Constructors and Destructors

        public CultureTranslations(string culture, Dictionary<string, string> translations)
        {
            this.Culture = culture;
            this.Translations = translations;
        }

        #endregion

        #region Public Properties

        [NotNullAndEmpty]
        public string Culture { get; private set; }

        [NotNull]
        public Dictionary<string, string> Translations { get; private set; }

        #endregion
    }
}