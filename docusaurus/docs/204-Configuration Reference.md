---
id: configuration
title: Configuring Dyndle
sidebar_label: Configuring Dyndle
---

Below you can find an overview of all configuration values that are used by each Dyndle module:

## Core

The following table describes the configuration values in the appsettings section of the web config that are used in the Dyndle Core module. Mandatory configuration should be added to the web.config when Dyndle.Core is installed to a web project.

### General

| Config key                    | Example Value             | Description                                                  |
| ----------------------------- | ------------------------- | ------------------------------------------------------------ |
| Dyndle.ViewModelNamespaces    | Dyndle.Modules, Acme.Core | Namespaces which contain viewmodel classes (multiple namespaces can be comma-separated) |
| Dyndle.ControllerNamespaces   | Dyndle.Modules, Acme.Web  | Namespaces which contain controller classes (multiple namespaces can be comma-separated) |
| Dyndle.DefaultRegionView      | AcmeRegion                | The name of the default view to use when Html.RenderRegion or Html.RenderRegions is called. Defaults to `Region`. |
| Dyndle.DefaultEntityTypeName  | AcmeEntity                | Full type name (including the namespace) of the default entity (which is returned if no matching entity model is found). Defaults to `Dyndle.Modules.Core.Models.Defaults.DefaultEntity` |
| Dyndle.DefaultWebPageTypeName | AcmePage                  | Full type name (including the namespace) of the default webpage (which is returned if no matching page model is found). Defaults to `Dyndle.Modules.Core.Models.Defaults.DefaultWebPage` |
| DD4T.IncludeFileExtensions    | true/false                | Defines whether urls should use file extensions.             |

### System pages

| Config key                 | Example Value                   | Description                                                                                                                 |
| -------------------------- | ------------------------------- | --------------------------------------------------------------------------------------------------------------------------- |
| Dyndle.SiteConfigUrl       | /system/site-configuration.html | Configuration key containing the path to the site configuration page                                                        |
| Dyndle.IncludesUrl         | /system/header.html             | Configuration key containing the path(s) to include pages                                                                   |
| Dyndle.IncludesUrlMask     | /system/includes/{0}.html       | Configuration key containing the url mask used in case you are calling Html.RenderIncludes with an include name             |
| Dyndle.DefaultIncludesView | AcmeIncludes                    | Configuration key containing the name of the default view to use when Html.RenderIncludes is called. Defaults to `Includes` |

### Caching and Binaries

| Config key                         | Example Value            | Description                                                                                                                                                                  |
| ---------------------------------- | ------------------------ | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Dyndle.EnableCache                 | true                     | Configuration key to enable the cache                                                                                                                                        |
| Dyndle.DisableOutputCachingForUrls | /search,/some/other/path | Configuration key containing the urls (or rather, paths) for which output caching is disabled. Multiple urls can be entered, separated by a comma                            |
| Dyndle.BinaryUrlPattern            | ^/Images/.\*             | Configuration key indicating the regular expression (pattern) to match all binary files. Matching URLs will be handled by the BinaryController instead of the PageController |
| Dyndle.BinaryCacheFolder           | /binarydata              | Configuration key containing the name of the folder where binaries are to be cached on the file system (default = binarydata)                                                |

### Error handling and redirects

| Config key                 | Example Value          | Description                                                                                                                           |
| -------------------------- | ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------- |
| Dyndle.EnableSectionErrors | true                   | Configuration key to indicate whether or not you want to see errors per region or entity (rather than one big error message per page) |
| Dyndle.ErrorPages.404      | /404.html              | 404 error page url                                                                                                                    |
| Dyndle.ErrorPages.500      | /505.html              | 500 error page url                                                                                                                    |
| Dyndle.EnableRedirects     | true                   | Configuration key to enable or disable redirect functionality                                                                         |
| Dyndle.RedirectsUrl        | /system/redirects.html | Configuration key containing the path to the Dyndle redirects page managed in Tridion                                                 |

