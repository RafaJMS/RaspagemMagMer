using RaspagemMagMer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Program;

namespace RaspagemMagMer.Operations
{
    public class SendEmail
    {
        public static bool EnviarEmail(string destinatario,string nomeProduto,string nomeMag ,string precoMag, string nomeMerc, string precoMerc, string responseBench)
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

                MailMessage mensagem = new(remetente, destinatario)
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
                           "Robo: 1806\n"+
                           "Usuario: rafaelMecenas"

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

        public static bool ValidarEmail(string email)
        {
            // Padrão de expressão regular para verificar o formato do e-mail
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Verifica se o e-mail corresponde ao padrão
            return Regex.IsMatch(email, pattern);
        }

        public static string OpcaoEmail()
        {
            Console.Write("Insira o Email para recebimento do Resultado: ");
            string email = Console.ReadLine();
            bool opt = SendEmail.ValidarEmail(email);
            if (opt)
            {
                Console.WriteLine("Email Informado: "+email);
                return email;
            }
            return null;
        }
    }
    
}



