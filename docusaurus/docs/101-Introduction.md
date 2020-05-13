---
id: index
title: What is Dyndle?
sidebar_label: What is Dyndle?
---

Dyndle (shorthand for Dynamic Delivery) is the new way to develop a web site in .NET, powered by SDL Tridion Sites. Dyndle is built on top of the popular [DD4T](https://dd4t.org/) framework. It is open source software and offered free-of-charge. [Trivident](https://trivident.com) is the company that manages pull request and releases, but anyone can contribute. [Trivident](https://trivident.com) can also arrange support for Dyndle as part of its professional services offering.

From an architecture point of view, Dyndle preserves one of the big benefits of DD4T: the fact that you own and control your web application. At the same time, Dyndle saves you a lot of time, because it adds many useful features to DD4T, while leaving you in full control of your own web application.


## Where to start

To use Dyndle, you need to install it first. The installation consists of the following steps:

1. Check the [prerequisites](prerequisites.html)
2. [Prepare the Tridion content manager](preparing-cm.html)
3. Install Dyndle [in the content manager](installation-cm.html)
4. Install the web application, using either the [Quickstart process](quickstart-installation.html) or by [installing it manually](manual-installation.html)


In the Installation section, you can find all the information you need on how to install Dyndle, plus the full [Configuration reference](configuration.html).


## Dyndle Modules

Dyndle is organized in modules. The core module is mandatory - without it you cannot run Dyndle. It comes with a lot of functionality, which is described in the Implementing Dyndle section.

You can add additional features by adding a NuGet reference to the various modules:

- [Management](management.html)
  - Adds a cache browser
  - Adds a debugging mode
- [Image Enhancement](image-enhancement.html)
  - Adds a controller that lets you transform images on the fly
- [Navigation](navigation.html)
  - Adds a service that allows you generate any navigation based on the sitemap
- [Search](search.html)
  - Adds an interface to talk to Solr

More modules will be released shortly.
