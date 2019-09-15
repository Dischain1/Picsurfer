using System.Data.Entity;

namespace Data
{
    public class PictureSurferContext : DbContext
    {
        public PictureSurferContext() : base("PictureSurfer")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Rate> Rates { get; set; }
    }
}
