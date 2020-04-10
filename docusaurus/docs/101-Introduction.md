---
id: introduction
title: Introduction
sidebar_label: Introduction
---

Here is quick walkthrough of what Dyndle is and how to get started with it.

## What is Dyndle?

Dyndle (short from Dynamic Delivery) is the new .Net development kit for websites powered by SDL Tridion Sites, built on top of the renowned [DD4T](https://dd4t.org/) framework. Dyndle is a product that is offered free-of-charge and developed open-source. [Trivident](https://trivident.com) is the company that manages pull request and releases, but anyone can contribute. [Trivident](https://trivident.com) can also arrange support for Dyndle as part of the professional services.

From an architecture point of view, Dyndle preserves one of the big benefits of DD4T: the fact that you own and control your web application. Dyndle adds many useful features to DD4T, but still leaves you in full control of your own web application.

The Dyndle framework consists of multiple modules that provide different functionality. During implementation process, you can choose which modules to use, based on the requirements.

## Getting Started

The easiest way to try out Dyndle is to set up a fresh solution and using a Dyndle.Quickstart.Web NuGet package.

### All you need to get your first Dyndle application running

- Create an empty .Net Framework MVC5 project
- Install [Dyndle.Quickstart.Web](https://www.nuget.org/packages/Dyndle.Quikstart.Web) NuGet package
- Add a few [configuration values](configuration)
- Use [Dyndle CLI](cli) to install everything you need in Tridion
- Use [Dyndle CLI](cli) to generate models and views for you
- Run!

You can find a more detailed walkthrough in [Getting started with Dyndle](getting-started)

In [Installation and setup](installation) you can find more information, such [Adding Dyndle to an existing implementation](existing-implementation) and [Full list of configuration values](configuration).

## Dyndle Modules

- Core
  - Provides foundation for other Dyndle modules
  - Adds MVC capabilities: controllers, model binders, areas, etc.
  - Adds Web capabilities: HTML helpers, object and output caching, etc.
- Management
  - Adds a cache browser
  - Adds a debugging mode
- Image Enhancement
  - Adds a controller that lets you transform images on the fly
- Navigation
  - Adds a service that allows you generate any navigation based on the sitemap
- Search
  - Adds an interface to talk to Solr
- ... more to come!
