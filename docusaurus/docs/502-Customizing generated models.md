---
id: customizing-models
title: Customizing generated models
description: Documentation for Dyndle
sidebar_label: Customizing generated models
---

In [previous] chapter we describe how Dyndle CLI can be run. One of the available commands creates a NuGet package with auto-generated view models. In this chapter we discuss how you can customize the models and how to maintain your solution to continiously benefit from model auto-generation.

## Extensibility

If you inspect generated models closely, you can see that the generated classes are declared `partial` and properties are `virtual`. Both of these keywords allow for the extensibility wihout modifying the partial class itself (open/closed principle). Let us have a closer look at an example:

```c#
///<summary>
/// Class is auto-generated from Tridion schema Event (tcm:5-216-8)
/// Date: 3/30/2020 12:05:02 PM
/// </summary>
[ContentModel("Event", true)]
public partial class Event : EntityModel
{
    [TextField]
    public virtual string EventTitle { get; set; }
    [TextField]
    public virtual string Location { get; set; }
    [DateField]
    public virtual DateTime Date { get; set; }
    [KeywordTitleField]
    public virtual string EventType { get; set; }
    [TextField]
    public virtual string Description { get; set; }
    [DateField(IsMetadata = true)]
    public virtual DateTime ReviewDate { get; set; }
}
```

It is likely that some of these models need to be modified. For instance, Tridion keyword field can be modelled in multiple ways, namely via: `KeywordId`, `KeywordTitleField`, `KeywordKeyField`, `KeywordField`, `NumericKeywordKeyField`, `RawKeywordFieldAttribute`. Another common reason is to added some business logic that belongs to the model. Below is an example of a partial class that we maintain in the same project and namespace and where we make changes.

```c#
public partial class Event
{
    /// <summary>
    /// Numeric code value for event type classification used in search
    /// </summary>
    [NumericKeywordKeyField]
    public double EventTypeCode { get; set; }
    /// <summary>
    /// Indicates whether an event is expired and should no longer be displayed in search
    /// </summary>
    public bool IsExpired => ReviewDate < DateTime.Now;
}

```

**By preserving the generated model as is, we can always regenerate models later.** If schemas were updated or new schemas were introduced, view models can be regenerated and customized logic will be preserved.

> **IMPORTANT NOTE**:
>
> If you modify partial classes generated by Dyndle CLI, all your changes will be overwritten during the next generation process. This may result in compile and runtime errors and a lengthy git diff session to merge the changes.

By complying to the approach of **not** modifying generated classes and instead utilizing partial classes, you get the benefit for being able to continuously keep the view models up to date with Tridion schemas. Moreover, managing models via NuGet packages brings in benefits of versioning, distribution and code reuse.