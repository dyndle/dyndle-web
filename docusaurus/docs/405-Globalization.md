---
id: globalization
title: Globalization
description: Documentation for Dyndle
sidebar_label: Globalization
---

The Globalization module is an extension to Dyndle that can be used to retrieve useful information about all the publications in your Tridion environment through easy to use helper methods in a Dyndle web application. The information that is available through this module is for example the url of a publication. This can for instance be used to easily create a language selector dropdown for your Dyndle website that will redirect users to the url for the selected language`s website.

To install this module simply install the `Dyndle.Modules.Globalization` nuget package available through the nuget.org package library.

## Using the helper methods

The helper methods for the globalization module can be used by adding a `@using Dyndle.Modules.Globalization` statement at the top of your views, or by adding `Dyndle.Modules.Globalization` as a namespace in your views web config. Information from your publications can then be retrieved with the `Html.Publications()` statement as demonstrated below.

```html
<table>
	<tr>
        <td>Publication metadata</td>
        <td>
            <ul>
                @foreach (var publicationMeta in Html.Publications())
                {
                    <li>Title: @publicationMeta.Title</li>
                    <li>ID: @publicationMeta.Id</li>
                    <li>Key: @publicationMeta.Key</li>
                    <li>MultimediaPath: @publicationMeta.MultimediaPath</li>
                    <li>MultimediaUrl: @publicationMeta.MultimediaUrl</li>
                    <li>PublicationPath: @publicationMeta.PublicationPath</li>
                    <li>PublicationUrl: @publicationMeta.PublicationUrl</li>
                }
            </ul>
        </td>
    </tr>
</table>
```



## Publication Custom Metadata

The globalization module can also be used to retrieve custom user defined metadata from Tridion. The Dyndle installer will have added a template building block named `PublicationCustomMeta` which is added to a page template named `PublicationMeta`. This page template can be used to generate a page which contains the custom metadata for all the publications in your Tridion environment.  Custom metadata can be added to a publication by accessing the publication properties and selecting a metadata schema. You can create your own schema and select it here, the only requirement is that it must be an embeddable schema. Now, when the page template is used on a page the values entered in the custom metadata will be available to the globalization module when it is published.

The Globalization module needs to be configured to know which page is used for the custom metadata. Simply provide the url of the page in the following app setting and the custom metadata will be made available as part of the same html helper method as the regular publication metadata. 

`<add key="Globalization.SourceUrl" value="/system/publication-meta.html" />`

In this example the page that used the `PublicationMeta` template was stored in a structure group named `system` with a page named `publication-meta`. 

In the following code example the use of the html helper method to retrieve the custom publication metadata is demonstrated. 

```html
<table>
	<tr>
        <td>Custom publication metadata</td>
        <td>
            <ul>
                @foreach (var publicationMeta in Html.Publications())
                {
                    if (publicationMeta.CustomMeta != null)
                    {
                        <li>Custom meta:
                            <ul>
                                @foreach (var item in publicationMeta.CustomMeta)
                                {
                                	<li>@item.Key - @item.Value</li>
                                }
                            </ul>
                        </li>
                    }
                }
            </ul>
        </td>
    </tr>
</table>
```

The `CustomMeta` property of the publications returned by the `Html.Publications()` method will contain a list of key value pairs where the key is the name of the field that was defined in the custom metadata schema assigned to the publication properties and the value contains the value set for that field. Custom logic for the values of specific fields can be easily added by matching the name of the field to the key of the custom meta property.