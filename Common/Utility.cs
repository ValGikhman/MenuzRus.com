using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

public static class Common {

    public enum Side {
        Left, Center, Right
    }

    public enum Monitor {

        [Display(Name = "#1")]
        First = 1,

        [Display(Name = "#2")]
        Second = 2
    }

    public enum Status {

        [Display(Name = "Show item")]
        Active = 1,

        [Display(Name = "Do not show this item")]
        NotActive = 0
    }

    public enum Settings {

        [Display(Name = "#1 Image")]
        Image1 = 2,

        [Display(Name = "#2 Image")]
        Image2 = 4,

        [Display(Name = "Wall Background")]
        WallBackground = 8,

        [Display(Name = "Page Background")]
        PageBackground = 16,

        [Display(Name = "Offset X")]
        OffsetX = 32,

        [Display(Name = "Offset Y")]
        OffsetY = 64
    }

    public static Boolean SendEmail(String emailTo, String emailSubject, String emailBody) {
        Boolean success = true;

        MailAddress from = new MailAddress("contactme@menuzrus.com");
        MailAddress to = new MailAddress(emailTo);
        MailMessage email = new MailMessage(from, to);
        email.Subject = "MenuzRus message.";
        email.IsBodyHtml = true;
        SmtpClient client = new SmtpClient("relay-hosting.secureserver.net");
        try {
            email.Body = emailBody;
            client.Send(email);
        }
        catch {
            success = false;
        }
        return success;
    }
}