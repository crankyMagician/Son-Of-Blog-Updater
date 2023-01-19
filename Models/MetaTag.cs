namespace SonOfBlogUpdater.Models
{
    public class MetaTag
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public ICollection<BlogPostMetaTag> BlogPostMetaTags { get; set; }
    }
}
