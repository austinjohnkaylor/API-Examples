using System.Collections.Specialized;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.AspNetCore.OData.Query;

namespace API.Examples.SharedResources.OData.CustomSkipTokenHandler;

/*
 * What does a SkipTokenHandler do?
 *     - The skip token handler generates the next link in an OData response when using server-driven paging.
 *     - It's also responsible for applying the $skiptoken to the results.
 *     - SkipTokenHandler is an abstract class that defines the following methods to be implemented by child classes:
 *             - GenerateNextPageLink:
 *                   generates the Uri that's used as the next link in the response.
 *             - ApplyTo:
 *                   applies the $skiptoken to an IQueryable collection. This should transform the query to perform the paging.
 *                   It has two overloads, a generic ApplyTo<T> and non-generic ApplyTo to handle the generic IQueryable<T> and non-generic IQueryable respectively.
 */
/// <summary>
/// 
/// </summary>
public class CustomSkipTokenHandler : DefaultSkipTokenHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseUri">This is the request URL. This is the URL we want to add a $skiptoken to. It's possible that it contains the $skiptoken from the previous page</param>
    /// <param name="pageSize">The maximum number of items per page. This is the same value that was passed to the PageSize property of the EnableQuery attribute</param>
    /// <param name="instance">The entity that is used to generate the skip token. It corresponds to the last item in the previous page</param>
    /// <param name="context">Allows you to access information about the current serialization pipeline, such as the request instance, the EDM model, etc</param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/custom-skiptokenhandler?tabs=net60%2Cvisual-studio#generating-custom-skip-tokens</remarks>
    public override Uri GenerateNextPageLink(
        Uri baseUri,
        int pageSize,
        object instance,
        ODataSerializerContext context)
    {
        // The method first calls the GenerateNextPageLink method of the parent class (DefaultSkipTokenHandler) to generate the next link. 
        Uri? uri = base.GenerateNextPageLink(baseUri, pageSize, instance, context);
        // The method returns null if we're on the last page
        if (uri is null)
        {
            return null;
        }

        // Extracts the value of the $skiptoken query option from the generated URI.
        // We use the HttpUtility class from the System.Web namespace to parse the query options
        NameValueCollection queryOptions = HttpUtility.ParseQueryString(uri.Query);
        string skipToken = queryOptions.Get("$skiptoken");
        if (skipToken == null)
        {
            return uri;
        }

        // The next block of code generates a base64-encoded version of the skip token to replace the original value
        string base64SkipToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(skipToken));
        queryOptions.Set("$skiptoken", base64SkipToken);

        // Finally, we create a new URI by replacing the original query options with the updated version:
        var encodedQuery = queryOptions.ToString();
        UriBuilder builder = new(uri)
        {
            Query = encodedQuery
        };
        return builder.Uri;
    }
}