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
    // Lista para armazenar produtos já verificados
    static List<Produto> produtosVerificados = new List<Produto>();

    static void Main(string[] args)
    {
        string phoneNumber = SendMessage.OpcaoMsg();
        // Definir o intervalo de tempo para 5 minutos (300.000 milissegundos)
        int intervalo = 60000;

        // Criar um temporizador que dispara a cada 5 minutos
        Timer timer = new(state => VerificarNovoProduto(phoneNumber), null, 0, intervalo);

        // Manter a aplicação rodando
        while (true)
        {
            // Isso evita que o aplicativo encerre imediatamente após iniciar o timer
            Thread.Sleep(Timeout.Infinite);
        }

    }

    static async void VerificarNovoProduto(object state)

    {
        string phoneNumber = state as string;
        string username = "11164448";
        string senha = "60-dayfreetrial";
        string url = "http://regymatrix-001-site1.ktempurl.com/api/v1/produto/getall";

        
        try
        {
            // Criar um objeto HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Adicionar as credenciais de autenticação básica
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{senha}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                // Fazer a requisição GET à API
                HttpResponseMessage response = await client.GetAsync(url);

                // Verificar se a requisição foi bem-sucedida (código de status 200)
                if (response.IsSuccessStatusCode)
                {
                    // Ler o conteúdo da resposta como uma string
                    string responseData = await response.Content.ReadAsStringAsync();
                       
                
                    // Processar os dados da resposta
                    List<Produto> novosProdutos = ObterNovosProdutos(responseData);
                    foreach (Produto produto in novosProdutos)
                    {
                        if (!produtosVerificados.Exists(p => p.Id == produto.Id))
                        {
                            // Se é um novo produto, faça algo com ele
                            Console.WriteLine($"Novo produto encontrado: ID {produto.Id}, Nome: {produto.Nome}\n");
                            // Adicionar o produto à lista de produtos verificados
                            produtosVerificados.Add(produto);
                            

                            // Registra um log no banco de dados apenas se o produto for novo
                            if (!ProdutoJaRegistrado(produto.Id))
                            {
                                RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "ConsultaAPI - Verificar Produto", "Sucesso", produto.Id);

                                MercadoLivreScraper mercadoLivreScraper = new();
                                string[] mercadoLivre = mercadoLivreScraper.ObterPreco(produto.Nome, produto.Id);
                                
                                string mercadoLivrePreco = mercadoLivre[0];

                                string mercadoLivreNome = mercadoLivreScraper.ObterNome(produto.Nome);
                                
                                string mercadoLivreLink = mercadoLivre[2];

                                MagazineLuizaScraper magazineLuizaScraper = new();
                                string magazineLuiza =  magazineLuizaScraper.ObterPreco(produto.Nome, produto.Id);
                                
                                string magazineLuizaPreco = magazineLuiza;
                                
                                string magazineLuizaNome = magazineLuizaScraper.ObterNome(produto.Nome);
                                Console.WriteLine(magazineLuizaNome);
                                
                                string magazineLuizaLink = magazineLuiza[2];

                                string responseBench = Benchmarking.CompareValue(magazineLuizaPreco, mercadoLivrePreco,mercadoLivreLink,magazineLuizaLink);

                                if (responseBench != null) RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "Benchmarking", "Sucesso", produto.Id);
                                
                                else RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "Benchmarking", "Erro", produto.Id);

                                bool responseEmail = SendEmail.Enviaremail(produto.Nome,magazineLuizaNome,magazineLuizaPreco,mercadoLivreNome,mercadoLivrePreco,responseBench);
                                
                                if (responseEmail == true ) RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "SendEmail", "Sucesso", produto.Id);

                                else RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "SendEmail", "Erro", produto.Id);

                                if (phoneNumber != null)
                                {
                                    SendMessage.EnviarMsg(phoneNumber, produto.Nome, magazineLuizaNome, magazineLuizaPreco, mercadoLivreNome, mercadoLivrePreco, responseBench);
                                    RegistrarLog("180312", "rafaelmecenas", DateTime.Now, "SendZap", "Sucesso", produto.Id);
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
            // Imprimir mensagem de erro caso ocorra uma exceção
            Console.WriteLine($"Erro ao fazer a requisição: {ex.Message}");
        }
    }

    // Método para processar os dados da resposta e obter produtos
    static List<Produto> ObterNovosProdutos(string responseData)
    {
        // Desserializar os dados da resposta para uma lista de produtos
        List<Produto> produtos = JsonConvert.DeserializeObject<List<Produto>>(responseData);
        return produtos;
    }

    // Método para verificar se o produto já foi registrado no banco de dados
    static bool ProdutoJaRegistrado(int idProduto)
    {
        using (var context = new LogContext())
        {
            return context.Logs.Any(log => log.IdProd == idProduto);
        }
    }

    // Método para registrar um log no banco de dados
    public static void RegistrarLog(string codRob, string usuRob, DateTime dateLog, string processo, string infLog, int idProd)
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

