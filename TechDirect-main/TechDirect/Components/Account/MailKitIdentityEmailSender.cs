using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using TechDirect.Data;
using TechDirect.Services;

namespace TechDirect.Components.Account
{
    internal sealed class MailKitIdentityEmailSender : IEmailSender<ApplicationUser>
    {
        private readonly MailKitEmailSender _emailSender;

        public MailKitIdentityEmailSender(MailKitEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            var subject = "Confirm your email";
            var body = $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.";
            return _emailSender.SendEmailAsync(email, subject, body);
        }

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            var subject = "Reset your password";
            var body = $"Please reset your password by <a href='{resetLink}'>clicking here</a>.";
            return _emailSender.SendEmailAsync(email, subject, body);
        }

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            var subject = "Reset your password";
            var body = $"Please reset your password using the following code: {resetCode}";
            return _emailSender.SendEmailAsync(email, subject, body);
        }
    }
}
