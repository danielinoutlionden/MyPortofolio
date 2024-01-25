using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Backtowork
{


    public class SavedataContext : DbContext
    {
        public DbSet<mahasiswa> Students { get; set; }

        public string DbPath { get; }

        public SavedataContext()
        { 
            var path = Environment.CurrentDirectory;
            DbPath = System.IO.Path.Join(path, "Savedata.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

    public class mahasiswa
    {
        public int Id { get; set; }
        public string text { get; set; } 
    }

}
