# Search Module

## Overview

This module contains generic search functionality. The module consists of the following elements:

* An **ISearchProvider** interface for executing search queries
* An **IResultMapper** interface for mapping result data to models
* Generic Implementations of **ISearchProvider** and **IResultMapper** for Findwise (a Solr wrapper)
* Interfaces and Generic Models for **Queries, Results and Filters**
* A **SearchController** and **SearchResultsModelBinder** to execute search from MVC applications
* A **SearchQueryBinder** for binding a **SearchQuery** object from the request parameters 
* Example views to demonstrate rendering the results, filters and pagination links

## Provider & ResultMapper

The search implementation is abstracted through the ISearchProvider interface, an implementation of which is provided with the **SCA.Modules.Search.Providers.Findwise.SearchProvider** class. This wraps HTTP requests to the Findwise Web Service, which returns search results in JSON format. 

Although the basic structure of search results is the same no matter what the query, the JSON format of each actual search result (web pages, products etc.) can vary so mapping is required to put this JSON data into .NET objects. 

### Mapping from search index

Although the search index contains many fields, we only map the following (you can see this in the SearchResult models)

Web Pages:

* url
* body (first 200 characters)
* pagetitle

Products:

* AssetUrl
* productdescription
* ProductName
* FamilyURL
* ProductURL

If required, more fields can be added to the models using JsonProperty attributes to map them.

### Labels

The existing labels have been reused for displaying text in the results;
ChooseCategory, All, Showing, of, SearchResults

New labels were added for NoResult and SearchError (for showing messages when there are no results found, or an error happens when searching)

### Changes to current implementation

1. Query parsing. There was some logic in the controls to parse the query into name/value pairs if it contained an ampersand. There is no text to indicate this is possible in the search page, or to indicate what fields you can search, so no-one would ever be able to use this. 

1. Filters and result processing. Previously when a filter was selected the data shown would often not make sense (the result count is the count without filters, paging would not work, and the number of results didn't match the actual results shown). This was due to some hard to understand post-processing on the result set. To simplify things (and make them actually work) how only show the filtered result count; the correct items are shown and the paging works. The only functional change is that we do not show the currently applied filter as an option in the list of possible filters.

These changes are done to ensure clean, correctly working code and reduce technical debt.

                