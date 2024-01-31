# Uri Versioning
Shows how to implement API versioning via the URI.

<!-- TOC -->
* [Uri Versioning](#uri-versioning)
  * [What is it?](#what-is-it)
  * [Why should I use it?](#why-should-i-use-it)
  * [Further Reading](#further-reading)
  * [Main Readme](#main-readme)
<!-- TOC -->

## What is it?
Uri versioning is a way to version your web api via the Url.
A real-life example of this would be:

Version 1 
`GET https://yourwebapi/api/v1/someresource`
</br>
Version 2
`GET https://yourwebapi/api/v2/someresource`
</br>
Version 3
`GET https://yourwebapi/api/v3/someresource`
</br>
Version N (where N equals the latest version of your API) </br>
`GET https://yourwebapi/api/vN/someresource`

## Why should I use it?
Versioning enables existing client applications to continue functioning unchanged while allowing new client applications to take advantage of new features and resources

## Further Reading
- [C-Sharp Corner API Versioning Article](https://www.c-sharpcorner.com/article/implementing-versioning-in-asp-net-core-webapi/#:~:text=In%20your%20controller%20classes%2C%20apply%20the%20%5BApiVersion%5D%20attribute,ControllerBase%20%7B%20%2F%2F%20Controller%20actions%20and%20endpoints%20%7D)
- [Microsoft's API Best Practices | API Versioning](https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-design#versioning-a-restful-web-api)
- [Postman's Article on API Versioning](https://www.postman.com/api-platform/api-versioning/)

## Main Readme
[Back to Solution README](../README.md)