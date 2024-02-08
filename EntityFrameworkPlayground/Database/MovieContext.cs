using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.Database;

internal class MovieContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Actor> Actors { get; set; }

    public string DbPath { get; }

    public MovieContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "movie.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class Movie
{
    public int MovieId { get; set; }
    public string Name { get; set; } = "";

    public int ReleaseYear { get; set; }

    public List<Actor> Actors { get; } = [];
}

public class Actor
{
    public int ActorId { get; set; }
    public string Name { get; set; } = "";
    public ushort BirthYear { get; set; }
    public List<Movie> Movies { get; } = [];
}