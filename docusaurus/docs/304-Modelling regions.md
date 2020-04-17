---
id: modelling-regions
title: Modelling regions
sidebar_label: Modelling regions
---

Dyndle supports regions out-of-the-box and also allows you to make customizations. Following DD4T metadata approach, regions can be used even if they are not supported in Tridion. In the case of Tridion 9 and later, regions can be specified both in the page schema and on the metadata of the component templates. However, if a page template has region schema and any component presentations are placed in it, then metadata values will be ignored.

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
There is a setting 'Dyndle.DefaultRegionView' in [configuration](configuration) that getermines which view is used to render region by default. `Html.RenderRegion` also has an overload that allows you to specify a viewname to be used to render the region.

## Special usages

Regions are also used to manage includes. You can configure a url to be used as an includes page. Then you can include the full page content via `Html.RenderIncludes` or render a specific region of the include page using `RenderIncludesRegion`. This allows you to controll site-wide regions, such as cookie opt-in, header and footer, in a centralized place. Include pages are also cached once, so they bring performance benefits.

There is also possibility to group component presentations in regions yourself. This can be done by adding properties to a page model, where component presentations are grouped. You can create and use RegionModel for this purpose, if you would like to reuse region rendering functionality.
