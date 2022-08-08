using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CurrencyExchange.Api.Filters
{
	public class SwaggerHeader : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			if (operation.Parameters == null)
				operation.Parameters = new List<OpenApiParameter>();

			operation.Parameters.Add(new OpenApiParameter()
			{
				Name = "M-direct-client",
				In = ParameterLocation.Header,
				AllowEmptyValue = false,
				Schema = new OpenApiSchema { Type = "String" }
			});
		}
	}
}
