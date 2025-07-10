using eAgenda.Dominio.ModuloContato;
using eAgenda.WebApp.Extensions;
using eAgenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Controllers;

[Route("contatos")]
public class ContatoController : Controller
{
    private readonly IRepositorioContato repositorioContato;

    public ContatoController(IRepositorioContato repositorioContato)
    {
        this.repositorioContato = repositorioContato;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var registros = repositorioContato.SelecionarRegistros();

        var visualizarVM = new VisualizarContatosViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        var cadastrarVM = new CadastrarContatoViewModel();

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarContatoViewModel cadastrarVM)
    {
        var registros = repositorioContato.SelecionarRegistros();

        foreach (var item in registros)
        {
            if (item.Nome.Equals(cadastrarVM.Nome))
            {
                ModelState.AddModelError("CadastroUnico", "Já existe um contato registrado com este nome.");
                return View(cadastrarVM);
            }

            if (item.Email.Equals(cadastrarVM.Email))
            {
                ModelState.AddModelError("CadastroUnico", "Já existe um contato registrado com este endereço de e-mail.");
                return View(cadastrarVM);
            }
        }

        var entidade = cadastrarVM.ParaEntidade();

        repositorioContato.CadastrarRegistro(entidade);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public ActionResult Editar(Guid id)
    {
        var registroSelecionado = repositorioContato.SelecionarRegistroPorId(id);

        var editarVM = new EditarContatoViewModel(
            id,
            registroSelecionado.Nome,
            registroSelecionado.Telefone,
            registroSelecionado.Email,
            registroSelecionado.Empresa,
            registroSelecionado.Cargo
        );

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(Guid id, EditarContatoViewModel editarVM)
    {
        var registros = repositorioContato.SelecionarRegistros();

        foreach (var item in registros)
        {
            if (!item.Id.Equals(id) && item.Nome.Equals(editarVM.Nome))
            {
                ModelState.AddModelError("CadastroUnico", "Já existe um contato registrado com este nome.");
                return View(editarVM);

            }

            if (!item.Id.Equals(id) && item.Email.Equals(editarVM.Email))
            {
                ModelState.AddModelError("CadastroUnico", "Já existe um contato registrado com este endereço de e-mail.");
                return View(editarVM);
            }
        }

        var entidadeEditada = editarVM.ParaEntidade();

        repositorioContato.EditarRegistro(id, entidadeEditada);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        var registroSelecionado = repositorioContato.SelecionarRegistroPorId(id);

        var excluirVM = new ExcluirContatoViewModel(
            registroSelecionado.Id,
            registroSelecionado.Nome
            );

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id)
    {
        repositorioContato.ExcluirRegistro(id);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public IActionResult Detalhes(Guid id)
    {
        var registroSelecionado = repositorioContato.SelecionarRegistroPorId(id);

        var detalhesVM = new DetalhesContatoViewModel(
            id,
            registroSelecionado.Nome,
            registroSelecionado.Telefone,
            registroSelecionado.Email,
            registroSelecionado.Empresa,
            registroSelecionado.Cargo
        );

        return View(detalhesVM);
    }
}