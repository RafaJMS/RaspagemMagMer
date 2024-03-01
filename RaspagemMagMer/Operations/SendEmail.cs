using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static Program;

namespace RaspagemMagMer.Operations
{
    public class SendEmail
    {
        public static bool Enviaremail(string nomeProduto,string nomeMag ,string precoMag, string nomeMerc, string precoMerc, string responseBench)
        {
            // configurações do servidor smtp do gmail
            string smtpserver = "smtp-mail.outlook.com"; // servidor smtp do gmail
            int porta = 587; // porta smtp do gmail para tls/starttls
            string remetente = "enviotesterafamece@outlook.com"; // seu endereço de e-mail do gmail
            string senha = "teste123@"; // sua senha do gmail

            // configurar cliente smtp
            using (SmtpClient client = new SmtpClient(smtpserver, porta))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(remetente, senha);
                client.EnableSsl = true; // habilitar ssl/tls

                // construir mensagem de e-mail
                MailMessage mensagem = new(remetente, "enviotesterafamece@outlook.com")
                {
                    Subject = "Resultado da Comparação de Preços",
                    Body = $"Produto Pesquisado: {nomeProduto}\n" +
                           "\n" +
                           $"Mercado Livre:\n" +
                           $"Nome: {nomeMerc} \n" +
                           $"Preço: R$ {precoMerc}\n" +
                           "\n" +
                           $"Magazine Luiza:\n" +
                           $"Nome: {nomeMag} \n" +
                           $"Preço: {precoMag}\n" +
                           "\n" +
                           $"Resultado:\n" +
                           $"{responseBench}\n"

                };

                // enviar e-mail
                try
                {
                    client.Send(mensagem);
                    return true;
                
                }catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                




            }

        }
    }


}

