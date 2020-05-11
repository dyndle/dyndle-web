---
id: installation-cm
title: Installing Dyndle in the content manager
sidebar_label: Content Manager installation
---


## Preparation

1. Download the Dyndle CLI from https://www.dyndle.com/download.html
2. Unzip in a folder of your choice and store the file dyndle.exe in a convenient location (ideally in a folder that is part of the PATH environment variable of your Windows machine)
3. Next, you need to tell Dyndle about your Tridion Content Manager. You do this by running the following command:

```shell=
dyndle add-environment --name AnyName --url Url --username UserName --domain UserDomain --password Password
```

**Explanation of the arguments:**

&dash;&dash;name: this can be any name, it is just for you to recognize it later

&dash;&dash;url: the address of the Tridion Content Manager (just like you would type it in the browser)

&dash;&dash;username: the name of a user with Administrator rights within Tridion

&dash;&dash;domain: the user domain (if any, for a local user you can leave this out)

&dash;&dash;password: the password

## Installing the Dyndle Content Manager items

Install the Content Manager items of Dyndle with the following command:

```shell=
dyndle install --dyndle-folder tcm:x-y-z --dyndle-sg tcm:x-y-z --environment EnvironmentName
```

**Explanation of the arguments:**

&dash;&dash;dyndle-folder: the TCM URI of the folder where you want to install Dyndle's building blocks. We recommend that you pick a folder somewhere high up in the BluePrint hierarchy, like your 'System Parent', 'Template Master', or something similar. This should be an empty folder.

&dash;&dash;dyndle-sg: the TCM URI of the structure group where you want to put the Dyndle system pages. We would recommend to create a System structure group (directory name 'system') within the root structure group of your website master. You can also choose an structure group with pages in it.

&dash;&dash;environment: this is the name of the environment you created before.

## Multiple environments

The Dyndle CLI lets you install Dyndle in multiple Content Manager environments, by simply adding new environments and running the 'dyndle install' command for each of them.
Of course, you can also choose to use the Content Porter to copy the Dyndle items from one Content Manager to the next.


### Next steps

Next, you need to create a web application. You have two options:

1. Set up a web application quickly with the [Quickstart Installation](quickstart-installation.html). We will make a couple of choices for you, to make it even easier. The most important choice we made is to use Autofac as the dependency injection framework.
2. Take full control by following the steps in the [Manual Installation](manual-installation.html). With this approach, you can choose another one of the supported dependency injection frameworks (Unity, Ninject or Simple Injector).