### Experience Manager

| Config key             | Example Value        | Description                                                                                                                                                            |
| ---------------------- | -------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Dyndle.StagingSite     | true                 | Configuration key indicating whether or not we are dealing with a staging website (set to true or false, this will enable/disable the debugging and XPM functionality) |
| DD4T.IsPreview         | true                 | Configuration key indicating whether or not we are dealing with a preview website (set to true or false, this will enable/disable the XPM functionality)               |
| DD4T.ContentManagerUrl | https://cme.acme.com | Configuration key containing the url of the content manager (used for Experience Manager)                                                                              |

### Publication resolving

| Config key                                        | Example Value | Description                                                                                                             |
| ------------------------------------------------- | ------------- | ----------------------------------------------------------------------------------------------------------------------- |
| Dyndle.DirectorySegmentsUsedForPublicationMapping | 3             | The number of segments used to detect the publication we are in, based on the topology manager mapping. Defaults to `1` |
| Dyndle.PublicationBasePath                        |               | The base path of the publication (used only in combination with the DD4T.PublicationId appSetting)                      |



## Navigation

The following table describes the configuration values in the appsettings section of the web config that are used in the Dyndle Navigation module.

| Config key                          | Default Value             | Description                                                                                                                         |
| :---------------------------------- | :------------------------ | :---------------------------------------------------------------------------------------------------------------------------------- |
| Navigation.SourceUrl                | `/system/navigation.json` | Setting that determines the url of the navigation source page                                                                       |
| Navigation.BaseUrl                  |                           | Setting that determines the navigation base url with which the URLs of the pages are prefixed in the sitemap.xml                    |
| Navigation.SubNavDefaultsToMainNav  | `false`                   | If true, the subnavigation is read from the root for pages whose level falls below the start level                                  |
| Navigation.IncludeAllPagesInSitemap | `true`                    | Setting that determines whether to include all pages in sitemap. If set to false empty structuregroups are removed from the sitemap |

## Search

The following table describes the configuration values in the  appsettings section of the web config that are used in the Dyndle Image  Enhancement module.

| Config key                 | Example Value                              | Description                                                  |
| :------------------------- | :----------------------------------------- | :----------------------------------------------------------- |
| Search.BaseUrl             | `http://serverhostname:8984/solr/staging/` | Setting that specifies the url of the Solr endpoint.         |
| Search.BaseField           | `title,body`                               | The field or fields to be used in the search query. Multiple fields need to be comma separated. Defaults to `title` and `body` when not set. |
| Search.TimeoutMilliseconds | `5000`                                     | The timout in milliseconds for the http request to the Solr client. Defaults to 5000 if not set. |
| Search.ResponseItemModel   | `MyCustomResultItemModel`                  | Optional override for the model to be used for the result of the search query. Defaults to the generic `SearchResultItem` model included in the search module if not set. |
| Search.PageSize            | `10`                                       | The number of results to display per page. Defaults to 10 per page if not set. |
| Search.GroupingPageSize    | `5`                                        | The number of results to display per group when the search query contained a group by statement. |

## Image Enhancement

The following table describes the configuration values in the appsettings section of the web config that are used in the Dyndle Image Enhancement module.

| Config key                       | Example Value     | Description                                                  |
| -------------------------------- | ----------------- | ------------------------------------------------------------ |
| ImageEnhancement.Localpath       | `/EnhancedImages` | Where the enhanced versions of the images will be stored (defaults to /EnhancedImages) |
| ImageEnhancement.Backgroundcolor | `#fff`            | The color that is used when you crop an image and provide a width and height that's higher than the cropped area. |
| ImageEnhancement.Cacheseconds    | `300`             | How long the enhanced image will be retrieved from the cache before creating a new enhanced version. (defaults to 300) |

Note that the caching of enhanced images is independent from the normal DD4T / Dyndle caching. It is NOT possible to automatically delete enhanced images when the parent image is unpublished or republished (decaching). It is generally advisable to keep the value low.
