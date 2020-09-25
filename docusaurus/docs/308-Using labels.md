---
id: labels
title: Using labels
description: Documentation for Dyndle
sidebar_label: Using labels
---

## What are labels?

Labels provide developers with a way to easily insert short descriptive pieces of text into their website, they can be reused where ever desired through the use of a HTML helper provided by Dyndle. Labels can also be translated using blueprinting in Tridion.

## Configuring labels

To be able to use labels in your web application, they need to be configured in Tridion. As part of the Dyndle quickstart, the necessary templates and schemas will have already been created. They are stored in the "Building Blocks/Templates/Dyndle" folder.

To create a label, simply create a component based on the "Label" schema. Provide a Title, which will be used as a key to retrieve the label with the HTML helper, and a Label Text, the text the label will display. These components are typically stored in a "system" folder. Localize the component in a child publication to provide a translated value for the label for that publication's website. You can also choose to configure the use of labels in a different manner. For instance, you could use the title of the component as the label key instead of a field. A table with all of the different configuration options can be found in the label settings section on this page. 

The created label components need to be published to be able to access them in the web application. This is done behind the scenes through the "Dyndle Site Configuration" page template. A page using this page template has been created by the Dyndle CLI when you installed Dyndle in the content manager. It is also named "Dyndle Site Configuration" and can be found in the "system" structure group. When you publish this page, the components using the label schema will automatically be published with it.

## Using labels

The labels can now be used in your views in the following fashion:

`@Html.GetLabel("Label Key", "Default Value")`

The "Label Key" should match the "Title" field that was set in the component. The "Default Value" is an optional parameter that can be used as a fallback value when the "Label Text" value matching the title could not be found.

Labels can also be used to automatically generate a list of tcm uris for a set of Tridion items. This can be used to reference a specific item in Tridion without having a hardcoded tcm uri in the code. The reference for instance can be set to the root element name or the title of a schema. It will then be translated to its tcm uri on runtime. It's also possible to get category ids by its title or xml name or getting a publication's metadata.

These additional functionalities can be added directly to the Dyndle Site Configuration page template and don't require any label components. They come in the form of "Template Building Blocks" which have been installed in Tridion as part of the Dyndle quickstart. To add these building blocks to the Dyndle Site Configuration page template, use the Tridion Template Builder. There you can open the template and drag an drop template building blocks to add it to the template. For a more detailed guide on using the template builder refer to the [template builder documentation](https://docs.sdl.com/LiveContent/content/en-US/SDL%20Web-v5/GUID-FD25A36E-4B1C-4346-BB7E-919B293B8748). Refer to the advanced section at the bottom of this page for a detailed breakdown of all of the different label options and configurations.

Using labels from a controller or a model can be done by injecting ISiteContext, which is part of Dyndle Core, through dependency injection. In a controller sitecontext can be injected in the constructor and can then be used to call the GetLabel method as follows:

```c#
public class YourController : Controller
{
    private readonly ISiteContext _siteContext;

    public YourController(ISiteContext siteContext)
    {
        _siteContext = siteContext;
    }

    public ActionResult Index()
    {
        var label = _siteContext.GetLabel("Label Key", "Default Value");
    }
}
```

In a model the dependency resolver from .NET MVC can be used to get an instance of the class like this:

`DependencyResolver.Current.GetService<ISiteContext>().GetLabel("Label Key", "Default Value");`

## Label settings

The following template building block parameters can be used to configure the way the labels are used.

`Name of the INPUT field that contains the label value` (defaults to "value")
`Name of the INPUT field that contains the label key` (if empty, the key is assumed to be the title of the component)
`Name of the INPUT field that contains embedded labels` (if it has a value, the embedded fields are assumed to have a key/value pair)

This allows 5 different scenarios:

1. Label components have text fields which are used as value (with the field name as the key).
2. Label components have only a value text field, the component title is the key.
3. Label components have a key and a value text field.
4. Label components have a multiple value embedded field with key/value pairs (both text fields).
5. Label components have a multiple value embedded field with a key text field and a value component link field, which links to a label component of scenario one.

## Advanced

This section describes the different label mappings packaged with Dyndle. To use these template building blocks, add them to the site configuration page and the list of mappings will be generated when you publish the page. The building block will use the publication it was published from to filter the list of items to the ones contained in that publication.

| Building block                         | Function                                                     |
| -------------------------------------- | ------------------------------------------------------------ |
| Schema root element name to schema URI | Will create a list of schemas that are indexed by the **root element name** and will return the schema's **tcm uri**. |
| Schema title to schema URI             | Will create a list of schemas that are indexed by their **title** and will return the schema's **tcm uri**. |
| Category title to category id          | Creates a list of categories indexed by their **title** and returns the category's **id**. |
| Category xml name to category id       | Creates a list of categories indexed by their **xml name** and returns the category's **id**. |
| Category xml name to category title    | Creates a list of categories indexed by their **xml name** and returns the category's **title**. |
| Publication metadata                   | Will return the publication metadata using the fieldnames as the keys. |
| View name to template URI              | Will create a list of templates with the **view names** as the index and returns the template's **tcm uri**. |

These mappings can be used through the htmlhelper or through the sitecontext.

 `_siteContext.GetLabel("Label Key", "Default Value")`

`@Html.GetLabel("Label Key", "Default Value")`