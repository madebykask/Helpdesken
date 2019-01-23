using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace DH.Helpdesk.WebApi.Infrastructure.Swagger
{
    internal class GetOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                return;
            var complexParameters = operation.parameters.Where(x => x.@in == "query" && !string.IsNullOrWhiteSpace(x.name)).ToArray();

            foreach (var parameter in complexParameters)
            {
                if (!parameter.name.Contains('.')) continue;
                var name = parameter.name.Split('.')[1];

                var opParams = operation.parameters.Where(x => x.name == name);
                var parameters = opParams as Parameter[] ?? opParams.ToArray();

                if (parameters.Length > 0)
                {
                    operation.parameters.Remove(parameter);
                }
            }
        }
    }
}