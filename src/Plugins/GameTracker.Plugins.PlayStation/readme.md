# PlayStation Network Integration

This integration requires your PSN 'NPSSO' token to fetch owned games from your profile. 

To obtain this, first [log into the PlayStation network](https://www.playstation.com/). Once you are logged in, you can retrieve your token from [this](https://ca.account.sony.com/api/v1/ssocookie) link.

**Note:** Only your 10 most recently played titles are exposed by the PSN API. These include non-game titles (e.g. streaming services). These titles can be included or excluded from the results.

## Integration Features

The below grid indicates which fields are returned or supported by titles returned by this provider.

| Data Point | Available |
|--|--|
| Artwork | 🟢 Yes |
| Control Schemes | 🟠 Inferred - Controller only |
| Date/Time Last Played: | 🟢 Yes |
| Development Studio | 🔴 No |
| Description | 🔴 No |
| Direct Launch | 🔴 No |
| Gameplay Modes | 🔴 No |
| Genres | 🔴 No |
| Multiplayer Availability | 🔴 No |
| Platforms | 🟢 Yes |
| Playtime | 🔴 No |
| Publisher | 🔴 No |
| Release Date | 🔴 No |
| Reviews | 🔴 No |
| Tags | 🔴 No |