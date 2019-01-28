using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ExtendedCase.Dal.Connection;
using ExtendedCase.Dal.Data;

namespace ExtendedCase.Dal.Repositories
{
    public interface IFormRepository
    {
        ExtendedCaseData GetExtendedCaseDataByUniqueId(Guid uniqueId, bool loadFieldsValues = true);
        ExtendedCaseData GetExtendedCaseDataById(int id, bool loadFieldsValues = true);

        ExtendedCaseForm GetFormById(int id);

        IList<ExtendedCaseForm> GetForms();

        ExtendedCaseForm GetMetaDataByAssignment(int? userRole, int? caseStatus, int? customerId);

        int SaveFormData(Guid uniqueIdString, int formId, IList<ExtendedCaseFieldValue> fieldsValues, string modifiedBy);

        IList<ExtendedCaseFormStateItem> GetFormStateItems(Guid uniqueId);
        void SaveFormState(Guid uniqueIdString, int caseDataId, IList<ExtendedCaseFormStateItem> stateItems);

    }

    public class FormRepository : HelpdeskRespositoryBase, IFormRepository
    {
        #region ctor()

        public FormRepository(IDbConnectionFactory connectionFactory) 
            : base(connectionFactory)
        {
        }

        #endregion

        public ExtendedCaseForm GetFormById(int id)
        {
            const string sql = @"
                SELECT * FROM ExtendedCaseForms
                WHERE Id = @id
            ";

            var res = QueryById<int, ExtendedCaseForm>(sql, id);
            return res;
        }

        public IList<ExtendedCaseForm> GetForms()
        {
            const string sql = @"
                SELECT TOP 1000 * FROM ExtendedCaseForms
            ";

            var res = QueryList<ExtendedCaseForm>(sql, new Dictionary<string, string>());
            return res;
        }

        #region MetaData

        public ExtendedCaseForm GetMetaDataByAssignment(int? userRole, int? caseStatus, int? customerId)
        {
            const string sql = @"
                SELECT TOP 1 Id, MetaData, DefaultLanguageId FROM ExtendedCaseForms f
                JOIN ExtendedCaseAssignments fa ON fa.ExtendedCaseFormId = f.Id
                WHERE (fa.UserRole = @userRole OR (@userRole IS NULL AND fa.UserRole IS NULL))
                    AND (fa.CaseStatus = @caseStatus OR (@caseStatus IS NULL AND fa.CaseStatus IS NULL))
                    AND (fa.CustomerId = @customerId OR (@customerId IS NULL AND fa.CustomerId IS NULL))
            ";

            return QuerySingle<ExtendedCaseForm>(sql, new { userRole, caseStatus, customerId });
        }

        #endregion

        #region GetExtendedCaseData

        public ExtendedCaseData GetExtendedCaseDataByUniqueId(Guid uniqueId, bool loadFieldsValues = true)
        {
            var sql =
                "select Id, ExtendedCaseGuid, ExtendedCaseFormId, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy from  ExtendedCaseData where ExtendedCaseGuid = @id";

            var extendedCaseData = QuerySingle<ExtendedCaseData>(sql, new { id = uniqueId });

            if (loadFieldsValues)
            {
                extendedCaseData.FieldsValues = GetExtendedCaseFieldsValues(extendedCaseData.Id);
            }

            return extendedCaseData;
        }

        public ExtendedCaseData GetExtendedCaseDataById(int id, bool loadFieldsValues = true)
        {
            var sql =
                "select Id, ExtendedCaseGuid, ExtendedCaseFormId, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy from  ExtendedCaseData where Id = @id";
            
            var extendedCaseData = QueryById<int, ExtendedCaseData>(sql, id);

            if (loadFieldsValues)
            {
                extendedCaseData.FieldsValues = GetExtendedCaseFieldsValues(extendedCaseData.Id);
            }

            return extendedCaseData;
        }

