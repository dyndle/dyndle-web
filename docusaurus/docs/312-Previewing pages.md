---
id: preview
title: Previewing pages
description: Documentation for Dyndle
sidebar_label: Previewing pages
---

Dyndle makes it possible to use the preview functionality in the CME to see your pages exactly like they will be on the site, before you have even published them. 

All you need to do is add the *Preview pages* template building block (part of the Dyndle templates) in your page templates. 
You will see a set of parameters in the template builder:

- URL of the staging site

Here you need to enter the URL of the staging site that will be used to generate the preview. It must be a URL that is accessible from the CM machine.


- URL of the proxy server

In some cases, the staging site may not be accessible form the CM machine directly, but only through a proxy. In that case, enter the URL of the proxy (including the port) here.

Save and close the page template(s). You can now right-click any page that uses one of these page templates, and click on Preview. The page will be rendered by the web application itself, which guarantees that it is as close to the real thing as possible. All styles, scripts, images and other assets will load normally. The only limitation is that the preview is shown inside an iframe, which is how the CME handles this.