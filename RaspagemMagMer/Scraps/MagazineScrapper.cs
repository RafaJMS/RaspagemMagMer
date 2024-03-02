using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RaspagemMagMer.Models;
using RaspagemMagMer.Operations;
using System;
using System.Threading;

namespace RaspagemMagMer.Scraps
{
    public class MagazineLuizaScraper
    {
        public string ObterPreco(string descricaoProduto, int idProduto)
        {
            try
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--headless"); 

                using (IWebDriver driver = new ChromeDriver(options))
                {
                    string url = $"https://www.magazineluiza.com.br/busca/{descricaoProduto}";
                    driver.Navigate().GoToUrl(url);
                    Thread.Sleep(5000); 

                    IWebElement priceElement = driver.FindElement(By.CssSelector("[data-testid='price-value']"));
                    IWebElement nameElement = driver.FindElement(By.CssSelector("[data-testid='product-title']"));

                    if (priceElement != null)
                    {
                        string firstProductName = nameElement.Text;
                        string firstProductPrice = priceElement.Text;

                        LogRegister.RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "WebScraping - Magazine Luiza", "Sucesso", idProduto);

                        return firstProductPrice;
                    }
                    else
                    {
                        Console.WriteLine("Preço não encontrado.");

                        LogRegister.RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "WebScraping - Magazine Luiza", "Preço não encontrado", idProduto);

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao acessar a página: {ex.Message}");

                LogRegister.RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "Web Scraping - Magazine Luiza", $"Erro: {ex.Message}", idProduto);

                return null;
            }
        }

        public string ObterNome(string descricaoProduto)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless"); // Executa o Chrome em modo headless

            using (IWebDriver driver = new ChromeDriver(options))
            {
                string url = $"https://www.magazineluiza.com.br/busca/{descricaoProduto}";
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
        }

        public string ObterLink(string descricaoProduto)
        {
            string url = $"https://www.magazineluiza.com.br/busca/{descricaoProduto.Replace(' ', '+')}";
            return url;
        }
    }
}
