using IDGenWebsite.Models;
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

        public async Task SendNewOrderRequestEmail(List<EmployeeModel> admins)
        {
            //var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("idcardservices@franklincountyschools.org", "no-reply");
            var subject = "New Student Id Request Order";
            var to = new EmailAddress("clemieux@franklincountyschools.org");
            var plainTextContent = "There is a new Student Id request ready to be printed!";
            var htmlContent = "<strong>There is a new Student Id request ready to be printed!</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
