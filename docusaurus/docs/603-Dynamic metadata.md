---
id: dynamic-metadata
title: Dynamic metadata
description: Documentation for Dyndle
sidebar_label: Dynamic metadata
---

The Dyndle install package adds a couple template building blocks that can dynamically add certain useful properties to the metadata of a component or page when rendered. The functionality of these dynamic metadata building blocks are described here. All of these building blocks can be added to a component or page  template with the `Tridion Sites Template Builder`. Detailed instructions on how to use the template builder can be found [here.](https://docs.sdl.com/LiveContent/content/en-US/SDL%20Web-v5/GUID-FD25A36E-4B1C-4346-BB7E-919B293B8748) 

### Include original page title as metadata

When this building block is added to a page and the page is rendered, the title of the page in the original publication where it was first created will be added to the page metadata. The name of the metadata field to use for the page title is configurable through the building block parameters and its default is `originalPageTitle`.

### Add owning publication as component/page metadata

Add this building block to a page or component template and the id of the owning publication, the highest publication in the blueprinting hierarchy, will be added to a metadatafield when rendered. The fieldname to use for the metadata property is configured in the building block's parameters and is set to `owningPublicationId` by default.

### Add translated status component metadata

This building block will add a field to the metadata of a component that indicates if that component was translated or not. It does this by checking for a `language` metadata field and compares the value to the value of the owning publication's language. If they do not match the metadata field is set to `translated` and otherwise to `original`. The field to use to store the translation status is configurable through the building block parameters and its default is `translationStatus`.

### Copy field from component to page

Use this building block on a page to copy a certain field from the components it contains to a metadata field on the page itself. You have to specify which field to copy from the components and the name of the metadata field on the page to copy it to in the parameters of the building block. There is also a parameter that can be used to indicate whether only the first component on the page will have its field copied or all of the components.