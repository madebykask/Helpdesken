namespace DH.Helpdesk.Services.Localization
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Services.Infrastructure;

    using LinqLib.Sequence;

    public static class Translator
    {
        #region Static Fields

        private static readonly List<CultureTranslations> CultureTranslations;

        #endregion

        #region Constructors and Destructors

        static Translator()
        {
            var textTranslationRepository = ManualDependencyResolver.Get<ITextTranslationRepository>();
            var translations = textTranslationRepository.FindTranslations();
            var cultures = translations.Select(t => t.Culture).Distinct().ToList();

            CultureTranslations = new List<CultureTranslations>(cultures.Count);

            foreach (var culture in cultures)
            {
                var cultureTranslations =
                    translations.Where(t => t.Culture == culture).ToList()
                        .Distinct(t => t.TextId)
                        .ToDictionary(t => t.TextId, t => t.Text);

                CultureTranslations.Add(new CultureTranslations(culture, cultureTranslations));
            }
        }

        #endregion

        #region Public Methods and Operators

        public static string Translate(string source)
        {
            var currentUICulture = Thread.CurrentThread.CurrentUICulture.Name;

            if (CultureTranslations.All(t => t.Culture != currentUICulture))
            {
                currentUICulture = ApplicationDefaultParameters.Culture;
            }

            var cultureTranslations = CultureTranslations.SingleOrDefault(t => t.Culture == currentUICulture);

            string translation;
            cultureTranslations.Translations.TryGetValue(source, out translation);

            return translation ?? source;
        }

        #endregion
    }
}