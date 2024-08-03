using API.Examples.SharedResources.EntityFramework.ODataBasicCrud;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace API.Examples.OData.SimpleCrudWebApi;

public static class CustomerEdmModelBuilder
{
    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnumType<CustomerType>();
        builder.EntitySet<Customer>("Customers");
        return builder.GetEdmModel();
    }
}