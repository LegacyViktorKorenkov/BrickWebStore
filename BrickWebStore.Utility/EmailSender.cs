using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace BrickWebStore.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        private MailjetSettings _mailjet;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            _mailjet = _configuration.GetSection("Mailjet").Get<MailjetSettings>();

            MailjetClient client = new MailjetClient(_mailjet.ApiKey, _mailjet.SecretKey);

            MailjetRequest request = new MailjetRequest
            {
                Resource = SendV31.Resource,
            }
             .Property(Send.Messages, new JArray 
             {
                new JObject {
                      {
                       "From",
                       new JObject {
                        {"Email", "vkorenkovtestacc@gmail.com"},
                        {"Name", "Viktor"}
                       }
                      }, {
                       "To",
                       new JArray {
                        new JObject {
                         {
                          "Email",
                          email
                         }, {
                          "Name",
                          "DotNetMastery"
                         }
                        }
                       }
                      }, {
                       "Subject",
                       subject
                      }, {
                       "HTMLPart",
                       body
                      }
                     }
             });
            var result = await client.PostAsync(request);
        }
    }
}
