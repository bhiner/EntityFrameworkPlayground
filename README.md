# Setup and Run
## Database
### Create a SQLite database and apply migrations
1. Run `dotnet ef database update`

## Running it locally
1. Run `dotnet run`

## Running Console Application
### Movie Commands
- `c` Add Movie
- `r` List Movies
- `u` Update Movie
- `ua` Update Release Year for All Movies
- `d` Delete Movie
- `da` Delete All Movies

### Actor Commands
- `c` Add Actor
- `r` List Actors
- `u` Update Actor
- `addToMovie` Add Actor to Movie
- `d` Delete Actor

### ActorMovie Commands
- `c` Add Actor To Movie
- `r` List Actors For Movie
- `d` Delete Actor Movie

# DB Structure and Example Data
## Movies
| MovieId | Name | ReleaseYear |
| ----------- | ----------- | ----------- |
| 1 | Godzilla | 2014 |
| 2 | Godzilla: King of the Monsters | 2019 |
| 3 | Godzilla vs Kong | 2021 |
| 4 | Godzilla Minus One | 2023 |

## Actors
| ActorId | BirthYear | Name |
| ----------- | ----------- | ----------- |
| 1 | 1965 | Kyle Chandler |
| 2 | 1973 | Vera Farmiga |
| 3 | 2004 | Vera Farmiga |

## ActorMovie
| ActorsActorId | MoviesMovieId |
| ----------- | ----------- |
| 1 | 1 |
| 1 | 2 |
| 1 | 3 |
| 2 | 1 |
| 2 | 2 |
| 3 | 3 |
