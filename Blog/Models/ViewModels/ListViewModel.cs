namespace Blog.Models.ViewModels
{
    public class ListViewModel
    {
        public int CountComments { get; set; }

        public Article Article { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public int UserId { get; set; }
    }
}
