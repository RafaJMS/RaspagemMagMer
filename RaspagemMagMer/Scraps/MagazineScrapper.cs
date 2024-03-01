using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RaspagemMagMer.Models;
using System;
using static Program;

namespace RaspagemMagMer.Scraps
{
    public class MagazineLuizaScraper
    {
        public string ObterPreco(string descricaoProduto, int idProduto)
        {
            string[] response = new string[3];
            try
            {
                // Inicializa o ChromeDriver
                using IWebDriver driver = new ChromeDriver();
                string url = ($"https://www.magazineluiza.com.br/busca/{descricaoProduto}");
                // Abre a página
                driver.Navigate().GoToUrl(url);

                // Aguarda um tempo fixo para permitir que a página seja carregada (você pode ajustar conforme necessário)
                Thread.Sleep(5000);

                // Encontra o elemento que possui o atributo data-testid
                IWebElement priceElement = driver.FindElement(By.CssSelector("[data-testid='price-value']"));
                IWebElement nameElement = driver.FindElement(By.CssSelector("[data-testid='product-title']"));

                // Verifica se o elemento foi encontrado
                if (priceElement != null)
                {
                    // Obtém o preço do primeiro produto
                    string firstProductName = nameElement.Text;
                    string firspProductPrice = priceElement.Text;

                    // Registra o log com o ID do produto
                    RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "WebScraping - Magazine Luiza", "Sucesso", idProduto);

                    // Retorna o preço
                    return firspProductPrice;
                }
                else
                {
                    Console.WriteLine("Preço não encontrado.");

                    // Registra o log com o ID do produto
                    RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "WebScraping - Magazine Luiza", "Preço não encontrado", idProduto);

                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao acessar a página: {ex.Message}");

                // Registra o log com o ID do produto
                RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "Web Scraping - Magazine Luiza", $"Erro: {ex.Message}", idProduto);

                return null;
            }
        }

        private static void RegistrarLog(string codRob, string usuRob, DateTime dateLog, string processo, string infLog, int idProd)
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


        public string ObterNome(string descricaoProduto)
        {
            string url = ($"https://www.magazineluiza.com.br/busca/{descricaoProduto}");
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            IWebElement nameElement = driver.FindElement(By.CssSelector("[data-testid='product-title']"));

            if (nameElement != null)
            {
                return nameElement.Text;
            }
            else
            {
                return null;
            }
        }

        public string ObterLink(string descricaoProduto)
        {
            string url = ($"https://www.magazineluiza.com.br/busca/{descricaoProduto.Replace(' ', '+')}");
            return url;
        }
    }
}