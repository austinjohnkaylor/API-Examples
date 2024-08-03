namespace API.Examples.SharedResources.EntityFramework.ODataBasicCrud;

/// <summary>
/// 
/// </summary>
/// <remarks>Based off of https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/basic-crud?tabs=net60%2Cvisual-studio-2022%2Cvisual-studio%2Cvs2022</remarks>
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CustomerType CustomerType { get; set; }
    public decimal CreditLimit { get; set; }
    public DateTime CustomerSince { get; set; }
}