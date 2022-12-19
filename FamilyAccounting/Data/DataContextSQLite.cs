using FamilyAccounting.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyAccounting.Data
{
    public class DataContextSQLite : DbContext
    {
        static DataContextSQLite()
        {
            Database.SetInitializer(new NullDatabaseInitializer<DataContextSQLite>());
        }
        public DataContextSQLite()
            : base("name=dbSQLite")
        {
            Configuration.AutoDetectChangesEnabled = false;
            Database.CommandTimeout = Convert.ToInt32(TimeSpan.FromMinutes(20).TotalSeconds);


 
        }
 
        protected override void OnModelCreating(DbModelBuilder builder)
        {
          

            builder.Entity<Usages>()
                .ToTable($"Usages")
                .HasKey(t => t.id);
            builder.Entity<Categori>().ToTable($"Categories")
                .HasKey(t => t.id);
            builder.Entity<Family>().ToTable($"Family")
                .HasKey(t => t.id);
            builder.Entity<Source>().ToTable($"Sources")
                .HasKey(t => t.id);
            builder.Entity<Models.Data>().ToTable($"Data")
                .HasKey(t => t.Param);


        }
        public DbSet<Usages> Usages { get; set; }
        public DbSet<Categori> Categories { get; set; }
        public DbSet<Family> Family { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Models.Data> Data { get; set; }


    }
}
