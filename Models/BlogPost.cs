namespace SonOfBlogUpdater.Models
{
    public class BlogPost
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
     
     
        public ICollection<BlogPostMetaTag> BlogPostMetaTags { get; set; }
        public ICollection<BlogPostAuthor> BlogPostAuthors { get; set; }
      
    }
}
