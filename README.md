# Etch.OrchardCore.ContentPermissions

Module for Orchard Core to enable configuring access at a content item level.

## Build Status

[![NuGet](https://img.shields.io/nuget/v/Etch.OrchardCore.ContentPermissions.svg)](https://www.nuget.org/packages/Etch.OrchardCore.ContentPermissions)

## Orchard Core Reference

This module is referencing a stable build of Orchard Core ([`1.8.3`](https://www.nuget.org/packages/OrchardCore.Module.Targets/1.8.3)).

## Installing

This module is available on [NuGet](https://www.nuget.org/packages/Etch.OrchardCore.ContentPermissions). Add a reference to your Orchard Core web project via the NuGet package manager. Search for "Etch.OrchardCore.Fields", ensuring include prereleases is checked.

Alternatively you can [download the source](https://github.com/etchuk/Etch.OrchardCore.ContentPermissions/archive/master.zip) or clone the repository to your local machine. Add the project to your solution that contains an Orchard Core project and add a reference to Etch.OrchardCore.ContentPermissions.

## Usage

Enabled the "Content Permissions" feature, which will make a new "Content Permissions" part available. Attach this part to the desired content types, which will add a new "Security" tab to the content editor. From this tab the content item permissions can be enabled, which will display all the roles in the CMS. Select the roles that can access the content item and publish. Below are different ways of handling when the user isn't associated to one of the roles specified on the part.

### Redirection

Users can be redirected to a specific URL that can be defined in the settings for the content permissions part.

### Display Alternative Content

Users can be displayed an alternative by customising the view template, as shown below. This enables the ability to restrict whether users can see specific widgets on a page.

#### Liquid

Example of how to check users permission and display alternative content with Liquid template.

```
{% assign canViewContent = Model.ContentItem | user_can_view %}

{% if canViewContent %}
	<p>Awesome content that you have permission to view.</p>
{% else %}
	<p>Unfortunately you're not able to view this content.</p>
{% endif %}
```

#### Razor

Example of how to check users permission and display alternative content with Razor template.

```
@inject Etch.OrchardCore.ContentPermissions.Services.IContentPermissionsService ContentPermissionsService

@if (ContentPermissionsService.CanAccess(Model.ContentItem))
{
    <p>Awesome content that you have permission to view.</p>
}
else
{
    <p>Unfortunately you're not able to view this content.</p>
}
```