        private IList<ExtendedCaseFieldValue> GetExtendedCaseFieldsValues(int extendedCaseId)
        {
            var sql =
                "select Id, ExtendedCaseDataId, FieldId, Value, SecondaryValue, Properties from [dbo].[ExtendedCaseValues] where ExtendedCaseDataId=@id";

            var list = QueryList<ExtendedCaseFieldValue>(sql, new { id = extendedCaseId });
            return list;
        }

        #endregion

        #region SaveFormData methods

        public int SaveFormData(Guid uniqueId, int formId, IList<ExtendedCaseFieldValue> fieldValues, string modifiedBy)
        {
            //todo: add transaction
            
            if (uniqueId != Guid.Empty)
            {
                return UpdateFormData(uniqueId, formId, fieldValues, modifiedBy);
            }
            else
            {
                return InsertFormData(formId, fieldValues, modifiedBy);
            }
        }

        private int InsertFormData(int formId, IList<ExtendedCaseFieldValue> fieldValues, string createdBy)
        {
            var sql = "INSERT INTO dbo.ExtendedCaseData(ExtendedCaseFormId, CreatedOn, CreatedBy) " +
                      "VALUES(@extendedCaseFormId, GETDATE(), @createdBy);" +
                      "SELECT CAST(SCOPE_IDENTITY() as int)";

            //use QuerySingle instead ExecQuery to get select scope identity result
            var newId = QuerySingle<int>(sql, new { extendedCaseFormId = formId, createdBy });

            foreach (var fieldValue in fieldValues)
            {
                InsertFieldValue(newId, fieldValue);
            }

            return newId;
        }

        private int UpdateFormData(Guid uniqueId, int formId, IList<ExtendedCaseFieldValue> fieldsValues, string modifiedBy)
        {
            var sql = "UPDATE dbo.ExtendedCaseData " +
                      "SET ExtendedCaseFormId = @extendedCaseFormId," +
                      "UpdatedOn = GETDATE(), " +
                      "UpdatedBy = @modifiedBy " +
                      "where Id = @id";

            var currentCaseData = GetExtendedCaseDataByUniqueId(uniqueId);

            var newFormId = formId > 0  ? formId : currentCaseData.ExtendedCaseFormId;
            ExecQuery(sql, new { extendedCaseFormId = newFormId, modifiedBy, id = currentCaseData.Id});

            var itemsForUpdate = new List<ExtendedCaseFieldValue>();
            var itemsForInsert = new List<ExtendedCaseFieldValue>();
            var itemsForDelete = new List<ExtendedCaseFieldValue>();

            //1. get records for delete
            foreach (var currentFieldValue in currentCaseData.FieldsValues)
            {
                if (fieldsValues.All(o => !o.FieldId.Equals(currentFieldValue.FieldId, StringComparison.OrdinalIgnoreCase)))
                {
                    itemsForDelete.Add(currentFieldValue);
                }
            }

            //2. get records for update and insert
            foreach (var modifiedFieldValue in fieldsValues)
            {
                var fieldValue = 
                    currentCaseData.FieldsValues.FirstOrDefault(o => o.FieldId.Equals(modifiedFieldValue.FieldId, StringComparison.OrdinalIgnoreCase));

                if (fieldValue != null)
                {
                    modifiedFieldValue.Id = fieldValue.Id;
                    itemsForUpdate.Add(modifiedFieldValue);
                }
                else
                {
                    itemsForInsert.Add(modifiedFieldValue);
                }
            }

            //delete
            foreach (var fieldValue in itemsForDelete)
            {
                DeleteFieldValue(fieldValue);
            }

            //update
            foreach (var fieldValue in itemsForUpdate)
            {
                UpdateFieldValue(fieldValue);
            }

            //insert new
            foreach (var fieldValue in itemsForInsert)
            {
                InsertFieldValue(currentCaseData.Id, fieldValue);
            }

            return currentCaseData.Id;
        }

        private bool DeleteFieldValue(ExtendedCaseFieldValue fieldValue)
        {
            var sql = "DELETE FROM [dbo].[ExtendedCaseValues] WHERE Id = @id";

            var affectedRows = ExecQuery(sql, new { id = fieldValue.Id});
            return affectedRows > 0;
        }

