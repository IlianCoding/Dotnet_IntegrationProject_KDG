using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace NT.BL.services;

public class EmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    

    public async Task SendMailForKnowledgeOfUserCreation(string email, string emailSubject, string htmlMessage,
        string firstName, string lastName)
    {
        var apiKey = Environment.GetEnvironmentVariable("API_KEY");
        var fromName = "Phygital";
        var fromEmail = "ilian.elst@student.kdg.be";
        var toEmail = email;
        var subject = emailSubject;
        var body = htmlMessage;

        SmtpClient smtpClient = new SmtpClient()
        {
            Host = "smtp.sendgrid.net",
            Port = 587,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("apikey", apiKey)
        };

        var message = new MailMessage()
        {
            From = new MailAddress(fromEmail, fromName),
            To = { new MailAddress(toEmail, $"{firstName} {lastName}") },
            Subject = subject,
            IsBodyHtml = true,
            Body = body
        };

        try
        {
            await smtpClient.SendMailAsync(message);
            Console.WriteLine("Email Verzonden");
        }
        catch (Exception e)
        {
            Console.WriteLine("Fout bij verzenden van e-mail: " + e.Message);
        }
    }

    public async Task SendEmailFor2FactorAuthentication(string token, string emailSubject, IdentityUser user)
    {
        var apiKey = Environment.GetEnvironmentVariable("API_KEY");
        var fromName = "Phygital";
        var fromEmail = "ilian.elst@student.kdg.be";
        var toEmail = user.Email;
        var subject = emailSubject;
        var body = $"Hello this is your 2 Factor token: {token}";

        SmtpClient smtpClient = new SmtpClient()
        {
            Host = "smtp.sendgrid.net",
            Port = 587,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("apikey", apiKey)
        };

        var message = new MailMessage()
        {
            From = new MailAddress(fromEmail, fromName),
            To = { new MailAddress(toEmail, $"{user.UserName}") },
            Subject = subject,
            IsBodyHtml = true,
            Body = body
        };

        try
        {
            await smtpClient.SendMailAsync(message);
            Console.WriteLine("Email Verzonden");
        }
        catch (Exception e)
        {
            Console.WriteLine("Fout bij verzenden van e-mail: " + e.Message);
        }
    }
}