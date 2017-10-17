# movies
get and store now playing movies from themoviedb.org

Project 

-built in Visual Studio 2017 (C# console app .NET core 2)
-.NET Core download - https://www.microsoft.com/net/download/core
-might need a download of NuGet packages - https://docs.microsoft.com/en-us/nuget/consume-packages/package-restore
-app.config file for configurations about Apis, Database Connection
-publish and run from console guide: https://docs.microsoft.com/en-us/dotnet/core/tutorials/publishing-with-visual-studio


Database

- PostgreSQL 10
- see movies.sql file for DDL
- tables use:

  movie: store running movies (column last_update is updated on every insert/update process)
  crew: store movie persons
  movie_crew: store relation between directors and movies
