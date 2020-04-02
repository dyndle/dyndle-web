---
id: installation
title: Installation and setup
sidebar_label: Installation and setup
---

In order to use Dyndle, following steps must be carried out:

- Install NuGet package(s)
- Make sure Dyndle features are loaded in the application
- Add configuration (see [next chapter](configuration))
- Make sure there all neccessary items are present in Tridion (see [Dyndle CLI](cli)).

## Dyndle Core

### Install

NuGet package: Dyndle.Modules.Core

### Load

Core module contains a large amount of crucial functionality that is required for Dyndle to operate properly.

_To be rewritten once with explanation of StartDyndle approach_

## Dyndle Management

### Install

NuGet package: Dyndle.Modules.Management

### Load

Only controllers from this module need to be registered in the DI container.

## Dyndle Navigation

### Install

NuGet package: Dyndle.Modules.Navigation

### Load

Navigation module provides a NavigationService that can be used to generate any desired form of navigation. To use it ...

## Dyndle Search

### Install

NuGet package: Dyndle.Modules.Search

### Load

...
