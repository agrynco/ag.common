using System;
using Domain;
using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;

namespace Web.API.Helpers
{
    public class ODataModelBuilder
    {
        public IEdmModel GetEdmModel(IServiceProvider serviceProvider)
        {
            var builder = new ODataConventionModelBuilder(serviceProvider);

            builder.EntitySet<User>(nameof(User))
                .EntityType
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();
            builder.EntityType<User>().Filter("Id");

            return builder.GetEdmModel();
        }
    }
}