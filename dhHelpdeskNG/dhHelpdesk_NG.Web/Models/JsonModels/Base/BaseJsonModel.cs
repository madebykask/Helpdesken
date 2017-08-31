using DH.Helpdesk.BusinessData.Models.Shared.Input;
using DH.Helpdesk.Common.Constants;
using Newtonsoft.Json;
using System;

namespace DH.Helpdesk.Web.Models.JsonModels.Base
{
    public abstract class BaseJsonModel<T> where T:INewBusinessModel
    {

        public BaseJsonModel()
        {
            /*
             NOTE:
                This line will initiate the inherited class properties with "NotChangedValue". 
                It used to detect properties need to be updated.
                If you got NULL as value for a property which must have value, 
                check the defined types in Initiate() method.
            */
            Initiate();
        }

        public abstract T ToBussinessModel();

        protected void Initiate()
        {
            var properties = GetType().GetProperties();
            foreach (var prop in properties)
            {
                var type = prop.PropertyType;
                var typeCode = Type.GetTypeCode(type);
                switch (typeCode)
                {
                    case TypeCode.Int32:
                        prop.SetValue(this, NotChangedValue.INT);
                        break;

                    case TypeCode.Int64:
                        prop.SetValue(this, NotChangedValue.INT);
                        break;

                    case TypeCode.String:
                        prop.SetValue(this, NotChangedValue.STRING);
                        break;

                    case TypeCode.DateTime:
                        prop.SetValue(this, NotChangedValue.DATETIME);
                        break;

                    case TypeCode.Decimal:
                        prop.SetValue(this, NotChangedValue.DECIMAL);
                        break;

                    case TypeCode.Object:
                        if (type == typeof(int?))
                        {
                            prop.SetValue(this, NotChangedValue.NULLABLE_INT);
                        }
                        else
                        if (type == typeof(DateTime?))
                        {
                            prop.SetValue(this, NotChangedValue.NULLABLE_DATETIME);
                        }
                        break;

                    default:
                        prop.SetValue(this, null);
                        break;
                }
            }
        }

        public string Serialize()
        {
            return (this == null) ? string.Empty : JsonConvert.SerializeObject(this);
        }
    }
    
}