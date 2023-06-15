# GOG Integration

This integration requires an authentication code which you can retrieve by visiting [this GOG URL](https://auth.gog.com/auth?client_id=46899977096215655&redirect_uri=https://embed.gog.com/on_login_success?origin=client&response_type=code&layout=client2), pressing F12 to open your browser dev console, logging in to the page with your GOG details, and then capturing the **code** parameter returned in the response header from the 'Network' tab of the dev console.

For more details, see [here](https://gogapidocs.readthedocs.io/en/latest/auth.html).

**Note:** Because the authentication code is short-lived, it is not persisted between sessions. Hence, you will need to manually refresh this integration for each session.

## Integration Features

The below grid indicates which fields are returned or supported by titles returned by this provider.

| Data Point | Available |
|--|--|
| Artwork | 🟠 Limited resolution |
| Control Schemes | 🔴 No |
| Date/Time Last Played: | 🔴 No |
| Development Studio | 🔴 No |
| Description | 🔴 No |
| Direct Launch | 🟢 Yes |
| Gameplay Modes | 🔴 No |
| Genres | 🔴 No |
| Multiplayer Availability | 🔴 No |
| Platforms | 🟢 Yes |
| Playtime | 🔴 No |
| Publisher | 🔴 No |
| Release Date | 🟢 Yes |
| Reviews | 🔴 No |
| Tags | 🔴 No |