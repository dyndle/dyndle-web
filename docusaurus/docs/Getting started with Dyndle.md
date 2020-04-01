---
id: getting-started
title: Getting started with Dyndle
sidebar_label: Getting started with Dyndle
---

## Getting Started

The easiest way to try out Dyndle is setting up a fresh solution using a Visual Studio template or by using a Dyndle.Quickstart.Web NuGet package. Below you can find detailed steps of how to get your first Dyndle web application up and running. 

If you are interested in using Dyndle in an existing implementation, please check [Adding Dyndle to an existing implementation](). For more information on installation and setup, please refer to [the corresponding section](). 

### Visual studio template

- [Download and install Visual Studio template]()
- Create a new project
- Configure connection to Tridion

### Quickstart using NuGet package

- Create an empty .Net Framework MVC5 project
- Install [Dyndle.Quickstart.Web]() NuGet package
- Configure Global.asax to inherit from ApplicationBase
- Configure connection to Tridion

### Set up Tridion CM

Dyndle requires certain items to be present in Tridion. Individual modules and features may depend on their own set of items. To make your life easier, we have provided a Dyndle CLI to do this for you.

- [Download the latest version of the Dyndle CLI]()
- Run it and install 'Quickstart' configuration 

### Run your own test application

Now that you have set up both Visual Studio project and Tridion CM, you can run your first Dyndle application! 

- Build and run the web application
- Go to /dyndle/ 
- You should see a page containing a number of component presentations
- Go to /admin/cache
- You should see a cache viewer that shows all objects stored in the cache
- ... and you take it away from here! Happy dyndling!