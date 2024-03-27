namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface ISendEmailRepository
    {
        Task Sendemail(string email, string subject, string message);
        Task SendEmailwithAttachments(string email, string subject, string message, List<string> attachmentPaths);
    }
}
