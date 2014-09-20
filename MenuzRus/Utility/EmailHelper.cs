using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Services;

namespace MenuzRus {

    public static class EmailHelper {

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

        public static Boolean SendEmailConfirmation(ControllerContext context, User user) {
            String emailBody = String.Empty;
            ViewDataDictionary viewData = new ViewDataDictionary(user);

            using (StringWriter sw = new StringWriter()) {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, "Emails/_EmailConfirmationPartial");
                ViewContext viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);
                emailBody = sw.GetStringBuilder().ToString();
            }
            return SendEmail(user.Email, "MenuzRus email confirmation", emailBody);
        }
    }
}