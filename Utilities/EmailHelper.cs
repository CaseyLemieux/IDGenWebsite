using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Utilities
{
    public class EmailHelper
    {
        private string apiKey;
        public EmailHelper(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task Send()
        {
            //var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("idcardservices@franklincountyschools.org", "Franklin Email Test");
            var subject = "Your Student's Id is ready!";
            var to = new EmailAddress("clemieux@franklincountyschools.org", "Example User");
            var plainTextContent = "Test Email";
            var htmlContent = "<strong>Test Email</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
