using EntityFrameworkPlayground.Database;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main()
    {
        var running = true;

        do
        {
            Console.WriteLine("Woud you like to operate on Movies (movies), Actors (actors), or ActorMovie (actorMovies)?: ");
            var table = Console.ReadLine();

            switch (table)
            {
                case "movies":
                    HandleMovies(ref running);
                    break;
                case "actors":
                    HandleActors(ref running);
                    break;
                case "actorMovies":
                    HandleActorMovies(ref running);
                    break;
                default:
                    running = false;
                    break;
            }

            if (!running) break;

            Console.Write("Would you like to exit? (y/n): ");
            var key = Console.ReadKey();
            running = key.KeyChar.Equals('n');

            Console.WriteLine();
        } while (running);
    }
    #region Movies

    private static void HandleMovies(ref bool running)
    {
        Console.Write("Which CRUD (c/r/u/d) operation to you want to do on the Movie db?: ");
        var movieAnswer = Console.ReadLine();

        using var db = new MovieContext();

        switch (movieAnswer)
        {
            case "c":
                AddMovie(db);
                break;
            case "r":
                ListMovies(db);
                break;
            case "u":
                UpdateMovie(db);
                break;
            case "ua":
                UpdateReleaseYearForAllMovies(db);
                break;
            case "d":
                DeleteMovie(db);
                break;
            case "da":
                DeleteAllMovie(db);
                break;
            default:
                running = false;
                break;
        }
    }

    private static void ListMovies(MovieContext db)
    {
        //var query = from m in db.Movies
        //            orderby m.Name
        //            select m;

        var query = db.Movies
            .OrderBy(m => m.MovieId);

        Console.WriteLine("Current movies:");

        foreach (var movie in query)
        {
            Console.Write($"Id: {movie.MovieId}, Name: {movie.Name}, Year: {movie.ReleaseYear}\n");
        }
    }

    private static void AddMovie(MovieContext db)
    {
        Console.Write("Enter new Movie name: ");
        var name = Console.ReadLine();

        if (name is not null)
        {
            db.Movies.Add(new Movie { Name = name });
            db.SaveChanges();
        }

        ListMovies(db);
    }

    private static void UpdateMovie(MovieContext db)
    {
        ListMovies(db);

        Console.Write("Enter the id of the movie you would like to update: ");
        var id = Console.ReadLine();

        if (int.TryParse(id, out int parsedId))
        {
            var movieQuery = from m in db.Movies
                             where m.MovieId == parsedId
                             select m;

            var movieToBeUpdated = movieQuery.First();

            //(string PropertyName, Type PropertyType)[] properties = db.Movies.EntityType.GetProperties()
            //    .Select(p => (p.Name, p.PropertyInfo!.PropertyType))
            //    .ToArray();

            var propertyTypeDictionary = db.Movies.EntityType.GetProperties()
                .ToDictionary(p => p.Name, p => p.PropertyInfo!.PropertyType);

            Console.WriteLine("List of Properties:");
            foreach (var property in propertyTypeDictionary)
            {
                Console.WriteLine($"{property.Key}, {PrintPropertyType(property.Value)}");
            }

            Console.Write("Enter the property you would like to update: ");
            var propertyEntered = Console.ReadLine();

            if (propertyEntered != null)
            {
                var propertyType = propertyTypeDictionary.GetValueOrDefault(propertyEntered);

                if (propertyType != null)
                {
                    Console.Write($"Enter the updated value you would like for {propertyEntered}: ");
                    var value = Console.ReadLine();
                    var convertedValue = Convert.ChangeType(value, propertyType);

                    movieToBeUpdated.GetType().GetProperty(propertyEntered)!.SetValue(movieToBeUpdated, convertedValue);

                    db.SaveChanges();
                }
            }
        }

        ListMovies(db);
    }

    private static void UpdateReleaseYearForAllMovies(MovieContext db)
    {
        Console.Write("Enter the year you would like to set for all movies: ");
        var newReleaseYear = Console.ReadLine();

        if (int.TryParse(newReleaseYear, out int parsedNewReleaseYear))
        {
            db.Movies.ExecuteUpdate(setters => setters.SetProperty(m => m.ReleaseYear, parsedNewReleaseYear));

            // Creating new context so list shows properly since executeUpdate does not track updates
            using var db2 = new MovieContext();
            ListMovies(db2);
        }
    }

    private static void DeleteMovie(MovieContext db)
    {
        ListMovies(db);

        Console.Write("Enter the id of the movie you would like to delete: ");
        var id = Console.ReadLine();

        if (int.TryParse(id, out int parsedId))
        {
            var movie = db.Movies
                .Where(m => m.MovieId == parsedId)
                .First();

            db.Movies.Remove(movie);
            db.SaveChanges();
        }

        ListMovies(db);
    }

    private static void DeleteAllMovie(MovieContext db)
    {
        Console.Write("Are you sure you want to delete all movies (y/n): ");
        var areYouSure = Console.ReadLine();

        if (string.Equals(areYouSure ?? "", "y"))
        {
            db.Movies.ExecuteDelete();
        }

        // Creating new context so list shows properly since executeDelete does not track updates
        using var db2 = new MovieContext();
        ListMovies(db2);
    }

    #endregion

    #region Actors

    private static void HandleActors(ref bool running)
    {
        Console.Write("Which CRUD (c/r/u/d) operation to you want to do on the actor db?: ");
        var actorAnswer = Console.ReadLine();

        using var db = new MovieContext();

        switch (actorAnswer)
        {
            case "c":
                AddActor(db);
                break;
            case "r":
                ListActors(db);
                break;
            case "u":
                UpdateActor(db);
                break;
            case "addToMovie":
                AddActorToMovie(db);
                break;
            case "d":
                DeleteActor(db);
                break;
            default:
                running = false;
                break;
        }
    }

    private static void ListActors(MovieContext db)
    {
        var query = db.Actors
            .OrderBy(m => m.ActorId);

        Console.WriteLine("Current Actors:");

        foreach (var actor in query)
        {
            Console.Write($"Id: {actor.ActorId}, Name: {actor.Name}, Birth Year: {actor.BirthYear}\n");
        }
    }

    private static void AddActor(MovieContext db)
    {
        Console.Write("Enter new Actor name: ");
        var name = Console.ReadLine();

        if (name != null)
        {
            var newActor = new Actor { Name = name };
            db.Actors.Add(newActor);

            db.SaveChanges();
        }

        ListActors(db);
    }

    private static void UpdateActor(MovieContext db)
    {
        ListActors(db);

        Console.Write("Enter the id of the actor you would like to update: ");
        var id = Console.ReadLine();

        if (int.TryParse(id, out int parsedId))
        {
            var actorQuery = from m in db.Actors
                             where m.ActorId == parsedId
                             select m;

            var actorToBeUpdated = actorQuery.First();

            var test = db.Actors.EntityType.GetProperties();

            var propertyTypeDictionary = db.Actors.EntityType.GetProperties()
                .Where(p => p.PropertyInfo != null)
                .ToDictionary(p => p.Name, p => p.PropertyInfo!.PropertyType);

            Console.WriteLine("List of Properties:");
            foreach (var property in propertyTypeDictionary)
            {
                Console.WriteLine($"{property.Key}, {PrintPropertyType(property.Value)}");
            }

            Console.Write("Enter the property you would like to update: ");
            var propertyEntered = Console.ReadLine();

            if (propertyEntered != null)
            {
                var propertyType = propertyTypeDictionary.GetValueOrDefault(propertyEntered);

                if (propertyType != null)
                {
                    Console.Write($"Enter the updated value you would like for {propertyEntered}: ");
                    var value = Console.ReadLine();
                    var convertedValue = Convert.ChangeType(value, propertyType);

                    actorToBeUpdated.GetType().GetProperty(propertyEntered)!.SetValue(actorToBeUpdated, convertedValue);

                    db.SaveChanges();
                }
            }
        }

        ListActors(db);
    }

    private static void DeleteActor(MovieContext db)
    {
        ListActors(db);

        Console.Write("Enter the id of the actor you would like to delete: ");
        var id = Console.ReadLine();

        if (int.TryParse(id, out int parsedId))
        {
            var actor = db.Actors
                .Where(m => m.ActorId == parsedId)
                .First();

            db.Actors.Remove(actor);
            db.SaveChanges();
        }

        ListActors(db);
    }

    #endregion

    #region ActorMovie

    private static void HandleActorMovies(ref bool running)
    {
        Console.Write("Which supported CRUD (c/r/d) operation to you want to do on the ActorMovie db?: ");
        var actorMovieAnswer = Console.ReadLine();

        using var db = new MovieContext();

        switch (actorMovieAnswer)
        {
            case "c":
                AddActorToMovie(db);
                break;
            case "r":
                ListMovies(db);
                ListActorsForMovie(db);
                break;
            case "d":
                DeleteActorMovie(db);
                break;
            default:
                running = false;
                break;
        }
    }

    private static void AddActorToMovie(MovieContext db)
    {
        ListActors(db);

        Console.Write("Enter the id of the actor you would like to add: ");
        var id = Console.ReadLine();

        if (int.TryParse(id, out int parsedId))
        {
            var actor = db.Actors
                .First(a => a.ActorId == parsedId);

            ListMovies(db);

            Console.Write("Enter the id of the movie you would like to add the actor to: ");
            var movieId = Console.ReadLine();

            if (int.TryParse(movieId, out int parsedMovieId))
            {
                var movie = db.Movies
                    .First(m => m.MovieId == parsedMovieId);

                movie.Actors.Add(actor);
                db.SaveChanges();

                ListActorsForMovie(db, parsedMovieId);
            }
        }
    }

    private static void ListActorsForMovie(MovieContext db, int movieId = 0)
    {
        if (movieId == 0)
        {
            Console.Write("Enter the id of the movie you would like to see actors for: ");
            movieId = int.Parse(Console.ReadLine()!);
        }

        if (movieId != 0)
        {
            var movie = db.Movies
                .Include(m => m.Actors)
                .ThenInclude(a => a.Movies)
                .First(x => x.MovieId == movieId);

            foreach (var actor in movie.Actors)
            {
                Console.Write($"Id: {actor.ActorId}, Name: {actor.Name}, Birth Year: {actor.BirthYear}\n");
            }
        }
    }

    private static void DeleteActorMovie(MovieContext db)
    {
        ListMovies(db);

        Console.Write("Enter the id of the movie you would like to delete a actor from: ");
        var movieId = Console.ReadLine();

        if (int.TryParse(movieId, out int parsedMovieId))
        {
            ListActorsForMovie(db, parsedMovieId);

            Console.Write("Enter the id of the actor you would like to delete from the movie: ");
            var actorId = Console.ReadLine();

            if (int.TryParse(actorId, out int parsedActorId))
            {
                var movie = db.Movies.First(m => m.MovieId == parsedMovieId);
                var actor = db.Actors.First(a => a.ActorId == parsedActorId);

                movie.Actors.Remove(actor);

                db.SaveChanges();

                ListActorsForMovie(db, parsedMovieId);
            }
        }
    }

    #endregion

    #region Helpers

    private static string PrintPropertyType(Type propertyType)
    {
        var propertyTypeString = propertyType.ToString();
        return propertyTypeString.Substring(propertyTypeString.IndexOf("System.") + "System.".Length);
    }

    #endregion
};