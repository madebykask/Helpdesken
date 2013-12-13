namespace dhHelpdesk_NG.Data.Repositories.Notifiers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Notifiers;

    public sealed class NotifierFieldSettingLanguageRepository : RepositoryBase<ComputerUserFieldSettingsLanguage>, INotifierFieldSettingLanguageRepository
    {
        public NotifierFieldSettingLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<CaptionDto> FindByLanguageId(int languageId)
        {
            throw new NotImplementedException();
            //var captions = this.DataContext.ComputerUserFieldSettingsLanguages.Where(l => l.Language_Id == languageId);

            //return
            //    captions.Select(
            //        l => new CaptionDto { FieldName = l.ComputerUserFieldSettings.ComputerUserField, Text = l.Label })
            //            .ToList();
        }
    }
}
