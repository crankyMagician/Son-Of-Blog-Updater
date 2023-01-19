namespace SonOfBlogUpdater.Models
{
    public class BlogPostMetaTag
    {
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public int MetaTagId { get; set; }
        public MetaTag MetaTag { get; set; }
    }
}
