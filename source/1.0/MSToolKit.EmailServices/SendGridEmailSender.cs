using Microsoft.Extensions.Options;
using MSToolKit.EmailServices.Abstraction;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace MSToolKit.EmailServices
{
    /// <summary>
    /// Provides a default implementation for MSToolKit.EmailServices.Abstraction.IEmailSender.
    /// </summary>
    internal class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridOptions options;

        /// <summary>
        /// Initialize a new instance of MSToolKit.EmailServices.SendGridEmailSender.
        /// </summary>
        /// <param name="options">
        /// MSToolKit.EmailServices.SendGridOptions, that configure the SendGridClient.
        /// </param>
        public SendGridEmailSender(IOptions<SendGridOptions> options)
        {
            this.options = options.Value
                ?? throw new ArgumentNullException($"{nameof(options)} can not be null.");
        }

        /// <summary>
        /// Send email to a specified recipient.
        /// </summary>
        /// <param name="formEmailAddress">Sender email address</param>
        /// <param name="fromName">Sender name</param>
        /// <param name="toEmailAddress">Recipient email address</param>
        /// <param name="toName">Recipient name</param>
        /// <param name="title">Message title</param>
        /// <param name="htmlMessage">Message content</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        public async Task SendEmailAsync(
            string formEmailAddress,
            string fromName,
            string toEmailAddress,
            string toName,
            string title,
            string htmlMessage)
        {
            var client = new SendGridClient(this.options.SendGridApiKey);
            var from = new EmailAddress(formEmailAddress, fromName);
            var to = new EmailAddress(toEmailAddress, toName);
            var sendGridMessage = MailHelper.CreateSingleEmail(from, to, title, htmlMessage, htmlMessage);
            var response = await client.SendEmailAsync(sendGridMessage);
        }
    }
}
