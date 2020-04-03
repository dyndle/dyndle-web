---
id: configuration
title: Configuring Dyndle
sidebar_label: Configuring Dyndle
---

Below you can find an overview of all configuration values that are used by each Dyndle module:

## Core

...

## Management

...

## Navigation

...

## Search

...

## Image Enhancement

The following table describes the configuration values in the appsettings section of the web config that are used in the Dyndle Image Enhancement module.

| Config key                       | Example Value     | Description                                                  |
| -------------------------------- | ----------------- | ------------------------------------------------------------ |
| ImageEnhancement.Localpath       | "/EnhancedImages" | Where the enhanced versions of the images will be stored.    |
| ImageEnhancement.Backgroundcolor | "#fff"            | The color that is used when you crop an image and provide a width and height that's higher than the cropped area. |
| ImageEnhancement.Cacheseconds    | "20"              | How long the enhanced image will be retrieved from the cache before creating a new enhanced version. |