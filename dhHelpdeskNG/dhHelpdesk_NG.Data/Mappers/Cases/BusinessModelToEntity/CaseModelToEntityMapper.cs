using System;

namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using BusinessData.Models.Case;   
    using Domain;

    public sealed class CaseModelToEntityMapper : IBusinessModelToEntityMapper<CaseModel, Case>
    {
        public void Map(CaseModel businessModel, Case entity)
        {
            if (entity == null)
            {
                return;
            }
            
            var properties = businessModel.GetType().GetProperties();

            //note: also handles Case.IsAbout properties due to additional Case.IsAbout_<name> setters
            //note: this is existing logic moved from UniveralCaseService.cs\ConvertCaseModelToCase
            foreach (var prop in properties)
            {
                var type = prop.PropertyType;
                var typeCode = Type.GetTypeCode(type);
                var destProperty = entity.GetType().GetProperty(prop.Name);

                if (destProperty != null && destProperty.CanWrite)
                {
                    switch (typeCode)
                    {
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                            var intVal = (int)prop.GetValue(businessModel, null);
                            destProperty.SetValue(entity, intVal);
                            break;

                        case TypeCode.Decimal:
                            var decimalVal = (decimal)prop.GetValue(businessModel, null);
                            destProperty.SetValue(entity, decimalVal);
                            break;

                        case TypeCode.String:
                            var strVal = (string)prop.GetValue(businessModel, null);
                            destProperty.SetValue(entity, strVal);
                            break;

                        case TypeCode.Boolean:
                            var boolVal = (bool)prop.GetValue(businessModel, null);
                            destProperty.SetValue(entity, boolVal);
                            break;

                        case TypeCode.DateTime:
                            var dateVal = (DateTime)prop.GetValue(businessModel, null);
                            destProperty.SetValue(entity, dateVal);
                            break;

                        case TypeCode.Object:
                            if (type == typeof(int?))
                            {
                                var nullIntVal = (int?)prop.GetValue(businessModel, null);
                                destProperty.SetValue(entity, nullIntVal);
                            }
                            else if (type == typeof(DateTime?))
                            {
                                var nullDateVal = (DateTime?)prop.GetValue(businessModel, null);
                                destProperty.SetValue(entity, nullDateVal);
                            }
                            else if (type == typeof(Guid))
                            {
                                var guidVal = (Guid)prop.GetValue(businessModel, null);
                                destProperty.SetValue(entity, guidVal);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}