# Model State Validation
Shows how to implement Model State Validation in an ASP.NET Core Web API

<!-- TOC -->
* [Model State Validation](#model-state-validation)
  * [What is it?](#what-is-it)
    * [Model Binding](#model-binding)
    * [Model Validation](#model-validation)
  * [Why should I use it?](#why-should-i-use-it)
  * [Further Reading](#further-reading)
  * [Main Readme](#main-readme)
<!-- TOC -->

## What is it?
- `ModelState` is the field contained within `ControllerBase`. 
- It's type is [ModelStateDictionary](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.modelbinding.modelstatedictionary?view=aspnetcore-6.0) which represents errors that come from **model binding** and **model validation**
### Model Binding
- Retrieves data from various sources such as route data, form fields, and query strings.
- Provides the data to controllers and Razor pages in method parameters and public properties.
- Converts string data to .NET types.
- Updates properties of complex types.
- [More information about Model Binding](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-8.0)
### Model Validation
- Occurs after model binding and reports errors where data doesn't conform to business rules. 
- For example, a 0 is entered in a field that expects a rating between 1 and 5
- [More information about Model Validation](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-8.0)
## Why should I use it?
- It's a first check against incoming HTTP payloads for an ASP.NET Core Web API
- It allows you to validate incoming HTTP payloads against business logic to ensure the payloads conform with what you were expecting
- It's a first line of defense against passing bad data to an API call
## Further Reading
- [code-maze.com Model State Validation article](https://code-maze.com/aspnetcore-modelstate-validation-web-api/)
- [More information about Model Binding](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-8.0)
- [More information about Model Validation](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-8.0)
- [What is Model State?](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-8.0#model-state)
- [Custom Model Binding in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/mvc/advanced/custom-model-binding?view=aspnetcore-8.0)
## Main Readme
[Back to Solution README](../README.md)
