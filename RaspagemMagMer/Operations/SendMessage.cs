using System;
using System.IO;
using System.Net;
using System.Text;

namespace RaspagemMagMer.Operations
{
    public class SendMessage
    {
        public static async Task<bool> EnviarMsg(string nomeProduto, string nomeMag, string precoMag, string nomeMerc, string precoMerc, string responseBench)
        {
                Console.WriteLine("Insira o Número do Whatssap: DDD + Número (Apenas Números)");
                string num = ("55" + Console.ReadLine());


                try
                {
                    var parameters = new System.Collections.Specialized.NameValueCollection();
                    var client = new WebClient();

                    var url = "https://app.whatsgw.com.br/api/WhatsGw/Send/";


                    parameters.Add("apikey", "dd42786b-64c9-49fd-9b28-4a82a005a98d"); //switch to your api key
                    parameters.Add("phone_number", "5579998626376"); //switch to your connected number
                    parameters.Add("contact_phone_number", num); //switch to your number text to received message
                    parameters.Add("message_custom_id", "teste");
                    parameters.Add("message_type", "text");
                    parameters.Add("message_body",
                           "*Resultado da Comparação de Preços*" +
                           "\n" +
                           $"*Produto Pesquisado*: {nomeProduto}\n" +
                           "\n" +
                           $"*Mercado Livre*:\n" +
                           $"*Nome*: {nomeMerc} \n" +
                           $"*Preço*: R$ {precoMerc}\n" +
                           "\n" +
                           $"*Magazine Luiza*:\n" +
                           $"*Nome*: {nomeMag} \n" +
                           $"*Preço*: {precoMag}\n" +
                           "\n" +
                           $"*Resultado*:\n" +
                           $"{responseBench}\n");

                    byte[] response_data = await client.UploadValuesTaskAsync(url, "POST", parameters);
                    string responseString = Encoding.UTF8.GetString(response_data);

                    Console.WriteLine("Response String: " + responseString);
                    return true;

                }
                catch (Exception ex)
                {

                    Console.WriteLine($"FALHA: {ex.Message}");
                    return false;
                }
            
            

        }

    }
}