using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Services;
using StringExtensions;

namespace MenuzRus {

    public class CommonService {

        public Boolean SendEmailConfirmation(User user) {
            Boolean retVal = true;
            String html = System.IO.File.ReadAllText(String.Format("{0}/App_Data/emailTemplates/EmailConfirmation.html", AppDomain.CurrentDomain.BaseDirectory));
            html = html.Replace("<<FirstName>>", user.FirstName).Replace("<<LastName>>", user.LastName);
            retVal = Common.SendEmail(user.Email, "MenuzRus email confirmation", html);
            if (!retVal) {
                SessionData.exeption = new Exception("Cannot send Email");
            }

            return retVal;
        }
    }
}