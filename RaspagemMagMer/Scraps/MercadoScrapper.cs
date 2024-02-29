using HtmlAgilityPack;
using System;

namespace RaspagemMagMer.Scraps
{
    public class MercadoLivreScraper
    {
        public string[] ObterPreco(string descricaoProduto, int idProduto)
        {
            string[] response = new string[3];
            // URL da pesquisa no Mercado Livre com base na descrição do produto
            string url = $"https://lista.mercadolivre.com.br/{descricaoProduto}";

            try
            {
                // Inicializa o HtmlWeb do HtmlAgilityPack
                HtmlWeb web = new HtmlWeb();

                // Carrega a página de pesquisa do Mercado Livre
                HtmlDocument document = web.Load(url);

                // Encontra o elemento que contém o preço do primeiro produto            
                HtmlNode firstProductPriceNode = document.DocumentNode.SelectSingleNode("//span[@class='andes-money-amount__fraction']");
                HtmlNode firstProductPriceName = document.DocumentNode.SelectSingleNode("//h2[@class='ui-search-item__title']");
                
                // Verifica se o elemento foi encontrado
                if (firstProductPriceNode != null && firstProductPriceName!=null)
                {
                    // Obtém o preço do primeiro produto
                    string firstProductPrice = firstProductPriceNode.InnerText.Trim();
                    string firstProductName = firstProductPriceName.InnerText.Trim();
                    // Registra o log com o ID do produto
                    RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "WebScraping - Mercado Livre", "Sucesso", idProduto);

                    // Retorna o preço
                    response[0] = firstProductPrice;
                    response[1] = firstProductName;
                    response[2] = url.Replace(' ', '-');
                    return response;
                }
                else
                {
                    Console.WriteLine("Preço não encontrado.");

                    // Registra o log com o ID do produto
                    RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "WebScraping - Mercado Livre", "Preço não encontrado", idProduto);

                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao acessar a página: {ex.Message}");

                // Registra o log com o ID do produto
                RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "Web Scraping - Mercado Livre", $"Erro: {ex.Message}", idProduto);

                return null;
            }
        }

        private void RegistrarLog(string codRob, string usuRob, DateTime dateLog, string processo, string infLog, int idProd)
        {
            using (var context = new LogContext())
            {
                var log = new Log
                {
                    CodRob = codRob,
                    UsuRob = usuRob,
                    DateLog = dateLog,
                    Processo = processo,
                    InfLog = infLog,
                    IdProd = idProd
                };
                context.Logs.Add(log);
                context.SaveChanges();
            }
        }
    }
}