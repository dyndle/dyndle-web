---
id: preparing-cm
title: Getting your content manager ready for DD4T and Dyndle
description: Documentation for Dyndle
sidebar_label: Preparing the content manager
---

Dyndle is a framework built on top of DD4T framework. Therefore, in order to use Dyndle, you must first make sure all DD4T prerequisites are available.

If you start from an existing DD4T implementation, you can skip to [Installing Dyndle in the Content Manager](installation-cm.html).

## Install the DD4T templates

Please make sure [DD4T templates are installed](https://github.com/dd4t/DD4T.Core/wiki/Installing-the-DD4T-templates) on the Content Manager.

In case you are unfamiliar with DD4T templates, these Template Building Blocks transform data of components and pages into JSON. Moreover, additional metadata is passed along with the component template, that allows the web application to determine which controller actions are used to render the data.


## Template setup

We recommend that you use a compound template building block to include default behaviour for all your component templates (CT) and another for your page templates (PT). This makes changing all the templates at once much simpler in the future.

In order to run DD4T or Dyndle application, all CTs and PTs must use DD4T TBBs that transform data into JSON: `Generate dynamic component presentation` and `Generate dynamic page` respectively. When updating existing templates or creating new ones, also make sure to attach 'Dyndle Template Metadata' metadata schema to them. This metadata schema allows you to specify view, controller, action and region(for CTs).

## Usage

Create a page with a DD4T page template and place a number of components on it. All the components must be placed on the page with a DD4T template. Now publish the page and request this page URL in your DD4T web application.


