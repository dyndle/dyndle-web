---
id: dynamic-content
title: Querying dynamic content
sidebar_label: Querying dynamic content
---

To set up a Dyndle web application, the project must be set up with a Dyndle Provider NuGet package, that is specific to the Tridion version that is used. This package provides ContentQueryService and TaxonomyService that can be used to fetch desired content dynamically from Tridion.

## Benefits

Here are some of the benefits of using Dyndle providers as opposed to DD4T or Tridion CD API:

- Simple access to object instances via Dependency Injection
- Built-in caching layer with queries stored in separate configurable regions
- Ability to use strongly-typed Entity models as a return type
- Utilize Dyndle labels to retrieve cached mapping between schema root element names and schema IDs

## Usage

To get started, simply inject IContentQueryService in a class where you want to query dynamic content. You may supply to Criteria and simply get all Dynamic Component Presentations from which the target view model can be built:

```c#
public class NewsController : Controller
{
    private readonly IContentQueryService _contentQueryService;

    public NewsController(IContentQueryService contentQueryService)
    {
        _contentQueryService = contentQueryService;
    }
    public ActionResult All(int pageNumber = 0, int pageSize = 10)
    {
        var allNewsItems = _contentQueryService.Query<NewsItem>(pageNumber * pageSize, pageSize);

        return View("AllNews", allNewsItems);
    }
}
```

You may also construct a QueryCriteria that will contain custom sorting or a search query based on metadata, taxonomy category or keyword:

```c#
private static readonly QueryCriteria _premiumArticleCriteria = new QueryCriteria
{
    MetaSearches = new List<QueryCriteria.MetaSearch>
    {
        new QueryCriteria.MetaSearch
        {
            FieldName = "isPremium",
            FieldValue = "Yes"
        }
    },
    SortParameters = new List<SortParameter>
    {
        new SortParameter(SortColumn.ItemLastPublishedDate, SortDirection.Descending)
    }
};

public ActionResult Premium(int pageNumber = 0, int pageSize = 10)
{
    var premiumNewsItems = _contentQueryService.Query<NewsItem>(pageNumber * pageSize, pageSize, _premiumArticleCriteria);

    return View("PremiumNews", premiumNewsItems);
}
```
