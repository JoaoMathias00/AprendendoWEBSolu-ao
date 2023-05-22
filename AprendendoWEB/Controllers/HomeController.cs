using AprendendoWEB.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace AprendendoWEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Sobre()
        {
            return View();
        }
        public IActionResult informacao()
        {

            return View();
        }

        public IActionResult Produtos()
        {
            SqlConnection conexao = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ListaProdutos;Integrated Security=True");
            SqlCommand comando = new SqlCommand("select * from Produtos", conexao);
            conexao.Open();
            SqlDataReader retorno = comando.ExecuteReader();


            List<Produto> lista = new List<Produto>();
            while (retorno.Read())
            {
                Produto produto = new Produto();
                produto.nome = retorno["Nome"].ToString();
                produto.img = retorno["Imagem"].ToString();
                produto.descricao = retorno["Decricao"].ToString();
                produto.preco = retorno["Preco"].ToString();
                lista.Add(produto);

            }
            conexao.Close();

            ViewBag.novo = lista;
            ViewBag.produto = "exibindo 20 produtos";
            return View();
        }
        public IActionResult salvar(Produto produto)
        {
            SqlConnection conexao = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ListaProdutos;Integrated Security=True");
            SqlCommand comando = new SqlCommand("INSERT INTO PRODUTOS (NOME,IMAGEM,DECRICAO,PRECO) VALUES ('" + produto.nome + "','" + produto.img + "','" + produto.descricao + "','" + produto.preco + "')", conexao);
            conexao.Open();
            comando.ExecuteNonQuery();

            conexao.Close();

            return RedirectToAction("Produtos");

        }
		public IActionResult Deletar(int id)
		{
			SqlConnection conexao = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ListaProdutos;Integrated Security=True");
			SqlCommand comando = new SqlCommand("DELETE FROM Produtos WHERE Id = @id", conexao);
			comando.Parameters.AddWithValue("@id", id);
			conexao.Open();
			int linhasAfetadas = comando.ExecuteNonQuery();
			conexao.Close();

			if (linhasAfetadas > 0)
			{
				// Registro deletado com sucesso
				return RedirectToAction("Produtos");
			}
			else
			{
				// Não foi possível encontrar o registro com o ID especificado
				return View("Error");
			}
		}








		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}