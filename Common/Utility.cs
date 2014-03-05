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

public static class EnumHelper<T> {

    public static string GetDisplayValue(T value) {
        var fieldInfo = value.GetType().GetField(value.ToString());

        var descriptionAttributes = fieldInfo.GetCustomAttributes(
            typeof(DisplayAttribute), false) as DisplayAttribute[];

        if (descriptionAttributes == null) return string.Empty;
        return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
    }

    public static IList<string> GetDisplayValues(Enum value) {
        return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
    }

    public static IList<string> GetNames(Enum value) {
        return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
    }

    public static IList<T> GetValues(Enum value) {
        var enumValues = new List<T>();

        foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public)) {
            enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
        }
        return enumValues;
    }

    public static T Parse(string value) {
        return (T)Enum.Parse(typeof(T), value, true);
    }
}