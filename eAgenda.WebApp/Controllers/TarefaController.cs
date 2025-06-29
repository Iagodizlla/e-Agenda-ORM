using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Infraestrutura.Compartilhado;
using eAgenda.Infraestrutura.ModuloTarefa;
using eAgenda.WebApp.Extensions;
using eAgenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using static eAgenda.WebApp.Models.FormularioTarefaViewModel;

namespace eAgenda.WebApp.Controllers
{
    [Route("tarefas")]
    public class TarefaController : Controller
    {
        private readonly ContextoDeDados contextoDados;
        private readonly IRepositorioTarefa repositorioTarefa;

        public TarefaController()
        {
            contextoDados = new ContextoDeDados(true);
            repositorioTarefa = new RepositorioTarefaEmArquivo(contextoDados);
        }

        [HttpGet]
        public IActionResult Index(string? status)
        {
            List<Tarefa> registros;

            List<Tarefa> tarefas = repositorioTarefa.SelecionarTarefas();

            foreach (var tarefa in tarefas)
            {
                repositorioTarefa.AtualizarPercentual(tarefa.Id);
                repositorioTarefa.AtualizarStatus(tarefa.Id);
            }

            switch (status)
            {
                case "Pendente": registros = repositorioTarefa.SelecionarTarefasPendentes(); break;
                case "Concluído": registros = repositorioTarefa.SelecionarTarefasConcluidas(); break;
                default: registros = repositorioTarefa.SelecionarTarefas(); break;
            }

            var visualizarVM = new VisualizarTarefaViewModel(registros);

            return View(visualizarVM);
        }

        [HttpGet("cadastrar")]
        public IActionResult Cadastrar()
        {
            var cadastrarVM = new CadastrarTarefaViewModel();

            return View(cadastrarVM);
        }

        [HttpPost("cadastrar")]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(CadastrarTarefaViewModel cadastrarVM)
        {
            var registros = repositorioTarefa.SelecionarTarefas();

            foreach (var item in registros)
            {
                if (item.Titulo.Equals(cadastrarVM.Titulo))
                {
                    ModelState.AddModelError("CadastroUnico", "Já existe uma Tarefa registrado com este Titulo.");
                    break;
                }
            }

            for (int i = 0; i < cadastrarVM.TitulosItens.Count; i++)
            {
                var tituloItem = cadastrarVM.TitulosItens[i];

                if (string.IsNullOrWhiteSpace(tituloItem) || tituloItem.Length < 2 || tituloItem.Length > 100)
                {
                    ModelState.AddModelError($"TitulosItens[{i}]", $"O título do item {i + 1} deve ter entre 2 e 100 caracteres.");
                }
            }

            if (!ModelState.IsValid)
                return View(cadastrarVM);


            var entidade = cadastrarVM.ParaEntidade();

            repositorioTarefa.CadastrarTarefa(entidade);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("editar/{id:guid}")]
        public IActionResult Editar(Guid id)
        {
            var tarefa = repositorioTarefa.SelecionarPorId(id);

            if (tarefa == null)
                return NotFound();

            var editarVM = new EditarTarefaViewModel(
                tarefa.Id,
                tarefa.Titulo,
                tarefa.Prioridade,
                tarefa.DataCriacao,
                tarefa.StatusConcluida,
                tarefa.PercentualConcluida,
                tarefa.DataConclusao,
                tarefa.Items
            );

            return View(editarVM);


        }

        [HttpPost("editar/{id:guid}")]
        public IActionResult EditarConfirmado(Guid id, EditarTarefaViewModel editarVM)
        {
            for (int i = 0; i < editarVM.TitulosItens.Count; i++)
            {
                var tituloItem = editarVM.TitulosItens[i];

                if (string.IsNullOrWhiteSpace(tituloItem) || tituloItem.Length < 2 || tituloItem.Length > 100)
                {
                    ModelState.AddModelError($"TitulosItens[{i}]", $"O título do item {i + 1} deve ter entre 2 e 100 caracteres.");
                }
            }

            if (!ModelState.IsValid)
                return View("Editar", editarVM);

            var entidadeEditada = editarVM.ParaEntidade();

            repositorioTarefa.EditarTarefa(id, entidadeEditada);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet("excluir/{id:guid}")]
        public IActionResult Excluir(Guid id)
        {
            var registroSelecionado = repositorioTarefa.SelecionarPorId(id);

            var excluirVM = new ExcluirTarefaViewModel(registroSelecionado.Id, registroSelecionado.Titulo);

            return View(excluirVM);
        }

        [HttpPost("excluir/{id:guid}")]
        public IActionResult ExcluirConfirmado(Guid id)
        {
            repositorioTarefa.ExcluirTarefa(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("detalhes/{id:guid}")]
        public IActionResult Detalhes(Guid id)
        {
            var tarefa = repositorioTarefa.SelecionarPorId(id);

            if (tarefa == null)
                return NotFound();

            var detalhesVM = new DetalhesTarefaViewModel(
                tarefa.Id,
                tarefa.Titulo,
                tarefa.Prioridade,
                tarefa.DataCriacao,
                tarefa.StatusConcluida,
                tarefa.PercentualConcluida,
                tarefa.Items,
                tarefa.DataConclusao
            );

            return View(detalhesVM);
        }
    }
}