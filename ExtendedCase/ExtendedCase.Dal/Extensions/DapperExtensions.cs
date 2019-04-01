using System.Collections.Generic;
using Dapper;

namespace ExtendedCase.Dal.Extensions
{
    public static class DapperExtensions
    {
        public static DynamicParameters ToDynamicParameters(this IDictionary<string, string> parameters)
        {
            var dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }
            return dynamicParameters;
        }
    }
}