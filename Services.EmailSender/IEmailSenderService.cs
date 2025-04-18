namespace Services.EmailSender
{
    public interface IEmailSenderService
    {
        public Task SendEmailToStudents();
        public Task SendEmail(string toEmail, string subject, string message);

    }
}
