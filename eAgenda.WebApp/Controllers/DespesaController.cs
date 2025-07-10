using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.WebApp.Extensions;
using eAgenda.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eAgenda.WebApp.Controllers;

[Route("despesas")]
public class DespesaController : Controller
{
    private readonly IRepositorioDespesa repositorioDespesa;
    private readonly IRepositorioCategoria repositorioCategoria;

    public DespesaController(
        IRepositorioDespesa repositorioDespesa,
        IRepositorioCategoria repositorioCategoria
    )
    {
        this.repositorioDespesa = repositorioDespesa;
        this.repositorioCategoria = repositorioCategoria;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var registros = repositorioDespesa.SelecionarRegistros();

        var visualizarVM = new VisualizarDespesasViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        var categoriasDisponiveis = repositorioCategoria.SelecionarRegistros();

        var cadastrarVM = new CadastrarDespesaViewModel(categoriasDisponiveis);

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarDespesaViewModel cadastrarVM)
    {
        var categoriasDisponiveis = repositorioCategoria.SelecionarRegistros();

        if (!ModelState.IsValid)
        {
            foreach (var cd in categoriasDisponiveis)
            {
                var selecionarVM = new SelectListItem(cd.Titulo, cd.Id.ToString());

                cadastrarVM.CategoriasDisponiveis?.Add(selecionarVM);
            }

            return View(cadastrarVM);
        }

        var despesa = cadastrarVM.ParaEntidade();

        var categoriasSelecionadas = cadastrarVM.CategoriasSelecionadas;

        if (categoriasSelecionadas is not null)
        {
            foreach (var cs in categoriasSelecionadas)
            {
                foreach (var cd in categoriasDisponiveis)
                {
                    if (cs.Equals(cd.Id))
                    {
                        despesa.RegistarCategoria(cd);
                        break;
                    }
                }
            }
        }

        repositorioDespesa.CadastrarRegistro(despesa);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public ActionResult Editar(Guid id)
    {
        var categoriasDisponiveis = repositorioCategoria.SelecionarRegistros();

        var registroSelecionado = repositorioDespesa.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var editarVM = new EditarDespesaViewModel(
            id,
            registroSelecionado.Descricao,
            registroSelecionado.Valor,
            registroSelecionado.DataOcorencia,
            registroSelecionado.FormaPagamento,
            registroSelecionado.Categorias,
            categoriasDisponiveis
        );

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(Guid id, EditarDespesaViewModel editarVM)
    {
        var categoriasDisponiveis = repositorioCategoria.SelecionarRegistros();

        if (!ModelState.IsValid)
        {
            foreach (var cd in categoriasDisponiveis)
            {
                var selecionarVM = new SelectListItem(cd.Titulo, cd.Id.ToString());

                editarVM.CategoriasDisponiveis?.Add(selecionarVM);
            }

            return View(editarVM);
        }

        // Obtém dados editados
        var despesaEditada = editarVM.ParaEntidade();

        var categoriasSelecionadas = editarVM.CategoriasSelecionadas;

        // Adiciona as categorias selecionadas
        if (categoriasSelecionadas is not null)
        {
            foreach (var idSelecionado in categoriasSelecionadas)
            {
                foreach (var categoriaDisponivel in categoriasDisponiveis)
                {
                    if (categoriaDisponivel.Id.Equals(idSelecionado))
                    {
                        despesaEditada.RegistarCategoria(categoriaDisponivel);
                        break;
                    }
                }
            }
        }

        // Atualiza os dados da despesa selecionada
        repositorioDespesa.EditarRegistro(id, despesaEditada);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        var registroSelecionado = repositorioDespesa.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var excluirVM = new ExcluirDespesaViewModel(
            registroSelecionado.Id,
            registroSelecionado.Descricao
        );

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id)
    {
        var registroSelecionado = repositorioDespesa.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        foreach (var cat in registroSelecionado.Categorias.ToList())
            registroSelecionado.RemoverCategoria(cat);

        repositorioDespesa.ExcluirRegistro(id);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Detalhes(Guid id)
    {
        var registroSelecionado = repositorioDespesa.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var detalhesVM = new DetalhesDespesaViewModel(
            id,
            registroSelecionado.Descricao,
            registroSelecionado.Valor,
            registroSelecionado.DataOcorencia,
            registroSelecionado.FormaPagamento,
            registroSelecionado.Categorias
        );

        return View(detalhesVM);
    }
}