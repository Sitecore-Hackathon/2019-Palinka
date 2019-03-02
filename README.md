# Documentation

The documentation for Content Editor Toolbox module which was built for Sitecore Hackathon 2019.
## Summary

**Category:** - Best enhancement to the Sitecore Admin (XP) UI for Content Editors & Marketers

The purpose of the module is reducing the number of clicks for content editors. This module helps the content editors to reach 
the given Item within sitecore. On the other hand, it gives an overview about the content editor's activity. 
It was built with Sitecore SPEAK3.

### Features
The module has the following features

#### Bookmark items
Bookmark items feature allows to save items for content editors in their user profile from Context menu within Content Editor. 
A SPEAK3 application is built for checking and managing the previously bookmarked items. The following operations are supperted: 
- Publish the bookmarked item
- Open in Content Editor in a selected language
- Open in Experience Editor in a selected language
- Remove item from bookmarked item list
- Copy ItemID and ItemPath to clipboard by a single click

It is really useful, when content editors have to update item frequently in a heavy content hiearchy. 

#### Recent activities
Recent activities feature allows to check and manage previously modified items for content editors. It is really useful when content editors have to
update the previously created items. 
- Publish the recently modified/created item
- Open in Content Editor in a selected language
- Open in Experience Editor in a selected language
- Copy ItemID and ItemPath to clipboard by a single click

#### My locked items
My locked items feature allows to check and unlock unlocked items for content editors. 
- Unlock the locked items
- Unlock all locked items
- Open in Content editor in a selected language
- Copy ItemID and ItemPath to clipboard by a single click

## Pre-requisites

- Sitecore 9.1

## Installation

Provide detailed instructions on how to install the module, and include screenshots where necessary.

1. Use the Sitecore Installation wizard to install the [package](sc.package/ContentEditorToolbox.zip)
2. Make sure if your search indexes are working correctly
3. Go the LaunchPad and open the Content Editor Toolbox.

## Configuration

The module does not requires any configuration, configuration files are using the proper Server Roles. (Standalone or Content Management)

## Usage

### Bookmark items
When you navigating in Content Editor, you can bookmark items by right clicking on the item. 
![Hackathon Logo](documentation/images/context_menu.png?raw=true "Context menu")
Once you clicked the item, you can remove bookmark by the next right click. 

Open Sitecore's Launchpad and locate Content Tool Box shortcut. 
![Hackathon Logo](documentation/images/dashboard.png?raw=true "LaunchPad")

Click on "Bookmarks" menu item in the main content area or in the left hand sided navigation
![Hackathon Logo](documentation/images/app_main.png?raw=true "Start Screen")

The following page will be loaded
![Hackathon Logo](documentation/images/bookmark.png?raw=true "Hackathon Logo")

The table contains the following informations for a single bookmark item
- Icon of the item
- Item Name
- Item ID (can be copied to the clipboard)
- Template Name
- Path
- Workflow State
- If the item is published

There is a tools dropdown which has the following features.
- Shows 'Open in Experience Editor' action if there is presentation for the item
- Shows 'Open in Content Editor'
- Remove from bookmark page. (If you remove the item, the table refreshes automatically)
- Publish Item (Table refreshes automatically)

### Recent Activity
Click on "Recent activities" menu item in the main content area or in the left hand sided navigation
The following page will be loaded
![Hackathon Logo](documentation/images/recent.png?raw=true "Recent activities")
The table shows items which were created or updated by the context editor user. 

There is a tools dropdown which has the following features.
- Shows 'Open in Experience Editor' action if there is presentation for the item
- Shows 'Open in Content Editor'
- Publish Item (Table refreshes automatically)

### My Locked Items
Click on "My Locked items" menu item in the main content area or in the left hand sided navigation
The following page will be loaded
![Hackathon Logo](documentation/images/unlock.png?raw=true "Recent activities")
The table shows items which were locked by the context editor user. 

There is a tools dropdown which has the following features.
- Shows 'Open in Experience Editor' action if there is presentation for the item
- Shows 'Open in Content Editor'
- Unlock Item (Table refreshes automatically)

The page has the 'Unlock all items' option which unlocks all locked items. 

Before the application unlock the items, it shows a confirmation dialog for the editor user. 
![Hackathon Logo](documentation/images/unlock_warning.png?raw=true "Warning")


## Video

[Video](https://www.youtube.com/watch?v=6aWJVwSVP4c) 


