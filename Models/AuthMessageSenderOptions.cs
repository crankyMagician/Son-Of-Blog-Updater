namespace SonOfBlogUpdater.Models
{
    /*
     * https://learn.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-7.0&tabs=visual-studio
     * This is to send emails using sendgrid
     * 
     */
    public class AuthMessageSenderOptions
    {
        public string? SendGridKey { get; set; }
    }
}
