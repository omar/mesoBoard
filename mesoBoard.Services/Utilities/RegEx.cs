using System.Text.RegularExpressions;
using System;
using System.Net.Mail;

namespace mesoBoard.Services
{
    public static class RegEx
    {
        public const string HexColorPattern = @"#[0-9a-zA-Z]{3,6}";

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = new MailAddress(email).Address;
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }
    }
}