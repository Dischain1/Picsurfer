﻿using Data.Model;
using System.Data.Entity;

namespace Data
{
    public class PicsurferContext : DbContext
    {
        public PicsurferContext() : base("PictureSurfer") { }

        public PicsurferContext(string connectionStr) : base(connectionStr){ }

        public DbSet<User> Users { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Rate> Rates { get; set; }
    }
}
