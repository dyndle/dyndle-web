---
id: index
title: What is Dyndle?
sidebar_label: What is Dyndle?
---

Dyndle (shorthand for Dynamic Delivery) is the new way to develop a web site in .NET, powered by SDL Tridion Sites. Dyndle is built on top of the popular [DD4T](https://dd4t.org/) framework. It is open source software and offered free-of-charge. [Trivident](https://trivident.com) is the company that manages pull request and releases, but anyone can contribute. [Trivident](https://trivident.com) can also arrange support for Dyndle as part of its professional services offering.

From an architecture point of view, Dyndle preserves one of the big benefits of DD4T: the fact that you own and control your web application. At the same time, Dyndle saves you a lot of time, because it adds many useful features to DD4T, while leaving you in full control of your own web application.


## How to use Dyndle

- [Download the Dyndle CLI](/download)
- Create an empty .Net Framework MVC5 project
- Install [Dyndle.Quickstart.Web](https://www.nuget.org/packages/Dyndle.Quikstart.Web) NuGet package
- Add a few [configuration values](configuration)
- Use the CLI to install everything you need in Tridion
- Use the CLI to generate models and views for you
- Run!

You can find a more detailed walkthrough in [Getting started with Dyndle](getting-started)

In the Installation section, you can find more information on how to install Dyndle, plus the full [Configuration reference](configuration).

The core features of Dyndle are described in the Implementing Dyndle section.

## Dyndle Modules

Dyndle is organized in modules. The core module is mandatory - without it you cannot run Dyndle. It comes with a lot of functionality, which is described in the Implementing Dyndle section.

You can add additional features by adding a NuGet reference to the various modules:

- [Management](management)
  - Adds a cache browser
  - Adds a debugging mode
- [Image Enhancement](image-enhancement)
  - Adds a controller that lets you transform images on the fly
- [Navigation](navigation)
  - Adds a service that allows you generate any navigation based on the sitemap
- [Search](search)
  - Adds an interface to talk to Solr

More modules will be released shortly.
