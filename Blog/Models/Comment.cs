

namespace Blog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public System.DateTime CreateTime { get; set; }

        public User Author { get; set; }
        public string UserId { get; set; }

        public Article Article { get; set; }
        public int ArticleId { get; set; }
        public Comment()
        {
            CreateTime = System.DateTime.UtcNow;
        }
    }
}
