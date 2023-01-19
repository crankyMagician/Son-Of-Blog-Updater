namespace SonOfBlogUpdater.Models
{
    public class BlogPostAuthor
    {
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
