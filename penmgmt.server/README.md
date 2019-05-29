## How to run Server API (penmgmt.server.api)?
- Download server api docker image from Docker Hub
- Download docker-compose.yaml from Server (penmgmt.server)
- Edit docker-compose.yaml
    - Update source path in docker-compose.yaml for Sqlite dB folder (services > penmgmt-server-api > volumes > source)
- Open commnd prompt and traverse to to folder where docker-compose.yaml is downloaded
    - Traverse to folder where docker-compose.yaml is downloaded
    - Run "docker-compose up"
    - Ctrl+C to stop the Server

## DotNet CLI Commands

Create a new solution
```
dotnet new sln
```

New class library project. Project name will be same as current folder name in which the command is run
```
dotnet new classlib -f netstandard2.0
```

Add existing project to solution
```
dotnet sln .\penmgmt.server.sln add .\penmgmt.server.api\penmgmt.server.api.csproj
```

Add project reference to existing project
```
dotnet add .\penmgmt.server.persistence.sqlite.csproj reference ..\penmgmt.server.persistence\penmgmt.server.persistence.csproj
```

Add packages
```
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package penmgmt.common.domain -s E:\Projects\NGC1976\penmgmt\code\NuGetServer\packages
```

Add customer local package
```
nuget add ".\penmgmt.common.domain.0.1.0.nupkg" -source "E:\Projects\NGC1976\penmgmt\code\NuGetServer\packages"
```


# EF Core

Run following to scaffold a migration and create the initial set of tables for the model
```
dotnet ef migrations add InitialCreate
```

Run following to apply the new migration to the database. This command creates the database before applying migrations
```
dotnet ef database update
```

If you make changes to the model, you can use the following command to scaffold a new migration
Once you have checked the scaffolded code (and made any required changes), you can use the following command to apply the schema changes to the database
```
dotnet ef migrations add
dotnet ef database update 
```

**Notes**
> EF Core uses a "__EFMigrationsHistory" table in the database to keep track of which migrations have already been applied to the database

> The SQLite database engine doesn't support certain schema changes that are supported by most other relational databases. For example, the "DropColumn" operation is not supported. EF Core Migrations will generate code for these operations. But if you try to apply them to a database or generate a script, EF Core throws exceptions. See SQLite Limitations. For new development, consider dropping the database and creating a new one rather than using migrations when the model changes.


## ASP.NET Core Configurations

https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.2


## Docker Commands

Cleanup stopped containers
```
docker container prune
```

Cleanup intermediate images creating during build process
```
docker image rm $(docker images -q -f dangling=true)
```

Builds Docker image
```
docker build --rm -f "Dockerfile" -t penmgmt.api.dev:001 .
```

Runs Docker image
```
winpty docker run -it --rm -p 9091:80 penmgmt.api.dev:001
```
