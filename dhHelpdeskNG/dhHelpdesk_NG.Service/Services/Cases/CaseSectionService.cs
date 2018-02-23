using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Case.CaseSections;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Dal.Repositories.Cases;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Services.Services.Cases
{
    public class CaseSectionService : ICaseSectionService
    {
        private readonly ICaseSectionsRepository _caseSectionsRepository;

        public CaseSectionService(ICaseSectionsRepository caseSectionsRepository)
        {
            _caseSectionsRepository = caseSectionsRepository;
        }

        public List<CaseSectionModel> GetCaseSections(int customerId, int languageId)
        {
            var caseSections = _caseSectionsRepository.GetCaseSections(customerId);
            return caseSections.Select(section => new CaseSectionModel
            {
                Id = section.Id,
                SectionType = section.SectionType,
                IsEditCollapsed = section.IsEditCollapsed,
                IsNewCollapsed = section.IsNewCollapsed,
                SectionHeader = 
                        section.CaseSectionLanguages.SingleOrDefault(x => x.CaseSection_Id == section.Id && x.Language_Id == languageId) != null
                            ? section.CaseSectionLanguages.Single(x => x.CaseSection_Id == section.Id && x.Language_Id == languageId).Label
                            : string.Empty,
                CustomerId = section.Customer_Id,
                CaseSectionFields = section.CaseSectionFields.Select(x => x.CaseFieldSetting_Id).ToList()
            }).ToList();
        }

        public CaseSectionModel GetCaseSection(int sectionId, int customerId, int languageId)
        {
            var section = _caseSectionsRepository.GetCaseSection(sectionId, customerId);
            return section != null
                ? new CaseSectionModel
                {
                    Id = section.Id,
                    SectionType = section.SectionType,
                    IsEditCollapsed = section.IsEditCollapsed,
                    IsNewCollapsed = section.IsNewCollapsed,
                    SectionHeader = section.CaseSectionLanguages.SingleOrDefault(x => x.CaseSection_Id == section.Id && x.Language_Id == languageId) != null
                    ? section.CaseSectionLanguages.Single(x => x.CaseSection_Id == section.Id && x.Language_Id == languageId).Label
                    : string.Empty,
                    CustomerId = section.Customer_Id,
                    CaseSectionFields = section.CaseSectionFields.Select(x => x.CaseFieldSetting_Id).ToList()
                }
                : new CaseSectionModel
                {
                    CustomerId = customerId
                };
        }

        public int SaveCaseSection(CaseSectionModel caseSection)
        {
            if (caseSection.Id > 0)
            {
                var cs = _caseSectionsRepository.GetCaseSection(caseSection.Id, caseSection.CustomerId);
                if (cs != null)
                {
                    cs.IsEditCollapsed = caseSection.IsEditCollapsed;
                    cs.IsNewCollapsed = caseSection.IsNewCollapsed;
                    cs.SectionType = caseSection.SectionType;
                    cs.UpdatedDate = DateTime.Now;

                    var existFieldsIds = cs.CaseSectionFields.Select(x => x.CaseFieldSetting_Id).ToList();
                    var toDel = existFieldsIds.Except(caseSection.CaseSectionFields).ToList();
                    var toAdd = caseSection.CaseSectionFields.Except(existFieldsIds).ToList();

                    foreach (var fieldId in toAdd)
                    {
                        cs.CaseSectionFields.Add(new CaseSectionField
                        {
                            CaseFieldSetting_Id = fieldId,
                            CaseSection_Id = caseSection.Id
                        });
                    }
                    _caseSectionsRepository.DeleteCaseSectionFields(toDel);
                }
                return _caseSectionsRepository.UpdateCaseSection(cs);
            }
            else
            {
                var cs = new CaseSection
                {
                    CreatedDate = DateTime.Now,
                    Customer_Id = caseSection.CustomerId,
                    IsEditCollapsed = caseSection.IsEditCollapsed,
                    IsNewCollapsed = caseSection.IsNewCollapsed,
                    SectionType = caseSection.SectionType,
                    CaseSectionFields = caseSection.CaseSectionFields != null ? caseSection.CaseSectionFields.Select(x => new CaseSectionField
                    {
                        CaseFieldSetting_Id = x
                    }).ToList() : new List<CaseSectionField>()
                };
                return _caseSectionsRepository.AddCaseSection(cs);
            }
        }

        public void SaveCaseSections(int languageId, IEnumerable<CaseSectionModel> caseSections, int customerId)
        {
            foreach (var section in caseSections)
            {
                if (section.Id > 0)
                {
                    var sect = _caseSectionsRepository.GetCaseSection(section.Id, section.CustomerId);
                    var sectLng = sect.CaseSectionLanguages.SingleOrDefault(x => x.CaseSection_Id == section.Id && x.Language_Id == languageId);
                    if (sectLng == null)
                    {
                        sect.CaseSectionLanguages.Add(new CaseSectionLanguage
                        {
                            CaseSection_Id = section.Id,
                            Language_Id = languageId,
                            Label = section.SectionHeader
                        });
                    }
                    else
                    {
                        sectLng.Label = section.SectionHeader;
                    }
                }
                else
                {
                    var sectionId = SaveCaseSection(section);
                    var newSectLng = new CaseSectionLanguage
                    {
                        CaseSection_Id = sectionId,
                        Language_Id = languageId,
                        Label = section.SectionHeader
                    };
                    var sect = _caseSectionsRepository.GetCaseSection(sectionId, section.CustomerId);
                    sect.CaseSectionLanguages.Add(newSectLng);
                }
            }
            _caseSectionsRepository.ApplyChanges();
        }
    }

    public interface ICaseSectionService
    {
        List<CaseSectionModel> GetCaseSections(int customerId, int languageId);
        CaseSectionModel GetCaseSection(int sectionId, int customerId, int languageId);
        int SaveCaseSection(CaseSectionModel caseSection);
        void SaveCaseSections(int languageId, IEnumerable<CaseSectionModel> caseSections, int customerId);
    }
}
