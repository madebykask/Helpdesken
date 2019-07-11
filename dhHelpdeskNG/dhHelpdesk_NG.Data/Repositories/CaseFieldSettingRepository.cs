using System.Data.Entity;
using Z.EntityFramework.Plus;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using System;

    # region CASEFIELDSETTINGLANGUAGE

    public interface ICaseFieldSettingLanguageRepository : IRepository<CaseFieldSettingLanguage>
    {
        void DeleteByLanguageId(int languageId, int customerId);
        IEnumerable<CaseFieldSettingsWithLanguage> GetCaseFieldSettingsWithLanguages(int? customerId, int? languageId);
        IEnumerable<CaseFieldSettingsWithLanguage> GetAllCaseFieldSettingsWithLanguages(int? customerId, int? languageId);
        IEnumerable<CaseFieldSettingsForTranslation> GetCaseFieldSettingsForTranslation(int userId);
        IEnumerable<CaseFieldSettingsForTranslation> GetCustomerCaseFieldSettingsForTranslation(int customerId);
        IEnumerable<CaseFieldSettingsForTranslation> GetCaseFieldSettingsForTranslation();
        IEnumerable<CaseFieldSettingsWithLanguage> GetCaseFieldSettingsWithLanguagesForDefaultCust(int languageId);
    }

    public class CaseFieldSettingLanguageRepository : RepositoryBase<CaseFieldSettingLanguage>, ICaseFieldSettingLanguageRepository
    {
        public CaseFieldSettingLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void DeleteByLanguageId(int languageId, int customerId)
        {
            var  del = (from cfsl in this.DataContext.CaseFieldSettingLanguages
                       join cfs in this.DataContext.CaseFieldSettings on cfsl.CaseFieldSettings_Id equals cfs.Id
                       where cfs.Customer_Id == customerId && cfsl.Language_Id == languageId 
                       select cfsl).ToList();

            del.ForEach(d => this.DataContext.CaseFieldSettingLanguages.Remove(d));
        }

        /// <summary>
        /// Note: Only returns where ShowOnStartPage == 1
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public IEnumerable<CaseFieldSettingsWithLanguage> GetCaseFieldSettingsWithLanguages(int? customerId, int? languageId)
        {
            var query = from cfsl in this.DataContext.CaseFieldSettingLanguages
                            join cfs in this.DataContext.CaseFieldSettings on cfsl.CaseFieldSettings_Id equals cfs.Id
                            //join cs in this.DataContext.CaseSettings on cfs.Name equals cs.Name
                        where cfs.Customer_Id == customerId && cfsl.Language_Id == languageId && cfs.ShowOnStartPage == 1
                        group cfsl by new { cfsl.CaseFieldSettings_Id, cfsl.Label, cfsl.Language_Id, cfsl.FieldHelp, cfs.Name, cfs.EMailIdentifier } into grouped
                        select new CaseFieldSettingsWithLanguage
                        {
                            Id = grouped.Key.CaseFieldSettings_Id,
                            Label = grouped.Key.Label,
                            Language_Id = grouped.Key.Language_Id,
                            FieldHelp = grouped.Key.FieldHelp,
                            Name = grouped.Key.Name,
                            EMailIdentifier = grouped.Key.EMailIdentifier
                        };

            return query.OrderBy(x => x.Id);
        }

        public IEnumerable<CaseFieldSettingsWithLanguage> GetAllCaseFieldSettingsWithLanguages(int? customerId, int? languageId)
        {
            var query = from cfsl in this.DataContext.CaseFieldSettingLanguages
                        join cfs in this.DataContext.CaseFieldSettings on cfsl.CaseFieldSettings_Id equals cfs.Id
                        //join cs in this.DataContext.CaseSettings on cfs.Name equals cs.Name
                        where cfs.Customer_Id == customerId && cfsl.Language_Id == languageId
                        group cfsl by new { cfsl.CaseFieldSettings_Id, cfsl.Label, cfsl.Language_Id, cfsl.FieldHelp, cfs.Name, cfs.EMailIdentifier } into grouped
                        select new CaseFieldSettingsWithLanguage
                        {
                            Id = grouped.Key.CaseFieldSettings_Id,
                            Label = grouped.Key.Label,
                            Language_Id = grouped.Key.Language_Id,
                            FieldHelp = grouped.Key.FieldHelp,
                            Name = grouped.Key.Name,
                            EMailIdentifier = grouped.Key.EMailIdentifier
                        };

            return query.OrderBy(x => x.Id);
        }

        public IEnumerable<CaseFieldSettingsWithLanguage> GetCaseFieldSettingsWithLanguagesForDefaultCust(int languageId)
        {            
            var query = from cfsl in this.DataContext.CaseFieldSettingLanguages
                            join cfs in this.DataContext.CaseFieldSettings on cfsl.CaseFieldSettings_Id equals cfs.Id
                        where (cfsl.Language_Id == languageId) && (cfs.ShowOnStartPage == 1)
                        group cfsl by new {cfs.Customer_Id, cfsl.CaseFieldSettings_Id, cfsl.Label, cfsl.Language_Id, cfsl.FieldHelp, cfs.Name } into grouped
                        select new CustomCaseFieldSettingsWithLanguage
                        {
                            CustomerId = grouped.Key.Customer_Id == null ? null : grouped.Key.Customer_Id,
                            Id = grouped.Key.CaseFieldSettings_Id,
                            Label = grouped.Key.Label,
                            Language_Id = grouped.Key.Language_Id,
                            FieldHelp = grouped.Key.FieldHelp,
                            Name = grouped.Key.Name
                        };

            var res1 = query.Select(q => new { q.Id, q.Label, q.Language_Id, q.FieldHelp, q.Name, q.CustomerId }).ToList();
            var res2 = res1.Where(q => (int?)q.CustomerId == null)
                           .Select(q => new CaseFieldSettingsWithLanguage 
                              { Id = q.Id, 
                                Label = q.Label, 
                                Language_Id = q.Language_Id, 
                                FieldHelp = q.FieldHelp, 
                                Name = q.Name
                              })
                           .OrderBy(x => x.Id)
                           .ToList();
            return res2;
        }


        public IEnumerable<CaseFieldSettingsForTranslation> GetCaseFieldSettingsForTranslation(int userId)
        {
            var query = from s in this.DataContext.CaseFieldSettings
                            join sl in this.DataContext.CaseFieldSettingLanguages on s.Id equals sl.CaseFieldSettings_Id
                            join cu in this.DataContext.CustomerUsers on s.Customer_Id equals cu.Customer_Id
                        where (cu.User_Id == userId)  
                            //&& (sl.Language_Id == languageId)
                            //&& (cu.ShowOnStartPage == 1)
                        group s by new { s.Customer_Id, s.Name, sl.Label, sl.Language_Id } into grouped
                        select new CaseFieldSettingsForTranslation
                        {
                            Customer_Id = grouped.Key.Customer_Id.HasValue ? grouped.Key.Customer_Id.Value : 0,
                            Language_Id = grouped.Key.Language_Id,   
                            Label = grouped.Key.Label,
                            Name = grouped.Key.Name
                        };

            return query;
        }

        public IEnumerable<CaseFieldSettingsForTranslation> GetCustomerCaseFieldSettingsForTranslation(int customerId)
        {
            var query = from s in this.DataContext.CaseFieldSettings
                        join sl in this.DataContext.CaseFieldSettingLanguages on s.Id equals sl.CaseFieldSettings_Id
                        join cu in this.DataContext.CustomerUsers on s.Customer_Id equals cu.Customer_Id
                        where (cu.Customer_Id == customerId)
                        group s by new { s.Customer_Id, s.Name, sl.Label, sl.Language_Id } into grouped
                        select new CaseFieldSettingsForTranslation
                        {
                            Customer_Id = grouped.Key.Customer_Id.HasValue ? grouped.Key.Customer_Id.Value : 0,
                            Language_Id = grouped.Key.Language_Id,
                            Label = grouped.Key.Label,
                            Name = grouped.Key.Name
                        };

            return query;
        }

        public IEnumerable<CaseFieldSettingsForTranslation> GetCaseFieldSettingsForTranslation()
        {
            var query = from s in this.DataContext.CaseFieldSettings
                        join sl in this.DataContext.CaseFieldSettingLanguages on s.Id equals sl.CaseFieldSettings_Id
                        join cu in this.DataContext.CustomerUsers on s.Customer_Id equals cu.Customer_Id
                        //where (cu.User_Id == userId)
                        //&& (sl.Language_Id == languageId)
                        //&& (cu.ShowOnStartPage == 1)
                        group s by new { s.Customer_Id, s.Name, sl.Label, sl.Language_Id } into grouped
                        select new CaseFieldSettingsForTranslation
                        {
                            Customer_Id = grouped.Key.Customer_Id.HasValue ? grouped.Key.Customer_Id.Value : 0,
                            Language_Id = grouped.Key.Language_Id,
                            Label = grouped.Key.Label,
                            Name = grouped.Key.Name
                        };

            return query;
        }
    }

    #endregion

    #region CASEFIELDSETTING

    public interface ICaseFieldSettingRepository : IRepository<CaseFieldSetting>
    {
        IEnumerable<CaseListToCase> GetListToCustomerCase(int customerId, int languageId);
        IEnumerable<ListCases> GetListCasesToCaseSummary(int? customerId, int? languageId, int? userGroupId);
        IEnumerable<ListCases> GetCaseFieldSettingsListToCustomerCaseSummary(int? customerId, int? languageId, int? userGroupId);

        IEnumerable<CaseFieldSetting> GetCaseFieldSettingsForDefaultCust();
        IEnumerable<CaseFieldSetting> GetCustomerCaseFieldSettings(int customerId, int? languageId = null);

    }

    public class CaseFieldSettingRepository : RepositoryBase<CaseFieldSetting>, ICaseFieldSettingRepository
    {
        public CaseFieldSettingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        // CasefielName in db shall be removed
        public IEnumerable<CaseListToCase> GetListToCustomerCase(int customerId, int languageId)
        {
            var query = from cfs in this.DataContext.CaseFieldSettings
                        join cfsl in this.DataContext.CaseFieldSettingLanguages on cfs.Id equals cfsl.CaseFieldSettings_Id
                        where cfs.Customer_Id == customerId && cfsl.Language_Id == languageId
                        group cfs by new
                        {
                            cfs.Id,
                            customerId,
                            cfs.FieldSize,
                            cfsl.Label,
                            //cfs.NameOrigin,
                            languageId,
                            cfs.Required,
                            cfs.ShowExternal,
                            cfs.ShowOnStartPage,
                            cfs.Name,
                            cfs.EMailIdentifier
                        }
                            into grouped
                            select new CaseListToCase
                            {
                                CFS_Id = grouped.Key.Id,
                                Customer_Id = grouped.Key.customerId,
                                FieldSize = grouped.Key.FieldSize,
                                //LabelNotToChange = grouped.Key.NameOrigin,
                                LabelToChange = grouped.Key.Label,
                                Language_Id = grouped.Key.languageId,
                                Required = grouped.Key.Required,
                                ShowExternal = grouped.Key.ShowExternal,
                                ShowOnStartPage = grouped.Key.ShowOnStartPage,
                                Name = grouped.Key.Name,
                                EMailIdentifier = grouped.Key.EMailIdentifier
                            };

            if (query.Count() == 0)
            {
                var q = from cfs in this.DataContext.CaseFieldSettings
                        join cfsl in this.DataContext.CaseFieldSettingLanguages on cfs.Id equals cfsl.CaseFieldSettings_Id
                        where cfsl.Language_Id == languageId// && cfs.Id > 1828 //TODO: make this list dynamic later => with && customer_Id == null => it doesn't work at the moment.. 
                        group cfs by new
                        {
                            cfs.Id,
                            cfs.FieldSize,
                            cfsl.Label,
                            //cfs.NameOrigin,
                            languageId,
                            cfs.Required,
                            cfs.ShowExternal,
                            cfs.ShowOnStartPage,
                            cfs.Name
                        }
                            into grouped
                            select new CaseListToCase
                            {
                                CFS_Id = grouped.Key.Id,
                                FieldSize = grouped.Key.FieldSize,
                                LabelNotToChange = grouped.Key.Label,
                                //LabelToChange = grouped.Key.NameOrigin,
                                Language_Id = grouped.Key.languageId,
                                Required = grouped.Key.Required,
                                ShowExternal = grouped.Key.ShowExternal,
                                ShowOnStartPage = grouped.Key.ShowOnStartPage,
                                Name = grouped.Key.Name
                            };

                query = q;
            }

            return query;
        }

        public IEnumerable<CaseFieldSetting> GetCaseFieldSettingsForDefaultCust()
        {


            //var result = from cfs in this.DataContext.CaseFieldSettings
            //             where cfs.Customer_Id == null
            //             select cfs;

            //return result;


            var query = from cfs in this.DataContext.CaseFieldSettings
                        where cfs.Customer_Id == null
                        group cfs by new { cfs.Name, cfs.Required, cfs.ShowExternal, cfs.ShowOnStartPage } into g
                        select new CaseFieldSetting
                        {
                            Name = g.Key.Name,
                            Required = g.Key.Required,
                            ShowOnStartPage = g.Key.ShowOnStartPage,
                            ShowExternal = g.Key.ShowExternal
                        };

            return query;
        }

        public IEnumerable<ListCases> GetListCasesToCaseSummary(int? customerId, int? languageId, int? userGroupId)
        {
            var query = from cfs in this.DataContext.CaseFieldSettings
                        join cfsl in this.DataContext.CaseFieldSettingLanguages on cfs.Id equals cfsl.CaseFieldSettings_Id
                        join cs in this.DataContext.CaseSettings on cfs.Name equals cs.Name
                        where cfs.Customer_Id == customerId && cfsl.Language_Id == languageId && cs.UserGroup == userGroupId && cfs.ShowOnStartPage == 1
                        group cfs by new { cfs.Id, cfsl.CaseFieldSettings_Id, cfsl.Label, cfs.Name } into grouped
                        select new ListCases
                        {
                            CFS_Id = grouped.Key.Id,
                            CFSL_CaseFieldSettings_Id = grouped.Key.CaseFieldSettings_Id,
                            Label = grouped.Key.Label,
                            Name = grouped.Key.Name
                        };

            return query;
        }

        public IEnumerable<ListCases> GetCaseFieldSettingsListToCustomerCaseSummary(int? customerId, int? languageId, int? userGroupId)
        {
            var query = from cfs in this.DataContext.CaseFieldSettings
                        join cfsl in this.DataContext.CaseFieldSettingLanguages on cfs.Id equals cfsl.CaseFieldSettings_Id
                        join cs in this.DataContext.CaseSettings on cfs.Name equals cs.Name
                        where cs.Customer_Id == customerId && cfsl.Language_Id == languageId && cs.UserGroup == userGroupId && cfs.ShowOnStartPage == 1 && cs.User_Id == null
                        group cfs by new { cfs.Id, cfsl.CaseFieldSettings_Id, cfsl.Label, cfs.Name } into grouped
                        select new ListCases
                        {
                            CFS_Id = grouped.Key.Id,
                            CFSL_CaseFieldSettings_Id = grouped.Key.CaseFieldSettings_Id,
                            Label = grouped.Key.Label,
                            Name = grouped.Key.Name
                        };

            return query;
        }

        public IEnumerable<CaseFieldSetting> GetCustomerCaseFieldSettings(int customerId, int? languageId = null)
        {
            var query = DataContext.CaseFieldSettings.Where(c => c.Customer_Id == customerId);
            if (languageId.HasValue)
                query = query.Include(c =>
                    c.CaseFieldSettingLanguages);
            return query;
        }
    }

    #endregion
}
