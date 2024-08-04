# OData
This solution and group of projects show to implement different things using ASP.NET Web API's, Entity Framework Core, and [OData](https://learn.microsoft.com/en-us/odata/).

# Projects
## SimpleCrudWebApi
This project is a simple Web API that uses Entity Framework Core to interact with a database. It has a single controller that allows for basic CRUD operations.
- The project follows the Microsoft Docs tutorial [Basic CRUD in ASP.NET Core OData 8](https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/basic-crud?tabs=net60%2Cvisual-studio-2022%2Cvisual-studio%2Cvs2022)

The controller uses OData to interact with the [SchoolSystemDbContext](../Shared%20Resources/EntityFramework/SchoolSystem/SchoolSystemDbContext.cs) and query information about [Students](../Shared%20Resources/EntityFramework/SchoolSystem/Student.cs).
# Further Reading
- [OData](https://learn.microsoft.com/en-us/odata/)
- [OData with ASP.NET Web API](https://learn.microsoft.com/en-us/odata/webapi)
- [OData with Entity Framework Core](https://learn.microsoft.com/en-us/odata/odata-v4-aspnet-core)
- [OData with ASP.NET Core](https://learn.microsoft.com/en-us/odata/odata-v4-web-api)

# Main Readme
[Back to Solution README](../README.md)