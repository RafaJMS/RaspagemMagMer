using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using RaspagemMagMer.Scraps;
using RaspagemMagMer.Operations;
using static Program;
using RaspagemMagMer.Models;

class Program
{
    static void Main(string[] args)
    {
        string email = SendEmail.OpcaoEmail();
            while (email==null)
        {
            Console.WriteLine("Email Inválido, tente novamente!");
            email = SendEmail.OpcaoEmail();
            
        }

        string phoneNumber = SendMessage.OpcaoMsg();
        
        int intervalo = 300000;

        Timer timer = new(state => DBCheck.VerificarNovoProduto(phoneNumber,email), null, 0, intervalo);

        while (true)
        {
            Thread.Sleep(Timeout.Infinite);
        }

    }

    
    


}

