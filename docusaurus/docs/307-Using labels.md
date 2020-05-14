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

To create a label, simply create a component based on the "Label" schema. Provide a Title, which will be used as a key to retrieve the label with the HTML helper, and a Label Text, the text the label will display. These components are typically stored in a "system" folder. Localize the component in a child publication to provide a translated value for the label for that publication's website.

The created label components need to be published to be able to access them in the web application. This is done behind the scenes through the "Dyndle Site Configuration" page template. A page using this page template has been created by the Dyndle CLI when you installed Dyndle in the content manager. It is also named "Dyndle Site Configuration" and can be found in the "system" structure group. When you publish this page, the components using the label schema will automatically be published with it.

## Using labels

The labels can now be used in your views in the following fashion:

`@Html.GetLabel("Label Key", "Default Value")`

The "Label Key" should match the "Title" field that was set in the component. The "Default Value" is an optional parameter that can be used as a fallback value when the "Label Text" value matching the title could not be found.

Labels can also be used to automatically generate a list of tcm uris for a set of Tridion items. This can be used to reference a specific item in Tridion without having a hardcoded tcm uri in the code. The reference for instance can be set to the root element name or the title of a schema. It will then be translated to its tcm uri on runtime. It's also possible to get category ids by its title or xml name or getting a publication's metadata.

These additional functionalities can be added directly to the Dyndle Site Configuration page template and don't require any label components. They come in the form of "Template Building Blocks" which have been installed in Tridion as part of the Dyndle quickstart. To add these building blocks to the Dyndle Site Configuration page template, use the Tridion Template Builder. There you can open the template and drag an drop template building blocks to add it to the template. For a more detailed guide on using the template builder refer to the [template builder documentation](https://docs.sdl.com/LiveContent/content/en-US/SDL%20Web-v5/GUID-FD25A36E-4B1C-4346-BB7E-919B293B8748).

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
