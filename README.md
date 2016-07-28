# ASP.NET Core Demo with SignalR

This is a simple showcase of a trivial multiplayer game using SignalR. It also showcases many of the new features of ASP.NET Core, including:+1:

- Strongly-typed Options
- Cascading configuration
- Custom Tag Helpers
- View dependency injection
- Bundling/minifying

Run the app:

```
dotnet restore
dotnet run
```

In Visual Studio it will run under IIS Express. You can also publish it to Azure to test it out 
(be sure to enable Web Sockets in Azure, probably will need S1 or above).

Join the game by typing a name and press the arrow keys to move your player. Other players will be shown
and updated when they move.

To keep things simple, moves are step-based (32px) and bounded to the map. Player state is stored in a static dictionary.

**THIS IS NOT AN EXAMPLE OF HOW TO MAKE A MULTIPLAYER GAME.** It is merely a fun exercise to show .NET Core and SignalR working
together.