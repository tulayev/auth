namespace Web_App_Identity.Services
{
    public interface IEmailService
    {
        Task SendAsync(string from, string to, string subject, string body);
    }
}