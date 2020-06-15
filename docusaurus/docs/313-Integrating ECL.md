---
id: integrating-ECL
title: Integrating External Content Libraries
description: Documentation for Dyndle
sidebar_label: Integrating ECL

---

Using external content libraries in a Dyndle webapplication is supported out of the box with the Dyndle Core module. It's default model for multimedia components can handle regular images uploaded through Tridion as well as images provided from an external application through an ECL. It also provides the external metadata from the ECL provider. Below is an example implementation of an image from Bynder, a digital asset management system, provided to Tridion with an ECL and used in a asp.net web application with Dyndle.

### Implementing

We have a model in our web app that was generated with the Dyndle Tools CLI. It contains an image field using the `MultimediaEntityModel` from Dyndle Core which looks like this.

```c#
[LinkedComponentField]
public virtual MultimediaEntityModel Image { get; set; }
```

It can be used generically in a view regardless of it being a regular image or an ECL provided image. The following code works for both.

```html
<img src="@Model.Image.Multimedia.Url" />
```

The `MultimediaEntityModel` also contains an `ExternalMetadata` field which contains a list of metadata properties that can also easily be used in a view like so.

```c#
@if (Model.Image.ExternalMetadata != null)
{
    <ul>
        @foreach (var item in Model.Image.ExternalMetadata)
        {
            <li>@item.Value.Name - @item.Value.Value</li>
        }
    </ul>
}
```

A simple null check prevents the metadata field from being read when it doesn't contain any values, like when using a regular image, after which the `ExternalMetadata` list can be iterated over to display the key and value of the items it contains.

### Advanced

The `MultimediaEntityModel` also exposes the method used to retrieve the external metadata field so it can be used to retrieve other fields from the raw model data that is provided by DD4T. It takes the name of the property to retrieve as its argument and returns an `IFieldSet` dictionary containing the keys and values of that property. It could for example be implemented in a custom model. All that's needed to use it is to inherit the model from the `MultimediaEntityModel` and to call the method with the required fieldname.

```c#
[ContentModel("MyModel")]
public partial class MyModel : MultimediaEntityModel
{
	public IFieldSet MyExtensiondata => GetExtensionDataField("Fieldname");
}
```

