using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public static class Common {

    public enum Settings {

        [Display(Name = "Wall Background")]
        WallBackground = 2,

        [Display(Name = "Page Background")]
        PageBackground = 4,

        [Display(Name = "Category")]
        Category = 8,

        [Display(Name = "Category Description")]
        CategoryDescription = 16,

        [Display(Name = "Item")]
        Item = 32,

        [Display(Name = "Item Description")]
        ItemDescription = 64,

        [Display(Name = "Price")]
        Price = 128
    }

    public enum Side {
        Left, Center, Right
    }

    public enum Status {

        [Display(Name = "Show item")]
        Active = 1,

        [Display(Name = "Do not show this item")]
        NotActive = 0
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