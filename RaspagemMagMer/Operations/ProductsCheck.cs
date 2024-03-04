using Newtonsoft.Json;
using RaspagemMagMer.Models;
using RaspagemMagMer.Scraps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemMagMer.Operations
{
    public class ProductsCheck
    {
        static List<Produto> produtosVerificados = new List<Produto>();
        public static async void VerificarNovoProduto(object state, object state2)

        {
            string email = state2 as string;
            string phoneNumber = state as string;
            string username = "11164448";
            string senha = "60-dayfreetrial";
            string url = "http://regymatrix-001-site1.ktempurl.com/api/v1/produto/getall";


            try
            {

                using (HttpClient client = new HttpClient())
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{username}:{senha}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {

                        string responseData = await response.Content.ReadAsStringAsync();

                        List<Produto> novosProdutos = ObterNovosProdutos(responseData);

                        foreach (Produto produto in novosProdutos)
                        {
                            if (!produtosVerificados.Exists(p => p.Id == produto.Id))
                            {

                                Console.WriteLine($"Novo produto encontrado: ID {produto.Id}, Nome: {produto.Nome}\n");

                                produtosVerificados.Add(produto);



                                if (!ProdutoJaRegistrado(produto.Id))
                                {
                                    LogRegister logRegister = new LogRegister();
                                    LogRegister.RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "ConsultaAPI - Verificar Produto", "Sucesso", produto.Id);

                                    MercadoLivreScraper mercadoLivreScraper = new();
                                    string mercadoLivrePreco = mercadoLivreScraper.ObterPreco(produto.Nome, produto.Id);

                                    string mercadoLivreNome = mercadoLivreScraper.ObterNome(produto.Nome);

                                    string mercadoLivreLink = mercadoLivreScraper.ObterLink(produto.Nome);

                                    MagazineLuizaScraper magazineLuizaScraper = new();
                                    string magazineLuizaPreco = magazineLuizaScraper.ObterPreco(produto.Nome, produto.Id);

                                    string magazineLuizaNome = magazineLuizaScraper.ObterNome(produto.Nome);

                                    string magazineLuizaLink = magazineLuizaScraper.ObterLink(produto.Nome);

                                    string responseBench = Benchmarking.CompareValue(magazineLuizaPreco, mercadoLivrePreco, mercadoLivreLink, magazineLuizaLink);

                                    if (responseBench != null) LogRegister.RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "Benchmarking", "Sucesso", produto.Id);

                                    else LogRegister.RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "Benchmarking", "Erro", produto.Id);

                                    bool responseEmail = SendEmail.EnviarEmail(email, produto.Nome, magazineLuizaNome, magazineLuizaPreco, mercadoLivreNome, mercadoLivrePreco, responseBench);

                                    if (responseEmail == true) LogRegister.RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "SendEmail", "Sucesso", produto.Id);

                                    else LogRegister.RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "SendEmail", "Erro", produto.Id);

                                    if (phoneNumber != null)
                                    {
                                        SendMessage.EnviarMsg(produto.Id, phoneNumber, produto.Nome, magazineLuizaNome, magazineLuizaPreco, mercadoLivreNome, mercadoLivrePreco, responseBench);
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        // Imprimir mensagem de erro caso a requisição falhe
                        Console.WriteLine($"Erro: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Erro ao fazer a requisição: {ex.Message}");
            }
        }

        static List<Produto> ObterNovosProdutos(string responseData)
        {

            List<Produto> produtos = JsonConvert.DeserializeObject<List<Produto>>(responseData);
            return produtos;
        }

        static bool ProdutoJaRegistrado(int idProduto)
        {
            using (var context = new LogContext())
            {
                return context.LOGROBO.Any(log => log.IdProd == idProduto);
            }
        }

    }
}
