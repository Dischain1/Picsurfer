﻿using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class Rate
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int PictureId { get; set; }
        public bool Like { get; set; }

        public User User { get; set; }
        public Picture Picture { get; set; }
    }
}
