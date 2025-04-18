﻿using System.Net.Mail;
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
        private readonly string smtpUser = "selmer30@ethereal.email";
        private readonly string smtpPass = "BmFBRr6sbwJszPdzVS";

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

                        string message = $"Poštovani {student.Name}" +
                            $"\n\n Sutra se održava ispit iz predmeta: {subjectDb.Subject} u {examDb.DeadlineDate}." +
                            $"\n\nIspit će se održati na lokaciji: {examDb.ExamLocation}" +
                            $"\n\n S poštovanjem.";

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
            mail.IsBodyHtml = true; // Set false if you're sending plain text

            using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mail);
            }
        }

    }
}
