using System;
using MailKit.Net.Smtp;
using MimeKit;

namespace OutlookMeetingTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var message = new MimeMessage();


            message.From.Add(new MailboxAddress("Meeting Organizer", "your_email@outlook.com"));
            message.To.Add(new MailboxAddress("Attendee Name", "receiver_email@gmail.com"));
            message.Subject = "Urgent Meeting Request";


            string meetingRequest =
                "BEGIN:VCALENDAR\r\n" +
                "VERSION:2.0\r\n" +
                "METHOD:REQUEST\r\n" +
                "BEGIN:VEVENT\r\n" +
                "DTSTART:20260501T150000Z\r\n" +
                "DTEND:20260501T160000Z\r\n" +
                "SUMMARY:Outlook Test Meeting\r\n" +
                "DESCRIPTION:This is a meeting request sent via C# SMTP.\r\n" +
                "ORGANIZER;CN=Organizer:mailto:your_email@outlook.com\r\n" +
                "ATTENDEE;RSVP=TRUE:mailto:receiver_email@gmail.com\r\n" +
                "END:VEVENT\r\n" +
                "END:VCALENDAR";

            var calendarPart = new TextPart("calendar") { Text = meetingRequest };
            calendarPart.ContentType.Parameters.Add("method", "REQUEST");
            calendarPart.ContentType.Parameters.Add("component", "VEVENT");

            message.Body = calendarPart;

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.office365.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                    client.Authenticate("your_email@outlook.com", "password");
                    client.Send(message);
                    Console.WriteLine("Meeting request sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occurred: " + ex.Message);
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
        }
    }
}
