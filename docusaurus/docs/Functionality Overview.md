---
id: functionality-overview
title: Functionality Overview
sidebar_label: Functionality Overview
---

## General overview

In order to use Dyndle, following steps must be carried out:

- Install NuGet package(s)
- Make sure Dyndle is loaded in the application
- Add configuration
- Make sure there all neccessary items are present in Tridion


## Dyndle Core

### Install

NuGet package: Dyndle.Modules.Core 

### Load

Core module contains a large amount of crucial functionality that is required for Dyndle to operate properly. 

*To be rewritten once we choose ApplicationBase / StartDyndle approach*

### Configure

Dyndle.Modules.Core adds some of the required configuration values on installation. Below you find the full list of configuration values for this module:


### Tridion

Use Dyndle CLI to add all neccesary items to the content manager. Below you find an overview of what needs to be added:

Schemas
Templates
Components
Pages

## Dyndle Management

### Install

NuGet package: Dyndle.Modules.Management 

### Load

Only controllers from this module need to be registered in the DI container. 

## Dyndle Image Enhancement

### Install

NuGet package: Dyndle.Modules.ImageEnhancement 

### Load

Only controllers from this module need to be registered in the DI container. 

## Dyndle Navigation

### Install

NuGet package: Dyndle.Modules.Navigation 

### Load

Navigation module provides a NavigationService that can be used to generate any desired form of navigation. To use it ....

### Tridion

Structure group naming convention


## Dyndle Search

### Install

NuGet package: Dyndle.Modules.Search 

### Load

...

### Tridion

...
