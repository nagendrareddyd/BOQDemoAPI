using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace BOQ.API.Configuration
{
    /// <summary>
    /// Swagger security document filter
    /// </summary>
    public class SwaggerSecurityDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="document"></param>
        /// <param name="context"></param>
        public void Apply(SwaggerDocument document, DocumentFilterContext context)
        {
            document.Security = new List<IDictionary<string, IEnumerable<string>>>()
            {
                new Dictionary<string, IEnumerable<string>>()
                {
                    { "Bearer", new string[]{ } },
                    { "Basic", new string[]{ } },
                }
            };
        }
    }
}
