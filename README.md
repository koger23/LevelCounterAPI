# Level Counter Web API
Web API for Android Level Counter App written in .NET Core


### Purpose
To create a Web API for counting player's level and bonus for any games (e.g. for the card game Munchkin) working with Android Level Counter App


### Features on endpoints
- user can register then login
- user can host a game adding friends to it
- host can save and load the game
- friends can join to the host
- friends can manipulate their points real-time (with SignalR WebSockets)
- connect to players to make friends to play with
- discard connection request, ban players
- check their statistics (played game/time/rounds)
- users can edit their profile