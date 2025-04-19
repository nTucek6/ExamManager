using System.Net.Mail;
using System.Net;
using DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Services.EmailSender
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly ExamManagerContext database;

        private readonly string smtpServer = "smtp.ethereal.email";
        private readonly int smtpPort = 587;
        private readonly string smtpUser = "ardella.jaskolski59@ethereal.email";
        private readonly string smtpPass = "z9ZN5tUHDfD5R6xCH5";

        private readonly string subject = "Obavijest o ispitu za sutra iz predmeta: ";

        public EmailSenderService(ExamManagerContext examManagerContext) {
        database = examManagerContext;
        }


        public async Task SendEmailToStudents()
        {
            var tomorrow = DateTime.UtcNow.Date.AddDays(1);
            var dayAfter = tomorrow.AddDays(1);

            var exams = await database.Exams.Where(q => q.DeadlineDate >= tomorrow.ToUniversalTime() && q.DeadlineDate < dayAfter.ToUniversalTime()).Select(s=> s.Id).ToListAsync();

            if(exams.Count > 0)
            {
                var registeredExams = await database.ExamRegistrations.Where(q => exams.Contains(q.ExamId)).ToListAsync();

                if(registeredExams.Count > 0)
                {
                    foreach (var exam in registeredExams)
                    {
                        var student = await database.Users.Where(q => q.Id == exam.StudentId).FirstOrDefaultAsync();
                        var examDb = await database.Exams.Where(q => q.Id == exam.ExamId).FirstOrDefaultAsync();
                        var subjectDb = await database.Subjects.Where(q => q.Id == examDb.SubjectId).FirstOrDefaultAsync();

                        string header = subject + subjectDb.Subject;

                        string message = $"<h3>Poštovani {student.Name},</h3></br>" +
                            $"<p>Sutra se održava ispit iz predmeta: {subjectDb.Subject} u {examDb.DeadlineDate}.</p>" +
                            $"<p>Ispit će se održati na lokaciji: {examDb.ExamLocation}</p>" +
                            $"</br><h4>S poštovanjem.</h4>";

                        await SendEmail(student.Email, header, message);
                    }
                }
            } 
        }


        public async Task SendEmail(string toEmail, string subject, string message)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(smtpUser);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;

            using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mail);
            }
        }

    }
}
