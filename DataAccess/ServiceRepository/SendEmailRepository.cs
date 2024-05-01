using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using HelloDoc;
using System.Collections;
using System.Net;
using System.Net.Mail;

namespace DataAccess.ServiceRepository
{
    public class SendEmailRepository : ISendEmailRepository
    {
        private readonly HelloDocDbContext _context;
        private readonly IAdminRepository _admin;
        private readonly IPhysicianRepository _physician;
        public SendEmailRepository(HelloDocDbContext helloDocDbContext, IAdminRepository adminRepository, IPhysicianRepository physicianRepository)
        {
            _context = helloDocDbContext;
            _admin = adminRepository;
            _physician = physicianRepository;
        }
        public Task Sendemail(string email, string subject, string message)
        {
            var mail = "tatva.dotnet.yashpatel@outlook.com";
            var password = "Yash@7046";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            if (_admin.GetSessionAdminId() != -1)
            {
                Emaillog emaillog = new Emaillog();
                emaillog.Emailid = email;
                emaillog.Sentdate = DateTime.Now;
                emaillog.Createdate = DateTime.Now;
                emaillog.Subjectname = subject;
                emaillog.Emailtemplate = message;
                emaillog.Roleid = 1;
                emaillog.Adminid = _admin.GetSessionAdminId();
                BitArray fortrue = new BitArray(1);
                fortrue[0] = true;
                emaillog.Isemailsent = fortrue;
                emaillog.Senttries = 1;
                _context.Emaillogs.Add(emaillog);
                _context.SaveChanges();
            }
            if (_physician.GetSessionPhysicianId() != -1)
            {
                Emaillog emaillog = new Emaillog();
                emaillog.Emailid = email;
                emaillog.Sentdate = DateTime.Now;
                emaillog.Createdate = DateTime.Now;
                emaillog.Subjectname = subject;
                emaillog.Emailtemplate = message;
                emaillog.Roleid = 2;
                emaillog.Physicianid = _physician.GetSessionPhysicianId();
                BitArray fortrue = new BitArray(1);
                fortrue[0] = true;
                emaillog.Isemailsent = fortrue;
                emaillog.Senttries = 1;
                _context.Emaillogs.Add(emaillog);
                _context.SaveChanges();
            }
            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }

        public async Task SendEmailwithAttachments(string email, string subject, string message, List<string> attachmentPaths)
        {
            try
            {
                var mail = "tatva.dotnet.yashpatel@outlook.com";
                var password = "Yash@7046";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(mail),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true // Set to true if your message contains HTML
                };

                mailMessage.To.Add(email);

                foreach (var attachmentPath in attachmentPaths)
                {
                    if (!string.IsNullOrEmpty(attachmentPath))
                    {
                        var attachment = new Attachment(attachmentPath);
                        mailMessage.Attachments.Add(attachment);
                    }
                }

                await client.SendMailAsync(mailMessage);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public void Sendsms(string phone, string subject, string message, int p)
        {
            Smslog smslog = new Smslog();
            if (p != 0) {
                smslog.Action = p;
            }
            smslog.Mobilenumber = phone;
            smslog.Sentdate = DateTime.Now;
            smslog.Createdate = DateTime.Now;
            smslog.Smstemplate = message;
            if (_admin.GetSessionAdminId() != -1)
            {
            smslog.Roleid = 1;
                smslog.Adminid = _admin.GetSessionAdminId();
            }
            if (_physician.GetSessionPhysicianId() != -1)
            {
                smslog.Roleid = 2;

                smslog.Physicianid = _physician.GetSessionPhysicianId();
            }
                BitArray fortrue = new BitArray(1);
            fortrue[0] = true;
            smslog.Issmssent = fortrue;
            smslog.Senttries = 1;
            _context.Smslogs.Add(smslog);
            _context.SaveChanges();
        }
    }
}
