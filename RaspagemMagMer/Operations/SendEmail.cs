using RaspagemMagMer.Models;
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
            
            string smtpserver = "smtp-mail.outlook.com";
            int porta = 587; 
            string remetente = "rafaelMecenasRobo@outlook.com"; 
            string senha = "teste123@"; 

           
            using (SmtpClient client = new SmtpClient(smtpserver, porta))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(remetente, senha);
                client.EnableSsl = true; 

                
                MailMessage mensagem = new(remetente, "rafaelMecenasRobo@outlook.com")
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
                           $"{responseBench}\n"+
                           "\n"+
                           "Robo: 0001\n"+
                           "Usuario: rafaelmecenas"


                };

                
                try
                {
                    client.Send(mensagem);
                    return true;
                
                }catch (Exception ex)
                {

                    Console.WriteLine("Erro no Email: "+ex.Message);
                    return false;
                }
                




            }

        }
    }


}

