# GameTracker

GameTracker is a project I started with the intent of aggregating all of my game libraries across multiple storefronts and platforms into a single interface, from which I can do the following:

 - **Filter** - e.g. find adventure games, with less than 2 hours of playtime, rated over 90% by critics.
 - **Sort** - e.g. by rating, by hours played, by title, etc.
 - **Launch** a title directly from the interface.
 - **View** statistics about my game library in the form of charts (e.g. played vs unplayed, broken down by platform, genre, storefront, etc).

## Tech Stack

 - .NET 6.0 LTS
 - Blazor Server (UI)
 - SQLite (cached game data, user accounts)

## Setup Instructions

The endgame for this project is for it to run as a container hosted on a local network, so that it can be accessed by multiple devices on the same LAN e.g. PC, laptop, Steam Deck, etc. 

Whilst it is of course technically possible to expose this service externally over the internet, **I definitely would not recommend it**.

Until the image is set up, the easiest way to run it is directly from Visual Studio. Before you launch it for the first time, however, the SQLite database needs to be initialized. From a PowerShell prompt (within the src folder, or from Visual Studio), run:

`dotnet ef database update`

This will scaffold the SQLite database file and user account tables that the authentication middleware requires for registration/login functionality to work.

Once done, set **GameTracker.Frontend** as the startup project, and run it.

You need to register an account first (use any email and password you like, it doesn't matter - no emails will be sent), once done you can log in and start configuring your game library.

## Integrations

Currently, this project supports importing game libraries from the following providers:

 - Epic Games
 - GOG
 - Nintendo eShop
 - PlayStation Network
 - Steam
 - Xbox

Not all game providers expose the same level of detail, and there are some caveats that apply to certain providers. For more details, see the readme files within each provider's plugin directory.

## What works (mostly!):

 - User accounts, so that multiple users can each manage their own aggregated game libraries.
 - Basic grid view of your aggregated game library.
 - Filtering your library based on certain criteria.
 - Selecting a title for more information.
 - Launching a title directly from the UI.
	 - Not all titles or providers support direct launching.
 - 'Stats for Nerds' page, with doughnut charts showing statistics about your aggregated library.

## What doesn't work (yet!)

 - Library sorting!
 - The grid view sometimes does not update when a filter is changed, selecting another UI element or changing another filter may cause the state to update when this happens.

## What needs to be added:

 - Multiple review sources (currently we only have Metacritic data from Steam titles).
 - Additional filter options.
 - Additional data sources to supplement providers that do not expose basic game details e.g. description, genres, etc.
 - ??? (Suggestions welcome!)