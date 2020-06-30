---
id: replace-special-characters
title: Replace special characters
description: Documentation for Dyndle
sidebar_label: Replace special characters

---

With the `Replace special characters` template building block you'll be able to automatically replace characters or even individual words on a DD4T component when it is published. This building block requires a list of characters to replace in a specific format which can be configured in its parameters. 

Add the building block to a component template with the `Tridion Sites Template Builder` after the `Generate dynamic component presentation` building block from DD4T. Detailed instructions on how to use the template builder can be found [here.](https://docs.sdl.com/LiveContent/content/en-US/SDL%20Web-v5/GUID-FD25A36E-4B1C-4346-BB7E-919B293B8748) 

After adding the building block, specify a list of characters to replace in the following format.

`(replace1=replaceby1)(replace2=replaceby2)...(replaceN=replacebyN)`

The brackets are required and specify where one replacement ends and the other begins. Provide the character to replace and the character to replace it with and connect them with an equals symbol in between. Now save the template and use it for a component. When you publish a page with that component on it, or publish the component dynamically, the characters that were configured will be replaced.