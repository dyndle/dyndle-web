---
id: doc1
title: Installation and Setup
sidebar_label: Installation and Setup
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

## Adding Dyndle to an existing implementation

### Quickstart

Dyndle offers a number of quickstart packages that lets you kickstart your Dyndle implementation by adding neccesary dependencies and providing all neccssary loading mechanisms.

Please note, that each Quickstart package assumes that you are using a certain Dependency Injection framework. Please select a package that reflects what you are using already, unless you want to switch DI framemwork at the same time as you add Dyndle.

### Manual 

Dyndle is a modular system that can be configured and loaded in any desired capacity. Please consult the above installation guide to properly install and configure the features you would like to include in your application.