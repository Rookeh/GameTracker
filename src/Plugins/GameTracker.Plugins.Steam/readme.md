# Steam Integration

This integration requires your Steam Web API key for authentication, and your profile's SteamID64 value to fetch game data.

To obtain a Steam Web API key, visit [this](https://steamcommunity.com/dev/apikey) page.
To find your SteamID64 value, use a tool like [steamid.io](https://steamid.io).

## Special Notes

Due to the propensity for most Steam users to have large game libraries, combined with the 'unique' design and rate limits imposed by Valve's API, this integration loads game titles slightly differently from others:

 1. On startup, or when you select 'Load Games' from the provider menu, this integration will fetch basic details for all the games linked to your Steam account - this consists of the application ID, title and playtime. These basic fields will be available to query/filter on immediately.
 2. When the game library is opened, details and artwork for Steam games are lazily loaded. This is because Valve does not provide an API endpoint to bulk query for game details (e.g. description, genre, rating, artwork, etc), and each title has to be requested individually.
 3. As such, only games for which extended details have been populated will have those fields available to query/filter on in the game library view.
 4. Due to rate limits, only 18 game detail requests can be made every 5 minutes. Once this limit is reached, any further Steam titles will show a placeholder image and description in their details pane until further requests can be made.
 5. All Steam game details are cached and, over time, the entire Steam library will be populated and no further API calls will be required, except for the initial call to verify which titles you own.
 6. **Note:** Even with this quite aggressive client-side throttling, it may still be possible to be rate-limited by Steam's API. If this happens, a toast message will notify you.

## Integration Features

The below grid indicates which fields are returned or supported by titles returned by this provider.

| Data Point | Available |
|--|--|
| Artwork | 🟢 Yes |
| Control Schemes | 🟢 Yes |
| Date/Time Last Played: | 🟢 Yes |
| Development Studio | 🟢 Yes |
| Description | 🟢 Yes |
| Direct Launch | 🟢 Yes |
| Gameplay Modes | 🟢 Yes |
| Genres | 🟢 Yes |
| Multiplayer Availability | 🟢 Yes |
| Platforms | 🟢 Yes |
| Playtime | 🟢 Yes |
| Publisher | 🟢 Yes |
| Release Date | 🟢 Yes |
| Reviews | 🟢 Yes |
| Tags | 🟢 Yes |