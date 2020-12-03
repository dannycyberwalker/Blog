namespace Blog.Models.ViewModels
{
    public class ListViewModel
    {
        public int CountComments { get; set; }

        public Article Article { get; set; }
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }
        public string UserId { get; set; }
    }
}
