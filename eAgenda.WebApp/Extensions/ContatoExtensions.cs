using eAgenda.Dominio.ModuloContato;
using eAgenda.WebApp.Models;
using static eAgenda.WebApp.Models.FormularioContatoViewModel;

namespace eAgenda.WebApp.Extensions;

public static class ContatoExtensions
{
    public static Contato ParaEntidade(this FormularioContatoViewModel formularioVM)
    {
        return new Contato(
            formularioVM.Nome,
            formularioVM.Telefone,
            formularioVM.Email,
            formularioVM.Empresa,
            formularioVM.Cargo
        );
    }

    public static DetalhesContatoViewModel ParaDetalhesVM(this Contato contato)
    {
        return new DetalhesContatoViewModel(
                contato.Id,
                contato.Nome,
                contato.Telefone,
                contato.Email,
                contato.Empresa,
                contato.Cargo
        );
    }
}