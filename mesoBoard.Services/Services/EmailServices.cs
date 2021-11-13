using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using mesoBoard.Data;

namespace mesoBoard.Services
{
    public class EmailServices
    {
        private class EmailTemplates
        {
            public static readonly string TemplateLocation = "~/App_Data/EmailTemplates";

            public static readonly string Welcome = "Welcome.html";
            public static readonly string Registration = "Registration.html";
            public static readonly string ResendActivationCode = "ResendActivationCode.html";
            public static readonly string NewThreadReply = "NewThreadReply.html";
            public static readonly string PasswordChange = "PasswordChange.html";
            public static readonly string EmailChange = "EmailChange.html";
            public static readonly string PasswordResetRequest = "PasswordResetRequest.html";
            public static readonly string NewMessage = "NewMessage.html";

            public static string GenerateEmailPath(string email)
            {
                return string.Format("{0}/{1}", TemplateLocation, email);
            }
        }

        private void SendEmail(
            string toEmail,
            string fromEmail,
            string subject,
            string template,
            Dictionary<string, string> replacements,
            bool isHtml = true)
        {
            string boardUrl = SiteConfig.BoardURL.Value;
            if (boardUrl.EndsWith("/"))
                boardUrl = boardUrl.TrimEnd('/');

            replacements.Add("{BOARD_URL}", boardUrl);
            replacements.Add("{BOARD_NAME}", SiteConfig.BoardName.Value);

            string templateBody = File.ReadAllText(EmailTemplates.GenerateEmailPath(template));
            
            foreach (var replacement in replacements)
            {
                templateBody = templateBody.Replace(replacement.Key, replacement.Value);
            }
            
            var message = new MailMessage();
            message.IsBodyHtml = isHtml;
            message.From = new MailAddress(fromEmail);
            message.Subject = subject;
            message.Body = templateBody;

            SmtpClient client = Settings.GetSmtpClient();

            try
            {
                client.SendAsync(message, null);
            }
            catch
            {
            }
        }

        public void WelcomeEmail(User user)
        {
            var replacements = new Dictionary<string, string>();

            replacements.Add("{USERNAME}", user.Username);

            SendEmail(
                user.Email,
                SiteConfig.AutomatedFromEmail.Value,
                "Welcome - " + SiteConfig.BoardName,
                EmailTemplates.Welcome,
                replacements);
        }

        public void Registration(User user, string confirm_url)
        {
            var replacements = new Dictionary<string, string>();

            replacements.Add("{USERNAME}", user.Username);
            replacements.Add("{CONFIRM_URL}", confirm_url);

            SendEmail(
                user.Email,
                SiteConfig.AutomatedFromEmail.Value,
                "Account Activation Required - " + SiteConfig.BoardName.Value,
                EmailTemplates.Registration,
                replacements);
        }

        public void ResendActivationCode(User user, string confirm_url)
        {
            var replacements = new Dictionary<string, string>();

            replacements.Add("{USERNAME}", user.Username);
            replacements.Add("{CONFIRM_URL}", confirm_url);

            SendEmail(
                user.Email,
                SiteConfig.AutomatedFromEmail.Value,
                "Account Activation Required - " + SiteConfig.BoardName.Value,
                EmailTemplates.ResendActivationCode,
                replacements);
        }

        public void NewPostEmail(IEnumerable<Subscription> Subs, Post ThePost, Thread thread, string PostURL)
        {
            var replacements = new Dictionary<string, string>();
            string AutoFromEmail = SiteConfig.AutomatedFromEmail.Value;

            replacements.Add("{REPLY_USERNAME}", ThePost.User.Username);
            replacements.Add("{REPLY_TEXT}", ThePost.Text);
            replacements.Add("{THREAD_TITLE}", thread.Title);
            replacements.Add("{POST_URL}", PostURL);
            replacements.Add("{BOARD_URL}", SiteConfig.BoardURL.Value);
            replacements.Add("{BOARD_NAME}", SiteConfig.BoardName.Value);

            string templateBody = File.ReadAllText(EmailTemplates.GenerateEmailPath(EmailTemplates.NewThreadReply));
            foreach (var replacement in replacements)
            {
                templateBody = templateBody.Replace(replacement.Key, replacement.Value);
            }

            var message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(AutoFromEmail);
            message.Subject = "Reply to Thread '" + thread.Title + "' - " + SiteConfig.BoardName.Value;
            message.Body = templateBody;

            //Send the message
            SmtpClient client = Settings.GetSmtpClient();

            foreach (Subscription s in Subs)
            {
                if (s.UserID == ThePost.UserID) continue;
                User u = s.User;
                message.To.Clear();
                message.To.Add(u.Email);
                try
                {
                    System.Threading.ThreadPool.QueueUserWorkItem(state => client.Send(message));
                }
                catch
                {
                }
            }
        }

        public void PasswordChanged(User user, string NewPassword)
        {
            var replacements = new Dictionary<string, string>();

            replacements.Add("{USERNAME}", user.Username);
            replacements.Add("{USERPASSWORD}", NewPassword);

            SendEmail(
                user.Email,
                SiteConfig.AutomatedFromEmail.Value,
                "Password Changed - " + SiteConfig.BoardName,
                EmailTemplates.PasswordChange,
                replacements);
        }

        public void EmailChanged(User user)
        {
            var replacements = new Dictionary<string, string>();

            replacements.Add("{USERNAME}", user.Username);
            replacements.Add("{EMAIL}", user.Email);

            SendEmail(
                user.Email,
                SiteConfig.AutomatedFromEmail.Value,
                "Email changed - " + SiteConfig.BoardName.Value,
                EmailTemplates.EmailChange,
                replacements);
        }

        public void PasswordResetRequest(User user, string ResetURL)
        {
            var replacements = new Dictionary<string, string>();

            replacements.Add("{USERNAME}", user.Username);
            replacements.Add("{RESET_URL}", ResetURL);

            SendEmail(
                user.Email,
                SiteConfig.AutomatedFromEmail.Value,
                "Password Reset Request - " + SiteConfig.BoardName.Value,
                EmailTemplates.PasswordResetRequest,
                replacements);
        }

        public void NewMessage(Message message, User ToUser, string messageURL)
        {
            var replacements = new Dictionary<string, string>();

            replacements.Add("{FROMUSERNAME}", message.FromUser.Username);
            replacements.Add("{MESSAGEURL}", messageURL);

            SendEmail(
                ToUser.Email,
                SiteConfig.AutomatedFromEmail.Value,
                "New Message - " + SiteConfig.BoardName.Value,
                EmailTemplates.NewMessage,
                replacements);
        }
    }
}