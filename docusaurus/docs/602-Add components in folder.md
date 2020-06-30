---
id: add-components-in-folder
title: Add components in folder
description: Documentation for Dyndle
sidebar_label: Add components in folder


---

The `Add components in folder` template building block can be used to automatically add components that are contained in the same folder to a page based on a single component from that folder. By using this building block, publishing a group of components is made much easier and doesn't require CM users to individually add every component to a page by hand. The building block can be configured to only add components that use a specified schema and also which component template the components should use. 

Add the building block to a page template with the `Tridion Sites Template Builder`. Detailed instructions on how to use the template builder can be found [here.](https://docs.sdl.com/LiveContent/content/en-US/SDL%20Web-v5/GUID-FD25A36E-4B1C-4346-BB7E-919B293B8748) 

The parameters on this building block can be set to refine which components are added by providing the root element name of a schema that the component should use. If this value is set, only the components that use a schema with the specified root element name will be added to the page. A tcm-id of a component template can also be provided to make the building block add the components using that template when they are added. Both of these parameters can be left empty, in which case the components in the folder won't be filtered on what schema they use and the component template that is used by the component that is on the page will be used for the components to add. There is also a parameter where the link level for the components can be specified. This will influence how far down the chain of embedded component links will be resolved in the process. It is set to 1 by default.

When the building block has been added to the page template and the parameters have been set or left empty as desired, the feature is now active and will run when a page with that page template is published. If the page has at least one component on it the building block will find the folder that that component is in and look for other components. It will then filter them based on the configuration and add the components that match to the page with the same component template as the component that was already on the page, or with the component template from the parameters. 