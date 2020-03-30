using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ATCPortal
{
    public class Email
    {

        public static string SendEmail(string title, string Msg, string DestEmail)
        {
            String userName = "q@amerinode.com";
            String password = "Ilikechocolate5!";
            //String userName = "axlmenaaxlm77enaxlmena0000@hotmail.com";
            //String password = "Ilikechocolate5!";
            try
            {
                MailMessage email = new MailMessage();
                email.To.Add(DestEmail);
                email.From = new MailAddress(userName);
                email.Subject = title;
                string body = "Message generated automatically -- Do not reply.<br/><br/>";
                body += Msg;
                email.IsBodyHtml = true;
                email.Body = body;
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.office365.com";
                client.Credentials = new System.Net.NetworkCredential(userName, password);
                client.Port = 587;
                client.EnableSsl = true;
                client.Send(email);
            }
            catch (Exception e2)
            {
                //return "Error sending email to " + DestEmail + ". Inform System Administrator. Error:" + e2.Message;
                String a = e2.Message;
            }
            return "Email sent to Amerinode Support at " + DestEmail + " for immediate processing.";
        }

        public static string SendEmail(string title, string Msg, string DestEmail, string FileName)
        {
            //String userName = "q@zurichtechsolutions.com";
            //String password = "Zurich2018!";
            String userName = "q@amerinode.com";
            String password = "Ilikechocolate5!";
            try
            {
                MailMessage email = new MailMessage();
                email.To.Add(DestEmail);
                email.From = new MailAddress(userName);
                email.Subject = title;
                string body = "Message generated automatically -- Do not reply.<br/><br/>";
                body += Msg;

                email.Attachments.Add(new Attachment(FileName));

                email.IsBodyHtml = true;
                email.Body = body;
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.office365.com";
                client.Credentials = new System.Net.NetworkCredential(userName, password);
                client.Port = 587;
                client.EnableSsl = true;
                client.Send(email);
            }
            catch (Exception e2)
            {
                //return "Error sending email to " + DestEmail + ". Inform System Administrator. Error:" + e2.Message;
                String b = e2.Message;
            }
            return "Email sent to " + DestEmail + " for approval or notification.";
        }

    }
}