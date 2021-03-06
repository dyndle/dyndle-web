---
id: search
title: Dyndle Search
description: Documentation for Dyndle
sidebar_label: Dyndle Search
---

## Prerequisites

### Solr

The Dyndle search module is built to be used with the Solr search platform. To be able to use it, an installation of Solr is needed on the content management server. The link below contains detailed instructions on the installation and configuration of Solr.

 [Apache Solr Quickstart](https://lucene.apache.org/solr/guide/8_5/solr-tutorial.html)

### SI4T

The Search Integration 4 Tridion project is used to index Tridion pages in the Solr platform. It provides a set of template building blocks to be used on any page that you want to index in search. The link below contains instructions for installing and using SI4T.

[SI4T Github Wiki](https://github.com/SI4T/SI4T/wiki)

## Install

Add a reference to the NuGet package: Dyndle.Modules.Search. Then provide the module with the endpoint of the Solr client. This can be done using the `BaseUrl` config setting in the web config. Refer to the [Configuring Dyndle](configuration.html) chapter for an example value and all the other settings that can be used for customization of the functionality of the search module.

## Using the SI4T TBBs

After having installed the TBBs from SI4T you can use them on the page templates of pages that you would want to be searchable. This will provide Solr with the indexing data of a page when you publish it using that page template. You can add a new TBB to a page template using the Tridion Sites Template Builder. For a detailed guide on using the template builder refer to the [template builder documentation](https://docs.sdl.com/LiveContent/content/en-US/SDL%20Web-v5/GUID-FD25A36E-4B1C-4346-BB7E-919B293B8748). You will also need to modify the Tridion deployer to handle the TBBs.  Instructions on how to do this can be found [here](https://github.com/SI4T/Solr/wiki/SI4T-Solr-Configuration-101).

## Html helper

The Dyndle search module provides an Html helper class that can be used to retrieve search results in dynamic locations in your website. This way you don't have to create a page with a search component in Tridion. There are two helper functions you can use:

`@Html.Search(query)`

This function takes a query and returns a `SearchResults` object. You can then render the results manually.

`@Html.RenderSearchResults(viewName, query)`

This function also takes a `viewname`, which can be used to directly render the result of the query in the specified view. This way you can reuse the view in other places in your webapp.

### Example query

Below is an example query that will return a total of all indexed documents and will return 10 documents as objects.


```c#
@using Dyndle.Modules.Search.Models

var searchQuery = new SearchQuery()
{
    Query = "*:*",
    PageSize = 10
};
```

## Custom search result model

The Search method returns an object of the type `Dyndle.Modules.Search.Models.SearchResults`. This class contains a property called Items, which is a list of the type `Dyndle.Modules.Search.Contracts.ISearchResultItem`. This interface captures all the fields in the SOLR document. Out of the box, Dyndle will populate the Items property with instances of `Dyndle.Modules.Search.Models.SearchResultItem`, so you have access to the entire SOLR document.

However, a common scenario is to enhance the SOLR document with extra fields. You can do this by configuring the SI4T template building blocks or by writing custom ones, as explained in the [SI4T documentation](https://github.com/SI4T/SI4T/wiki/Configuring-Templates-:-What-Gets-Indexed%3F). 
If you do this, you need to map the extra fields to the SearchResultItem.
You can do this by creating your own class and configure Dyndle to use that. Your class must implement `Dyndle.Modules.Search.Contracts.ISearchResultItem` (preferably by extending `Dyndle.Modules.Search.Models.SearchResultItem`). You can add the extra fields to this class.

Next, you need to tell Dyndle to use this custom model instead of the default. You can do this in the Web.config, by configuring the model class name with the`Search.ResponseItemModel` appSetting, and the name of the assembly it is located in using the `Search.Assembly` appSetting. The search result will then try to bind the returned items to your own model. 


## Adding a search results page

To view the results of a search query on your website, you will need to create a page that points to a controller that retrieves the data from Solr. There are two ways to achieve this, using the built in controller from the search module or using your own custom controller.

### Search module controller

The Dyndle search module provides a controller out of the box to retrieve data from Solr. All you need to do is create a page in Tridion with a component on it which specifies the routing to that controller. Provide the following values for the page template:

| Field      | Value  |
| ---------- | ------ |
| area       | Search |
| controller | Search |
| action     | List   |

You also need to provide a value for the `view` field. Create a view that uses `Dyndle.Modules.Search.Models.SearchResults` as a model and set the name of the view in this field. You can then render the results of the query in this view.

### Custom controller

It is also possible to use your own controller for handling the search request. This way you can add custom logic to the search request. To do this you need to provide the routing to your controller and action in the `Modules Metadata Template`. Set the controller and the action to the names you used and set the area to the area name registered in the `CoreAreaRegistration` class. Below is an example controller to get started with.

```c#
public class MySearchController : ModuleControllerBase
    {
        private readonly IContentProvider _contentProvider;

        public MySearchController(IContentProvider contentProvider, ILogger logger) : base(contentProvider, logger)
        {
            this._contentProvider = contentProvider;
        }

        [ChildActionOnly]
        public ActionResult GetSearchResults(DyndleSearchResults entity)
        {
            var searchLinkResolver = DependencyResolver.Current.GetService<ISearchLinkResolver>();
            return View("GetSearchResults", entity);
        }
    }
```
