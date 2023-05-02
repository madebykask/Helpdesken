using System;
using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    using BusinessData.Models.Case;
    using Infrastructure;
    using Domain.ExtendedCaseEntity;
    using Mappers;
    using DH.Helpdesk.BusinessData.Models.ExtendedCase;
    using Newtonsoft.Json;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Dal.DbQueryExecutor;
    using DH.Helpdesk.BusinessData.Models.Language.Output;
    using DH.Helpdesk.Common.Tools;
    using System.Text.RegularExpressions;

    //NOTE: This is performance optimised class - pls do not use mappers!
    public sealed class ExtendedCaseFormRepository : RepositoryBase<ExtendedCaseFormEntity>, IExtendedCaseFormRepository
    {
        private readonly IExtendedCaseDataRepository _extendedCaseDataRepository;

        #region ctor()

        public ExtendedCaseFormRepository(
            IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }


        public ExtendedCaseFormRepository(
            IDatabaseFactory databaseFactory,
            IExtendedCaseDataRepository extendedCaseDataRepository)
            : base(databaseFactory)
        {
            _extendedCaseDataRepository = extendedCaseDataRepository;
        }

        #endregion

        public Guid CreateExtendedCaseData(int formId, string userGuid)
        {
            var newGuid = Guid.NewGuid();

            var extendedCaseData = new ExtendedCaseDataEntity
            {
                ExtendedCaseGuid = newGuid,
                ExtendedCaseFormId = formId,
                //TODO: THIS is temp
                CreatedBy = userGuid,
                CreatedOn = DateTime.Now
            };

            _extendedCaseDataRepository.AddEcd(extendedCaseData);

            return newGuid;
        }

        public ExtendedCaseDataOverview GetExtendedCaseFormForSolution(int caseSolutionId, int customerId)
        {
            var extendedFormData =
                (from cs in DataContext.CaseSolutions
                 from exCaseForm in cs.ExtendedCaseForms
                 where cs.Customer_Id == customerId &&
                       cs.Id == caseSolutionId
                 select new ExtendedCaseDataOverview
                 {
                     ExtendedCaseFormId = exCaseForm.Id,
                     ExtendedCaseFormName = exCaseForm.Name,
                     Version = exCaseForm.Version
                 })
                .FirstOrDefault();

            return extendedFormData;
        }

        public ExtendedCaseDataOverview GetCaseSectionExtendedCaseFormForSolution(int caseSolutionId, int customerId, int sectionType)
        {
            var extendedFormData =
                   (from cs in DataContext.CaseSolutions
                    from exCaseSec in cs.CaseSectionsExtendedCaseForm
                    where cs.Customer_Id == customerId &&
                          cs.Id == caseSolutionId &&
                          exCaseSec.CaseSection.SectionType == sectionType
                    let extendedCaseForm = exCaseSec.ExtendedCaseForm
                    select new ExtendedCaseDataOverview
                    {
                        CaseId = 0,
                        SectionType = sectionType,
                        ExtendedCaseFormId = exCaseSec.ExtendedCaseFormID,
                        ExtendedCaseFormName = extendedCaseForm.Name
                    })
                .FirstOrDefault();

            return extendedFormData;
        }

        public ExtendedCaseDataOverview GetExtendedCaseFormForCase(int caseId, int customerId)
        {
            var extendedFormData =
                   (from _case in DataContext.Cases
                    from sec in _case.CaseExtendedCaseDatas
                    where _case.Customer_Id == customerId &&
                          _case.Id == caseId
                    let extendedCaseForm = sec.ExtendedCaseData.ExtendedCaseForm
                    select new ExtendedCaseDataOverview
                    {
                        CaseId = caseId,
                        ExtendedCaseFormId = sec.ExtendedCaseData.ExtendedCaseForm.Id,
                        ExtendedCaseFormName = extendedCaseForm.Name,
                        ExtendedCaseGuid = sec.ExtendedCaseData.ExtendedCaseGuid
                    })
                .SingleOrDefault();

            return extendedFormData;
        }

        public ExtendedCaseDataOverview GetCaseSectionExtendedCaseFormForCase(int caseId, int customerId)
        {
            var extendedFormData =
                    (from _case in DataContext.Cases
                     from sec in _case.CaseSectionExtendedCaseDatas
                     where _case.Customer_Id == customerId &&
                           _case.Id == caseId
                     let extendedCaseForm = sec.ExtendedCaseData.ExtendedCaseForm
                     select new ExtendedCaseDataOverview
                     {
                         CaseId = caseId,
                         ExtendedCaseFormId = sec.ExtendedCaseData.ExtendedCaseForm.Id,
                         ExtendedCaseFormName = extendedCaseForm.Name,
                         ExtendedCaseGuid = sec.ExtendedCaseData.ExtendedCaseGuid,
                         SectionType = sec.CaseSection.SectionType
                     })
                 .Single();

            return extendedFormData;
        }

        public List<ExtendedCaseDataOverview> GetExtendedCaseFormsForSections(int caseId, int customerId)
        {
            var extendedForm =
                (from _case in DataContext.Cases
                 from sec in _case.CaseSectionExtendedCaseDatas
                 where _case.Customer_Id == customerId && _case.Id == caseId
                 let extendedCaseForm = sec.ExtendedCaseData.ExtendedCaseForm
                 select new ExtendedCaseDataOverview
                 {
                     CaseId = caseId,
                     ExtendedCaseFormId = sec.ExtendedCaseData.ExtendedCaseForm.Id,
                     SectionType = sec.CaseSection.SectionType,
                     ExtendedCaseGuid = sec.ExtendedCaseData.ExtendedCaseGuid,
                     ExtendedCaseFormName = extendedCaseForm.Name
                 }).ToList();

            return extendedForm;
        }

        public List<ExtendedCaseFormEntity> GetExtendedCaseFormsForCustomer(int customerId)
        {
            var query = DataContext.ExtendedCaseForms
                .Where(o => o.CaseSolutions.Any(f => f.Customer_Id == customerId));

            return query.ToList();
        }

        public List<ExtendedCaseFormFieldTranslationModel> GetExtendedCaseFormFields(int extendedCaseFormId, int languageID)
        {
            var fieldIds = DataContext.ExtendedCaseValues.Where(o => o.ExtendedCaseData.ExtendedCaseFormId == extendedCaseFormId)
                .Where(o => !string.IsNullOrEmpty(o.Value) || !string.IsNullOrEmpty(o.SecondaryValue))
                .Select(o => o.FieldId)
                .Distinct();

            var fieldTranslations = DataContext.ExtendedCaseTranslations
                .Where(o => o.LanguageId == languageID)
                .Join(
                    fieldIds,
                    t => t.Property.ToLower(),
                    f => ("Control." + f.Substring(f.IndexOf(".controls.") + 10, f.Length - f.IndexOf(".controls.") - 9)).ToLower(),
                    (t, f) => new ExtendedCaseFormFieldTranslationModel
                    {
                        FieldId = f,
                        Text = t.Text,
                        LanguageId = t.LanguageId
                    }
                ).ToList()
                .OrderBy(o => o.Text)
                .ToList();


            return fieldTranslations;

        }

        public List<ExtendedCaseFormSectionTranslationModel> GetExtendedCaseFormSections(int extendedCaseFormId, int languageID)
        {
            var fieldIds = DataContext.ExtendedCaseValues.Where(o => o.ExtendedCaseData.ExtendedCaseFormId == extendedCaseFormId)
                .Where(o => !string.IsNullOrEmpty(o.Value) || !string.IsNullOrEmpty(o.SecondaryValue))
                .Select(o => o.FieldId)
                .Distinct();

            var sectionTranslations = DataContext.ExtendedCaseTranslations.AsEnumerable()
                .Where(o => o.LanguageId == languageID)
                .Join(
                    fieldIds,
                    t => t.Property.ToLower(),
                    s => ("Section." + GetSectionName(s)).ToLower(),
                    (t, s) => new ExtendedCaseFormSectionTranslationModel
                    {
                        SectionId = s,
                        Text = t.Text,
                        LanguageId = t.LanguageId
                    }
                ).ToList()
                //.GroupBy(x => x.Text).Select(x => x.First())
                .OrderBy(o => o.Text)
                .ToList();


            foreach (var section in sectionTranslations)
            {
                var MultisectionNumber = section.SectionId.Substring(section.SectionId.IndexOf("[") + 1, 1);
                int mSectionNumber = int.Parse(MultisectionNumber);
                if (mSectionNumber != 0)
                {
                    if (mSectionNumber == 1)
                    {
                        var firstFieldId = section.SectionId.Replace("1", "0");
                        var firstSection = sectionTranslations.Where(f => f.SectionId == firstFieldId).FirstOrDefault();
                        firstSection.Text = firstSection.Text + " " + (mSectionNumber).ToString();
                    }

                    section.Text = section.Text + " " + (mSectionNumber + 1).ToString();
                }
            }
            return sectionTranslations;

        }

        private string GetSectionName(string FieldId)
        {
            var prefix = ".sections.";
            var prefixLen = prefix.Length;
            var startPrefix = FieldId.IndexOf(prefix);
            var startSectionText = FieldId.Remove(0, startPrefix + prefixLen); //"ValAvveckling.instances[0].controls.Kommentarer";            
            var firstDelimiter = startSectionText.IndexOf(".");
            var sectionName = startSectionText.Substring(0, firstDelimiter);

            return sectionName;
        }

        public int SaveExtendedCaseForm(ExtendedCaseFormJsonModel entity, string userId, List<ExtendedCaseFormTranslation> translations)
        {
            ExtendedCaseFormEntity res = new ExtendedCaseFormEntity();

            if (entity.id > 0)
            {
                res = DataContext.ExtendedCaseForms.Where(e => e.Id == entity.id).FirstOrDefault();
                res.Status = entity.status ? 1 : 0;
                res.Version = res.Version++;
                res.Name = entity.name;
                res.Description = entity.description;
                res.UpdatedBy = userId;
                res.UpdatedOn = DateTime.Now;
            }

            else
            {
                res = DataContext.ExtendedCaseForms.Add(new ExtendedCaseFormEntity()
                {
                    MetaData = "",
                    CreatedOn = DateTime.Now,
                    CreatedBy = userId,
                    Status = entity.status ? 1 : 0,
                    Version = 1,
                    Guid = Guid.NewGuid(),
                    CreatedByEditor = true,
                    Name = entity.name,
                    Description = entity.description,
                    Customer_Id = entity.customerId

                });

                DataContext.Commit();

                entity.id = res.Id;
            }

            foreach (var tab in entity.tabs)
            {
                string tabId = tab.id;
                var cleanTabName = StringHelper.GetCleanString(tab.id);
                var tabNameWithFormId = cleanTabName.EndsWith("_" + entity.id) ? cleanTabName : cleanTabName + "_" + entity.id;
                if (!tabId.Contains("Tab."))
                { tabId = "Tab." + tabNameWithFormId; }

                foreach (var t in translations.Where(x => x.ControlType == "Tab" && StringHelper.GetCleanString(x.Property) == cleanTabName))
                {
                    if (t.TranslationId != 0)
                    {
                        var updatedTranslation = DataContext.ExtendedCaseTranslations.FirstOrDefault(x => x.Id == t.TranslationId);
                        updatedTranslation.Property = tabId;
                        updatedTranslation.Text = t.Text ?? "";
                        updatedTranslation.ExtendedCaseForm_Id = entity.id;
                    }
                    else if (DataContext.ExtendedCaseTranslations.Where(ct => ct.Property == tabId && t.LanguageId == ct.LanguageId).Count() == 0)
                    {
                        DataContext.ExtendedCaseTranslations.Add(
                        new ExtendedCaseTranslationEntity()
                        {
                            LanguageId = t.LanguageId,
                            Property = tabId,
                            Text = t.Text ?? "",
                            ExtendedCaseForm_Id = entity.id
                        });
                    }
                    else
                    {
                        var updatedTranslation = DataContext.ExtendedCaseTranslations.Where(u => u.Property == tabId && u.LanguageId == t.LanguageId).FirstOrDefault();
                        updatedTranslation.Text = t.Text ?? "";
                        updatedTranslation.ExtendedCaseForm_Id = entity.id;
                    }
                    DataContext.Commit();
                }

                tab.name = "@Translation." + tabId;
                tab.id = tabNameWithFormId;
            }


            foreach (var s in entity.tabs[0].sections.Where(x => x.id != "InitiatorInfo" && x.id != "HiddenFields"))
            {
                string sectionId = s.id;
                var cleanSectionName = StringHelper.GetCleanString(s.id);
                var sectionNameWithFormId = cleanSectionName.EndsWith("_" + entity.id) ? cleanSectionName : cleanSectionName + "_" + entity.id;
                if (!sectionId.Contains("Section."))
                { sectionId = "Section." + sectionNameWithFormId; }

                foreach (var t in translations.Where(x => x.ControlType == "Section" && StringHelper.GetCleanString(x.Property) == cleanSectionName))
                {
                    if (t.TranslationId != 0)
                    {
                        var updatedTranslation = DataContext.ExtendedCaseTranslations.FirstOrDefault(x => x.Id == t.TranslationId);
                        updatedTranslation.Property = sectionId;
                        updatedTranslation.Text = t.Text ?? "";
                        updatedTranslation.ExtendedCaseForm_Id = entity.id;
                    }
                    else if (DataContext.ExtendedCaseTranslations.Where(ct => ct.Property == sectionId && t.LanguageId == ct.LanguageId).Count() == 0)
                    {
                        DataContext.ExtendedCaseTranslations.Add(
                        new ExtendedCaseTranslationEntity()
                        {
                            LanguageId = t.LanguageId,
                            Property = sectionId,
                            Text = t.Text ?? "",
                            ExtendedCaseForm_Id = entity.id
                        });
                    }
                    else
                    {
                        var updatedTranslation = DataContext.ExtendedCaseTranslations.Where(u => u.Property == sectionId && u.LanguageId == t.LanguageId).FirstOrDefault();
                        updatedTranslation.Text = t.Text ?? "";
                        updatedTranslation.ExtendedCaseForm_Id = entity.id;
                    }
                    DataContext.Commit();
                }

                s.name = "@Translation." + sectionId;
                s.id = sectionNameWithFormId;

                foreach (var c in s.controls)
                {
                    string controlId = c.id;
                    var cleanControlName = StringHelper.GetCleanString(c.id);
                    var controlNameWithFormId = cleanControlName.EndsWith("_" + entity.id) ? cleanControlName : cleanControlName + "_" + entity.id;
                    if (!controlId.Contains("Control."))
                    { controlId = "Control." + controlNameWithFormId; }

                    foreach (var t in translations.Where(x => x.ControlType != "Section" && StringHelper.GetCleanString(x.Property) == cleanControlName))
                    {

                        var text = (c.valueBinding != null && c.type != "html") ? t.Text.Replace(@"""", String.Empty) : (t.Text ?? "");

                        if (t.TranslationId != 0)
                        {
                            var updatedTranslation = DataContext.ExtendedCaseTranslations.FirstOrDefault(x => x.Id == t.TranslationId);

                            updatedTranslation.Property = controlId;
                            updatedTranslation.Text = text;
                            updatedTranslation.ExtendedCaseForm_Id = entity.id;
                        }
                        else if (DataContext.ExtendedCaseTranslations.Where(ct => ct.Property == controlId && t.LanguageId == ct.LanguageId).Count() == 0)
                        {
                            DataContext.ExtendedCaseTranslations.Add(
                            new ExtendedCaseTranslationEntity()
                            {
                                LanguageId = t.LanguageId,
                                Property = controlId,
                                Text = text,
                                ExtendedCaseForm_Id = entity.id
                            });
                        }
                        else
                        {
                            var updatedTranslation = DataContext.ExtendedCaseTranslations.Where(u => u.Property == controlId && u.LanguageId == t.LanguageId).FirstOrDefault();
                            updatedTranslation.Text = text;
                            updatedTranslation.ExtendedCaseForm_Id = entity.id;
                        }
                        DataContext.Commit();
                    }
                    c.label = "@Translation." + controlId;
                    c.id = controlNameWithFormId;

                    if (c.valueBinding != null)
                    {
                        c.valueBinding = "function(m) { return \"" + "@Translation." + controlId + ";" + "\"" + " }";
                    }

                    if (!String.IsNullOrEmpty(c.addonText))
                    {
                        var addOnTextId = c.addonText;
                        var cleanAddOnText = StringHelper.GetCleanString(addOnTextId);
                        var cleanAddOnTextWithFormId = cleanAddOnText.EndsWith("_" + entity.id) ? cleanAddOnText : cleanAddOnText + "_" + entity.id;
                        if (!addOnTextId.Contains("Message."))
                        { addOnTextId = "Message." + cleanAddOnTextWithFormId; }

                        foreach (var t in translations.Where(x => x.ControlType == "fileUpload" && StringHelper.GetCleanString(x.Property) == cleanAddOnText))
                        {
                            if (t.TranslationId != 0)
                            {
                                var updatedTranslation = DataContext.ExtendedCaseTranslations.FirstOrDefault(x => x.Id == t.TranslationId);

                                updatedTranslation.Property = addOnTextId;
                                updatedTranslation.Text = t.Text ?? "";
                                updatedTranslation.ExtendedCaseForm_Id = entity.id;
                            }
                            else if (DataContext.ExtendedCaseTranslations.Where(ct => ct.Property == addOnTextId && t.LanguageId == ct.LanguageId).Count() == 0)
                            {
                                DataContext.ExtendedCaseTranslations.Add(
                                new ExtendedCaseTranslationEntity()
                                {
                                    LanguageId = t.LanguageId,
                                    Property = addOnTextId,
                                    Text = t.Text ?? "",
                                    ExtendedCaseForm_Id = entity.id
                                });
                            }
                            else
                            {
                                var updatedTranslation = DataContext.ExtendedCaseTranslations.Where(u => u.Property == addOnTextId && u.LanguageId == t.LanguageId).FirstOrDefault();
                                updatedTranslation.Text = t.Text ?? "";
                                updatedTranslation.ExtendedCaseForm_Id = entity.id;
                            }
                            DataContext.Commit();
                        }
                        c.addonText = "@Translation." + addOnTextId;
                    }

                    if (c.dataSource != null)
                    {
                        foreach (var d in c.dataSource)
                        {
                            var dataSourceTextId = d.value;
                            var cleanDataSourceText = StringHelper.GetCleanString(dataSourceTextId);
                            var cleanDataSourceTextWithFormId = cleanDataSourceText.EndsWith("_" + entity.id) ? cleanDataSourceText : cleanDataSourceText + "_" + entity.id;
                            if (!dataSourceTextId.Contains("DataSource.Value"))
                            { dataSourceTextId = "DataSource.Value." + cleanDataSourceTextWithFormId; }

                            foreach (var t in translations.Where(x => (x.ControlType == "radio" || x.ControlType == "checkbox-list" || x.ControlType == "dropdown") && StringHelper.GetCleanString(x.Property) == cleanDataSourceText))
                            {
                                if (t.TranslationId != 0)
                                {
                                    var updatedTranslation = DataContext.ExtendedCaseTranslations.FirstOrDefault(x => x.Id == t.TranslationId);

                                    updatedTranslation.Property = dataSourceTextId;
                                    updatedTranslation.Text = t.Text ?? "";
                                    updatedTranslation.ExtendedCaseForm_Id = entity.id;
                                }
                                else if (DataContext.ExtendedCaseTranslations.Where(ct => ct.Property == dataSourceTextId && t.LanguageId == ct.LanguageId).Count() == 0)
                                {
                                    DataContext.ExtendedCaseTranslations.Add(
                                    new ExtendedCaseTranslationEntity()
                                    {
                                        LanguageId = t.LanguageId,
                                        Property = dataSourceTextId,
                                        Text = t.Text ?? "",
                                        ExtendedCaseForm_Id = entity.id
                                    });
                                }
                                else
                                {
                                    var updatedTranslation = DataContext.ExtendedCaseTranslations.Where(u => u.Property == dataSourceTextId && u.LanguageId == t.LanguageId).FirstOrDefault();
                                    updatedTranslation.Text = t.Text ?? "";
                                    updatedTranslation.ExtendedCaseForm_Id = entity.id;
                                }
                                DataContext.Commit();
                            }
                            d.text = "@Translation." + dataSourceTextId;
                            d.value = cleanDataSourceTextWithFormId;
                        }
                    }
                }
            }

            var data = JsonConvert.SerializeObject(entity,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            var metaData = data.Replace(@"\""", String.Empty)                               
                                .Replace(@"""function(m) { return ", @"function(m) { return """)
                                .Replace(@"> }""", @">"";}")
                                .Replace(@"; }"",", @""" },")
                                .Replace(@"""function(m) { if (m.formInfo.applicationType == helpdesk) return true; }""",
                                        @"function(m) { if (m.formInfo.applicationType == ""helpdesk"") return true; }")
                                .Replace(@"""function(m) { if (m.formInfo.applicationType == selfservice) return true; }""",
                                        @"function(m) { if (m.formInfo.applicationType == ""selfservice"") return true; }")

                                .Replace(@"tabs.EditorInitiator.", "tabs." + entity.tabs[0].id + ".")
                                .Replace("{\"id\":\"InitiatorSectionData_" + entity.id.ToString() + "\",\"name\":\"@Translation.Section.InitiatorSectionData_" + entity.id.ToString() + "\",\"controls\":[]}", ExtendedCaseFormsHelper.GetEditorInitiatorData(entity.tabs[0].id, entity.customerGuid) + "]}");



            res.MetaData = metaData;

            if (entity.caseSolutionIds != null)
            {
                foreach (var c in entity.caseSolutionIds)
                {
                    if (DataContext.CaseSolutions.Where(x => x.Id == c).FirstOrDefault().ExtendedCaseForms.Count == 0)
                    {
                        DataContext.CaseSolutions.Where(x => x.Id == c).FirstOrDefault().ExtendedCaseForms.Add(res);
                    }
                }
            }

            IList<CaseSolution> items = DataContext.CaseSolutions.Where(f => f.ExtendedCaseForms.Any(x => x.Id == res.Id)).ToList();
            foreach (var e in items)
            {
                if (entity.caseSolutionIds != null)
                {
                    if (!entity.caseSolutionIds.Contains(e.Id))
                    {
                        DataContext.CaseSolutions.Where(c => c.Id == e.Id).FirstOrDefault().ExtendedCaseForms.Remove(res);
                    }
                }
                else
                {
                    DataContext.CaseSolutions.Where(c => c.Id == e.Id).FirstOrDefault().ExtendedCaseForms.Remove(res);
                }
            }

            DataContext.Commit();
            return res.Id;
        }

        public List<CaseSolution> GetCaseSolutionsWithExtendedCaseForm(ExtendedCaseFormPayloadModel formModel)
        {
            List<CaseSolution> caseSolutions = new List<CaseSolution>();
            if (formModel.CaseSolutionIds != null)
            {
                caseSolutions = (from cs in DataContext.CaseSolutions
                                 from exCaseForm in cs.ExtendedCaseForms
                                 where formModel.CaseSolutionIds.Contains(cs.Id) && exCaseForm.Id != formModel.Id
                                 select cs).ToList();
            }
            return caseSolutions;
        }

        public IList<ExtendedCaseFormEntity> GetExtendedCaseFormsCreatedByEditor(Customer customer)
        {
            var forms = (from e in DataContext.ExtendedCaseForms
                         where e.Customer_Id == customer.Id && e.CreatedByEditor == true
                         select e
                         ).ToList();

            return forms;
        }

        public ExtendedCaseFormEntity GetExtendedCaseFormById(int extendedCaseId)
        {
            var form = (from f in DataContext.ExtendedCaseForms
                        where f.Id == extendedCaseId
                        select f).FirstOrDefault();

            return form;
        }

        public bool DeleteExtendedCaseForm(int extendedCaseFormId)
        {
            if (DataContext.ExtendedCaseForms.Where(o => o.Id == extendedCaseFormId).FirstOrDefault() is ExtendedCaseFormEntity entity)
            {

                DataContext.ExtendedCaseForms.Remove(entity);

                DataContext.Commit();

                return true;
            }
            return false;
        }

        public bool ExtendedCaseFormInCases(int extendedCaseFormId)
        {
            return DataContext.Case_ExtendedCases.Any(x => x.ExtendedCaseForm_Id == extendedCaseFormId);
        }
    }
}