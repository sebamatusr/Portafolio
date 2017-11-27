using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using PortafolioWeb.Controllers;
using System;
using MimeKit;
using Google.Apis.Auth.OAuth2.Mvc.Controllers;

namespace PortafolioWeb.Controllers
{
    public class MailController : Controller
    {
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        public ActionResult Index(bool? action)
        {
            return View();
        }

        public async Task sendMail()
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            await IndexAsync();
        }

        public async Task IndexAsync()
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(CancellationToken.None);

            if (result.Credential != null)
            {
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "ASP.NET MVC Sample"
                });

                // YOUR CODE SHOULD BE HERE..
                // SAMPLE CODE:

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Vista Hermosa", "sebamatusr@gmail.com"));
                message.To.Add(new MailboxAddress("Alice", "seb.matus@alumnos.duoc.cl"));
                message.Subject = "Solicitud de permiso";

                var builder = new BodyBuilder();

                builder.TextBody = @"Estimado funcionario,

                    Es de nuestro agrado comunicarle que su solicitud de permiso ha sido APROBADA.
                    Se ha adjuntado su permiso en formato pdf.

                    -- Ilustre Municipalidad de Vista Hermosa
                    ";
                builder.Attachments.Add(@"C:\Users\Mr Eko\Source\Repos\Portafolio\PortafolioWeb\PortafolioWeb\Content\img\logoMuni.png");

                message.Body = builder.ToMessageBody();



                var newMsg = new Google.Apis.Gmail.v1.Data.Message();

                newMsg.Raw = Base64UrlEncode(message.ToString());

                service.Users.Messages.Send(newMsg, "me").Execute(); 

                return;
            }
            else
            {
                return;
            }
        }

        public static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }
    }
}