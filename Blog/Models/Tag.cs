
namespace Blog.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Article Article { get; set; }
        public int ArticleId { get; set; }
    }
}
