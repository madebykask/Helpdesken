using System;
using System.Collections.Generic;

public static class DbQueryHelper
{
    public static IDictionary<string, string> ResolveParameters(IDictionary<string, string> inputParameters, IDictionary<string, string> metaDataParameters, IDictionary<string, string> constantParameters)
    {
        //include const parameters first
        var parameters = new Dictionary<string, string>(constantParameters, StringComparer.OrdinalIgnoreCase);

        foreach (var parameter in inputParameters)
        {
            if (metaDataParameters.ContainsKey(parameter.Key))
            {
                var value = parameter.Value;
                var columnName = metaDataParameters[parameter.Key];
                    
                // need to check in case const parameters may exist
                if (!parameters.ContainsKey(columnName))
                {
                    parameters.Add(columnName, value);
                }
            }
        }

        return parameters;
    }
}