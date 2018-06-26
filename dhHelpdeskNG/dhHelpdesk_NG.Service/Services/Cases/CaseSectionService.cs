﻿using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseSections;
using DH.Helpdesk.BusinessData.OldComponents;
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
                ShowUserSearchCategory = section.ShowUserSearchCategory,
                DefaultUserSearchCategory = section.DefaultUserSearchCategory,
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
            
            if (section != null)
            {
                var sectionWithLang = section.CaseSectionLanguages.SingleOrDefault(x => x.CaseSection_Id == section.Id && x.Language_Id == languageId);
                return new CaseSectionModel
                {
                    Id = section.Id,
                    SectionType = section.SectionType,
                    IsEditCollapsed = section.IsEditCollapsed,
                    IsNewCollapsed = section.IsNewCollapsed,
                    SectionHeader = sectionWithLang != null ? sectionWithLang.Label : string.Empty,
                    CustomerId = section.Customer_Id,
                    CaseSectionFields = section.CaseSectionFields.Select(x => x.CaseFieldSetting_Id).ToList(),
                    DefaultUserSearchCategory = section.DefaultUserSearchCategory,
                    ShowUserSearchCategory = section.ShowUserSearchCategory
                };
            }

            return new CaseSectionModel { CustomerId = customerId };
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
                    cs.ShowUserSearchCategory = caseSection.ShowUserSearchCategory;
                    cs.DefaultUserSearchCategory = caseSection.DefaultUserSearchCategory;

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
                    ShowUserSearchCategory = caseSection.ShowUserSearchCategory,
                    DefaultUserSearchCategory = caseSection.DefaultUserSearchCategory,
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

        public CaseSectionInfo GetSectionInfoByField(string fieldName)
        {
            var caseSectionInfo = new CaseSectionInfo();

            if (fieldName == GlobalEnums.TranslationCaseFields.ReportedBy.ToString() ||
                fieldName == GlobalEnums.TranslationCaseFields.Persons_Name.ToString() ||
                fieldName == GlobalEnums.TranslationCaseFields.Persons_EMail.ToString() ||
                fieldName == GlobalEnums.TranslationCaseFields.Persons_Phone.ToString() ||
                fieldName == GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString() ||
                fieldName == GlobalEnums.TranslationCaseFields.Region_Id.ToString() ||
                fieldName == GlobalEnums.TranslationCaseFields.Department_Id.ToString() ||
                fieldName == GlobalEnums.TranslationCaseFields.OU_Id.ToString() ||
                fieldName == GlobalEnums.TranslationCaseFields.CostCentre.ToString() ||
                fieldName == GlobalEnums.TranslationCaseFields.Place.ToString() ||
                fieldName == GlobalEnums.TranslationCaseFields.UserCode.ToString() ||
                fieldName == GlobalEnums.TranslationCaseFields.AddUserBtn.ToString() ||
                fieldName == GlobalEnums.TranslationCaseFields.UpdateNotifierInformation.ToString() ||
                fieldName == GlobalEnums.TranslationCaseFields.AddFollowersBtn.ToString())
            {
                caseSectionInfo.DefaultName = CaseSections.InitiatorHeader;
                caseSectionInfo.Type = CaseSectionType.Initiator;
            }
            else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.IsAbout_OU_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.IsAbout_CostCentre.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Place.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString())
            {
                caseSectionInfo.DefaultName = CaseSections.RegardingHeader;
                caseSectionInfo.Type = CaseSectionType.Regarding;
            }
            else if (fieldName == GlobalEnums.TranslationCaseFields.InventoryNumber.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.InventoryLocation.ToString())
            {
                caseSectionInfo.DefaultName = CaseSections.ComputerInfoHeader;
                caseSectionInfo.Type = CaseSectionType.ComputerInfo;
            }
            else if (fieldName == GlobalEnums.TranslationCaseFields.CaseNumber.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.RegTime.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.ChangeTime.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.User_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.CaseType_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.System_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Urgency_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Impact_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Category_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Supplier_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.InvoiceNumber.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Caption.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Description.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Miscellaneous.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.ContactBeforeAction.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.SMS.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.AgreedDate.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Available.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Cost.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Filename.ToString())
            {
                caseSectionInfo.DefaultName = CaseSections.CaseInfoHeader;
                caseSectionInfo.Type = CaseSectionType.CaseInfo;
            }
            else if (fieldName == GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Priority_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Status_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Project.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Problem.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.PlanDate.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.WatchDate.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.Verified.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.SolutionRate.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.CausingPart.ToString())
            {
                caseSectionInfo.DefaultName = CaseSections.CaseManagementHeader;
                caseSectionInfo.Type = CaseSectionType.CaseManagement;
            }
            else if (fieldName == "tblLog.Text_External" ||
                     fieldName == "tblLog.Text_Internal" ||
                     fieldName == "tblLog.Charge" ||
                     fieldName == "tblLog.Filename" ||
                     fieldName == GlobalEnums.TranslationCaseFields.FinishingDescription.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.FinishingDate.ToString() ||
                     fieldName == GlobalEnums.TranslationCaseFields.ClosingReason.ToString())
            {
                caseSectionInfo.DefaultName = CaseSections.CommunicationHeader;
                caseSectionInfo.Type = CaseSectionType.Communication;
            }
            //else if (fieldName == GlobalEnums.TranslationCaseFields.CostCentre.ToString())
            //{
            //    caseSectionInfo.DefaultName = CaseSections.StatusHeader;
            //    caseSectionInfo.Type = CaseSectionType.Status;
            //}
            //else if (fieldName == GlobalEnums.TranslationCaseFields.CostCentre.ToString())
            //{
            //    caseSectionInfo.DefaultName = CaseSections.InvoicesHeader;
            //    caseSectionInfo.Type = CaseSectionType.Invoices;
            //}
            //else if (fieldName == GlobalEnums.TranslationCaseFields.CostCentre.ToString())
            //{
            //    caseSectionInfo.DefaultName = CaseSections.InvoicingHeader;
            //    caseSectionInfo.Type = CaseSectionType.Invoicing;
            //}
            //else if (fieldName == GlobalEnums.TranslationCaseFields.CostCentre.ToString())
            //{
            //    caseSectionInfo.DefaultName = CaseSections.ExtendedCaseHeader;
            //    caseSectionInfo.Type = CaseSectionType.ExtendedCase;
            //}
            else
            {
                return null;
            }

            return caseSectionInfo;
        }
    }

    public interface ICaseSectionService
    {
        List<CaseSectionModel> GetCaseSections(int customerId, int languageId);
        CaseSectionModel GetCaseSection(int sectionId, int customerId, int languageId);
        int SaveCaseSection(CaseSectionModel caseSection);
        void SaveCaseSections(int languageId, IEnumerable<CaseSectionModel> caseSections, int customerId);
        CaseSectionInfo GetSectionInfoByField(string fieldName);
    }
}
