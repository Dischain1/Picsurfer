using System;

namespace Data
{
    public class Rate
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int PictureId { get; set; }
        public bool Like { get; set; }

        public User User { get; set; }
        public Picture Picture { get; set; }
    }
}
