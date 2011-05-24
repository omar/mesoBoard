using System.Web.Mvc;
using System.Web.Security;
using mesoBoard.Data;
using mesoBoard.Services;
using mesoBoard.Framework.Core;
using mesoBoard.Framework.Models;
using mesoBoard.Framework;

namespace mesoBoard.Web.Controllers
{
    public class AuthController : BaseController
    {
        UserServices _userServices;
        EmailServices _emailServices;

        public AuthController(
            UserServices userServices, 
            EmailServices emailServices)
        {
            _userServices = userServices;
            _emailServices = emailServices;
        }

        public ActionResult ActivateUser(string uname, string code)
        {
            SetBreadCrumb("Activate Account");

            if (_userServices.ActivateUser(uname, code))
            {
                SetSuccess("Your account has been activated. Please use the information you registered with to login.");
                return RedirectToAction("Login");
            }
            else
            {
                SetError("The activation link you provided was invalid. Please use the link provided in the activation email");
                return View();
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            SetBreadCrumb("Register");
            var model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (IsModelValidAndPersistErrors())
            {
                string ActivationType =  SiteConfig.AccountActivation.Value;
                User user = _userServices.Register(model.Username, model.Password, model.Email);
                string CreateSucess = "User <b>" + user.Username + "</b> was created.";

                if (ActivationType == "Email")
                {
                    CreateSucess += " An activation email has been sent to <b>" +
                       user.Email + "</b> with details on how to activate the account";

                    string confirm_url = Url.Action("ActivateUser", "Auth", new { uname = user.Username, code = user.ActivationCode });
                    _emailServices.Registration(user, confirm_url);
                }
                else if (ActivationType == "Admin")
                    CreateSucess += " Admin activation is required. You will not be able to login until an admin activates your account";

                SetSuccess(CreateSucess);
                
                return RedirectToAction("Login");
            }

            return RedirectToSelf();
        }

        [HttpGet]
        [DefaultAction]
        [AllowOffline]
        public ActionResult Login(string ReturnUrl)
        {
            SetBreadCrumb("Login");

            if (!string.IsNullOrEmpty(ReturnUrl))
                SetNotice("You must login to access this page");
            return View();
        }

        [HttpPost]
        [AllowOffline]
        public ActionResult Login(LoginViewModel model, string ReturnUrl)
        {
            if (IsModelValidAndPersistErrors())
            {
                if (_userServices.UserExists(model.Username))
                {
                    User user = _userServices.GetUser(model.Username);
                    if (!string.IsNullOrEmpty(user.ActivationCode))
                    {
                        TagBuilder link = new TagBuilder("a");
                        link.Attributes.Add("href", Url.Action("ResendActivationCode", new { UserID = user.UserID }));
                        link.InnerHtml = "resend the activation email";
                        SetNotice("<b>" + user.Username + "</b>'s account has not been activated yet. Click here to " + link.ToString());
                    }
                    else if (_userServices.ValidatePassword(user, model.Password))
                    {
                        ReturnUrl = ReturnUrl ?? Url.Action("Index", "Board");
                        _userServices.LoginRoutine(user, Request.UserHostAddress);
                        FormsAuthentication.SetAuthCookie(user.UserID.ToString(), model.RememberMe);
                        Session[SessionKeys.UserID] = user.UserID;
                        SetSuccess("Successfully Logged In");
                        return Redirect(ReturnUrl);
                    }

                    SetError("Invalid Login Credentials");
                }
                else
                    SetError("Invalid Username");
            }              

            return RedirectToSelf();
        }

        [Authorize]
        [AllowOffline]
        public ActionResult Logout()
        {
            _userServices.LogoutRoutine(CurrentUser.UserID);
            Session.Remove(SessionKeys.LastActivityUpdate);
            Session.Remove(SessionKeys.UserID);
            FormsAuthentication.SignOut();

            SetSuccess("Successfully Logged out");
            return RedirectToAction("Index", "Board");
        }

        [HttpGet]
        [AllowOffline]
        public ActionResult ForgotPassword()
        {
            SetBreadCrumb("Forgot Password");
            return View();
        }

        [HttpPost]
        [AllowOffline]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (IsModelValidAndPersistErrors())
            {
                User user = _userServices.GetUser(model.UsernameOrEmail);
                if (user != null)
                {
                    string token = _userServices.RequestPasswordReset(user.UserID);
                    string resetUrl = Url.Action("ResetPassword", "Auth", new { token = token });
                    _emailServices.PasswordResetRequest(user, resetUrl);
                    SetSuccess("A password reset request link has been sent to the associated email address. Please check your email to reset your password");
                    return RedirectToAction("Login");
                }
                else
                    SetError("Email/Username was not found");
            }

            return RedirectToSelf();
        }

        [HttpGet]
        [AllowOffline]
        public ActionResult ResetPassword(string token)
        {
            if (_userServices.ValidatePasswordResetRequest(token))
            {
                User user = _userServices.GetUser(int.Parse(token.Split('-')[0]));
                string newPass = _userServices.ResetPassword(user.UserID);
                _emailServices.PasswordChanged(user, newPass);
                _userServices.DeletePasswordResetRequest(user.UserID);
                SetSuccess("Password reset, an email as been set to the associated email address of the account");
                return RedirectToAction("Login");
            }
            else
            {
                SetError("Invalid reset password token. The token is incorrect or expired. Please submit another reset request");
                return RedirectToAction("ForgotPassword");
            }
        }

        [HttpGet]
        public ActionResult ResendActivationCode()
        {
            SetBreadCrumb("Resend Activation Code");
            return View();
        }

        [HttpPost]
        public ActionResult ResendActivationCode(ResendActivationCodeViewModel model)
        {
            if (IsModelValidAndPersistErrors())
            {
                User user = _userServices.GetUser(model.UsernameOrEmail);
                if (user != null)
                {
                    if (string.IsNullOrEmpty(user.ActivationCode))
                        SetNotice("This account is already active. Visit the Password Reminder page if you have forgotten your password.");
                    else
                    {
                        string confirm_url = Url.Action("ActivateUser", "Auth", new { uname = user.Username, code = user.ActivationCode });
                        _emailServices.ResendActivationCode(user, confirm_url);

                        string success = string.Format("An email has been sent to <b>{0}</b> with the activation link for this account", user.Email);

                        SetSuccess(success);
                    }
                    return RedirectToAction("Login");
                }
                else
                    SetError("Email/Username was not found");

            }

            return RedirectToSelf();
        }
    }
}
