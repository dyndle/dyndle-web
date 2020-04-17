---
id: installation-cm
title: Getting your content manager ready for DD4T and Dyndle
sidebar_label: Setting up content manager
---

Dyndle is a framework built on top of DD4T framework. Therefore, in order to use Dyndle, you must first make sure all DD4T prerequisites are available.

## Prerequisites

Please make sure [DD4T templates are installed](https://github.com/dd4t/DD4T.Core/wiki/Installing-the-DD4T-templates) on the Content Manager (CM).

In case you are unfamiliar with DD4T templates, these Template Building Blocks transform data of components into JavaScript Object Notation(JSON). Moreover, additional metadata is passed along with the component template, that allows the web application to determine which controller actions are used to render the data.

In order to set up Dyndle CM prerequisites, you can use [Dyndle CLI](cli) to get everything installed. This will make sure the new TBBs, templates and schemas are also present in Tridion. After DD4T and Dyndle are both installed on the CM, you can proceed to configure component templates.

## Template setup

We recommend that you use a compound template building block to include default behaviour for all your component templates (CTs) and another for your page templates (PTs). This makes changing all the templates at once much simpler in the future.

In order to run DD4T or Dyndle application, all CTs and PTs must use DD4T TBBs that transform data into JSON: `Generate dynamic component presentation` and `Generate dynamic page` respectively. When updating existing templates or creating new ones, also make sure to attach 'Dyndle Template Metadata' metadata schema to them. This metadata schema allows you to specify view, controller, action and region(for CTs).

## Usage

Create a page with a DD4T page template and place a number of components on it. All the components must be placed on the page with a DD4T template. Now publish the page and request this page URL in your DD4T web application.

- Page template name is used to construct a page model.
- Page template metadata is used to select a controller and its action to render the page.
- PageController (or a custom controller) renders component presentations from the page:
  - Schema root element name (or schema title) is used to construct an entity model in Dyndle (a content model in DD4T)
  - Component template metadata is used to determine a controller and its action to render the entity

This is the general summary of what DD4T or Dyndle request looks like and why you need all those prerequisites. DD4T and Dyndle come with more features that require CM setup, but those are described separately in corresponding pages related to the features.
