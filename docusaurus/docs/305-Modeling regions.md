---
id: modeling-regions
title: Modeling regions
description: Documentation for Dyndle
sidebar_label: Modeling regions
---

A region, in Tridion terms, is a group of component presentations, which should be shown in a particular area on the page. 
Regions were introduced in Tridion 9.0. Dyndle allows you to add regions to your page model and render them with a RegionController and a region view.

But even if you are not yet on Tridion 9, you can already make use of this functionality. Dyndle will group the component presentations on the page for you, based on the **region** metadata field in the component templates. However, if a page has a region schema and any component presentations are placed in it, then the component template metadata will be ignored.

In Dyndle there are multiple ways to get and render regions. These different approaches can be used depending on the use case. Let us take a closer look:

## Region Models

If you have used Dyndle CLI to generate models, you have seen that page models have one property that stores all Entities in a flat list and another property with a list of regions:

```c#
///<summary>
/// Class is auto-generated from Tridion page template Default Page Template (tcm:5-154-128)
/// Date: 3/30/2020 12:05:02 PM
/// </summary>
[PageViewModel(TemplateTitle = "Default Page Template")]
public partial class DefaultPageTemplate : WebPage
{
    [PageTitle]
    public virtual string PageTitle { get; set; }
    [PageId]
    public virtual TcmUri PageId { get; set; }
    [Regions]
    public virtual List<IRegionModel> Regions { get; set; }
    [ComponentPresentations]
    public virtual List<IEntityModel> Entities { get; set; }
}
```

`Regions` property gives us access to a list of regions, based on either regions specified on the page in Tridion, or on component template metadata. As described above, Tridion regions overrule metadata.

If metadata values are used, there is an important note: if multiple component presentations are placed in a region, but order of these component presentations on the page is not sequential: there will be multiple region models with the same name. For example:

| Component Presentation | Region |
| ---------------------- | ------ |
| Article                | Main   |
| Side banner            | Side   |
| Related articles       | Main   |
| Related products       | Main   |

will result in three region models:

| Region | Component Presentations            |
| ------ | ---------------------------------- |
| Main   | Article                            |
| Side   | Side banner                        |
| Main   | Related articles, Related products |

## Rendering

Rendering of a region normally means sequential rendering of all entities in it, but this can also be further customized by using more advanced logic and/or layout. For instance, to render the whole footer region with proper HTML markup and some business logic.

Rendering of the regions can be done using HTML helpers `Html.RenderRegion(region)` and `Html.RenderRegions(regions)`.
There is a setting 'Dyndle.DefaultRegionView' in [configuration](configuration.html) that determines which view is used to render a region by default. `Html.RenderRegion` also has an overload that allows you to specify a view name to be used to render the region.

## Regions and includes

Regions can also be used to manage includes, which are reusable pages that can be inserted in any view in your web application. To read more about includes refer to <a href="include-pages.html">Including content from other pages</a>.

## Grouping component presentations without using regions
If you don't want to use regions at all, you can group component presentations in different ways. One approach is to use the annotations that are available in DD4T:

```c#
[PresentationsByView(ViewPrefix = "CTA")]
public List<IEntityModel> CallsToAction { get; set; }
```

This will put all the component presentations with a view name that starts with 'CTA' in a property called CallsToAction. You could use this to write out all calls to action in one block on the page.

But you can also take full control of the matter easily. Just add a property 'AllEntities' to your page model, and create various subsets yourself, using Linq and so-called 'Expression-bodied properties'. For example:

```c#
[ComponentPresentations]
public virtual List<IEntityModel> AllEntities { get; set; }

// The Banner
public Banner Banner => AllEntities.FirstOrDefault(e => e is Banner) as Banner;


public IEnumerable<IEntityModel> Articles => AllEntities.Where(e => e is Article);
```

In this example, the first entity on the page which is of the type Banner, will be available in a separate property called *Banner*. All entities of the type Article are available through the property *Articles*. Since the type of an entity is determined by the schema of the component, you are effectively grouping component presentations by schema.

But you can also group them by view, like this:

```c#
public IEnumerable<IEntityModel> Summaries => Entities.Where(e => e.MvcData.View == "summary");
```


