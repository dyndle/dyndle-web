---
id: cli
title: Running Dyndle CLI
description: Documentation for Dyndle
sidebar_label: Running Dyndle CLI
---

The Dyndle command-line interface (CLI) is a simple executable which runs on any Windows machine. It can be downloaded from https://www.dyndle.com/download.html.

Unzip in a folder of your choice and store the file dyndle.exe in a convenient location (ideally in a folder that is part of the PATH environment variable of your Windows machine).

The CLI supports the following commands:

- **install** - install the necessary items in the Tridion Content Manager
- **add-environment** - add configuration for a new Tridion environment
- **list-environments** - list the configured Tridion environments
- **update-environment** - update an existing Tridion environment configuration
- **delete-environment** - delete an existing Tridion environment configuration
- **models** - generate C# ViewModel classes from the schemas in Tridion
- **views** - generate Razor views from the templates in Tridion

It has a built-in help function as well. You can type 'dyndle help [command]' for any of the commands listed above to get detailed instructions.

## Environments

The first thing you need to do is configure one or more Tridion environments. The Dyndle CLI uses the core service to connect to Tridion, so it will need to know things like:

- The URL of the Tridion CM server
- The username, domain and password of a user with access to Tridion
- The name of the environment (can be any string)

Dyndle lets you configure any number of Tridion environments. When you run any of the other commands (like *install* or *models*) you need to specify one of the configured environments. This is why you have to name the environments you configure.

All this information is stored in a file called environments.json which is stored in the folder where you ran the command. You can share this file with other people, store it in a source code repository, etc.

### Default environment

One of your environments is the *default environment*. If you run any of the other commands and you do not specify an environment with &dash;&dash;environment, the default environment is used. You can make an environment the default by adding &dash;d (or &dash;&dash;default) when you add it. 

### Add a new environment

You can add a new environment in the following way

```shell
dyndle add-environment --name Develop --url http://cm-dev.mycompany.com --username dyndleUser --domain MYDOMAIN --password mypassword
```

### List all environments
If you want to see which environments you have already configured, you type:

```shell
dyndle list-environments
```

providing a -v switch to the list-environments command show the environment information in verbose mode.
Example:
```shell
> dyndle list-environments -v

Name:        Develop
URL:         http://cm-dev.mycompany.com
User name:   dyndleUser 
User domain: MYDOMAIN
Password:    ***
Default:     True

```

### Update an environment
If you want to update an existing environment that you have already configured, you type:

```shell
dyndle update-environment --name Develop --url http://new-cm-dev.mycompany.com --username new-dyndleUser --domain new-MYDOMAIN --password new-mypassword
```

### Delete an environment
If you want to delete an existing environment that you have already configured, you type:

```shell
dyndle delete-environment --name Develop
```

## Install

Before you can use Dyndle, you need to create some items in the Tridion Content Manager, like:

- The Dyndle template building blocks
- The Dyndle template metadata schema
- A number of page templates
- The *Site configuration* page
- The *Navigation* page

The install command handles this for you. You need to supply two mandatory arguments: 

- The URI of the folder where you want to store all Dyndle items except the pages (using the command-line argument &dash;&dash;dyndle-folder)
- The URI of the structure group where you want to store the Dyndle pages (using the command-line argument &dash;&dash;dyndle-sg)

For example:

```shell
dyndle install --dyndle-folder tcm:2-123-2 --dyndle-sg tcm:8-456-4
```

Note that the folder and the structure group can be in different publications. However, the publication containing the folder must be a parent of the publication containing the structure group.

The installation may take a minute or so. At the end, the tool will report the number of uploaded items.

## Generating models or views

One of the most helpful features of Dyndle is its ability to generate models and even views for you. 

Models (or ViewModels) are C# classes representing the schemas in Tridion. If your schemas are called Article, Banner and Image, you will want to have ViewModel classes with the same names. Although it is possible to create these manually, you can also opt to let Dyndle create them for you.

