using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace PortafolioWeb.Controllers
{
    public class NetMailController : Controller
    {
        // GET: NetMail
        public bool sendMail(string emailDestinatario, string nombreDestinatario, MemoryStream memstream)
        {
            bool status = false;

            var fromAddress = new MailAddress("vistahermosamunicipalidad@gmail.com", "Municipalidad Vista Hermosa");
            var toAddress = new MailAddress(emailDestinatario, nombreDestinatario);
            const string fromPassword = "munivistahermosa2017";
            const string subject = "Solicitud de permiso";
            const string body = @"Estimado funcionario, 
Es de nuestro agrado comunicarle que su solicitud de permiso ha sido APROBADA.
Se ha adjuntado su permiso en formato pdf.

--Ilustre Municipalidad de Vista Hermosa";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            memstream.Position = 0;
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
        })
            {
                //var filename = @"C:\Users\Mr Eko\Source\Repos\Portafolio\PortafolioWeb\PortafolioWeb\ReporteCuantitativo.pdf";
                
                //var stream = new MemoryStream();
                //stream.Position = 0;
                message.Attachments.Add(new Attachment(memstream, "Permiso.pdf"));
                smtp.Send(message);
            }

            status = true;
            return status;
        }

        public bool sendMailRechazo(string emailDestinatario, string nombreDestinatario)
        {
            bool status = false;

            var fromAddress = new MailAddress("vistahermosamunicipalidad@gmail.com", "Municipalidad Vista Hermosa");
            var toAddress = new MailAddress(emailDestinatario, nombreDestinatario);
            const string fromPassword = "munivistahermosa2017";
            const string subject = "Solicitud de permiso";
            const string body = @"Estimado funcionario, 
Lamentamos comunicarle que su solicitud de permiso ha sido RECHAZADA.
Para más información comuníquese con su jefe de unidad.

--Ilustre Municipalidad de Vista Hermosa";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

            status = true;
            return status;
        }

    }
}