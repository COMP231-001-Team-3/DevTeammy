﻿using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace teammy
{
    public partial class teammyEntities : DbContext
    {
        public teammyEntities()
            : base("name=teammyEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        //public virtual DbSet<assignee> assignees { get; set; }
        //public virtual DbSet<category> categories { get; set; }
        //public virtual DbSet<preference> preferences { get; set; }
        //public virtual DbSet<privilege> privileges { get; set; }
        //public virtual DbSet<progress_indicators> progress_indicators { get; set; }
        //public virtual DbSet<project> projects { get; set; }
        //public virtual DbSet<task> tasks { get; set; }
        //public virtual DbSet<team_mates> team_mates { get; set; }
        //public virtual DbSet<team> teams { get; set; }
        //public virtual DbSet<user> users { get; set; }
    }
}
