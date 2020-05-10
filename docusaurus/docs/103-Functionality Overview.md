---
id: functionality-overview
title: Functionality Overview
sidebar_label: Functionality Overview
---

Dyndle consists of a core and a number of modules, which enhance the core functionality. You always need the Dyndle Core, the modules are optional.



Before we describe each module in greater detail, we should first introduce new concepts that are used in Dyndle, as compared to DD4T.
In DD4T components are treated as ViewModels or Content Models. Component templates are used to determine a view or a controller action that is responsible for rendering the model. In Dyndle, concept of Entities is used instead. An entity combines the component and component template and adds additional possibilities for rendering the entity: area and region.
Dyndle is built to support regions even before there were introduced in Tridion 9. Component presentations can be grouped based on:

- Metadata in the component template
- The schema used by the component
- Custom logic
- And of course, the ‘real regions’ as introduced in Tridion 9.

Next to the ability to group entities into regions and render those regions, Dyndle also introduces standard way of using includes. Include pages can for example contain component presentations for: cookies, header and/or footer. These include pages can then be reused across the entire site or sections of it, depending on the business logic and requirements. This functionality also takes advantage of regions, as a single include page can contain header and footer regions, that will then be rendered in specific places of the page.

## Dyndle Core

The Core module is a mandatory module as it enables the core features of Dyndle. This module contains enhancements to the existing functionality of DD4T and provides new functionality for the web application. It offers the following features (and more):

- Controllers

  All controllers needed to run your site come out of the box. This includes a binary controller that caches files on the local file system, and a region controller with full support of regions in Tridion 9. Of course, you can still create your own controllers for specific fucntionality.

- Extensionless URLs

  If you run Dyndle, you don't have to use file extensions in your URLs. For example, a Tridion page called 'news.html' in the Root structure group, is accessible through the URL 'http://my.company/news'. Links to pages are automatically rendered without the extension. This behavior can be disabled in the [configuration](configuration.html). 

- Smart output caching, which caches the entire output of your page but drops it as soon as the page is republished or unpublished

- A so-called ‘preview-aware’ cache agent, which is especially designed for use with Experience Management, SDL’s ‘in context editing’ feature.

- Labels, redirects, and much more. See under Implementing Dyndle in the left menu of this documentation for more details.


## Dyndle Management

The Management module improves development and management of the web application. Key features of this module include cache management capabilities and debug information for the web application.

See [Dyndle Management](management.html)


## Dyndle Navigation

The navigation module makes it easier than ever to generate navigation based on the structure groups in Tridion. When you add this module to your application, you can write out different types of navigation from your views, like top navigation, left navigation, breadcrumbs and complete sitemaps.

See [Dyndle Navigation](navigation.html)


## Dyndle Search

The search module provides search functionality to your web application through the Solr search platform. It makes use of  SI4T to easily define which pages to index in Tridion and provides many customization options to tailor search results.  

See [Dyndle Search](search.html)


## Dyndle Templates

Dyndle ships with C# Templates that enable functionality of other modules. Moreover, there are useful utility template building blocks that are commonly requested by the clients.

## Dyndle Tools

Alongside the core Dyndle product, there are also additional tools that have been developed to simplify migrating to Dyndle, as well as, maintaining the existing Dyndle implementation.

One of such highlighted tools is model generator. It is a tool that automatically generates entity models (C# classes) based on schemas defined in Tridion. Once models are generated, they are managed as a NuGet package that adds these classes into the web application. The architecture of this solution allows defining custom logic is separate partial classes that enhance generated models. That way, generated models are never changed and generation process can be rerun, resulting in a new version of the NuGet package, that can afterwards be updated in the project.

Model generator allows for rapid development cycles. Once schemas have been designed in Tridion, within minutes new corresponding entity models can be automatically generated and added to the project. This not only speeds up the initial setup time, but also removes possibility of human-error in content mapping between Tridion and code. Moreover, since models are managed as a NuGet package, tracking versions and managing deployments can be streamlined.
