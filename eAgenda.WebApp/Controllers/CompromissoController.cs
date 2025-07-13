using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Infraestrutura.Orm.Compartilhado;
using eAgenda.WebApp.Extensions;
using eAgenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eAgenda.WebApp.Controllers;

[Route("compromissos")]
public class CompromissoController : Controller
{
    private readonly eAgendaDbContext contexto;
    private readonly IRepositorioCompromisso repositorioCompromisso;
    private readonly IRepositorioContato repositorioContato;

    public CompromissoController(
        eAgendaDbContext contexto,
        IRepositorioCompromisso repositorioCompromisso,
        IRepositorioContato repositorioContato

    )
    {
        this.contexto = contexto;
        this.repositorioCompromisso = repositorioCompromisso;
        this.repositorioContato = repositorioContato;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var registros = repositorioCompromisso.SelecionarRegistros();

        var visualizarVM = new VisualizarCompromissosViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        var contatosDisponiveis = repositorioContato.SelecionarRegistros();

        var cadastrarVM = new CadastrarCompromissoViewModel(contatosDisponiveis);

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarCompromissoViewModel cadastrarVM)
    {
        var contatosDisponiveis = repositorioContato.SelecionarRegistros();

        if (!ModelState.IsValid)
        {
            foreach (var cd in contatosDisponiveis)
            {
                var selecionarVM = new SelectListItem(cd.Nome, cd.Id.ToString());

                cadastrarVM.ContatosDisponiveis?.Add(selecionarVM);
            }

            return View(cadastrarVM);
        }

        var despesa = cadastrarVM.ParaEntidade(contatosDisponiveis);

        var transacao = contexto.Database.BeginTransaction();

        try
        {
            repositorioCompromisso.CadastrarRegistro(despesa);

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
    public ActionResult Editar(Guid id)
    {
        var contatosDisponiveis = repositorioContato.SelecionarRegistros();

        var registroSelecionado = repositorioCompromisso.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var editarVM = new EditarCompromissoViewModel(
            id,
            registroSelecionado.Assunto,
            registroSelecionado.Data,
            registroSelecionado.HoraInicio,
            registroSelecionado.HoraTermino,
            registroSelecionado.Tipo,
            registroSelecionado.Local,
            registroSelecionado.Link,
            registroSelecionado.Contato?.Id,
            contatosDisponiveis
        );

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(Guid id, EditarCompromissoViewModel editarVM)
    {
        var contatosDisponiveis = repositorioContato.SelecionarRegistros();

        if (!ModelState.IsValid)
        {
            foreach (var cd in contatosDisponiveis)
            {
                var selecionarVM = new SelectListItem(cd.Nome, cd.Id.ToString());

                editarVM.ContatosDisponiveis?.Add(selecionarVM);
            }

            return View(editarVM);
        }

        var compromissoEditado = editarVM.ParaEntidade(contatosDisponiveis);

        var transacao = contexto.Database.BeginTransaction();

        try
        {
            repositorioCompromisso.EditarRegistro(id, compromissoEditado);

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
        var registroSelecionado = repositorioCompromisso.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var excluirVM = new ExcluirCompromissoViewModel(registroSelecionado.Id, registroSelecionado.Assunto);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id)
    {
        var registroSelecionado = repositorioCompromisso.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var transacao = contexto.Database.BeginTransaction();

        try
        {
            repositorioCompromisso.ExcluirRegistro(id);

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
}