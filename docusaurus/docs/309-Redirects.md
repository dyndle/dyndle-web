---
id: redirects
title: Managing redirects
description: Documentation for Dyndle
sidebar_label: Managing redirects
---

The Dyndle core module provides a way to manage redirects in your webapplication through Tridion. You can define a source url and a target url in a component and can choose which type of redirect it should use. The schema for the component is included in the installation of the Dyndle templates. 

## Setting up redirects

To use redirects in your webapplication you need to create a page that contains redirect components. Create a component using the `Redirects` schema and put it on a page. You can provide multiple redirects in the same component. Provide a source url and a target url to use for the redirect. Then pick a http redirect type you would like to use.  The component and the page can use the default DD4T template, since it doesn't require any special logic when publishing. 

When the page is published, the url needs to be set in the web config of your web application so the core module knows where to find it. Note that you can only set one url in this setting, so make sure you put all of your redirects on the same page. The setting should look like this:

`<add key="Dyndle.RedirectsUrl" value="/your-redirect-page.html" />`

If the page is contained in a stucture group add this to the path as well, like so:

`<add key="Dyndle.RedirectsUrl" value="/your-structure-group/your-redirect-page.html" />`

Then include the following setting to enable the redirects. This also provides a way to easily toggle the functionality on or off.

`    <add key="Dyndle.EnableRedirects" value="true" />`

The redirects you have defined should now be operational. Whenever a page is being requested that can not be found the redirect functionality will kick in and will redirect the requested url to the url configured in the redirects page.