Views are Razor pages used by your application to generate the HTML which makes up your site. The views are based on the models - each view generates HTML for one model.

Both models and views are generated in the form of NuGet packages - one for the models, one for the views. Add a reference to these packages, and you will see the files (.cs files for models, .cshtml files for views) appear in your application. It is important to realize that these NuGet packages are in this sense different from most other packages. Most packages contain DLLs, and by installing these packages you are actually creating references to these DLLs. But - as said - the packages generated by the Dyndle CLI only put files in your application.

### Models

You can generate models for all schemas in a specific publication or folder, or for individual schemas. You do that by adding one of the following command-line arguments:

- &dash;p or &dash;&dash;publication
- &dash;f or &dash;&dash;folder
- &dash;s or &dash;&dash;schema

Note that schemas can have relationships with other schemas, for example in the case of embedded fields or component links. If that is the case, the linked schemas are ALWAYS exported along with the schemas you have selected (even if they are not in the specified folder, for example).

Dyndle applications use models to represent component presentations (also referred to as entities), but also to represent pages. Entity models are based on schemas, but page models are based on page templates. It is therefore also possible to generate models for a template (but only if it is a page template!). This is done with this command-line argument:

- &dash;t or &dash;&dash;template

If you want to be sure you include all models in one package, you should probably generate a package for an entire publication.

Most of the other options (there are many) are the same for models and views, so they are described below (under 'Options for models and views').

### NuGet repository
The NuGet packages you are creating must be placed in a NuGet repository. There are 3 options for this:

1. The central NuGet repository (nuget.org). This is almost NEVER what you want.
2. A private NuGet repository for your organization. This is normally the best solution.
3. A local NuGet repository, which is simply a folder on your file system.

If you don't have a private NuGet repository, you can use a little trick that makes the lives of your developers easy.

- Create a folder inside your GIT repository called 'local-packages'
- Create a NuGet.config file and put it in the same folder as your Visual Studio solution. VS will load this configuration whenever you open the solution.
- In this config file, add a package source for this local-packages folder, like this:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>

  <packageRestore>
    <add key="enabled" value="True" />
    <add key="automatic" value="True" />
  </packageRestore>

  <activePackageSource>
    <add key="All" value="(Aggregate source)" />
  </activePackageSource>

  <packageSources>
    <!-- Add the private NuGet package source for this solution -->
    <add key="Local Packages" value="..\local-packages" />
  </packageSources>

  <disabledPackageSources />
</configuration>
```
Finally, make sure the NuGet.config file and the local-packages folder with all its contents are uploaded to the GIT repository. You may need to change the .gitignore to achieve this.

### Views

You can generate views for all page and/or component templates in a specific publication or folder, or for individual templates. You do that by adding one of the following command-line arguments:

- &dash;p or &dash;&dash;publication
- &dash;f or &dash;&dash;folder
- &dash;t or &dash;&dash;template


### Options for models and views

There are many ways to customize the model and view generation. This is all done through command line options. The most important ones are listed here. To see the full set, just type 'dyndle help models' or 'dyndle help views'.


| Argument                          | Default Value            | Description |
| :---------------------------------- | :------------------------ | :----------------------------------------------------------- |
| &dash;&dash;environment                | The default environment | Name of the environment from which you want to generate the models or views |
| &dash;&dash;package-name                | MyModels or MyViews | Name of the NuGet package you are creating |
| &dash;&dash;package-version                | 1.0.0 | Version of the NuGet package you are creating |
| &dash;&dash;prerelease                | false| If true, automatically creates a prerelease NuGet package with a unique build number |
| &dash;&dash;namespace                | My.Models | Namespace of the generated models (setting is ignored for views) |
| &dash;&dash;output-folder                | a subfolder of %TEMP% | The folder where the generated NuGet package is stored. If you use a local-packages folder, it makes sense to use this as your output-folder! |




