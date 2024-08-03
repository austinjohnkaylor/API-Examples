using Bogus;

namespace API.Examples.SharedResources.EntityFramework.ODataBasicCrud;

/// <summary>
/// 
/// </summary>
/// <remarks>Based off of https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/basic-crud?tabs=net60%2Cvisual-studio-2022%2Cvisual-studio%2Cvs2022</remarks>
public class Customer
{
    public int Id { get; init; }
    public string Name { get; init; }
    public CustomerType CustomerType { get; init; }
    public decimal CreditLimit { get; init; }
    public DateTime CustomerSince { get; init; }

    public static Faker<Customer> GetCustomerFaker()
    {
        return new Faker<Customer>()
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.Name, f => f.Name.FullName())
            .RuleFor(c => c.CustomerType, f => f.Random.Enum<CustomerType>())
            .RuleFor(c => c.CustomerSince, f => f.Date.Between(new DateTime(2020, 01, 01), DateTime.Now))
            .RuleFor(c => c.CreditLimit, f => f.Random.Int(1000, 10000));
    }
}