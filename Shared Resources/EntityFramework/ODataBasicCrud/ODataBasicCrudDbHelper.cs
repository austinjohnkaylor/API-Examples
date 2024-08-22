namespace API.Examples.SharedResources.EntityFramework.ODataBasicCrud;

/// <summary>
/// 
/// </summary>
/// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/basic-crud?tabs=net60%2Cvisual-studio-2022%2Cvisual-studio%2Cvs2022#add-a-database-context</remarks>
public static class ODataBasicCrudDbHelper
{
    public static void PopulateDatabase(ODataBasicCrudDbContext db, int customerCount = 1000)
    {
        db.Database.EnsureCreated();
        
        if (db.Customers.Any())
            return;

        var customerFaker = Customer.GetCustomerFaker();
        var customers = customerFaker.Generate(customerCount);

        db.Customers.AddRange(customers);
        db.SaveChanges();
    }
}