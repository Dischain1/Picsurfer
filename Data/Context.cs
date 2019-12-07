using Data.Model;
using System.Data.Entity;

namespace Data
{
    public class FileHubContext : DbContext
    {
        public FileHubContext() : base("FileHub") { }

        public FileHubContext(string connectionStr) : base(connectionStr){ }

        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Download> Downloads { get; set; }
    }
}
