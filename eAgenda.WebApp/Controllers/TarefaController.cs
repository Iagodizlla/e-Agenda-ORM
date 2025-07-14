using eAgenda.Dominio.ModuloContato;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Infraestrutura.Orm.Compartilhado;
using eAgenda.WebApp.Extensions;
using eAgenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Controllers;

[Route("tarefas")]
public class TarefaController : Controller
{
    private readonly eAgendaDbContext contexto;
    private readonly IRepositorioTarefa repositorioTarefa;

    public TarefaController(eAgendaDbContext contexto, IRepositorioTarefa repositorioTarefa)
    {
        this.contexto = contexto;
        this.repositorioTarefa = repositorioTarefa;
    }

    [HttpGet]
    public IActionResult Index(string? status)
    {
        List<Tarefa> registros;

        switch (status)
        {
            case "pendentes": registros = repositorioTarefa.SelecionarTarefasPendentes(); break;
            case "concluidas": registros = repositorioTarefa.SelecionarTarefasConcluidas(); break;
            default: registros = repositorioTarefa.SelecionarRegistros(); break;
        }

        var visualizarVM = new VisualizarTarefasViewModel(registros);

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
        var registros = repositorioTarefa.SelecionarRegistros();

        foreach (var item in registros)
        {
            if (item.Titulo.Equals(cadastrarVM.Titulo))
            {
                ModelState.AddModelError("CadastroUnico", "Já existe uma tarefa registrada com este título.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(cadastrarVM);

        var entidade = cadastrarVM.ParaEntidade();

        var transacao = contexto.Database.BeginTransaction();

        try
        {
            repositorioTarefa.CadastrarRegistro(entidade);

            contexto.SaveChanges();

            transacao.Commit();
        }
        catch (Exception)
        {
            transacao.Rollback();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public IActionResult Editar(Guid id)
    {
        var registroSelecionado = repositorioTarefa.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var editarVM = new EditarTarefaViewModel(
            registroSelecionado.Id,
            registroSelecionado.Titulo,
            registroSelecionado.Prioridade
        );

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, EditarTarefaViewModel editarVM)
    {
        var registros = repositorioTarefa.SelecionarRegistros();

        foreach (var item in registros)
        {
            if (!item.Id.Equals(id) && item.Titulo.Equals(editarVM.Titulo))
            {
                ModelState.AddModelError("CadastroUnico", "Já existe uma tarefa registrada com este título.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(editarVM);

        var registroEditado = editarVM.ParaEntidade();

        var transacao = contexto.Database.BeginTransaction();

        try
        {
            repositorioTarefa.EditarRegistro(id, registroEditado);

            contexto.SaveChanges();

            transacao.Commit();
        }
        catch (Exception)
        {
            transacao.Rollback();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        var registroSelecionado = repositorioTarefa.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var excluirVM = new ExcluirTarefaViewModel(registroSelecionado.Id, registroSelecionado.Titulo);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    public IActionResult ExcluirConfirmado(Guid id)
    {
        var transacao = contexto.Database.BeginTransaction();

        try
        {
            repositorioTarefa.ExcluirRegistro(id);

            contexto.SaveChanges();

            transacao.Commit();
        }
        catch (Exception)
        {

            transacao.Rollback();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost, Route("/tarefas/{id:guid}/alternar-status")]
    public IActionResult AlternarStatus(Guid id)
    {
        var tarefaSelecionada = repositorioTarefa.SelecionarRegistroPorId(id);

        if (tarefaSelecionada is null)
            return RedirectToAction(nameof(Index));

        if (tarefaSelecionada.Concluida)
            tarefaSelecionada.MarcarPendente();
        else
            tarefaSelecionada.Concluir();

        var transacao = contexto.Database.BeginTransaction();

        try
        {
            repositorioTarefa.EditarRegistro(id, tarefaSelecionada);

            contexto.SaveChanges();

            transacao.Commit();
        }
        catch (Exception)
        {
            transacao.Rollback();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet, Route("/tarefas/{id:guid}/gerenciar-itens")]
    public IActionResult GerenciarItens(Guid id)
    {
        var tarefaSelecionada = repositorioTarefa.SelecionarRegistroPorId(id);

        if (tarefaSelecionada is null)
            return RedirectToAction(nameof(Index));

        var gerenciarItensViewModel = new GerenciarItensViewModel(tarefaSelecionada);

        return View(gerenciarItensViewModel);
    }

    [HttpPost, Route("/tarefas/{id:guid}/adicionar-item")]
    public IActionResult AdicionarItem(Guid id, string tituloItem)
    {
        var tarefaSelecionada = repositorioTarefa.SelecionarRegistroPorId(id);

        if (tarefaSelecionada is null)
            return RedirectToAction(nameof(Index));

        var itemAdicionado = tarefaSelecionada.AdicionarItem(tituloItem);

        var item = tarefaSelecionada.AdicionarItem(itemAdicionado);

        contexto.ItensTarefa.Add(item);

        contexto.SaveChanges();

        var gerenciarItensViewModel = new GerenciarItensViewModel(tarefaSelecionada);

        return View(nameof(GerenciarItens), gerenciarItensViewModel);
    }

    [HttpPost, Route("/tarefas/{idTarefa:guid}/alternar-status-item/{idItem:guid}")]
    public IActionResult AlternarStatusItem(Guid idTarefa, Guid idItem)
    {
        var tarefaSelecionada = repositorioTarefa.SelecionarRegistroPorId(idTarefa);

        if (tarefaSelecionada is null)
            return RedirectToAction(nameof(Index));

        var itemSelecionado = tarefaSelecionada.ObterItem(idItem);

        if (itemSelecionado is null)
            return RedirectToAction(nameof(Index));

        if (!itemSelecionado.Concluido)
            tarefaSelecionada.ConcluirItem(itemSelecionado);
        else
            tarefaSelecionada.MarcarItemPendente(itemSelecionado);

        tarefaSelecionada.AtualizarItem(itemSelecionado);

        contexto.SaveChanges();

        var gerenciarItensViewModel = new GerenciarItensViewModel(tarefaSelecionada);

        return View(nameof(GerenciarItens), gerenciarItensViewModel);
    }

    [HttpPost, Route("/tarefas/{idTarefa:guid}/remover-item/{idItem:guid}")]
    public IActionResult RemoverItem(Guid idTarefa, Guid idItem)
    {
        var tarefaSelecionada = repositorioTarefa.SelecionarRegistroPorId(idTarefa);

        if (tarefaSelecionada is null)
            return RedirectToAction(nameof(Index));

        var itemSelecionado = tarefaSelecionada.ObterItem(idItem);

        if (itemSelecionado is null)
            return RedirectToAction(nameof(Index));

        tarefaSelecionada.RemoverItem(itemSelecionado);

        tarefaSelecionada.RemoverItem(itemSelecionado);

        contexto.SaveChanges();

        var gerenciarItensViewModel = new GerenciarItensViewModel(tarefaSelecionada);

        return View(nameof(GerenciarItens), gerenciarItensViewModel);
    }
}