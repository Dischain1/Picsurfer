using Data.Model;

namespace Picsurfer.Models
{
    public class RatedPicture
    {
        public Picture Picture;
        public int Rating;
        public int Likes;
        public int Dislikes;
    }

    public enum UserRate
    {
        Dislike = -1,
        NotRated = 0,
        Like = 1
    }

    public class UserRatedPicture
    {
        public Picture Picture;
        public UserRate UserRate;
    }
}