        private bool UpdateFieldValue(ExtendedCaseFieldValue fieldValue)
        {
            var sql = "UPDATE dbo.ExtendedCaseValues " +
                      "SET FieldId = @fieldId, Value = @value, SecondaryValue = @secondaryValue, Properties = @properties " +
                      "where Id = @id";

            var affectedRows = ExecQuery(sql,
                new
                {
                    id = fieldValue.Id,
                    fieldId = fieldValue.FieldId,
                    value = fieldValue.Value,
                    secondaryValue = fieldValue.SecondaryValue,
                    properties = fieldValue.Properties
                });

            return affectedRows > 0;
        }

        private int InsertFieldValue(int extendedCaseDataId, ExtendedCaseFieldValue fieldValue)
        {
            var sql = "INSERT INTO dbo.ExtendedCaseValues(ExtendedCaseDataId, FieldId, Value, SecondaryValue, Properties) " +
                      "VALUES(@extendedCaseDataId, @fieldId, @value, @secondaryValue, @properties); " +
                      "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters =
                new
                {
                    extendedCaseDataId,
                    fieldId = fieldValue.FieldId,
                    value = fieldValue.Value,
                    secondaryValue = fieldValue.SecondaryValue,
                    properties = fieldValue.Properties
                };

            var newId = QuerySingle<int>(sql, parameters);
            return newId;
        }

        #endregion

        #region SaveFormState methods

        public void SaveFormState(Guid uniqueId, int caseDataId, IList<ExtendedCaseFormStateItem> modifiedStateItems)
        {
            var existingItems = GetFormStateItems(uniqueId);

            if (existingItems.Count > 0)
            {
                foreach (var modifiedItem in modifiedStateItems)
                {
                    var existingItem = existingItems.FirstOrDefault(o => o.Equals(modifiedItem));
                    if (existingItem != null)
                    {
                        modifiedItem.Id = existingItem.Id;
                    }
                }
            }

            foreach (var modifiedItem in modifiedStateItems)
            {
                if (modifiedItem.Id > 0)
                {
                    UpdateFormStateItem(modifiedItem);
                }
                else
                {
                    //insert new item
                    modifiedItem.ExtendedCaseDataId = caseDataId;
                    InsertFormStateItem(modifiedItem);
                }
            }
        }

        public IList<ExtendedCaseFormStateItem> GetFormStateItems(Guid uniqueId)
        {
            const string sql = @"select fs.Id, fs.ExtendedCaseDataId, fs.TabId, fs.SectionId, fs.SectionIndex, fs.[Key], fs.Value 
                                 from dbo.ExtendedCaseFormState fs 
                                     inner join ExtendedCaseData cd on cd.Id = fs.ExtendedCaseDataId 
                                 where  cd.ExtendedCaseGuid = @uniqueId";

            var items = QueryList<ExtendedCaseFormStateItem>(sql, new { uniqueId });
            return items;
        }

        public int UpdateFormStateItem(ExtendedCaseFormStateItem stateItem)
        {
            const string sql = "UPDATE dbo.ExtendedCaseFormState SET Value = @newValue WHERE Id = @Id";

            var args = new { Id = stateItem.Id, newValue = stateItem.Value };
            var affectedRows = ExecQuery(sql, args);

            return affectedRows;
        }

        public int InsertFormStateItem(ExtendedCaseFormStateItem stateItem)
        {
            const string sql = @"INSERT INTO dbo.ExtendedCaseFormState (ExtendedCaseDataId, TabId, SectionId, SectionIndex, [Key], Value) 
                                 VALUES (@extendedCaseDataId, @tabId, @sectionId, @sectionIndex, @key, @value);
                                 SELECT CAST(SCOPE_IDENTITY() as int)";

            var args = new
            {
                extendedCaseDataId = stateItem.ExtendedCaseDataId,
                tabId = stateItem.TabId,
                sectionId = stateItem.SectionId,
                sectionIndex = stateItem.SectionIndex,
                key = stateItem.Key,
                value = stateItem.Value
            };

            var newId = QuerySingle<int>(sql, args);
            return newId;
        }

        #endregion
    }
}
