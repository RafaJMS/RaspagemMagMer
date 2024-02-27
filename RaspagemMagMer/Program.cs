using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

class Program
{
    static List<Produto> produtosVerificados = new List<Produto>;
    static void Main()
    {
        int intervalo = 60000;

        Timer time = new Timer(VerificarNovoProduto,null,0,intervalo);

        Console.WriteLine("Pressionar qualquer tecla para sair...");
        Console.ReadKey();
    }
    static async void VerificarNovoProduto(object state)
    {
        string username = "11164448";
        string senha = "60-dayfreetrial";
        string url = "http://regymatrix-001-site1.ktempurl.com/api/v1/produto/getall";

        try
        {
            using HttpClient client = new();
            var bytearray = Encoding.ASCII.GetBytes($"{username}:{senha}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytearray));
        
            HttpResponseMessage response = await client.GetAsync(url);

            if(response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseData);
                List<Produto> novosProdutos = ObterNovosProdutos(responseData);
                foreach(Produto produto in novosProdutos)
                {
                    if(!produtosVerificados.Exists(p=>p.Id == produto.Id))
                    {
                        Console.WriteLine($"Novo produto encontrado: ID {produto.Id}, Nome: {produto.Nome}");
                        produtosVerificados.Add(produto);
                    }
                }           
            }
            else
            {
                Console.WriteLine($"Erro:{response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao fazer a requisição. {ex.Message}");
        }
    }
    
    static List<Produto> ObterNovosProdutos(string responseData)
    {
        List<Produto> produtos = JsonConvert.DeserializeObject<List<Produto>>(responseData);
        return produtos;
    }
    public class Produto
    {
        public int Id { get; set; }

        public string Nome { get; set; }
    }

}