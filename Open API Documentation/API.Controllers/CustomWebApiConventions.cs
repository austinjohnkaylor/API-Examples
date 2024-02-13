using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace API.Controllers;

/// <summary>
/// Contains a set of conventions for the API controllers
/// </summary>
/// <remarks>https://learn.microsoft.com/en-us/aspnet/core/web-api/advanced/conventions?view=aspnetcore-8.0#create-web-api-conventions</remarks>
public static class CustomWebApiConventions
{
    /*
     * The following conventions are applied to the API controllers:
     * - The convention method applies to any action named Find.
     * - A parameter named id is present on the Find action.
     */
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static void Find(int id)
    {
    }
}