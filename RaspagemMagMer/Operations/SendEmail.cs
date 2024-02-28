using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemMagMer.Operations
{
    public class SendEmail
    {
        static void Enviaremail(string nomeProduto, string precoMag, string precoMerc, string responseBench)
        {
            // configurações do servidor smtp do gmail
            string smtpserver = "smtp-mail.outlook.com"; // servidor smtp do gmail
            int porta = 587; // porta smtp do gmail para tls/starttls
            string remetente = "wallacemaximus@hotmail.com"; // seu endereço de e-mail do gmail
            string senha = "teste"; // sua senha do gmail

            // configurar cliente smtp
            using (SmtpClient client = new SmtpClient(smtpserver, porta))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(remetente, senha);
                client.EnableSsl = true; // habilitar ssl/tls

                // construir mensagem de e-mail
                MailMessage mensagem = new(remetente, "wallace@docente.senai.br")
                {
                    Subject = "resultado da comparação de preços",
                    Body = $"Mercado Livre:\n" +
                           $"Nome: {nomeProduto} \n" +
                           $"Preço: {precoMerc}\n" +
                           "" +
                           $"Magazine Luiza:\n" +
                           $"Nome: {nomeProduto} \n" +
                           $"Preço: {precoMag}\n" +
                           "" +
                           $"Resultado:\n" +
                           $"{responseBench}\n"



                };

                // enviar e-mail
                client.Send(mensagem);




            }

        }
    }


}

