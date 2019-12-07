using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class Download
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int FileId { get; set; }

        public int Count { get; set; }

        public User User { get; set; }
        public File File { get; set; }
    }
}
