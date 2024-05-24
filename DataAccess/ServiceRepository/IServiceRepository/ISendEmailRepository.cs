namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface ISendEmailRepository
    {
        Task Sendemail(string email, string subject, string message);
        void Sendsms(string phone, string subject, string message, int physicianid);
        Task SendEmailwithAttachments(string email, string subject, string message, List<string> attachmentPaths);
    }
}
