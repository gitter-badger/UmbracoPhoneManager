Umbraco Phone Manager &middot; ![version](https://img.shields.io/badge/version-1.0.2-orange.svg) + Personalisation Groups plugin &middot; ![version](https://img.shields.io/badge/version-1.0.2-orange.svg)
=====================================================

**Umbraco Phone Manager** is an Umbraco package to manage and display marketing campaign phone numbers on your site to your visitors. Based on various criteria, a relevant telephone number can be automatically displayed to each user visiting your website based on how they got there.

The package is designed for organisations who use multiple telephone numbers to track the marketing source of a visitor.

The criteria include:
- Custom querystrings (such as used by various campaign systems)
- Referrer domain (e.g. a search engine)
- The page the user first enters your site on
- ...or a combination of any of the above.

The telephone number (and some campaign detail properties) the visitor gets is persisted by default for their current session, but this can be extended (via cookie) for a set number of days (specified in the settings), so every time they return to the site they see the original telephone number, irrespective of how they get to your site in future. It is also possible to override an exisiting persisting campaign cookie if required.


## Links

- <a href="#installation">Installation</a>
- <a href="#implementing">Implementing</a>
- <a href="#personalisation-groups-plugin">Personalisation Groups plugin</a>
- <a href="#extending">Extending</a>
- <a href="#acknowledgements">Acknowledgements</a>
- <a href="#version-history">Version History</a>


## Installation

Install the package through the Umbraco backoffice:
- In Developer -> Packages backoffice. Search for 'Phone Manager', in Website utilities
- or download the package as a zip from [**https://our.umbraco.org/projects/website-utilities/phone-manager**][OurUmbraco]. Manually install via 'Install local' in your Umbraco package backoffice.
- or download the package as a zip from this project's GitHub page [**https://github.com/willroscoe/UmbracoPhoneManager/blob/master/releases/umbraco**][GitHubRelease]. Manually install via 'Install local' in your Umbraco package backoffice.

or via NuGet:
- ```PM> Install-Package UmbracoPhoneManager```
- or manually download the [**NuGet Package**][NuGetPackage]. Install the NuGet package in your Visual Studio project. Please note that required Document Types and Data Types are not included in the Nuget Package so it is recommended to install the Package through the Umbraco backoffice before installing the Nuget package.

[NuGetPackage]: https://www.nuget.org/packages/UmbracoPhoneManager/
[OurUmbraco]: https://our.umbraco.org/projects/website-utilities/phone-manager/
[GitHubRelease]: https://github.com/willroscoe/UmbracoPhoneManager/blob/master/releases/umbraco


## Implementing

The first step is to add one 'Phone Manager' document type to your content structure. It is recommended to add it once below your site homepage(s) (if you have multiple sites, you can add it to all your sites to allow you to set up separate campaign details per site). **You might need to update your homepage(s) content type permissions to allow 'Phone Manager' as a child.**
You will probably also want to create a 'custom list view' on the 'Phone Manager' document type so you can see relevant phone details easily. Some useful field you could use are: telephoneNumber; campaingCode; isDefault. You will also want to hide the Phone Manager item from your font-end navigation menu/site map!

**Note:** If you want to place the Phone Manager in the root of you Umbraco site (i.e. above the homepage) you will need to add the following line to you web.config appSettings:
```<add key="phoneManager.EnablePhoneManagerInRoot" value="true"/>```

![Phone Manager overview](/assets/phonemanager.jpg?raw=true "Phone Manager overview")

Then add a new Campaign detail record for each possible user. You must include a Telephone number in each record. **Please note that you will need to add a 'default' record (with 'IsDefault' checked) to cater for any general site visitors who are not coming from a campaign.**

![Phone Manager detail](/assets/campaigndetail.jpg?raw=true "Phone Manager detail")


**Finally**, you need to set a start node for the **'Phone Manager - Campaign detail - Entry page - Content Picker'** data type (in Developer -> Data Types). Choose your Homepage node.


### Setup Templates

You will then need to update any templates or views where you want the telephone number displayed. You can do this to any content which inherits from IPublishedContent. The package extends IPublishedContent to include the following detail:
- Telephone number for the campaign
- Campaign Code
- Alternative Marketing Code

It is added like this:
```
	<div class="phonenumber">@Model.Content.PhoneManager().TelephoneNumber</div>
```

If needed you can also display or use the folling properties:
```
	@Model.Content.PhoneManager().Id (this is the nodeId of the selected campaign detail)
	@Model.Content.PhoneManager().CampaignCode
	@Model.Content.PhoneManager().AltMarketingCode
```


## Personalisation Groups plugin

Supplied as a separate package, the Phone Manager Personalisation Groups plugin package adds 'Phone Mananger' as a new criteria for the [**Personalisation Groups package**][PersonalisationGroupsLink]. This will allow you to personalise your site based on which Phone Manager campaign a user is currently linked to.

![Phone Manager Personalisation Groups plugin](/assets/personalisationplugin.jpg?raw=true "Phone Manager Personalisation Groups plugin")

[PersonalisationGroupsLink]: https://our.umbraco.org/projects/website-utilities/personalisation-groups/

You will need to have already installed the Phone Manager package and the [**Personalisation Groups package**][PersonalisationGroupsLink] before installing the plugin package.

Install the package through the Umbraco backoffice:
- In Developer -> Packages backoffice. Search for 'Phone Manager Personalisation Groups plugin', in Website utilities
- or download the package as a zip from [**https://our.umbraco.org/projects/website-utilities/phone-manager-personalisation-groups-plugin/**][OurUmbracoPersonalisationPackage]. Manually install via 'Install local' in your Umbraco package backoffice.
- or download the package as a zip from this project's GitHub page [**https://github.com/willroscoe/UmbracoPhoneManager/blob/master/releases/umbraco**][GitHubPersonalisationRelease]. Manually install via 'Install local' in your Umbraco package backoffice.

or via NuGet:
- ```PM> Install-Package UmbracoPhoneManagerPersonalisationGroupsPlugin```
- or manually download the [**NuGet Package**][NuGetPersonalisationPackage] - Install the NuGet package in your Visual Studio project.

[NuGetPersonalisationPackage]: https://www.nuget.org/packages/UmbracoPhoneManagerPersonalisationGroupsPlugin/
[OurUmbracoPersonalisationPackage]: https://our.umbraco.org/projects/website-utilities/phone-manager-personalisation-groups-plugin/
[GitHubPersonalisationRelease]: https://github.com/willroscoe/UmbracoPhoneManager/blob/master/releases/umbraco


## Extending

It is possible to extend the package with additional campaign criteria.
- In your project, reference the Phone Manager project
- If you need to add additioanl fields to the Campaign Detail document type then you will need to also extend the partial class /Models/CampaignPhoneManagerModel using the required attributes that are used on the existing properties.
- Create a class inheriting from IPhoneManagerCriteria.
- Extend the currently used storage repository (the default is XPathRepository) with a method to select matching campaign detail records.
- Add the following line to you web.config appSettings:
```<add key="phoneManager.DiscoverNewCriteria" value="true"/>``` 
- Add some unit test methods to check your logic is correct :)


## Acknowledgements

Inspiration from the [**Personalisation Groups package**][PersonalisationGroupsLink].


## Version History

### Phone Manager
- 1.0.1
	- Remove unnecessary dependancies
	- Added umbraco package icon
- 1.0.0
	- Initial release

### Personalisation Groups plugin
- 1.0.1
	- Remove unnecessary dependancies
	- Added umbraco package icon
- 1.0.0
	- Initial release
