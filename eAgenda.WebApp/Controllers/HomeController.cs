using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Infraestrutura.SqlServer.ModuloCompromisso;
using eAgenda.Infraestrutura.SqlServer.ModuloTarefa;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositorioCompromisso repositorioCompromisso;
        private readonly IRepositorioTarefa repositorioTarefa;

        public HomeController()
        {
            repositorioCompromisso = new RepositorioCompromissoEmSql();
            repositorioTarefa = new RepositorioTarefaEmSql();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var compromissos = repositorioCompromisso.SelecionarRegistros().Select
                (c => new { Id = c.Id, Tipo = "Compromisso", Titulo = c.Assunto, Data = c.DataOcorrencia });

            var tarefas = repositorioTarefa.SelecionarTarefas().Select
                (t => new { Id = t.Id, Tipo = "Tarefa", Titulo = t.Titulo, Data = t.DataConclusao != default ? t.DataConclusao : t.DataCriacao });

            var itensDaAgenda = compromissos.Concat(tarefas).OrderBy(x => x.Data).ToList();

            ViewBag.ItensDaAgenda = itensDaAgenda;

            return View();
        }

        [HttpGet("erro")]
        public IActionResult Erro()
        {
            return View();
        }
    }
}
